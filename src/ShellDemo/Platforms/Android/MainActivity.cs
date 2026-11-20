using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ShellDemo;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
#if DEBUG
    private bool checksStarted;
    protected override void OnPostResume()
    {
        base.OnPostResume();
        StartChecksIfRequested();
    }

    protected override void OnNewIntent(global::Android.Content.Intent? intent)
    {
        base.OnNewIntent(intent);
        Intent = intent;
        checksStarted = false;
        StartChecksIfRequested();
    }

    private void StartChecksIfRequested()
    {
        if (!checksStarted && Intent?.GetBooleanExtra("runRouteChecks", false) == true)
        {
            checksStarted = true;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await Task.Delay(1500);
                    var report = await Services.RouteChecks.RunAsync();
                    global::Android.Util.Log.Info("ShellWorkshop", report);
                }
                catch (Exception ex) { global::Android.Util.Log.Error("ShellWorkshop", ex.ToString()); }
            });
        }
    }
#endif
}
