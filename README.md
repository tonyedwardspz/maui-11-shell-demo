# Shell Workshop · .NET MAUI 11

A working companion to **A Practical Guide to the New Shell in .NET MAUI 11**. Start on the Guide tab and follow the numbered exercises. All buttons invoke MAUI APIs; route captures come from real `IQueryAttributable` callbacks.

## Run

Requires .NET SDK 11.0.100 (or its latest patch), the MAUI workload, and platform tooling compatible with that workload. MAUI Controls is pinned to **11.0.10** and CommunityToolkit.Mvvm to **8.4.2**. This repository uses the standard MAUI template platform entry points.

```sh
dotnet workload restore src/ShellDemo/ShellDemo.csproj
# Start an Android emulator or connect a device first.
dotnet build src/ShellDemo/ShellDemo.csproj -t:Run -f net11.0-android
# On a Mac with the matching Xcode:
dotnet build src/ShellDemo/ShellDemo.csproj -t:Run -f net11.0-maccatalyst -p:RuntimeIdentifier=maccatalyst-arm64
# On Windows:
dotnet build src/ShellDemo/ShellDemo.csproj -t:Run -f net11.0-windows10.0.19041.0
```

Open `src/ShellDemo/ShellDemo.csproj` in a MAUI-capable IDE to choose an iOS simulator or a particular device. `global.json` selects .NET 11. Apple Shell handlers are enabled by default in this sample.

## Follow the post

| Post section | Exercise | Source under `src/ShellDemo/` |
| --- | --- | --- |
| 1. Tab badges | Inbox: count, zero, dot, hidden, text, 1234 | `AppShell.xaml`, `ViewModels/AppShellViewModel.cs` |
| 2. Toolbar badges | Observe the Inbox toolbar icon and tab together | `Views/NotificationsPage.xaml` |
| 3. Overflow | Native lab: Search plus Settings / Feedback / About | `Views/NativeLabPage.cs` |
| 4. Route templates | Bookings: run individual cases or all route checks | `AppShell.xaml.cs`, `Services/RouteScenarios.cs`, `ViewModels/BookingDetailsViewModel.cs` |
| 5. Handler architecture | Native lab: inspect actual root handler type | `MauiProgram.cs`, `ShellDemo.csproj` |
| 6. Appearance trackers | Android native subtitle and title-colour comparison; Apple red tab-bar border | `Platforms/Android/CustomShellHandler.cs`, `Handlers/Apple/CustomShellHandler.cs` |
| 7. Tab reselection | Open a booking, push another detail, reselect Bookings on Android | `Platforms/Android/CustomShellHandler.cs` |
| 8. Toolbar/back | Booking details: custom icon, text, command, accessibility label | `Views/BookingDetailsPage.cs` |
| 9. Transitions | Native lab: animated versus instant push; Android fade toggle | `Views/NativeLabPage.cs`, `Platforms/Android/CustomShellHandler.cs` |
| 10. Migration | Rebuild using switches below and compare | `MauiProgram.cs`, `ShellDemo.csproj` |
| 11. Other features | Guide/Native lab: standalone TabbedPage demo; C# UI template command | `Views/NativeLabPage.cs`, `Views/GuidePage.cs` |

The app has four bottom tabs to keep navigation usable on phones. Detail pages display the delivered parameter values and CLR types. All six constraints have an exercise. Invalid integers and relative template URIs deliberately demonstrate rejection. The default-value exercise deliberately asserts the post's claim rather than supplying a fallback in application code.

Constrained and unconstrained booking templates use different literal prefixes (`integer/` versus `booking/`) so that the unconstrained template cannot absorb an invalid integer. Bookings are illustrative identifiers; no network or backend is required.

The Android reselect override calls `section.Navigation.PopToRootAsync()`, guards repeated callbacks, and catches errors at the asynchronous native event boundary. Disabling its switch delegates to MAUI's base behaviour. Apple keeps its native navigation transitions; the post does not supply a complete custom Apple transition, and this sample does not invent one.

Shared Apple source lives under `Handlers/Apple`, protected by `#if IOS || MACCATALYST`. A new `Platforms/Apple` folder is not automatically included by the MAUI single-project platform conventions.

## Compare migration modes

Clean between modes to prevent stale native or generated assets. The switches remove the custom registrations as well as changing the built-in implementation.

```sh
dotnet clean src/ShellDemo/ShellDemo.csproj -f net11.0-android
dotnet build src/ShellDemo/ShellDemo.csproj -f net11.0-android -p:ShellDemoLegacyAndroid=true

dotnet clean src/ShellDemo/ShellDemo.csproj -f net11.0-maccatalyst
dotnet build src/ShellDemo/ShellDemo.csproj -f net11.0-maccatalyst -p:UseiOSShellHandler=false
```

Omit those flags to return to the sample's custom handlers. The Android tracker retains the post’s native red-title call for comparison. In on-device testing, the configured white Shell title colour remained visible; a native subtitle makes the tracker visibly identifiable. Use `Shell.TitleColor` for stable title colours.

## Validate

On Bookings, tap **Run all route checks**. The app first checks seven real tab/toolbar badge binding updates, then navigates every route case, verifies actual page selection and parameter delivery, checks string types and expected `ArgumentException`s, then returns to Bookings. Results are displayed and saved to `FileSystem.AppDataDirectory/route-checks.txt`. A failure means the runtime did not meet the assertion; it is not silently fixed by the app.

Debug Android builds also accept the `runRouteChecks` boolean intent extra for repeatable emulator validation. See `scripts/check-android.sh`. Set `ANDROID_SERIAL` when multiple devices are attached. The script installs only this sample's APK and restarts only its package. Debug APKs embed their assemblies so direct installation works without IDE fast deployment. Optional `ADB_PORT` selects a local ADB server; `SKIP_BUILD=1` reuses an already-built Debug APK.

Use [docs/manual-checks.md](docs/manual-checks.md) for native UI and accessibility checks and [docs/tutorial-validation.md](docs/tutorial-validation.md) for findings and precise validation limits.
