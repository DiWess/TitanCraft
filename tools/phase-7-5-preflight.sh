#!/bin/bash
###############################################################################
# Phase 7.5: Pre-Flight Validation Script
#
# Purpose: Automated checks before Phase 7.5 hardware testing begins.
#          Validates build artifact, dependencies, offline capability, and
#          asset integrity.
#
# Usage: bash tools/phase-7-5-preflight.sh
#
# Platform: Windows (requires WSL, Git Bash, or MSYS2 for bash)
#          OR run on Linux/Mac with appropriate path adjustments
#
# Author: TitanCraft QA
# Date Created: 2026-07-06
###############################################################################

set -e  # Exit on error

# Colors for output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Counters
PASS_COUNT=0
FAIL_COUNT=0
WARN_COUNT=0

###############################################################################
# Utility Functions
###############################################################################

log_pass() {
    echo -e "${GREEN}✓ PASS:${NC} $1"
    ((PASS_COUNT++))
}

log_fail() {
    echo -e "${RED}✗ FAIL:${NC} $1"
    ((FAIL_COUNT++))
}

log_warn() {
    echo -e "${YELLOW}⚠ WARN:${NC} $1"
    ((WARN_COUNT++))
}

log_info() {
    echo -e "${BLUE}ℹ INFO:${NC} $1"
}

log_section() {
    echo ""
    echo -e "${BLUE}=== $1 ===${NC}"
}

###############################################################################
# Pre-Flight Checks
###############################################################################

log_section "Phase 7.5 Pre-Flight Validation"
echo "Test Date: $(date)"
echo "Working Directory: $(pwd)"
echo ""

# Check 1: Build artifact exists
log_section "1. Build Artifact Verification"

if [ -f "builds/Windows/TitanCraft.exe" ]; then
    SIZE=$(du -h "builds/Windows/TitanCraft.exe" | cut -f1)
    TIMESTAMP=$(stat -c "%y" "builds/Windows/TitanCraft.exe" 2>/dev/null || stat -f "%Sm" "builds/Windows/TitanCraft.exe" 2>/dev/null || echo "unknown")
    log_pass "Executable found: builds/Windows/TitanCraft.exe (Size: $SIZE)"
else
    log_fail "Executable not found: builds/Windows/TitanCraft.exe"
    log_info "Build Phase 7.4 output before running Phase 7.5 tests"
    exit 1
fi

# Check 2: Build size sanity check
log_section "2. Build Size Sanity Check"

EXECUTABLE_SIZE=$(stat -c "%s" "builds/Windows/TitanCraft.exe" 2>/dev/null || stat -f "%z" "builds/Windows/TitanCraft.exe" 2>/dev/null)
MIN_SIZE=$((10 * 1024 * 1024))  # 10 MB minimum

if [ "$EXECUTABLE_SIZE" -gt "$MIN_SIZE" ]; then
    SIZE_MB=$((EXECUTABLE_SIZE / 1024 / 1024))
    log_pass "Build size acceptable: ${SIZE_MB}MB (minimum: 10MB)"
else
    SIZE_MB=$((EXECUTABLE_SIZE / 1024 / 1024))
    log_fail "Build too small: ${SIZE_MB}MB (expected >10MB, possibly missing assets)"
fi

# Check 3: Required asset directories exist
log_section "3. Asset Directory Verification"

REQUIRED_DIRS=(
    "assets"
    "assets/audio"
    "assets/Production"
)

for dir in "${REQUIRED_DIRS[@]}"; do
    if [ -d "$dir" ]; then
        log_pass "Directory found: $dir"
    else
        log_fail "Directory missing: $dir"
    fi
done

# Check 4: Check for suspicious network strings (if strings command available)
log_section "4. Network URL Detection"

if command -v strings &> /dev/null; then
    SUSPICIOUS_URLS=$(strings "builds/Windows/TitanCraft.exe" 2>/dev/null | grep -iE "(http|https|telemetry|api\..*\.|cloudflare|google|amazonaws)" | grep -v "docs.godotengine" | head -5 || true)

    if [ -z "$SUSPICIOUS_URLS" ]; then
        log_pass "No suspicious URLs detected in binary"
    else
        log_warn "Potential URLs found (verify these are safe):"
        echo "$SUSPICIOUS_URLS" | sed 's/^/  /'
    fi
else
    log_info "strings command not available, skipping URL scan"
    log_info "  (Install binutils on Linux/Mac for this check)"
fi

# Check 5: Verify C# build succeeded
log_section "5. C# Build Verification"

if [ -d "obj/Debug" ] || [ -d "obj/Release" ]; then
    log_pass "C# build artifacts found (build completed)"
else
    log_warn "C# build artifacts not found (build may need refresh)"
fi

# Check 6: Scene file integrity
log_section "6. Scene File Integrity"

