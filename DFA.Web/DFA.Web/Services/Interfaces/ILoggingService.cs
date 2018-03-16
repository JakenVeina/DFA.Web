using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using DFA.Web.Models;
using DFA.Web.Models.Entities;

namespace DFA.Web.Services.Interfaces
{
    public interface ILoggingService
    {
        /**********************************************************************/
        #region Methods

        Task<LogEntry> CreateEntry(LogLevelType logLevelType, string actionName, object data = null);

        #endregion Methods
    }
}
