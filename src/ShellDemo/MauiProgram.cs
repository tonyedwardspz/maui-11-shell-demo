using Microsoft.Extensions.Logging;
using ShellDemo.ViewModels;
namespace ShellDemo;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<AppShell>();
        builder.ConfigureMauiHandlers(handlers =>
        {
#if ANDROID
#if LEGACY_ANDROID_SHELL
            handlers.AddHandler<Shell, Microsoft.Maui.Controls.Handlers.Compatibility.ShellRenderer>();
#else
            handlers.AddHandler<Shell, Platforms.Android.CustomShellHandler>();
            handlers.AddHandler<ShellItem, Platforms.Android.ReselectShellItemHandler>();
            handlers.AddHandler<ShellSection, Platforms.Android.FadeShellSectionHandler>();
#endif
#elif (IOS || MACCATALYST) && APPLE_SHELL_HANDLER
            handlers.AddHandler<Shell, Platforms.Apple.CustomShellHandler>();
#endif
        });
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
