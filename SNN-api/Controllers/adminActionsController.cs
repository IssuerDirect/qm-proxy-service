using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;
using net3000;
using net3000.common;
using net3000.common.models;

namespace snn.Controllers
{
    [ApiController]
    public class adminActionsController : ControllerBase
    {
        apiResponse myResponse = new apiResponse();
        lib clib = new lib();
        SNNLib lib = new SNNLib();
        int pageSize = 24;
        public adminActionsController(IConfiguration config, platformDB platformDB) {
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
            if (!isAdmin()) { return standardMessages.unauthorized; }
            var myList = lib.platformDB.snn_Insight.Skip(index * pageSize).Take(pageSize).OrderByDescending(s => s.id);
            myResponse.data = lib;
            myResponse.count = myList.Count();
            myResponse.pageIndex = index;
            myResponse.pageSize = pageSize;
            myResponse.data = myList.ToList();
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
            if (!isAdmin()) { return standardMessages.unauthorized; }
            lib.platformDB.snn_Insight.Add(insight);
            lib.platformDB.SaveChanges();
            myResponse = standardMessages.saved;
            myResponse.data = insight;
            return myResponse;
        }
        #endregion


        bool isAdmin() {
            if (HttpContext.Request.Headers != null && HttpContext.Request.Headers.ContainsKey("Authorization")) {
                //Temporarily
                return HttpContext.Request.Headers["Authorization"] == "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9leHBpcmF0aW9uIjoiOS8xNi8yMDIxIDQ6MDY6NDEgQU0iLCJyb2xlIjoiMC0xIiwibmJmIjoxNjMxNzM2NDAxLCJleHAiOjE2MzE3Nzk2MDEsImlhdCI6MTYzMTczNjQwMX0.IWs7Dn-k0KaH08BQ4_FJKyUyVbx5nqkku0vNr6YqW1luVl0iuifGbWTht1rSdc9_KRU9x9TDY74kyPttZxj2vQ";
            }
            return false;
        }

    }

}
