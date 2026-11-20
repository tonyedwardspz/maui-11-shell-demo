#if IOS || MACCATALYST
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;
namespace ShellDemo.Platforms.Apple;
public class CustomShellHandler : ShellHandler
{
    protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker() => new CustomTabBarAppearanceTracker();
}
public class CustomTabBarAppearanceTracker : ShellTabBarAppearanceTracker
{
    public override void SetAppearance(UITabBarController controller, ShellAppearance appearance)
    {
        base.SetAppearance(controller, appearance);
        controller.TabBar.Layer.BorderColor = UIColor.Red.CGColor;
        controller.TabBar.Layer.BorderWidth = 1;
    }
}
#endif
