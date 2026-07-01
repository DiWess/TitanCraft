# Phase 3A production integration review artifacts

Raster captures were intentionally removed from this pull request per review direction.

`contact_sheet.svg` is a vector-only review manifest that lists the eight real `res://scenes/Main/Main.tscn` capture views without embedding raster screenshots.

To regenerate local raster captures outside the PR, run:

```bash
xvfb-run -a godot --path . --script tools/visual_review/capture_phase3a_production_integration.gd
```

Generated captures are temporary human-review artifacts, are not production textures, must not be wired into gameplay, and may be removed after human approval, rejection, or replacement by a later review capture.

Current review verdict: `PRODUCTION_VISUAL_SLICE_NOT_GO` because the Linux visual capture and Windows export succeeded previously, but Windows playtest/FPS measurement and full human visual acceptance scoring were not completed in this container session.
