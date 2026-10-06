"""Build and verify a runtime-only candidate from the current approved workspace."""
from pathlib import Path
from datetime import datetime
import hashlib,json,zipfile,xml.etree.ElementTree as ET
ROOT=Path(__file__).resolve().parents[1]
OUT=ROOT/'TestResults/ReleaseCandidates';OUT.mkdir(parents=True,exist_ok=True)
stamp=datetime.now().strftime('%Y%m%d-%H%M%S')
archive=OUT/f'AncientMedievalJapanEnvironment-{stamp}.zip'
payload={}
for folder in ('About','Assemblies','Defs','Languages','Patches','Textures'):
 for p in sorted((ROOT/folder).rglob('*')):
  if p.is_file():payload[p.relative_to(ROOT).as_posix()]=p.read_bytes()
for name in ('LICENSE',):
 payload[name]=(ROOT/name).read_bytes()
payload['loadFolders.xml']=b'<?xml version="1.0" encoding="utf-8"?>\n<loadFolders><v1.6><li>/</li></v1.6></loadFolders>\n'
assert 'Assemblies/AncientMedievalJapanEnvironment.dll' in payload
assert ET.fromstring(payload['About/About.xml']).findtext('packageId')=='sucro.ancientmedievaljapan.environment'
for p in ET.fromstring(payload['Defs/ThingDefs_Plants/AMJ_WildPlants.xml']):
 for field in ('graphicData/texPath','plant/leaflessGraphicPath','plant/snowOverlayGraphicPath','plant/leaflessSnowOverlayGraphicPath'):
  path=p.findtext(field)
  if path:assert any(k.startswith('Textures/'+path+'/') and k.endswith('.png') for k in payload),(p.findtext('defName'),field)
 assert p.findtext('defName')!='AMJ_Shrub_Haimatsu' or p.findtext('uiIconPath')=='Things/Plant/AMJ/Haimatsu/Haimatsu_A'
assert not any(k.startswith(('DevQuickstarts/','Tests/','TestResults/','Art/','Scripts/','Docs/','Source/')) for k in payload)
assert 'README.md' not in payload
import subprocess
import tempfile
with tempfile.TemporaryDirectory() as check_dir:
 check_root=Path(check_dir)
 for name,content in payload.items():
  target=check_root/name;target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(content)
 subprocess.run([__import__('sys').executable,str(ROOT/'Tests/validate_workshop_payload.py'),'--payload',str(check_root),'--expected-assembly','AncientMedievalJapanEnvironment.dll'],check=True)
with zipfile.ZipFile(archive,'w',zipfile.ZIP_DEFLATED) as z:
 for name,content in payload.items():z.writestr('AncientMedievalJapanEnvironment/'+name,content)
with zipfile.ZipFile(archive) as z:
 assert z.testzip() is None
 for name,content in payload.items():assert z.read('AncientMedievalJapanEnvironment/'+name)==content
manifest={'candidate':archive.name,'sha256':hashlib.sha256(archive.read_bytes()).hexdigest(),'files':{k:hashlib.sha256(v).hexdigest() for k,v in payload.items()},'source':'current local working tree; uncommitted','runtime_validation':'Source build/static and native visual checks passed; this ZIP has not separately undergone a clean installed runtime test','release_load_folders':'root only; development quickstarts excluded'}
archive.with_suffix('.manifest.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print(str(archive));print('PASS: archive CRC/exact bytes, production assembly, four plant state paths and UI fix;',len(payload),'files');print('SHA256',manifest['sha256'])

