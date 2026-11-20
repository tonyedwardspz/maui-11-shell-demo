using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform.Compatibility;
namespace ShellDemo.Platforms.Android;
public class CustomShellHandler : ShellHandler
{
    protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker() => new CustomToolbarAppearanceTracker(this);
}
public class CustomToolbarAppearanceTracker(IShellContext context) : ShellToolbarAppearanceTracker(context)
{
    public override void SetAppearance(Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
    {
        base.SetAppearance(toolbar, toolbarTracker, appearance);
        // Retain the article's native title call so readers can compare its result.
        // MAUI may reapply Shell.TitleColor later; use the attached property for durable colours.
        toolbar.SetTitleTextColor(global::Android.Graphics.Color.Red);
        // Subtitle has no equivalent Shell appearance property and makes this native tracker visible.
        toolbar.Subtitle = "Custom native tracker";
        toolbar.SetSubtitleTextColor(global::Android.Graphics.Color.Red);
    }
}
public class ReselectShellItemHandler : ShellItemHandler
{
    private bool navigating;
    protected override async void OnTabReselected(ShellSection section)
    {
        if (!global::ShellDemo.Services.LabOptions.ReselectToRoot) { base.OnTabReselected(section); return; }
        if (navigating) return;
        navigating = true;
        try
        {
            global::ShellDemo.Services.LabOptions.LastNativeEvent = $"Reselected {section.Title}; pop to root.";
            await section.Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            global::ShellDemo.Services.LabOptions.LastNativeEvent = $"Reselect failed: {ex.Message}";
            System.Diagnostics.Debug.WriteLine(ex);
        }
        finally { navigating = false; }
    }
}
public class FadeShellSectionHandler : ShellSectionHandler
{
    protected override global::Android.Views.Animations.Animation? OnCreateNavigationAnimation(
        global::Android.Content.Context context, bool isPopping, bool enter)
    {
        if (!global::ShellDemo.Services.LabOptions.FadeTransitions) return base.OnCreateNavigationAnimation(context, isPopping, enter);
        global::ShellDemo.Services.LabOptions.LastNativeEvent = $"Fade: popping={isPopping}, entering={enter}";
        return new global::Android.Views.Animations.AlphaAnimation(enter ? 0f : 1f, enter ? 1f : 0f) { Duration = 220 };
    }
}
