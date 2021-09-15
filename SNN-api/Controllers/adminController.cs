using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;
using net3000.common;
using net3000.common.models;

namespace snn.Controllers
{
    [ApiController]
    public class adminController : ControllerBase
    {
        IConfiguration myConfig;
        apiResponse myResponse = new apiResponse();
        lib clib = new net3000.common.lib();
        SNNLib lib = new SNNLib();
        public adminController(IConfiguration config, platformDB platformDB) {
            lib.config = config;
            lib.platformDB = platformDB;
            clib.myConfiguration = config;
        }

        [HttpGet("/insights")]
        public apiResponse getInsights() {

            return myResponse;
        }
    }
    
}
