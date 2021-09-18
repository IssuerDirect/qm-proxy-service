using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace testConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string appSettingsFile = $"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json";
            var builder = new ConfigurationBuilder().AddJsonFile(appSettingsFile, optional: true, reloadOnChange: true).AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();
            
        }
    }
}
