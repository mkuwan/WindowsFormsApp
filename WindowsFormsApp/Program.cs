using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WindowsFormsApp.AppForm;
using WindowsFormsApp.Applications.CustomerService;
using WindowsFormsApp.Domain.DapperRepository;
using WindowsFormsApp.Infrastructure.DapperRepository;
using WindowsFormsApp.Infrastructure.DBConnection;
using WindowsFormsApp.Infrastructure.MyBatisRepository;

namespace WindowsFormsApp
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Configuration
            var configuration = BuildConfiguration();
            
            // DependencyInjection
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            var mainForm = serviceProvider.GetRequiredService<MainForm>();

            Application.Run(mainForm);
        }

        /// <summary>
        /// DI登録
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // sample
            //services.AddSingleton<IStart, Start>();

            // db config
            var dbConfig = configuration
                .GetSection(nameof(WinFormDbConnectionStringConfig))
                .Get<WinFormDbConnectionStringConfig>();
            services.AddSingleton<WinFormDbConnectionStringConfig>(dbConfig);
            
            // Form
            services.AddSingleton<MainForm>();
            services.AddTransient<DataGridForm>();  // SingletonだとDisposeされると次は使えないのでTransientにします

            // Repository
            //services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<ICustomerRepository, CustomerMyBatisRepository>();

            // Application Service
            services.AddSingleton<ICustomerService, CustomerService>();

        }

        private static IConfiguration BuildConfiguration()
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(appLocation)
                .AddJsonFile("appsettings.json")
                .Build();
        }
        

    }


}