if [ -f "scenes/Main/Main.tscn" ]; then
    SCENE_SIZE=$(stat -c "%s" "scenes/Main/Main.tscn" 2>/dev/null || stat -f "%z" "scenes/Main/Main.tscn" 2>/dev/null)
    if [ "$SCENE_SIZE" -gt 1000 ]; then
        log_pass "Main scene found and non-empty (${SCENE_SIZE} bytes)"
    else
        log_fail "Main scene appears corrupted or too small"
    fi
else
    log_fail "Main scene not found: scenes/Main/Main.tscn"
fi

# Check 7: Code structure verification
log_section "7. Code Structure Verification"

REQUIRED_SOURCE_DIRS=(
    "src/Core"
    "src/Player"
    "src/Enemies"
    "src/World"
)

for srcdir in "${REQUIRED_SOURCE_DIRS[@]}"; do
    if [ -d "$srcdir" ]; then
        FILE_COUNT=$(find "$srcdir" -name "*.cs" 2>/dev/null | wc -l)
        log_pass "Source found: $srcdir ($FILE_COUNT C# files)"
    else
        log_warn "Source directory missing: $srcdir (may not be required for Phase 7.5)"
    fi
done

# Check 8: Audio files availability
log_section "8. Audio Asset Verification"

if [ -d "assets/audio/sources" ]; then
    AUDIO_FILE_COUNT=$(find "assets/audio/sources" -type f 2>/dev/null | wc -l)
    if [ "$AUDIO_FILE_COUNT" -gt 0 ]; then
        log_pass "Audio files present: $AUDIO_FILE_COUNT files found"
    else
        log_fail "Audio directory exists but contains no files"
    fi
else
    log_warn "Audio sources directory not found: assets/audio/sources"
fi

# Check 9: Git status (if in git repo)
log_section "9. Repository Status"

if [ -d ".git" ]; then
    GIT_STATUS=$(git status --short 2>/dev/null || echo "unknown")
    if [ -z "$GIT_STATUS" ]; then
        log_pass "Git repository clean (no uncommitted changes)"
    else
        log_warn "Uncommitted changes detected (expected if Phase 7.4 in progress):"
        echo "$GIT_STATUS" | head -5 | sed 's/^/  /'
    fi

    CURRENT_BRANCH=$(git rev-parse --abbrev-ref HEAD 2>/dev/null || echo "unknown")
    log_info "Current branch: $CURRENT_BRANCH"
else
    log_info "Not in git repository (OK if testing standalone build)"
fi

# Check 10: Documentation completeness
log_section "10. Testing Documentation Status"

REQUIRED_DOCS=(
    "studio/orchestration/PHASE-7-5-TESTING-PLAN.md"
    "docs/testing/PHASE-7-5-GPU-MATRIX.md"
    "docs/testing/PHASE-7-5-PROCEDURES.md"
)

for doc in "${REQUIRED_DOCS[@]}"; do
    if [ -f "$doc" ]; then
        log_pass "Documentation found: $doc"
    else
        log_fail "Testing documentation missing: $doc"
    fi
done

# Check 11: Performance baseline (if available)
log_section "11. Performance Baseline Data"

if [ -f "docs/performance-baseline.md" ]; then
    log_pass "Performance baseline documentation found"
else
    log_warn "Performance baseline documentation not found (will be established during Phase 7.5)"
fi

# Check 12: README compliance
log_section "12. MVP Scope Compliance"

if grep -q "Windows-first" README.md 2>/dev/null; then
    log_pass "README confirms Windows-first platform requirement"
else
    log_warn "Could not verify Windows-first requirement in README"
fi

if grep -q "offline-first" README.md 2>/dev/null; then
    log_pass "README confirms offline-first requirement"
else
    log_warn "Could not verify offline-first requirement in README"
fi

###############################################################################
# Summary
###############################################################################

log_section "Pre-Flight Summary"

echo ""
echo "Results:"
echo -e "  ${GREEN}Passed: $PASS_COUNT${NC}"
echo -e "  ${RED}Failed: $FAIL_COUNT${NC}"
echo -e "  ${YELLOW}Warnings: $WARN_COUNT${NC}"
echo ""

if [ "$FAIL_COUNT" -eq 0 ]; then
    echo -e "${GREEN}✓ Pre-Flight Status: PASS${NC}"
    echo ""
    echo "The build is ready for Phase 7.5 hardware testing."
    echo "Proceed with:"
    echo "  1. Deploy builds/Windows/TitanCraft.exe to test machines"
    echo "  2. Run Phase 7.5 testing procedures (see: docs/testing/PHASE-7-5-PROCEDURES.md)"
    echo "  3. Document results in known-issues-phase-7-5.md"
    echo ""
    exit 0
else
    echo -e "${RED}✗ Pre-Flight Status: FAIL${NC}"
    echo ""
    echo "Resolve the above failures before proceeding to Phase 7.5."
    echo "Common resolutions:"
    echo "  - Run 'dotnet build' to rebuild C# project"
    echo "  - Run Godot export for Windows to regenerate build artifact"
    echo "  - Verify all assets are committed and present"
    echo ""
    exit 1
fi

###############################################################################
# End of Script
###############################################################################
