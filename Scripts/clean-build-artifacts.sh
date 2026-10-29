#!/usr/bin/env bash
# clean-build-artifacts.sh — remove regeneratable build artifacts from this repo.
# Installed by ~/bin/github-clean-all.sh.
#
# Deletes directories by NAME: node_modules, bin, obj, target, dist, __pycache__, TestResults,
# coverage, .pytest_cache. Those are also ordinary names for folders projects keep in git, so the
# name alone proves nothing. A candidate is deleted only when ALL of these hold, otherwise it is
# skipped, printed to stderr and counted:
#   - the root (printed first) is not /, $HOME, above $HOME or shallower than /a/b/c, and is the
#     top level of a git work tree — otherwise the run refuses to start;
#   - the candidate is under that root (find never follows symlinks out of it);
#   - the candidate lies inside a git work tree (a folder git does not know about is refused —
#     there is no way to tell it is regeneratable);
#   - `git ls-files` in the repo that OWNS the candidate (a submodule or nested repo, not just this
#     one) lists nothing at or under it — matched literally and case-insensitively, because the disk
#     is case-insensitive and an index entry for Dist/ is the same folder as dist/;
#   - no .git sits anywhere inside it (a nested repo's tracked files are invisible to its parent),
#     and find could read all of it — an unreadable subfolder could be hiding one;
#   - git itself answered, with the caller's GIT_* environment cleared first. Any git error is a
#     refusal, never "nothing tracked".
#
# The previous header said "Safe: NEVER deletes anything git tracks." That was false. Its check ran
# `git ls-files` with errors sent to /dev/null and read an empty answer as "untracked", so when git
# failed it deleted. On 2026-10-29 (11:41–12:07) a run whose root resolved to / deleted 7,810
# directories across the disk — ~/.nuget packages, app bundles, system-volume folders — and
# 17 tracked files in three repos (clean-pay, angelsix-consulting, arbiter). That run's log is
# ~/Library/Logs/build-cleanup/.log.
#
# Still NOT guaranteed: an UNTRACKED folder with one of these names is deleted even if it holds
# hand-made work that was never committed. Untracked means unprotected. Run with --dry-run first.
#@guard warn: recursively deletes every untracked node_modules, bin, obj, target, dist, __pycache__, TestResults, coverage and .pytest_cache folder in this repo; tracked ones are skipped and reported
#@guard duration: under a minute
#@guard approve recursively deletes untracked build output folders named node_modules, bin, obj, target, dist, __pycache__, TestResults, coverage and .pytest_cache across this repo (anything git tracks, or outside a git repo, is skipped and reported)
set -uo pipefail
# Gate compliance: source the shared runner library so this script goes through
# the AI Mecca execution gate (same convention as Scripts/gate_scoped.sh).
# Without it the scripts-gate treats this rm -rf helper as ungated and blocks publishes.
# The library's own stderr is kept: a denied run prints why, and the old 2>/dev/null hid it.
# The path is resolved before the source so a missing file is not an error, and the source line
# still names common.sh literally, which is what `guard scripts gated` looks for.
_guard_lib_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# Only this script's own folder: a fallback to $PWD/Scripts ran whatever common.sh sat in the caller's
# working directory as the gate (peer review F12, 2026-10-29).
# FAIL CLOSED: without the library the body below runs UNGATED. A missing gate is not permission —
# measured before this check existed: with common.sh absent, bash printed one error and then ran the
# script's body anyway, exit 0 (peer review, 2026-10-29).
if [ ! -f "$_guard_lib_dir/common.sh" ]; then
    printf '  ❌ cannot find the execution gate (common.sh) — refusing to run ungated\n' >&2
    exit 1
fi
source "$_guard_lib_dir/common.sh"

DRY=false
case "${1:-}" in
  "") ;;
  --dry-run) DRY=true ;;
  # A typo such as --dryrun used to be ignored, and the run deleted for real.
  *) printf '  ❌ unknown argument: %s (only --dry-run is accepted)\n' "$1" >&2; exit 2 ;;
esac

# The caller's GIT_* environment must not steer the guard. Measured on the fixture: with
# GIT_LITERAL_PATHSPECS=1 exported, the ":(literal,icase)" spec below is read as a literal file name,
# matches nothing, and every tracked candidate was deleted. GIT_DIR / GIT_WORK_TREE would likewise
# make every `git -C` answer for some other repo.
while IFS= read -r _v; do unset "$_v"; done < <(compgen -e | grep '^GIT_')

