#!/usr/bin/env python3
"""Fail CI when the Crash Site MVP smoke log is missing a required milestone."""

from __future__ import annotations

import argparse
from pathlib import Path

REQUIRED_MILESTONES = tuple(f"MVP_SMOKE_MILESTONE_{number:02d}" for number in range(1, 12))


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Verify that a Godot integration log contains every Crash Site MVP smoke milestone marker."
    )
    parser.add_argument("log_path", type=Path, help="Path to the integration log produced by IntegrationTestRunner.")
    return parser.parse_args()


def main() -> int:
    args = parse_args()
    if not args.log_path.is_file():
        print(f"MVP smoke log not found: {args.log_path}")
        return 1

    log_text = args.log_path.read_text(encoding="utf-8", errors="replace")
    missing = [milestone for milestone in REQUIRED_MILESTONES if milestone not in log_text]
    if missing:
        print("Missing Crash Site MVP smoke milestone marker(s):")
        for milestone in missing:
            print(f"- {milestone}")
        return 1

    print(f"Crash Site MVP smoke milestone log verified: {args.log_path}")
    for milestone in REQUIRED_MILESTONES:
        print(f"- {milestone}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
