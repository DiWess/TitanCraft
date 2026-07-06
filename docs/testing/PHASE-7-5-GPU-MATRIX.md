# Phase 7.5: GPU & Hardware Compatibility Matrix

**Purpose:** Document target GPU configurations, performance expectations, and test results.

**Last Updated:** 2026-07-06  
**Status:** Ready for Phase 7.5 testing

---

## GPU Test Matrix

### NVIDIA GPUs (Primary Target)

#### High-End Tier

| Model | VRAM | Gen | Target FPS | 1080p | 1440p | 4K | Notes |
|---|---|---|---|---|---|---|---|
| RTX 4090 | 24GB | Ada | 120+ | ✓✓ | ✓✓ | ✓ | Enthusiast |
| RTX 4080 | 16GB | Ada | 90+ | ✓✓ | ✓ | ✓ | Flagship (2022) |
| RTX 4070 Ti | 12GB | Ada | 75+ | ✓✓ | ✓ | ~ | Upper mid-range |

#### Mid-Range Tier

| Model | VRAM | Gen | Target FPS | 1080p | 1440p | 4K | Notes |
|---|---|---|---|---|---|---|---|
| RTX 3060 Ti | 8GB | Ampere | 60+ | ✓✓ | ✓ | ~ | 2020 workhorse |
| RTX 3070 | 8GB | Ampere | 70+ | ✓✓ | ✓ | ~ | 2020 mid-high |
| RTX 3060 | 12GB | Ampere | 55+ | ✓✓ | ✓ | ~ | 2021 budget VRAM |

#### Low-End Tier

| Model | VRAM | Gen | Target FPS | 1080p | 1440p | 4K | Notes |
|---|---|---|---|---|---|---|---|
| GTX 1660 Ti | 6GB | Turing | 45+ | ✓ | ~ | ✗ | Legacy 2019 |
| GTX 1050 Ti | 4GB | Pascal | 30+ | ✓ | ✗ | ✗ | Budget legacy |

---

### AMD GPUs (Secondary Target)

#### High-End Tier

| Model | VRAM | Gen | Target FPS | 1080p | 1440p | 4K | Notes |
|---|---|---|---|---|---|---|---|
| RX 7900 XTX | 24GB | RDNA3 | 90+ | ✓✓ | ✓✓ | ✓ | Current flagship |
| RX 7900 XT | 20GB | RDNA3 | 80+ | ✓✓ | ✓ | ✓ | 2022 high-end |
| RX 6900 XT | 16GB | RDNA2 | 70+ | ✓✓ | ✓ | ✓ | 2020 flagship |

#### Mid-Range Tier

| Model | VRAM | Gen | Target FPS | 1080p | 1440p | 4K | Notes |
|---|---|---|---|---|---|---|---|
| RX 6700 XT | 12GB | RDNA2 | 60+ | ✓✓ | ✓ | ~ | 2021 workhorse |
| RX 6600 | 8GB | RDNA2 | 50+ | ✓✓ | ✓ | ~ | 2021 budget |

#### Low-End Tier

| Model | VRAM | Gen | Target FPS | 1080p | 1440p | 4K | Notes |
|---|---|---|---|---|---|---|---|
| RX 5700 | 8GB | RDNA1 | 40+ | ✓ | ~ | ✗ | 2019 legacy |
| RX 570 | 4GB | Polaris | 30+ | ✓ | ✗ | ✗ | Budget legacy |

---

### Intel GPUs (Tertiary Target)

#### Dedicated Arc

| Model | VRAM | Gen | Target FPS | 1080p | 1440p | 4K | Notes |
|---|---|---|---|---|---|---|---|
| Arc A770 | 8GB | Alchemist | 55+ | ✓✓ | ✓ | ~ | Current flagship dGPU |
| Arc A750 | 8GB | Alchemist | 45+ | ✓ | ~ | ✗ | 2022 budget dGPU |
| Arc A380 | 6GB | Alchemist | 30+ | ✓ | ✗ | ✗ | Entry-level dGPU |

