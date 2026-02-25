#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
KEY_FILE="$SCRIPT_DIR/nuget-api-key.txt"
NUPKG_DIR="$SCRIPT_DIR/src/Ropro.AI/bin/Release"

# Pack first
"$SCRIPT_DIR/pack.sh"

# Read API key
if [ ! -f "$KEY_FILE" ]; then
  echo "Error: API key file not found: $KEY_FILE"
  echo "Create the file and paste your NuGet API key into it."
  exit 1
fi

API_KEY="$(tr -d '[:space:]' < "$KEY_FILE")"

if [ -z "$API_KEY" ] || [ "$API_KEY" = "REPLACE_WITH_YOUR_NUGET_API_KEY" ]; then
  echo "Error: Please put your actual NuGet API key in $KEY_FILE"
  exit 1
fi

# Log which key is being used (masked)
KEY_LEN=${#API_KEY}
if [ "$KEY_LEN" -gt 8 ]; then
  MASKED="${API_KEY:0:4}...${API_KEY: -4} (length: $KEY_LEN)"
else
  MASKED="****** (length: $KEY_LEN)"
fi
echo "Using API key: $MASKED"
echo "Key file: $KEY_FILE"

# Find the latest .nupkg
NUPKG="$(ls -t "$NUPKG_DIR"/*.nupkg 2>/dev/null | head -1)"

if [ -z "$NUPKG" ]; then
  echo "Error: No .nupkg file found in $NUPKG_DIR"
  exit 1
fi

# Publish to local feed
LOCAL_FEED="$HOME/Packages"
mkdir -p "$LOCAL_FEED"
echo ""
echo "Publishing $NUPKG to local feed ($LOCAL_FEED)..."
dotnet nuget push "$NUPKG" --source "$LOCAL_FEED" --skip-duplicate

# Publish to nuget.org
echo ""
echo "Publishing $NUPKG to nuget.org..."
dotnet nuget push "$NUPKG" --api-key "$API_KEY" --source https://api.nuget.org/v3/index.json --skip-duplicate

echo ""
echo "Published successfully (local + nuget.org)."
