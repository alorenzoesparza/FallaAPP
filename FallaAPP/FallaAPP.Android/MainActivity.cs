using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.Permissions;

namespace FallaAPP.Droid
{
    [Activity(Label = "FallaAPP", 
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTUyOEAzMTM2MmUzMjJlMzBBSThRWGFtUlBydHF1Zy9NWkJEMno0ai9HZENvRTZHcmNua0NVeE1FQ1ZVPQ==;MTUyOUAzMTM2MmUzMjJlMzBiWWEyK0VOWDVMRjI5ZTdvSHZYRVUyZVY4d3NMN2owR25xRTh2aHhzTHVrPQ==");

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(
            int requestCode, 
            string[] permissions, 
            [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(
                requestCode, 
                permissions, 
                grantResults);
        }
    }
}

