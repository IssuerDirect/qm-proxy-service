using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net3000;
using accesswireCore;
using Microsoft.Extensions.Configuration;
namespace snn
{
    public class SNNLib
    {
        commonCore clib = new commonCore();
        accesswireLib acc = new accesswireLib();
        public platformDB platformDB = new platformDB();

        public accesswireDB accesswireDB
        {
            get
            {
                return acc.accesswireDB;
            }
            set
            {
                acc.accesswireDB = value;
            }
        }

        public net3000.accounts.DBContext.accountsDB accountsDB
        {
            get
            {
                return clib.accountsDB;
            }
            set
            {
                acc.accountsDB = value;
                clib.accountsDB = value;
            }
        }

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
