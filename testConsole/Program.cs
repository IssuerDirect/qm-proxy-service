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
using System.ServiceModel.Syndication;
using System.Xml;

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
            snn.SNNLib lib = new snn.SNNLib();
            lib.config = configuration;

            var getThis = readFeed("http://feeds.feedburner.com/brontecapital");
            //using var reader = XmlReader.Create(url);
            //var feed = SyndicationFeed.Load(reader);
            //var items = feed.Items.Where(f => f.PublishDate > DateTime.Now.Date).Count();
            List<snn.cc_SnnInsight> readFeed(string url) {
                var results = new List<snn.cc_SnnInsight>();
                try {
                    using var reader = XmlReader.Create(url);
                    var feed = SyndicationFeed.Load(reader);
                    var items = feed.Items.ToList();
                    results = items.Select(i => new snn.cc_SnnInsight() {
                    headline = i.Title.Text,
                    occur = i.PublishDate.DateTime,
                    summary = i.Summary.Text,
                    thumb = feed.ImageUrl.ToString(),
                    type = url,
                    src = i.Links.First().ToString()
                    }).ToList();


                }
                catch { }
                return results;
            }
        }
    }
}
