namespace ShellDemo.Views;
public class GuidePage : WorkshopPage
{
    public GuidePage() : base("Shell Workshop · .NET 11")
    {
        Heading("Read it. Run it. Inspect it.");
        Note("Follow the blog in order. Every exercise uses the real MAUI Shell API. Source paths below are relative to src/ShellDemo.");
        Go("01–02 · Tab and toolbar badges", "//notifications");
        Note("Change counts, show a dot, hide the badge and compare text across platforms. Source: AppShell.xaml, NotificationsPage.xaml and AppShellViewModel.cs.");
        Go("03 · Toolbar overflow", "//lab");
        Note("Open Search, then the Settings / Feedback / About secondary actions in order. Source: Views/NativeLabPage.cs.");
        Go("04 · Route template playground", "//bookings");
        Note("Try required, constrained, optional, default, catch-all and mixed segments, literal precedence and expected failures. Source: AppShell.xaml.cs, Views/BookingsPage.cs.");
        Go("05–07 · Handlers and tab reselect", "//lab");
        Note("Inspect the active handler. On Android, open two booking details and reselect Bookings. Android adds a native toolbar subtitle; Apple tab bars have a red border. Source: Platforms/Android and Handlers/Apple.");
        Go("08 · Custom back button", "//bookings/booking/12345");
        Note("Use the toolbar back button and test its spoken accessibility label with TalkBack or VoiceOver. Source: Views/BookingDetailsPage.cs.");
        Go("09–10 · Transitions and migration", "//lab");
        Note("Compare animated/non-animated pushes and the Android fade toggle. Rebuild with legacy switches to compare native implementations.");
        Action("11 · Open standalone TabbedPage demo", () => { TabbedDemo.Open(); return Task.CompletedTask; });
        Note("TabbedPage temporarily replaces the window root, outside Shell. Return restores the workshop. For a C# UI starter: dotnet new maui --ui csharp. This app deliberately mixes XAML and C# so readers can compare both.");
        Code("""
dotnet build -f net11.0-android
dotnet build -f net11.0-maccatalyst -p:UseiOSShellHandler=false
dotnet build -f net11.0-android -p:ShellDemoLegacyAndroid=true
""");
    }
}
