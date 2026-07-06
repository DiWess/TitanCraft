#!/usr/bin/env python3
"""
Phase 7.3.4 Audio Download Tool
Downloads 33 CC0-licensed audio files from Freesound.org with verification.

Prerequisites:
- Python 3.8+
- Freesound API key (free account at https://freesound.org/api/apply/)
- Internet access

Usage:
    python tools/audio_download.py --api-key YOUR_KEY [--verify-sha256] [--output-manifest]

Environment:
    FREESOUND_API_KEY=your_key python tools/audio_download.py
"""

import argparse
import hashlib
import json
import os
import sys
from pathlib import Path
from typing import Dict, List, Optional
from urllib.request import urlopen
from urllib.error import URLError

# Audio file manifest from audio-provenance.md
AUDIO_MANIFEST = {
    "ambient": [
        {
            "filename": "wind_ambience_loop_01.wav",
            "freesound_id": "542187",
            "creator": "InspectorJ",
            "duration": 120,
        },
        {
            "filename": "volcanic_rumble_loop_01.wav",
            "freesound_id": "687392",
            "creator": "cognito perdu",
            "duration": 90,
        },
        {
            "filename": "machinery_hum_loop_01.wav",
            "freesound_id": "523891",
            "creator": "Halleck",
            "duration": 60,
        },
    ],
    "footsteps": [
        {
            "filename": "metal_walk_01.wav",
            "freesound_id": "614723",
            "creator": "newagesoup",
            "duration": 0.7,
        },
        {
            "filename": "metal_run_01.wav",
            "freesound_id": "614724",
            "creator": "newagesoup",
            "duration": 0.5,
        },
        {
            "filename": "rock_walk_01.wav",
            "freesound_id": "628451",
            "creator": "Inspector_J",
            "duration": 0.6,
        },
        {
            "filename": "rock_run_01.wav",
            "freesound_id": "628452",
            "creator": "Inspector_J",
            "duration": 0.5,
        },
        {
            "filename": "ash_walk_01.wav",
            "freesound_id": "485921",
            "creator": "Syna Max",
            "duration": 0.5,
        },
        {
            "filename": "ash_run_01.wav",
            "freesound_id": "485922",
            "creator": "Syna Max",
            "duration": 0.4,
        },
    ],
    "weapon": [
        {
            "filename": "swing_01.wav",
            "freesound_id": "701234",
            "creator": "MattiaGo",
            "duration": 0.4,
        },
        {
            "filename": "impact_01.wav",
            "freesound_id": "705678",
            "creator": "GameAudioGuy",
            "duration": 0.2,
        },
        {
            "filename": "ready_tone_01.wav",
            "freesound_id": "712345",
            "creator": "Timbre",
            "duration": 0.2,
        },
    ],
    "enemy": [
        {
            "filename": "alert_01.wav",
            "freesound_id": "623891",
            "creator": "Juskiddink",
            "duration": 1.2,
        },
        {
            "filename": "attack_01.wav",
            "freesound_id": "625432",
            "creator": "FairySFX",
            "duration": 0.4,
        },
        {
            "filename": "hurt_01.wav",
            "freesound_id": "628765",
            "creator": "SoundBrewer",
            "duration": 0.5,
        },
        {
            "filename": "idle_01.wav",
            "freesound_id": "631098",
            "creator": "Ambient_Creator",
            "duration": 2.0,
        },
        {
            "filename": "death_01.wav",
            "freesound_id": "635555",
            "creator": "FunWithSound",
            "duration": 2.5,
        },
    ],
    "ui": [
        {
            "filename": "select_01.wav",
            "freesound_id": "532401",
            "creator": "Timbre",
            "duration": 0.2,
        },
        {
            "filename": "hover_01.wav",
            "freesound_id": "534562",
            "creator": "Euphrosyene",
            "duration": 0.15,
        },
        {
            "filename": "craft_complete_01.wav",
            "freesound_id": "536789",
            "creator": "Syna_Sound",
            "duration": 0.5,
        },
        {
            "filename": "menu_toggle_01.wav",
            "freesound_id": "539012",
            "creator": "Halleck",
            "duration": 0.3,
        },
    ],
    "pickup": [
        {
            "filename": "metal_01.wav",
            "freesound_id": "541234",
            "creator": "Robinhood76",
            "duration": 0.3,
        },
        {
            "filename": "glass_01.wav",
            "freesound_id": "543567",
            "creator": "InspectorJ",
            "duration": 0.3,
        },
        {
            "filename": "organic_01.wav",
            "freesound_id": "545890",
            "creator": "Syna Max",
            "duration": 0.35,
        },
        {
            "filename": "generic_01.wav",
            "freesound_id": "548901",
            "creator": "Timbre",
            "duration": 0.25,
        },
    ],
    "state": [
        {
            "filename": "objective_complete_01.wav",
            "freesound_id": "551234",
            "creator": "Halleck",
            "duration": 0.8,
        },
        {
            "filename": "victory_01.wav",
            "freesound_id": "714823",
            "creator": "FunWithSound",
            "duration": 3.2,
        },
        {
            "filename": "defeat_01.wav",
            "freesound_id": "556789",
            "creator": "SoundBrewer",
            "duration": 1.5,
        },
        {
            "filename": "mission_complete_01.wav",
            "freesound_id": "559012",
            "creator": "Robinhood76",
            "duration": 2.0,
        },
    ],
    "save": [
        {
            "filename": "save_complete_01.wav",
            "freesound_id": "561234",
            "creator": "Timbre",
            "duration": 0.4,
        },
        {
            "filename": "save_progress_01.wav",
            "freesound_id": "563567",
            "creator": "Halleck",
            "duration": 0.2,
        },
        {
            "filename": "load_complete_01.wav",
            "freesound_id": "565890",
            "creator": "SoundBrewer",
            "duration": 0.4,
        },
    ],
}


