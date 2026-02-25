#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT="$SCRIPT_DIR/src/Ropro.AI/Ropro.AI.csproj"

echo "Packing Ropro.AI..."
dotnet pack "$PROJECT" -c Release

echo ""
echo "Done. Package output:"
ls -1 "$SCRIPT_DIR"/src/Ropro.AI/bin/Release/*.nupkg
