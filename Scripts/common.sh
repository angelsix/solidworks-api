#!/bin/bash
#
# Shared library for the scripts in this folder. Sourced, not run directly.
#
# This is the shared library the execution gate recognises (common.sh or _common.sh): a script that
# sources it runs INSIDE the gate, so an AI Mecca chat can warn before it runs, approve or deny it,
# and show its progress. A script that does not source it runs outside the gate entirely.
#
# The gate helpers are the ones in AI Mecca's own Scripts/common.sh, without that repo's build
# helpers. Run outside a chat (no AIMECCA_SESSION_ID: a terminal, cron, the nightly sweep) every
# gate call returns at once, so sourcing this never prompts, blocks or talks to the network.
#
set -u
set -o pipefail

# ${BASH_SOURCE[0]} is this file either way; $0 is this file only when it was EXECUTED rather than
# sourced. Say plainly that there is nothing to run here instead of appearing to do something.
if [ "${BASH_SOURCE[0]}" = "$0" ]; then
    printf 'This file is shared code the other scripts load. There is nothing to run here.\n'
    exit 0
fi

# The AI Mecca backend. Its port is per machine, so read it from machine.json — a hardcoded port
# points the gate at whatever else happens to listen there, which answers 404 and denies every run.
BACKEND_HOST="127.0.0.1"
BACKEND_PORT="5077"
MACHINE_STATE_FILE="$HOME/.aimecca/machine.json"
if [ -f "$MACHINE_STATE_FILE" ] && command -v python3 >/dev/null 2>&1; then
    READ_BACKEND_HOST="$(python3 -c "import json; data=json.load(open('$MACHINE_STATE_FILE')); print(data.get('serverHost') or data.get('ServerHost') or '')" 2>/dev/null || true)"
    READ_BACKEND_PORT="$(python3 -c "import json; data=json.load(open('$MACHINE_STATE_FILE')); print(data.get('serverPort') or data.get('ServerPort') or '')" 2>/dev/null || true)"
    [ -n "$READ_BACKEND_HOST" ] && BACKEND_HOST="$READ_BACKEND_HOST"
    [ -n "$READ_BACKEND_PORT" ] && BACKEND_PORT="$READ_BACKEND_PORT"
fi
if [ "$BACKEND_HOST" = "0.0.0.0" ] || [ "$BACKEND_HOST" = "::" ]; then
    BACKEND_HOST="127.0.0.1"
fi
BACKEND_URL="http://$BACKEND_HOST:$BACKEND_PORT"

# ---- The gate --------------------------------------------------------------

# Tell the chat this script has started, so it can warn up front and open a progress bar. Reporting
# only: every failure (no session, no backend, a slow reply) is silent and the script carries on.
# Reads the script's own #@guard duration/warn headers, so the timing is declared in one place.
aimecca_announce() {
    local script="${1:-$0}"
    [ -n "${AIMECCA_SESSION_ID:-}" ] || return 0
    command -v curl >/dev/null 2>&1 || return 0

    # A per-run progress id, so each run gets its own bar.
    AIMECCA_PROGRESS_ID="build-$(date +%s)-$$"
    export AIMECCA_PROGRESS_ID

    local duration warn
    duration="$(grep -m1 '^#@guard duration ' "$script" 2>/dev/null | awk '{print $3}')"
    warn="$(grep -m1 '^#@guard warn ' "$script" 2>/dev/null | cut -d' ' -f3-)"
    [ -n "$duration" ] || [ -n "$warn" ] || return 0

    # python3 does the JSON escaping — a warn line contains quotes often enough to break hand-built JSON.
    local payload
    payload="$(SCRIPT="$script" SESSION="$AIMECCA_SESSION_ID" DUR="$duration" WARN="$warn" PROGRESS_ID="$AIMECCA_PROGRESS_ID" python3 -c '
import json, os
d = os.environ.get("DUR") or ""
print(json.dumps({
    "sessionId": os.environ["SESSION"],
    "scriptPath": os.environ["SCRIPT"],
    "durationSeconds": int(d) if d.isdigit() else None,
    "warnText": os.environ.get("WARN") or None,
    "progressId": os.environ.get("PROGRESS_ID", "build"),
}))' 2>/dev/null)" || return 0
    [ -n "$payload" ] || return 0

    curl -sS -m 2 -X POST "$BACKEND_URL/api/scripts/announce" \
        -H 'Content-Type: application/json' -d "$payload" >/dev/null 2>&1 || true
    return 0
}

