using System.Reflection;
using System.Xml;
using GuardX.BLServices;
using GuardX.Interfaces;
using GuardX.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog;
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

            //As this exe is a single-release, self-contained, non-extracting files type of executable.
            //We require to read the resources from the embedded files. All resource files are embedded within exe
            //And to read those files from exe require using GetManifestResourceStream() that helps to read the embedded files.

            //Following code is writted to read the NLog.config file as by default it is expected that NLog.config file 
            //will be present from the exe executing location which is default in our case. So, we require special code
            //to read those files and code accordingly.
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("GuardX.NLog.config");
            LogManager.Configuration = new XmlLoggingConfiguration(XmlReader.Create(stream), null);

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
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
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