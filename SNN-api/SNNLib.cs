using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net3000.common;
using net3000.common.models;

using Microsoft.Extensions.Configuration;
namespace snn
{
    public class SNNLib
    {
        lib clib = new lib();
        public platformDB platformDB;
        public IConfiguration config {
            set {
                clib.myConfiguration = value;
            }
            get {
                return clib.myConfiguration;
            }
        }
    }
}
