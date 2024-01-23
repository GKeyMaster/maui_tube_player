﻿namespace MauiTubePlayer;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCompatibility()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("FiraSans-Light.ttf", "RegularFont");
                fonts.AddFont("FiraSans-Medium.ttf", "MediumFont");
            })
            .ConfigureLifecycleEvents(events =>
            {
#if ANDROID
                events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

                static void MakeStatusBarTranslucent(Android.App.Activity activity)
                {
                    activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);

                    activity.Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

                    activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                }
#endif
            }).ConfigureMauiHandlers(handlers =>
            {
                //Add Legacy MediaElement Renderer
                handlers.AddCompatibilityRenderer(
                    typeof(Xamarin.CommunityToolkit.UI.Views.MediaElement),
                    typeof(Xamarin.CommunityToolkit.UI.Views.MediaElementRenderer));
            });

        //Register Services
        RegisterAppServices(builder.Services);

        return builder.Build();
	}

    private static void RegisterAppServices(IServiceCollection services)
    {
        //Add Platform specific Dependencies
        services.AddSingleton<IConnectivity>(Connectivity.Current);

        //Register Cache Barrel
        Barrel.ApplicationId = Constants.ApplicationId;
        services.AddSingleton<IBarrel>(Barrel.Current);


        //Register API Service
        services.AddSingleton<IApiService, YoutubeService>();

        //Register FileDownloadService
        services.AddSingleton<IDownloadFileService, FileDownloadService>();

        //Register View Models
        services.AddSingleton<StartPageViewModel>();
        services.AddTransient<VideoDetailsPageViewModel>();

    }
}

