using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using net3000;

namespace testConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string appSettingsFile = $"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json";
            var builder = new ConfigurationBuilder().AddJsonFile(appSettingsFile, optional: true, reloadOnChange: true).AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();
            net3000.common.lib clib = new net3000.common.lib();
            clib.myConfiguration = configuration;
            string encrypted = clib.encrypt("stocknewsnow10");
        }
    }
}
