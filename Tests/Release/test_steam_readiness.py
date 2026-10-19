#!/usr/bin/env python3
"""Read-only publisher readiness checks without Steam or host credentials."""
from __future__ import annotations

import json
import os
from pathlib import Path
import sys
import tempfile
from unittest import mock

ROOT = Path(__file__).resolve().parents[2]
sys.path.insert(0, str(ROOT / "Tools/SteamPublisher"))
import readiness
import tags

REPO = "sucRo-RimWorld/Ancient-Medieval-Japan-Environment"
CFG = json.loads((ROOT / "Publishing/SteamWorkshop.json").read_text(encoding="utf-8"))

with tempfile.TemporaryDirectory() as temporary:
    tmp = Path(temporary)
    root = tmp / "linux-state"
    root.mkdir(mode=0o700)
    root.chmod(0o700)
    file = root / (REPO.split("/")[1] + ".json")
    record = {"schema": 1, "appid": "294100", "repository": REPO,
              "publishedfileid": CFG["workshop_id"],
              "visibility_policy": "private_only", "identity_verified": True}
    file.write_text(json.dumps(record), encoding="utf-8")
    file.chmod(0o600)
    original = file.read_bytes()
    env = {"AMJ_STEAM_STATE_DIR": str(root), "AMJ_STEAMCMD_BIN": sys.executable,
           "AMJ_STEAM_ACCOUNT": "offline-test-only"}
    with mock.patch.dict(os.environ, env):
        readiness.linux_probe(ROOT, REPO)
        assert file.read_bytes() == original, "Readiness changed publisher state"
        record["identity_verified"] = False
        file.write_text(json.dumps(record), encoding="utf-8")
        try:
            readiness.linux_probe(ROOT, REPO)
        except ValueError as exc:
            assert "independently verified" in str(exc)
        else:
            raise AssertionError("Unverified publisher state accepted")
        record["identity_verified"] = True
        record["publishedfileid"] = "999999999"
        file.write_text(json.dumps(record), encoding="utf-8")
        try:
            readiness.linux_probe(ROOT, REPO)
        except ValueError as exc:
            assert "conflicts" in str(exc)
        else:
            raise AssertionError("Wrong Workshop identity accepted")
    home = tmp / "localappdata"
    home.mkdir()
    receipts = home / "receipts"
    receipts.mkdir()
    env = {"LOCALAPPDATA": str(home), "AMJ_STEAM_TAG_RECEIPT_DIR": str(receipts),
           "AMJ_STEAMWORKS_AGENT_BIN": sys.executable}
    with mock.patch.dict(os.environ, env), mock.patch.object(
            tags, "agent_call",
            return_value={"ready": True, "steamUserLoggedOn": True, "steamAppId": 294100}
            ) as mock_agent:
        readiness.windows_probe(ROOT, REPO)
        mock_agent.assert_called_once()
        assert mock_agent.call_args.args[1] == ["steam-session-probe"]
        assert not list(receipts.iterdir()), "Readiness wrote receipt"
    with mock.patch.dict(os.environ, env), mock.patch.object(
            tags, "agent_call",
            return_value={"ready": False, "steamUserLoggedOn": False, "steamAppId": 294100}):
        try:
            readiness.windows_probe(ROOT, REPO)
        except ValueError as exc:
            assert "not authenticated" in str(exc)
        else:
            raise AssertionError("Unauthenticated session accepted")

workflow = (ROOT / ".github/workflows/steam-readiness.yml").read_text(encoding="utf-8")
assert "[check:amj-publisher-readiness]" in workflow
assert "issues:" in workflow and "steam-workshop-private" in workflow
assert "github.event.issue.user.login == 'sucRo0629'" in workflow
assert "source_sha=" in workflow and "refs/heads/main" in workflow
assert "READINESS_AUTHOR_APPROVAL_PASS" in workflow
for forbidden in ("publisher.py upload", "tags.py sync",
                  "[publish:amj-update]", "[publish:amj-create-private]",
                  "workshop_build_item"):
    assert forbidden not in workflow, f"Mutation operation in readiness workflow: {forbidden}"
print("PASS read-only Steam host checks, exact ID and approval guard, no mutations")
