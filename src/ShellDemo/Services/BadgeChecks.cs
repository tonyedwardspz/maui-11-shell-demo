using ShellDemo.ViewModels;
namespace ShellDemo.Services;
public static class BadgeChecks
{
    public static async Task<List<string>> RunAsync()
    {
        var shell = (AppShell)Shell.Current;
        var state = shell.State;
        var savedCount = state.UnreadNotificationCount;
        var savedMode = state.Mode;
        var results = new List<string>();
        await shell.GoToAsync("//notifications", false);
        var item = shell.Items[0].Items[2].Items[0];
        var toolbar = shell.CurrentPage.ToolbarItems[0];
        void Check(string name, string? expected)
        {
            var success = item.BadgeText == expected && toolbar.BadgeText == expected;
            results.Add($"{(success ? "PASS" : "FAIL")} badge {name}: tab={item.BadgeText ?? "<null>"}, toolbar={toolbar.BadgeText ?? "<null>"}");
        }
        try
        {
            state.Mode = BadgeMode.Count; state.UnreadNotificationCount = 5; Check("count", "5");
            state.AddNotificationCommand.Execute(null); Check("increment", "6");
            state.MarkAllReadCommand.Execute(null); Check("zero hides", null);
            state.ShowDotCommand.Execute(null); Check("empty string dot", "");
            state.HideBadgeCommand.Execute(null); Check("null hidden", null);
            state.ShowTextCommand.Execute(null); Check("text", "New");
            state.ShowLongCountCommand.Execute(null); Check("long count", "1234");
        }
        finally { state.UnreadNotificationCount = savedCount; state.Mode = savedMode; }
        return results;
    }
}
