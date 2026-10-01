#!/usr/bin/env bash
# clean-build-artifacts.sh — remove regeneratable build artifacts from this repo.
# Installed by ~/bin/github-clean-all.sh. Safe: NEVER deletes anything git tracks.
set -uo pipefail
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
DRY=false
[[ "${1:-}" == "--dry-run" ]] && DRY=true
LOG="$HOME/Library/Logs/build-cleanup/$(basename "$ROOT").log"
mkdir -p "$(dirname "$LOG")"
count=0; freed_mb=0
while IFS= read -r -d "" d; do
  rel="${d#"$ROOT"/}"
  # SAFETY GUARD: skip if ANYTHING under this dir is tracked by git
  if [[ -n "$(git -C "$ROOT" ls-files -- "$rel/" 2>/dev/null | head -1)" ]]; then
    echo "[skip-git-tracked] $rel" >>"$LOG"
    continue
  fi
  mb=$(du -sm "$d" 2>/dev/null | cut -f1); mb=${mb:-0}
  if [[ "$DRY" == "true" ]]; then
    echo "[would-remove] $rel (${mb} MB)" >>"$LOG"
  else
    rm -rf "$d" && echo "[removed] $rel (${mb} MB)" >>"$LOG"
  fi
  count=$((count+1)); freed_mb=$((freed_mb+mb))
done < <(find "$ROOT" -type d \( -name node_modules -o -name bin -o -name obj \
      -o -name target -o -name dist -o -name __pycache__ -o -name TestResults \
      -o -name coverage -o -name .pytest_cache \) -not -path "*/.git/*" -prune -print0 2>/dev/null)
echo "$ROOT :: $count dirs, ${freed_mb} MB $( [[ "$DRY" == "true" ]] && echo '(dry-run)' )[ok]"
