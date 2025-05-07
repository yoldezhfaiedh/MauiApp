using Microsoft.Extensions.Logging;
using MauiApp6.Data;
using Microsoft.AspNetCore.Components.Authorization;
using MauiApp6.Services;
using MauiApp6.Components.Pages;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using System.Configuration;
namespace MauiApp6
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });


            builder.Services.AddMauiBlazorWebView();
            //        builder.Services.AddIdentityCore<Employer>()
            //.AddRoles<IdentityRole>()
            //.AddEntityFrameworkStores<RecruitingContext>();
            //        builder.Services.AddMauiBlazorWebView();

            //        builder.Services.AddIdentityCore<Seeker>()
            //.AddRoles<IdentityRole>()
            //.AddEntityFrameworkStores<RecruitingContext>();

            //        builder.Services.AddSingleton<IJWTService>(new JWTService(
            //            secretKey: "votre_cle_secrete_tres_longue_32_caracteres_minimum",
            //            issuer: "MauiApp1",
            //            audience: "MauiApp1",
            //            expiresInMinutes: 1440)); // 24h
            //        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);


                        builder.Services.AddDbContext<RecruitingContext>(ServiceLifetime.Transient);
            builder.Services.AddTransient<IAppService, AppService>();
            builder.Services.AddTransient<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<LostItemsViewModel>();
            //builder.Services.AddSingleton<LostItemsViewModel>();
            builder.Services.AddSingleton<UserViewModel>();
            //builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddTransient<Dashboard>(); // 
            builder.Services.AddTransient<AddItemPage>(); // 

            builder.Services.AddTransient<LostItemAllPage>(); // 
            builder.Services.AddTransient<LostItemsbyUser>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();

            builder.Logging.AddDebug();
#endif
            // MauiProgram.cs
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddScoped<IAppService, AppService>();

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<UserViewModel>();
            builder.Services.AddSingleton<AuthenticationViewModel>();
            builder.Services.AddScoped<AuthenticationViewModel>();
            builder.Services.AddSingleton<LostItemsViewModel>();
            builder.Services.AddScoped<UserSessionService>();

            return builder.Build();
        }
    }
}
