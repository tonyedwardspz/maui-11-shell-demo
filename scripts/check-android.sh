#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
adb="${ANDROID_HOME:-$HOME/Library/Android/sdk}/platform-tools/adb"
serial="${ANDROID_SERIAL:-emulator-5554}"
package=com.tonyedwards.shelldemo
if [[ "${SKIP_BUILD:-0}" != 1 ]]; then
    dotnet build src/ShellDemo/ShellDemo.csproj -f net11.0-android --disable-build-servers
fi
adb_cmd() { "$adb" -P "${ADB_PORT:-5037}" -s "$serial" "$@"; }
adb_cmd wait-for-device
for ((i=0; i<90; i++)); do
    if [[ "$(adb_cmd shell getprop sys.boot_completed | tr -d '\r')" == 1 ]]; then break; fi
    sleep 1
done
apk=src/ShellDemo/bin/Debug/net11.0-android/com.tonyedwards.shelldemo-Signed.apk
adb_cmd install --no-incremental -r "$apk"
adb_cmd shell am force-stop "$package"
adb_cmd shell run-as "$package" rm -f files/route-checks.txt
activity=$(adb_cmd shell cmd package resolve-activity --brief "$package" | tr -d '\r' | tail -n 1)
adb_cmd shell am start -S -n "$activity" --ez runRouteChecks true
mkdir -p artifacts
for ((i=0; i<90; i++)); do
    if adb_cmd shell run-as "$package" cat files/route-checks.txt > artifacts/route-checks.txt 2>/dev/null; then
        cat artifacts/route-checks.txt
        if rg -q '^FAIL' artifacts/route-checks.txt; then exit 1; fi
        exit 0
    fi
    sleep 1
done
echo 'Route checks did not finish. Inspect adb logcat -s ShellWorkshop.' >&2
exit 1
