# Assemble the full Comorian Main.tscn

The full 71 kB scene was uploaded in two chunks because of tool size limits.

Run this from the repo root on the branch `fix/restore-main-tscn-comorian`:

```bash
cat scenes/Main/_restore_part1.txt scenes/Main/_restore_part2.txt > scenes/Main/Main.tscn
rm scenes/Main/_restore_part1.txt scenes/Main/_restore_part2.txt scenes/Main/RESTORE_FROM_CHUNKS.md
git add scenes/Main/Main.tscn
git commit -m "fix: restore complete Main.tscn hierarchy + Comorian visual ceiling

Full hierarchy restored from last good commit. Only the approved Comorian atmosphere values applied (warmer mineral horizon, heat of the base lamps, indigo ambient/fog, SSAO/glow refinements). Player and all gameplay nodes are present."
git push
```

Then merge the branch to main (or open a PR).

After it is on main, the windows-playtest-journey can be re-triggered for the aesthetic captures under the Comorian ceiling.