#### Integrated Graphics

| Model | VRAM | Gen | Target FPS | 1080p | 1440p | 4K | Notes |
|---|---|---|---|---|---|---|---|
| Iris Xe (DG1) | Shared | Tiger Lake | 25+ | ✓ | ~ | ✗ | Discrete variant |
| UHD 630 | Shared | 9th Gen | 20+ | ✓ | ✗ | ✗ | Desktop iGPU baseline |
| Iris Xe (11th Gen) | Shared | 11th Gen | 30+ | ✓ | ✗ | ✗ | Laptop iGPU baseline |

---

## Performance Expectation Table

### Baseline: RTX 3060 Ti @ 1920×1080

| Scene/State | FPS Target | Acceptable Range | Comments |
|---|---|---|---|
| **Idle (standing still)** | 60 | 55–60 | No enemies visible, exploring |
| **Exploration** | 60 | 50–60 | Walking/camera movement |
| **Resource collection** | 60 | 50–60 | UI interaction, minor physics |
| **Combat (scout visible)** | 55 | 45–55 | Scout in detection range |
| **Combat (scout attacking)** | 50 | 40–50 | Active combat, close range |
| **Workbench (crafting UI)** | 60 | 55–60 | UI-heavy, minimal 3D |
| **Beacon activation** | 60 | 55–60 | Final cinematic moment |
| **Game Over/Victory screen** | 60 | 55–60 | Static UI |

---

## Resolution Scaling Performance

### Expected FPS Drop by Resolution (vs. 1080p Baseline)

| Resolution | Pixel Count | vs. 1080p | RTX 3060 Ti | RTX 4080 | RX 6700 | UHD 630 |
|---|---|---|---|---|---|---|
| 1920×1080 | 2.1M | 0% | 60 | 120+ | 60 | 30 |
| 2560×1440 | 3.7M | +76% | 50 | 90 | 50 | 22 |
| 3840×2160 | 8.3M | +295% | 35 | 60 | 35 | 12 |
| 1280×720 | 0.9M | -62% | 60+ | 120+ | 60+ | 40 |

**Testing Notes:**
- Godot's native scaling may vary by viewport implementation
- Lower resolutions should maintain 60 FPS on all tested GPUs
- 4K is aspirational; minimum playable target is 1440p on mid-range

---

## Driver Version Compatibility

### Recommended Driver Versions (Test Against These)

| Vendor | Family | Minimum | Recommended | Notes |
|---|---|---|---|---|
| NVIDIA | GeForce (RTX/GTX) | 517.xx | Latest WHQL | Game Ready driver |
| AMD | Radeon (RX) | 22.12.1 | Latest WHQL | Adrenalin driver |
| Intel | Arc Alchemist | 27.0.1 | Latest | Intel Arc Graphics driver |
| Intel | iGPU (UHD/Iris) | Latest | Latest | Windows Update or Intel DSO |

**Outdated Driver Rule:**
- If test fails on outdated driver, request user upgrade and re-test
- Document as "resolved by driver update" if subsequent test passes

---

## Frame Rate Consistency Metrics

### FPS Stability Scoring

| Target | FPS Range | Stability Rating | User Experience |
|---|---|---|---|
| 60 FPS | 55–60 | Excellent | Smooth, no visible stuttering |
| 60 FPS | 45–55 | Good | Minor occasional dips, playable |
| 60 FPS | 30–45 | Fair | Noticeable dips, acceptable |
| 60 FPS | <30 | Poor | Unplayable stuttering |

**Measurement Method:**
- Capture 30-second gameplay footage at target FPS counter
- Calculate: Average FPS, Min FPS, Max FPS, Frame time variance
- Flag any sustained dips below minimum threshold

---

## CPU & RAM Requirements

### Tested CPU/RAM Combinations

