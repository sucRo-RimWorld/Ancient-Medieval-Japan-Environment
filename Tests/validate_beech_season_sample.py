"""Check accelerated native calendar/state sampling evidence, not visual approval."""
import argparse
import re
from pathlib import Path

parser = argparse.ArgumentParser()
parser.add_argument("log", type=Path)
args = parser.parse_args()
text = args.log.read_text(encoding="utf-8-sig")
result = re.search(r"SeasonSample\] result minFall=([\d.]+) maxFall=([\d.]+) "
                   r"leafless=(True|False) recovered=(True|False) validTextures=(True|False) "
                   r"minimumTemperature=([-\d.]+) leaflessThreshold=([-\d.]+)", text)
assert result, "Missing completed seasonal sample"
low, high, leafless, recovery, textures, temperature, threshold = result.groups()
assert float(low) < .1 and float(high) > .9, "Native fall factor did not span seasons"
assert leafless == recovery == textures == "True", "State transition/material check failed"
assert float(temperature) < float(threshold), "Cold trigger was not reached"
rows = re.findall(r"SeasonSample\] step=\d+.*leafless=(True|False) texture=(\w+)", text)
assert ("True", "Beech_Leafless_A") in rows, "Missing live leafless material"
leafless_index = rows.index(("True", "Beech_Leafless_A"))
assert ("False", "Beech_A") in rows[leafless_index + 1:], "Missing live recovery material"
assert "not full-world tick simulation" in text, "Missing test scope disclosure"
print("[OK] Native calendar factor, cold leafless state, recovery and live textures verified")