# Ask the chat for permission to run a script that declares `#@guard approve <reason>`. Never asks
# when no chat session is attached, when the script declares no reason, or when the backend already
# granted this run (AIMECCA_SCRIPT_APPROVAL_GRANTED). Otherwise it blocks until the card is
# answered: 200 is approved, anything else is a denial and the script must stop.
aimecca_approve() {
    local script="${1:-${BASH_SOURCE[1]:-$0}}"

    [ -n "${AIMECCA_SESSION_ID:-}" ] || return 0
    [ -z "${AIMECCA_SCRIPT_APPROVAL_GRANTED:-}" ] || return 0
    command -v curl >/dev/null 2>&1 || return 0
    [ -f "$script" ] || return 0

    local reason
    reason="$(grep -m1 '^#@guard approve ' "$script" 2>/dev/null | sed -E 's/^#@guard approve[ \t]+//')"
    reason="${reason#\"}"
    reason="${reason%\"}"
    [ -n "$reason" ] || return 0

    local decision_id="decide-$(date +%s)-$$"
    local payload
    payload="$(SCRIPT="$script" REASON="$reason" SESSION="$AIMECCA_SESSION_ID" DECISION="$decision_id" python3 -c '
import json, os
print(json.dumps({
    "sessionId": os.environ["SESSION"],
    "scriptPath": os.environ["SCRIPT"],
    "reason": os.environ["REASON"],
    "decisionId": os.environ["DECISION"],
}))' 2>/dev/null)" || return 1
    [ -n "$payload" ] || return 1

    local code
    code="$(curl -s -o /dev/null -m 300 -w '%{http_code}' \
        -X POST "$BACKEND_URL/api/scripts/approval" \
        -H 'Content-Type: application/json' -d "$payload")" || code="000"

    if [ "$code" = "200" ]; then
        return 0
    fi

    # A denied or unreachable approval is the whole reason the gate exists: do not carry on.
    printf '  ❌ Script approval DENIED: %s\n' "$reason" >&2
    return 1
}

# At SOURCE scope BASH_SOURCE[1] is the script that sourced this file — the one whose headers count.
# Inside a function it would be this file, which declares nothing.
aimecca_announce "${BASH_SOURCE[1]:-$0}"
aimecca_approve "${BASH_SOURCE[1]:-$0}" || exit 1

# ---- Progress and output ---------------------------------------------------

_aimecca_step_color() {
    case "$1" in
        1) printf 'grey' ;;
        2|3) printf 'blue' ;;
        4) printf 'amber' ;;
        5) printf 'green' ;;
        *) printf 'grey' ;;
    esac
}

# Print a step. A label starting [n/m] or [nb/m] also moves the chat's progress bar to that step's
# START, so a sub-step lands strictly between its parent and the next whole step.
aimecca_step() {
    local label="$1"

    if [[ "$label" =~ ^\[([0-9]+)([a-z]*)/([0-9]+)\][[:space:]]*(.*) ]]; then
        local n="${BASH_REMATCH[1]}"
        local suffix="${BASH_REMATCH[2]}"
        local m="${BASH_REMATCH[3]}"
        local step_text="${BASH_REMATCH[4]}"

        local span=$(( 100 / m ))
        local percent=$(( (n - 1) * span ))
        local targetPercent=$(( n * span ))
        if [ -n "$suffix" ]; then
            local ordinal=$(( $(printf '%d' "'${suffix:0:1}") - 96 ))
            percent=$(( percent + span - span / (ordinal + 1) ))
        fi

        local color
        color="$(_aimecca_step_color "$n")"
        echo "#@guard progress id=${AIMECCA_PROGRESS_ID:-build} percent=$percent targetPercent=$targetPercent color=$color label=\"$step_text\""
    fi

    printf '  %s\n' "$1"
}

# Close the progress bar: snap to 100%, go green, fade. Call once, when the script knows it is done.
aimecca_done() {
    local label="${1:-Done}"
    echo "#@guard progress id=${AIMECCA_PROGRESS_ID:-build} percent=100 color=green label=\"$label\" state=done fade=1000"
    printf '  %s\n' "$label"
}

aimecca_ok()   { printf '  ✅ %s\n' "$1"; }
aimecca_fail() { printf '  ❌ %s\n' "$1" >&2; }
aimecca_warn() { printf '  ⚠️  %s\n' "$1" >&2; }

# For a double-clicked .command only: hold the Terminal window open so the output can be read.
# Holds only when both stdin and stdout are a terminal, so a script run from a chat, cron or another
# script never waits on a keypress. Use as `trap aimecca_pause_on_exit EXIT`.
aimecca_pause_on_exit() {
    # First statement: anything before it would replace the exit status being reported.
    local status=$?
    if [ -t 0 ] && [ -t 1 ] && [ "${AIMECCA_NO_PAUSE:-0}" != "1" ]; then
        printf '\n  Finished with exit code %s. Press any key to close.\n' "$status"
        read -n 1 -s -r
    fi
    return $status
}