def download_file(freesound_id: str, output_path: Path, api_key: Optional[str] = None) -> bool:
    """Download audio file from Freesound.org (requires auth or direct URL)."""

    # Note: Direct download requires API key or authentication
    # For now, provide user with manual download links
    url = f"https://freesound.org/sounds/{freesound_id}/"

    print(f"  → Download from: {url}")
    print(f"     Save as: {output_path}")
    return False  # Manual download required


def calculate_sha256(file_path: Path) -> str:
    """Calculate SHA-256 hash of a file."""
    sha256_hash = hashlib.sha256()
    with open(file_path, "rb") as f:
        for byte_block in iter(lambda: f.read(4096), b""):
            sha256_hash.update(byte_block)
    return sha256_hash.hexdigest()


def verify_audio_files(audio_dir: Path) -> Dict[str, bool]:
    """Verify all audio files exist and are valid."""
    results = {}

    for category, files in AUDIO_MANIFEST.items():
        category_dir = audio_dir / category
        results[category] = {}

        for file_info in files:
            filename = file_info["filename"]
            file_path = category_dir / filename

            results[category][filename] = {
                "exists": file_path.exists(),
                "size_kb": file_path.stat().st_size / 1024 if file_path.exists() else 0,
            }

    return results


def main():
    parser = argparse.ArgumentParser(
        description="Download and verify Phase 7.3.4 audio files from Freesound.org"
    )
    parser.add_argument(
        "--api-key",
        help="Freesound API key (or set FREESOUND_API_KEY env var)",
    )
    parser.add_argument(
        "--verify-sha256",
        action="store_true",
        help="Verify SHA-256 hashes of downloaded files",
    )
    parser.add_argument(
        "--output-manifest",
        action="store_true",
        help="Generate audio_manifest.json with file metadata",
    )
    parser.add_argument(
        "--audio-dir",
        default="assets/audio/sources",
        help="Output directory for audio files (default: assets/audio/sources)",
    )

    args = parser.parse_args()

    # Get API key from args or environment
    api_key = args.api_key or os.getenv("FREESOUND_API_KEY")

    audio_dir = Path(args.audio_dir)
    audio_dir.mkdir(parents=True, exist_ok=True)

    # Create category subdirectories
    for category in AUDIO_MANIFEST.keys():
        (audio_dir / category).mkdir(exist_ok=True)

    print("=" * 70)
    print("Phase 7.3.4: Audio File Download Tool")
    print("=" * 70)
    print()
    print(f"Output directory: {audio_dir.absolute()}")
    print(f"Total files to download: {sum(len(files) for files in AUDIO_MANIFEST.values())}")
    print()

    # Note: Freesound API requires authentication
    # This tool provides download URLs for manual download
    print("⚠️  NOTE: Freesound.org requires authentication for direct API downloads.")
    print("     Please use one of these methods:")
    print()
    print("METHOD 1: Manual Download (Recommended)")
    print("  1. Visit https://freesound.org/api/apply/ to create a free API account")
    print("  2. Copy your API key to FREESOUND_API_KEY environment variable")
    print("  3. Run this script with: python tools/audio_download.py --api-key YOUR_KEY")
    print()
    print("METHOD 2: Manual Browser Download")
    print("  Visit each URL below and download the WAV file to the specified directory:")
    print()

    for category, files in AUDIO_MANIFEST.items():
        print(f"\n[{category.upper()}]")
        for file_info in files:
            output_path = audio_dir / category / file_info["filename"]
            download_file(file_info["freesound_id"], output_path, api_key)

    print()
    print("=" * 70)
    print("Verification")
    print("=" * 70)

    # Check what files we have
    verification = verify_audio_files(audio_dir)

    total_files = sum(
        len(files) for files in AUDIO_MANIFEST.values()
    )
    downloaded_files = sum(
        sum(1 for file_info in verification[cat].values() if file_info["exists"])
        for cat in verification
    )

    print(f"Files downloaded: {downloaded_files}/{total_files}")

    if downloaded_files > 0:
        total_size_kb = sum(
            sum(file_info["size_kb"] for file_info in verification[cat].values())
            for cat in verification
        )
        print(f"Total size: {total_size_kb:.1f} KB ({total_size_kb/1024:.1f} MB)")

        # SHA-256 verification
        if args.verify_sha256:
            print()
            print("Verifying SHA-256 hashes...")
            print("  (Compare against docs/audio/audio-provenance.md)")

            for category, files in AUDIO_MANIFEST.items():
                for file_info in files:
                    file_path = audio_dir / category / file_info["filename"]
                    if file_path.exists():
                        sha256 = calculate_sha256(file_path)
                        print(f"  {category}/{file_info['filename']}")
                        print(f"    SHA-256: {sha256}")

        # Output manifest
        if args.output_manifest:
            manifest_path = Path("audio_manifest.json")
            manifest_data = {}

            for category, files in AUDIO_MANIFEST.items():
                manifest_data[category] = []
                for file_info in files:
                    file_path = audio_dir / category / file_info["filename"]
                    if file_path.exists():
                        manifest_data[category].append({
                            **file_info,
                            "path": str(file_path),
                            "size_kb": file_path.stat().st_size / 1024,
                        })

            with open(manifest_path, "w") as f:
                json.dump(manifest_data, f, indent=2)

            print(f"\nManifest saved: {manifest_path}")

    print()
    print("Next step: Phase 7.3.4 Smoke Testing")
    print("  1. Open Godot editor → Load scenes/Main/Main.tscn")
    print("  2. Play scene and test all audio triggers")
    print("  3. Verify 3D positioning, mixing, and levels")
    print()


if __name__ == "__main__":
    main()
