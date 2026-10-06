"""Historical local-review ZIP; Workshop uses Build-WorkshopPayload.py."""
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
for name in ('LICENSE','README.md','Docs/SteamWorkshopDescription.txt','Docs/SteamWorkshopDescription-ja.txt'):
 payload[name]=(ROOT/name).read_bytes()
payload['loadFolders.xml']=b'<?xml version="1.0" encoding="utf-8"?>\n<loadFolders><v1.6><li>/</li></v1.6></loadFolders>\n'
assert 'Assemblies/AncientMedievalJapanEnvironment.dll' in payload
assert ET.fromstring(payload['About/About.xml']).findtext('packageId')=='sucro.ancientmedievaljapan.environment'
for p in ET.fromstring(payload['Defs/ThingDefs_Plants/AMJ_WildPlants.xml']):
 for field in ('graphicData/texPath','plant/leaflessGraphicPath','plant/snowOverlayGraphicPath','plant/leaflessSnowOverlayGraphicPath'):
  path=p.findtext(field)
  if path:assert any(k.startswith('Textures/'+path+'/') and k.endswith('.png') for k in payload),(p.findtext('defName'),field)
 assert p.findtext('defName')!='AMJ_Shrub_Haimatsu' or p.findtext('uiIconPath')=='Things/Plant/AMJ/Haimatsu/Haimatsu_A'
assert not any(k.startswith(('DevQuickstarts/','Tests/','TestResults/','Art/','Scripts/')) for k in payload)
with zipfile.ZipFile(archive,'w',zipfile.ZIP_DEFLATED) as z:
 for name,content in payload.items():z.writestr('AncientMedievalJapanEnvironment/'+name,content)
with zipfile.ZipFile(archive) as z:
 assert z.testzip() is None
 for name,content in payload.items():assert z.read('AncientMedievalJapanEnvironment/'+name)==content
manifest={'candidate':archive.name,'sha256':hashlib.sha256(archive.read_bytes()).hexdigest(),'files':{k:hashlib.sha256(v).hexdigest() for k,v in payload.items()},'source':'current local working tree; uncommitted','runtime_validation':'Not performed by this builder; local review only','workshop_release_gate':False,'publication_procedure':'Docs/GoldenPaths/WorkshopPublication.md','release_load_folders':'root only; development quickstarts excluded'}
archive.with_suffix('.manifest.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print(str(archive));print('PASS: archive CRC/exact bytes, production assembly, four plant state paths and UI fix;',len(payload),'files');print('SHA256',manifest['sha256'])
print('LOCAL REVIEW ONLY: Workshop publication requires Build-WorkshopPayload.py and the four-profile cutting gate.')
