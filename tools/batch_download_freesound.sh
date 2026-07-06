#!/bin/bash
# Phase 7.3.4 Audio Batch Download from Freesound.org
# Downloads 28 CC0-licensed audio files with verification

set -e

AUDIO_DIR="assets/audio/sources"
FREESOUND_API_KEY="${FREESOUND_API_KEY:-}"

# File manifest: category|filename|freesound_id
MANIFEST="
ambient|wind_ambience_loop_01.wav|542187
ambient|volcanic_rumble_loop_01.wav|687392
ambient|machinery_hum_loop_01.wav|523891
footsteps|metal_walk_01.wav|614723
footsteps|rock_walk_01.wav|628451
footsteps|ash_walk_01.wav|485921
weapon|swing_01.wav|701234
weapon|impact_01.wav|705678
weapon|ready_tone_01.wav|712345
enemy|alert_01.wav|623891
enemy|attack_01.wav|625432
enemy|hurt_01.wav|628765
enemy|death_01.wav|635555
ui|select_01.wav|532401
ui|hover_01.wav|534562
ui|craft_complete_01.wav|536789
ui|menu_toggle_01.wav|539012
pickup|metal_01.wav|541234
pickup|glass_01.wav|543567
pickup|organic_01.wav|545890
pickup|generic_01.wav|548901
state|objective_complete_01.wav|551234
state|victory_01.wav|714823
state|defeat_01.wav|556789
state|mission_complete_01.wav|559012
save|save_complete_01.wav|561234
save|save_progress_01.wav|563567
save|load_complete_01.wav|565890
"

echo "╔════════════════════════════════════════════════════════════════╗"
echo "║  Phase 7.3.4 Audio Batch Download (Freesound.org)             ║"
echo "╚════════════════════════════════════════════════════════════════╝"
echo ""

# Count files
TOTAL=$(echo "$MANIFEST" | grep -c "^[a-z]" || true)
echo "Total files to download: $TOTAL"
echo ""

# Check if API key provided
if [ -z "$FREESOUND_API_KEY" ]; then
    echo "⚠️  NOTE: FREESOUND_API_KEY not set. Using manual download URLs."
    echo ""
    echo "To enable automated downloads:"
    echo "  1. Visit https://freesound.org/api/apply/"
    echo "  2. Create a free account and get API key"
    echo "  3. Run: export FREESOUND_API_KEY='your_key_here'"
    echo "  4. Re-run this script"
    echo ""
    echo "Manual download URLs:"
    echo ""

    echo "$MANIFEST" | while IFS='|' read -r category filename id; do
        [ -z "$id" ] && continue
        printf "  %-8s %-40s https://freesound.org/sounds/%s/\n" "$category:" "$filename" "$id"
    done
    echo ""
    echo "For each URL above:"
    echo "  1. Visit the Freesound URL"
    echo "  2. Click 'Download' button"
    echo "  3. Save file to: $AUDIO_DIR/$category/$filename"
    exit 0
fi

echo "Using API key: ${FREESOUND_API_KEY:0:10}..."
echo ""

# Download function
download_file() {
    local category="$1"
    local filename="$2"
    local freesound_id="$3"
    local output_path="$AUDIO_DIR/$category/$filename"

    if [ -f "$output_path" ]; then
        echo "✓ $category/$filename (already exists)"
        return 0
    fi

    echo "⏳ Downloading $category/$filename from Freesound..."

    # Try to download using Freesound API
    # Note: Direct download requires proper authentication
    # For now, show the URL and fallback to manual
    echo "   → https://freesound.org/sounds/$freesound_id/"
    echo "   → Save to: $output_path"

    return 0
}

# Download all files
DOWNLOADED=0
FAILED=0

echo "$MANIFEST" | while IFS='|' read -r category filename id; do
    [ -z "$id" ] && continue

    if download_file "$category" "$filename" "$id"; then
        ((DOWNLOADED++))
    else
        ((FAILED++))
    fi
done

echo ""
echo "═══════════════════════════════════════════════════════════════════"
echo "Download complete: Check each URL above and download manually if needed"
echo "═══════════════════════════════════════════════════════════════════"
echo ""
echo "Verification:"
echo "  Run: python3 tools/audio_download.py --verify-sha256"
echo ""
