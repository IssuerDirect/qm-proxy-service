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
        apiResponse myResponse = new apiResponse();
        lib clib = new lib();
        SNNLib lib = new SNNLib();
        int pageSize = 24;
        public adminController(IConfiguration config, platformDB platformDB) {
            lib.config = config;
            lib.platformDB = platformDB;
            clib.myConfiguration = config;
        }

        #region insights
        /// <summary>
        /// Paged list of insights
        /// </summary>
        /// <param name="index">Optional, starts with 0</param>
        /// <remarks>Get a paged list of insights ordered by id desc. Page size is 24</remarks>
        [HttpGet("/insights")]
        public apiResponse getInsights(int index = 0) {

            return myResponse;
        }

        /// <summary>
        /// Create or update insight
        /// </summary>
        /// <param name="insight">Insight object</param>
        /// <remarks>To update, include id, if id is 0 it inserts</remarks>
        [HttpPost("/insight")]
        public apiResponse saveInsight([FromBody] snn_Insight insight)
        {

            return myResponse;
        }
        #endregion
    }

}
