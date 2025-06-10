using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace BjjTrainer.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState) // Updated nullability to match the overridden member
        {
            base.OnCreate(savedInstanceState);

            // Additional initialization code here, if necessary
        }

    }
}
