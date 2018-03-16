using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using DFA.Common.Extensions;
using DFA.Web.Authentication;
using DFA.Web.Models;
using DFA.Web.Models.Entities;
using DFA.Web.Services.Interfaces;

namespace DFA.Web.Filters
{
    public class ApiExceptionHandler : ExceptionFilterAttribute
    {
        /**********************************************************************/
        #region ExceptionFilterAttribute

        public override void OnException(ExceptionContext context)
        {
            OnExceptionAsync(context).GetAwaiter().GetResult();
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var loggingService = context.HttpContext.RequestServices.GetRequiredService<ILoggingService>();

            var logEntry = await loggingService.CreateEntry(LogLevelType.Error, "ApiException", context.Exception);

#if DEBUG
            var resultDetails = new
            {
                Response = new ExceptionViewModel()
                {
                    LogEntryId = logEntry.Id
                },
                Exception = context.Exception
            };
#else
            var resultObject = new ExceptionViewModel()
            {
                LogEntryId = logEntry.Id
            };
#endif
            context.Result = new InternalServerErrorResult(resultDetails);

            context.ExceptionHandled = true;
        }

        #endregion ExceptionFilterAttribute
    }
}
