#!/bin/sh
set -eu
cd "$(dirname "$0")"
case "${1:-demo}" in
    demo|demo2) project="${1:-demo}" ;;
    *) echo "Usage: sh ./startLocal.sh [demo|demo2]" >&2; exit 1 ;;
esac
dotnet tool restore
dotnet paket restore
cd "$project"
func start