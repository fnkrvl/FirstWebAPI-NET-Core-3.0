using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebAPI.Helpers
{
    public class FiltroDeAccion : IActionFilter
    {
        private readonly ILogger<FiltroDeAccion> logger;
        public FiltroDeAccion(ILogger<FiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogError("OnActionExecuted");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogError("OnActionExecuting");
        }
    }
}