# Physical path (-P): git reports physical paths, and the owner prefix below must match them.
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd -P)"
# Said first, before any check or deletion, so a wrong root is visible at once.
printf '  ▶ clean-build-artifacts: root = %s%s\n' "$ROOT" "$( [[ "$DRY" == "true" ]] && echo ' (dry-run)' )"
# A root that cannot be one project is refused outright, git repo or not (a dotfiles repo makes
# $HOME a work tree): /, $HOME or anything above it, or anything shallower than /a/b/c.
# A copy of this script run from /tmp once resolved ROOT to / and swept the whole disk.
_home="$(cd "$HOME" 2>/dev/null && pwd -P)"
_depth="$(printf '%s' "$ROOT" | tr -cd '/' | wc -c | tr -d ' ')"
if [[ "$ROOT" == "/" || -z "$_home" || "$_home/" == "$ROOT/"* || "$_depth" -lt 3 ]]; then
  printf '  ❌ refusing root %s — it is /, $HOME, above $HOME, or shallower than /a/b/c; not a project\n' "$ROOT" >&2
  exit 1
fi
# The root must BE a git work tree's top level.
root_top="$(git -C "$ROOT" rev-parse --show-toplevel 2>/dev/null)"
if [[ -z "$root_top" || "$(cd "$root_top" && pwd -P)" != "$ROOT" ]]; then
  printf '  ❌ %s is not the top of a git work tree — refusing to delete anything\n' "$ROOT" >&2
  exit 1
fi

LOG="$HOME/Library/Logs/build-cleanup/$(basename "$ROOT").log"
mkdir -p "$(dirname "$LOG")"

# skip <reason> <path>: loud on stderr, recorded in the log, counted.
skipped=0
skip() {
  printf '  ⚠️  SKIPPED [%s] %s\n' "$1" "$2" >&2
  echo "[skip-$1] $2" >>"$LOG"
  skipped=$((skipped+1))
}

count=0; freed_mb=0; failed=0
while IFS= read -r -d "" d; do
  rel="${d#"$ROOT"/}"
  # Never act on a path that is not under the root this run announced.
  if [[ "$rel" == "$d" || "$d" != "$ROOT/"* ]]; then
    skip outside-root "$d"; continue
  fi
  # The repo that owns this path: a submodule or nested repo, not necessarily this one.
  owner="$(git -C "$d" rev-parse --show-toplevel 2>/dev/null)"
  # No answer — outside any repo, or a broken .git — is a refusal: nothing vouches it is build output.
  if [[ -z "$owner" ]]; then
    skip not-in-git "$rel"; continue
  fi
  owner="$(cd "$owner" && pwd -P)"
  if [[ "$owner" == "$d" ]]; then
    # The candidate is itself a repo's root (or a submodule): never build output.
    skip is-a-repo "$rel"; continue
  fi
  orel="${d#"$owner"/}"
  if [[ "$orel" == "$d" ]]; then
    skip owner-path-mismatch "$rel"; continue
  fi
  # SAFETY GUARD: skip if ANYTHING at or under this dir is tracked. literal: a name holding * or [
  # is not a glob; icase: the index may spell the folder Dist while the disk says dist. git's exit
  # status is read through pipefail — a failure is a refusal, not an empty list.
  tracked_bytes="$(git -C "$owner" ls-files -z -- ":(literal,icase)$orel" | wc -c)"
  git_rc=$?
  if [[ $git_rc -ne 0 ]]; then
    skip git-error "$rel"; continue
  fi
  if [[ "${tracked_bytes// /}" != "0" ]]; then
    skip git-tracked "$rel"; continue
  fi
  # A nested repo inside the candidate: its tracked files are invisible to the owner's index.
  nested="$(find "$d" -name .git -print -quit 2>/dev/null)"
  find_rc=$?
  if [[ -n "$nested" ]]; then
    skip holds-nested-repo "$rel"; continue
  fi
  # find could not read part of it, so a nested .git may be hiding there: unverifiable, refused.
  if [[ $find_rc -ne 0 ]]; then
    skip unreadable "$rel"; continue
  fi
  mb=$(du -sm "$d" 2>/dev/null | cut -f1); mb=${mb:-0}
  if [[ "$DRY" == "true" ]]; then
    echo "[would-remove] $rel (${mb} MB)" >>"$LOG"
  elif rm -rf "$d"; then
    echo "[removed] $rel (${mb} MB)" >>"$LOG"
  else
    echo "[remove-failed] $rel" >>"$LOG"; failed=$((failed+1)); continue
  fi
  count=$((count+1)); freed_mb=$((freed_mb+mb))
done < <(find "$ROOT" -name .git -prune -o -type d \( -name node_modules -o -name bin -o -name obj \
      -o -name target -o -name dist -o -name __pycache__ -o -name TestResults \
      -o -name coverage -o -name .pytest_cache \) -prune -print0 2>/dev/null)
echo "$ROOT :: $count dirs, ${freed_mb} MB $( [[ "$DRY" == "true" ]] && echo '(dry-run) ' )[ok]"
if [[ $skipped -gt 0 ]]; then
  printf '  ⚠️  %d candidate dir(s) SKIPPED — tracked by git, outside git, or unverifiable. See above and %s\n' "$skipped" "$LOG" >&2
fi
if [[ $failed -gt 0 ]]; then
  printf '  ❌ %d dir(s) could not be removed — see %s\n' "$failed" "$LOG" >&2
  exit 1
fi
