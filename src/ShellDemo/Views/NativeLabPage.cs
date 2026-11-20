using ShellDemo.Services;
namespace ShellDemo.Views;
public class NativeLabPage : WorkshopPage
{
    public NativeLabPage() : base("03 / 05–10 · Native lab")
    {
        Heading("Inspect native navigation");
        Note("Search is primary. Settings, Feedback and About are secondary, with priorities 0, 1 and 2. Open the native overflow menu to verify order.");
        AddToolbar("Search", ToolbarItemOrder.Primary, 0, "search.png");
        AddToolbar("Settings", ToolbarItemOrder.Secondary, 0);
        AddToolbar("Feedback", ToolbarItemOrder.Secondary, 1);
        AddToolbar("About", ToolbarItemOrder.Secondary, 2);
        Action("Inspect current handler and last callback", () =>
        {
            Status.Text = $"Platform: {DeviceInfo.Platform}\nHandler: {Shell.Current.Handler?.GetType().FullName}\n{LabOptions.LastNativeEvent}";
            return Task.CompletedTask;
        });
        Note("06 · Android: the custom tracker adds a native subtitle. Compare the article’s red-title call: MAUI may reapply Shell.TitleColor afterward. Apple handler mode: the tab bar has a red border. Both call the base appearance tracker first.");
        Code("""
// Shared colours remain the first choice
Shell.SetBackgroundColor(this, Color.FromArgb("#102A43"));
Shell.SetTitleColor(this, Colors.White);
""");
        var reselect = new Switch { IsToggled = LabOptions.ReselectToRoot };
        reselect.Toggled += (_, e) => LabOptions.ReselectToRoot = e.Value;
        Note("07 · Android reselect to root (enabled by default)"); Body.Add(reselect);
        Go("Open booking, then push another detail", "//bookings/booking/12345");
        Note("09 · Android custom fade animation. This native override is independent of GoToAsync's animate flag. Apple retains native transitions.");
        var fade = new Switch { IsToggled = LabOptions.FadeTransitions };
        fade.Toggled += (_, e) => LabOptions.FadeTransitions = e.Value; Body.Add(fade);
        Go("Push traditional detail · animated", "bookingdetails?bookingId=animated");
        Go("Push traditional detail · no animation", "bookingdetails?bookingId=instant", false);
        Note("10 · Compare handler and renderer builds. Apple: UseiOSShellHandler=false. Android: ShellDemoLegacyAndroid=true. Clean between configurations. Custom handler registrations are excluded in legacy builds.");
        Action("11 · Standalone TabbedPage badge demo", () => { TabbedDemo.Open(); return Task.CompletedTask; });
    }
    private void AddToolbar(string text, ToolbarItemOrder order, int priority, string? icon = null)
    {
        ToolbarItems.Add(new ToolbarItem
        {
            Text = text, Order = order, Priority = priority, IconImageSource = icon,
            Command = new Command(() => Status.Text = $"Invoked {text} (priority {priority}).")
        });
    }
}
public static class TabbedDemo
{
    public static void Open()
    {
        var tabs = new TabbedPage { Title = "11 · TabbedPage badges" };
        var inbox = new ContentPage { Title = "Inbox", IconImageSource = "notifications.png" };
        TabbedPage.SetBadgeText(inbox, "3");
        TabbedPage.SetBadgeColor(inbox, Colors.Red);
        TabbedPage.SetBadgeTextColor(inbox, Colors.White);
        // Swap the window root: TabbedPage is never nested inside Shell, and this also works on iPhone.
        var window = Application.Current!.Windows.First(w => w.Page is AppShell);
        var original = window.Page!;
        var close = new Button { Text = "Return to Shell workshop" };
        close.Clicked += (_, _) => window.Page = original;
        var dot = new Button { Text = "Show dot" };
        dot.Clicked += (_, _) => TabbedPage.SetBadgeText(inbox, "");
        var hide = new Button { Text = "Hide badge" };
        hide.Clicked += (_, _) => TabbedPage.SetBadgeText(inbox, null);
        inbox.Content = new VerticalStackLayout
        {
            Padding = 24, Spacing = 16,
            Children = { new Label { Text = "A standalone TabbedPage, outside Shell. Initial badge: 3." }, dot, hide, close }
        };
        tabs.Children.Add(new ContentPage { Title = "Home", IconImageSource = "home.png", Content = new Label { Text = "Select Inbox to change its badge.", Margin = 24 } });
        tabs.Children.Add(inbox);
        window.Page = tabs;
    }
}