| CPU | RAM | GPU Partner | Expected FPS | Test Status |
|---|---|---|---|---|
| Intel i7-10700K | 16GB DDR4 | RTX 3060 Ti | 60 | Baseline ✓ |
| Intel i7-12700K | 32GB DDR5 | RTX 4080 | 120+ | High-end ✓ |
| AMD Ryzen 5 5600X | 16GB DDR4 | RX 6700 XT | 60 | TBD |
| Intel i3-10100 | 8GB DDR4 | GTX 1660 Ti | 45 | Budget ✓ |
| Intel Celeron | 4GB DDR3 | UHD 630 | 20 | Minimum viability |

**CPU Bottleneck Check:**
- If GPU shows >85% utilization but CPU shows <50%, GPU-bound (good)
- If CPU shows >85% but GPU <70%, CPU-bound (potential issue)

---

## Known Driver Issues

### NVIDIA

| Issue | Driver Versions | Workaround | Status |
|---|---|---|---|
| High VRAM consumption @ 4K | 520.x–523.x | Update to 530+ | Resolved in newer |
| Stuttering with high polling rate | 510.x–517.x | Reduce mouse polling to 500Hz | Unresolved |

### AMD

| Issue | Driver Versions | Workaround | Status |
|---|---|---|---|
| RDNA2 shader compilation stutter | 22.x early | Pre-compile shaders via startup | Mitigated 23.x+ |
| Frame pacing issues at high refresh | 21.x–22.x | Enable "FRTC" in driver | Partial fix in 23.x |

### Intel

| Issue | Driver Versions | Workaround | Status |
|---|---|---|---|
| Arc Alchemist cold startup lag | 27.0–27.1 | Allow 5–10s warmup | Resolved 27.2+ |
| iGPU thermal throttling | All | Ensure adequate cooling | Environmental |

---

## Test Result Template

```markdown
## Test: [GPU Model] @ [Resolution]

**Date:** 2026-07-xx  
**Tester:** [Name]  
**OS:** Windows 11 22H2  
**Driver:** [Version]  

### Measurements

| Scene | Avg FPS | Min FPS | Max FPS | Frame Time Variance | Status |
|---|---|---|---|---|---|
| Idle | 60 | 58 | 60 | 0.3ms | ✓ PASS |
| Exploration | 59 | 45 | 60 | 1.2ms | ✓ PASS |
| Combat | 52 | 38 | 60 | 2.1ms | ✓ PASS |
| Victory Screen | 60 | 60 | 60 | 0.1ms | ✓ PASS |

### Issues Encountered
- None observed

### Workarounds Applied
- None required

### Final Verdict
✓ **PASS** — Build meets performance targets on this configuration
```

---

## GPU Tier Retirement Criteria

Configurations **may be dropped** from future test cycles if:
- GPU is >6 years old (e.g., GTX 1050 Ti from 2017)
- VRAM <4GB and exhibiting poor performance
- Manufacturer has ended driver support
- <5% of anticipated player base uses config (optional data-driven criteria)

**Current Minimum Spec (Hard Stop):**
- GPU: Any GPU with ≥2GB VRAM manufactured after 2015
- OS: Windows 10 21H2 or Windows 11 22H2+
- CPU: Any quad-core from 8th Gen Intel / Ryzen 1000 series or newer

---

## Archive & Historical Comparisons

### Phase 7.4 → 7.5 Performance Deltas

To be populated after Phase 7.4 scene integration establishes baseline:

| Scene/State | Phase 7.4 FPS | Phase 7.5 FPS | Delta | Impact |
|---|---|---|---|---|
| Idle | (Baseline) | TBD | TBD | TBD |
| Combat | (Baseline) | TBD | TBD | TBD |

---

## References

- [Godot 4 Export Documentation](https://docs.godotengine.org/en/stable/tutorials/export/index.html)
- [Windows GPU Driver Stability](https://nvidia.com/drivers) (NVIDIA) / [AMD Radeon Drivers](https://amd.com/drivers)
- [TitanCraft Performance Baseline](../performance-baseline.md)

