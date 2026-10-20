#!/usr/bin/env python3
"""Read-only publisher-host prerequisites. Never uploads or changes Steam tags."""
from __future__ import annotations

import argparse
import os
from pathlib import Path
import sys

from publisher import config_from, require, state_file, state_load, validate_mode


def linux_probe(checkout: Path, repository: str) -> None:
    cfg = config_from(checkout, repository)
    validate_mode(cfg, "update_existing")
    require(cfg["visibility_policy"] == "preserve_public",
            "Environment update must preserve existing Workshop visibility")
    state = state_file(cfg)
    require(not state.resolve().is_relative_to(checkout.resolve()),
            "Host state is inside checkout")
    state_load(state, cfg, "update_existing")
    executable = Path(os.environ.get("AMJ_STEAMCMD_BIN", ""))
    account = os.environ.get("AMJ_STEAM_ACCOUNT", "")
    require(executable.is_file() and os.access(executable, os.X_OK),
            "Isolated SteamCMD executable not available")
    require(bool(account.strip()), "Isolated AMJ Steam account not provisioned")
    print("READINESS_LINUX_PASS owner state / identity and SteamCMD paths; no SteamCMD invoked")


def windows_probe(checkout: Path, repository: str) -> None:
    from tags import APPID, agent_call, private_receipt

    cfg = config_from(checkout, repository)
    validate_mode(cfg, "update_existing")
    require(cfg["visibility_policy"] == "preserve_public",
            "Environment update must preserve existing Workshop visibility")
    raw = os.environ.get("AMJ_STEAMWORKS_AGENT_BIN", "")
    require(bool(raw), "Windows Steamworks session-probe agent not provisioned")
    agent = Path(raw).resolve()
    require(agent.is_file() and os.access(agent, os.X_OK)
            and not agent.is_relative_to(checkout.resolve()),
            "Invalid Windows Steamworks session-probe executable")
    receipt = private_receipt(checkout, cfg)
    require(not receipt.exists() or receipt.is_file(),
            "Invalid tag receipt type")
    probe = agent_call(agent, ["steam-session-probe"], timeout=90)
    require(probe.get("ready") is True
            and probe.get("steamUserLoggedOn") is True
            and probe.get("steamAppId") == APPID,
            "Steamworks session is not authenticated for RimWorld")
    print("READINESS_WINDOWS_PASS isolated receipt and authenticated read-only session; no tags changed")


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("platform", choices=("linux", "windows"))
    parser.add_argument("--checkout", type=Path, required=True)
    parser.add_argument("--repo", required=True)
    args = parser.parse_args()
    checkout = args.checkout.resolve(strict=True)
    if args.platform == "linux":
        linux_probe(checkout, args.repo)
    else:
        windows_probe(checkout, args.repo)


if __name__ == "__main__":
    try:
        main()
    except (OSError, ValueError, KeyError, TypeError) as exc:
        print("READINESS_BLOCKED: " + str(exc), file=sys.stderr)
        sys.exit(2)
