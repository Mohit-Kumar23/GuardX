using GuardX.BLServices;
using GuardX.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton<IIDNConfigService,IDNConfigService>();
            services.AddScoped<IRegistryServices, GxRegistryServices>();
            services.AddScoped<IIdentificationFileService, GxIdentificationFileService>();
        }
    }
}