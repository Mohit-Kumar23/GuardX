using GuardX.BLServices;
using GuardX.Interfaces;
using GuardX.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace GuardX
{
    internal static class Program
    {
        public static IServiceProvider serviceProvider { get; private set; }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var serviceCollections = new ServiceCollection();
            ConfiguerServices(serviceCollections);
            serviceProvider = serviceCollections.BuildServiceProvider();
            Application.Run(serviceProvider.GetRequiredService<Gx_HomeForm>());
        }

        private static void ConfiguerServices(IServiceCollection services)
        {
            //Configure Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
                loggingBuilder.AddNLog();
            });


            //Singleton services
            services.AddSingleton<IIDNConfigService,IDNConfigService>();

            //Scoped services
            services.AddScoped<IRegistryServices, GxRegistryServices>();
            services.AddScoped<IIdentificationFileService, GxIdentificationFileService>();
            services.AddScoped<IVisibilityService, GxVisibilityService>();
            services.AddScoped<IEmailService, EmailService>();

            //Transient services
            services.AddTransient<Gx_HomeForm>();
            services.AddTransient<Gx_ProfileSetupForm>();
            services.AddTransient<Gx_OtpFrom>();
            services.AddTransient<Gx_PasswordInputForm>();
        }
    }
}