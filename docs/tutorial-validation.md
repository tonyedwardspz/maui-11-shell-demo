# Tutorial validation

Validated against .NET SDK **11.0.100**, MAUI Controls **11.0.10**, CommunityToolkit.Mvvm **8.4.2**, on 20 November 2026. The supplied article was reviewed without modifying the original attachment.

## Corrections to make

### Section 6: Android appearance tracker namespaces and Toolbar ambiguity

The Android snippets import `Microsoft.Maui.Controls.Platform`, but `IShellContext`, `IShellToolbarAppearanceTracker`, `IShellToolbarTracker` and `ShellToolbarAppearanceTracker` are in `Microsoft.Maui.Controls.Platform.Compatibility` in the installed release. Copying the snippets produces CS0246 errors. MAUI implicit usings also make `Toolbar` ambiguous with `Microsoft.Maui.Controls.Toolbar` (CS0104).

Use these imports in the Android files:

```csharp
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
```

The rest of the example compiles with those changes. The sample's `Platforms/Android/CustomShellHandler.cs` contains the working version. This mismatch also exists in the linked Learn handler example, so consulting that example alone does not catch the problem. The release's [actual tracker source](https://github.com/dotnet/maui/blob/11.0.10/src/Controls/src/Core/Compatibility/Handlers/Shell/Android/ShellToolbarAppearanceTracker.cs) confirms the namespace and signature.

### Section 8: generated back command name

The XAML uses `Command="{Binding BackCommand}"`, but the method shown later is `[RelayCommand] private async Task GoBackAsync()`. CommunityToolkit generates **GoBackCommand**, so those two snippets do not work together as written. Change the binding to `GoBackCommand`, or rename the method to `BackAsync`. The sample uses `BackAsync` with `BackCommand`.

This follows the [documented generator naming rule](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/generators/relaycommand): remove the Async suffix, then append Command.

### Section 6: the native title-colour change can be overwritten

After correcting the imports, the custom Android handler was active on both the API 35 emulator and the connected Pixel 8a (Android 17/API 37), but toolbar titles remained white with the sample's `Shell.TitleColor="White"`. The snippet's `toolbar.SetTitleTextColor(...Red)` therefore did not reliably produce the promised final appearance in this configuration. A later toolbar appearance update is a likely cause; this is an inference from the observed result and MAUI's toolbar colour mapping, not a traced ordering guarantee.

For standard title colours, show `Shell.TitleColor="Red"` as the durable public API, as the article already recommends. Use a native-specific feature to demonstrate why an appearance tracker is needed. The sample retains the original title call for comparison and also adds the native toolbar subtitle **Custom native tracker**, a separate visible native customization. The in-app text explains the difference instead of claiming the title will be red.

## Setup clarifications for readers

- Register the shared `AppShellViewModel` and inject it into `AppShell`; page binding contexts do not automatically become the Shell view model. The sample uses singleton DI registrations and explicitly shares the same state with the Inbox page.
- The two `AppShell` constructors shown in sections 1 and 4 are alternatives within an article. Readers combining examples should keep the injected constructor and add route registrations there.
- Do not register both `booking/{bookingId}` and `booking/{bookingId:int}` expecting invalid integers to be rejected by both: the unconstrained route can still accept arbitrary text. The sample gives the constrained demonstration the distinct prefix `integer/`.
- Shared Apple code should live in an included folder, guarded by `#if IOS || MACCATALYST`, or be explicitly included by MSBuild. Adding a physical `Platforms/Apple` folder alone does not make it shared across iOS and Mac Catalyst. The sample uses `Handlers/Apple`.
- Reverting `UseiOSShellHandler` to false also requires removing custom Shell handler registrations. The sample conditions these registrations on the same build setting.
- The reselect and native transition sections in the post discuss extension points rather than complete overrides. The sample supplies a complete Android reselect implementation with a concurrency guard, exception handling and the section's own navigation stack, plus an optional Android fade override. Apple custom item-transition animation is intentionally not presented as a complete tested example.

## Build evidence

- Android Debug application build: passed, zero warnings and zero errors.
- Android legacy renderer configuration, C# Compile target: passed, zero warnings and zero errors.
- Mac Catalyst handler configuration, C# Compile target: passed, zero warnings and zero errors.
- iOS simulator handler configuration, C# Compile target: passed, zero warnings and zero errors.
- Mac Catalyst renderer configuration, C# Compile target: passed, zero warnings and zero errors.

Apple compile checks used `-t:Compile -p:ValidateXcodeVersion=false` **only to validate C# and reference APIs**. Full Mac Catalyst build correctly fails because installed Xcode **26.2** is older than the installed .NET Apple workload's required **27.2**. Do not use this flag as a claim of runnable Apple build support. No Apple app, NativeAOT publish or Windows build has been validated here.

## Runtime evidence

**26/26 assertions passed on the connected Pixel 8a, Android 17/API 37**, using the final standalone Debug APK. The same assertions also passed on the Android API 35 emulator before the native subtitle was added.

- Seven actual tab/toolbar badge binding checks: count, increment, zero, empty string, null, text and long count.
- Nineteen real Shell navigation checks: required values, escaped spaces, all six constraints, invalid-integer rejection, optional absent/present, defaults absent/present, catch-all, mixed segments, literal precedence, path/query precedence, relative-template rejection and traditional query-string navigation.
- The default-absent case delivered `page = "1"`; the potential limitation noted in the original [route-template PR](https://github.com/dotnet/maui/pull/35110) did **not** reproduce for the article's URI shape on this release.
- On-device native menu order: Settings, Feedback, About.
- On-device native accessibility hierarchy: the booking toolbar back button exposes **Return to bookings**. Spoken TalkBack output has not been assessed.
- On-device tab reselection: opened a booking, pushed another detail, reselected Bookings and verified the visible page returned from `ParameterPage` to the Bookings root.
- On-device TabbedPage demo: switched the window root outside Shell, verified initial badge 3 and the dot/hidden controls, then restored the original Shell.
- On-device custom toolbar back command returned from an animated booking-detail push to Native lab. The native fade override reported `popping=True, entering=True` after the pop.
- Confirmed the active root handler is `ShellDemo.Platforms.Android.CustomShellHandler`.
- Observed the native red-title caveat described above; confirmed the final sample’s red native subtitle is visible.

The full physical-device assertion report is [validation/pixel-8a-checks.txt](validation/pixel-8a-checks.txt); the emulator report is [validation/android-checks.txt](validation/android-checks.txt). These assertions validate actual MAUI behaviour, not a reimplementation of its route parser. Native visual, accessibility and gesture checks remain separate in [manual-checks.md](manual-checks.md). Badges were inspected as native UI, but automated assertions validate the bound property values rather than pixels. No claim is made that every platform renders badges identically.

## Publication cleanup

The pasted article contains favicon/image placeholders, `GitHub+1` / `Microsoft Learn+2` fragments and `utm_source=chatgpt.com` tracking parameters. Remove those publishing artifacts. The closing implementation note saying that no complete sample was compiled can now be replaced with the concrete version and validation scope above.
