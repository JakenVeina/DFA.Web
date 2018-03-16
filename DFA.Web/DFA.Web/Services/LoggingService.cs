using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using DFA.Web.Authentication;
using DFA.Web.Models;
using DFA.Web.Models.Entities;
using DFA.Web.Services.Interfaces;

namespace DFA.Web.Services
{
    public class LoggingService : ILoggingService
    {
        /**********************************************************************/
        #region Constructors

        public LoggingService(DFAWebDataContext dataContext, IHttpContextAccessor httpContextAccessor, IAppAuthenticationService appAuthenticationService)
        {
            Contract.Requires(dataContext != null);
            Contract.Requires(httpContextAccessor != null);
            Contract.Requires(appAuthenticationService != null);

            _dataContext = dataContext;
            _httpContext = httpContextAccessor.HttpContext;
            _appAuthenticationService = appAuthenticationService;

            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };
        }

        #endregion Constructors

        /**********************************************************************/
        #region ILoggingService

        public async Task<LogEntry> CreateEntry(LogLevelType logLevelType, string actionName, object data = null)
        {
            var logEntry = (await _dataContext.LogEntries.AddAsync(new LogEntry()
               {
                   Created = DateTime.UtcNow,
                   UserId = _appAuthenticationService.CurrentUserClaims?.Id,
                   Level = await GetLogLevel(logLevelType),
                   Action = await GetLogAction(actionName),
                   SourceAddress = _httpContext.Connection.RemoteIpAddress.ToString(),
                   Data = JsonConvert.SerializeObject(data, _jsonSerializerSettings)
               })).Entity;

            await _dataContext.SaveChangesAsync();

            return logEntry;
        }

        #endregion ILoggingService

        /**********************************************************************/
        #region Private Methods

        private async Task<LogLevel> GetLogLevel(LogLevelType logLevelType)
        {
            if (_logLevelsByName == null)
                _logLevelsByName = await _dataContext
                    .LogLevels
                    .ToDictionaryAsync(x => x.Name);

            var loglLevelName = logLevelType.ToString();
            if (!_logLevelsByName.TryGetValue(loglLevelName, out var logLevel))
                await _dataContext.LogLevels.AddAsync(logLevel = new LogLevel()
                {
                    Name = loglLevelName
                });

            return logLevel;
        }

        private async Task<LogAction> GetLogAction(string actionName)
        {
            Contract.Requires(actionName != null);

            if (_logActionsByName == null)
                _logActionsByName = await _dataContext
                    .LogActions
                    .ToDictionaryAsync(x => x.Name);
            if (!_logActionsByName.TryGetValue(actionName, out var logAction))
                await _dataContext.LogActions.AddAsync(logAction = new LogAction()
                {
                    Name = actionName
                });

            return logAction;
        }

        #endregion Private Methods

        /**********************************************************************/
        #region Private Fields

        private readonly DFAWebDataContext _dataContext;

        private readonly HttpContext _httpContext;

        private readonly IAppAuthenticationService _appAuthenticationService;

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        private Dictionary<string, LogLevel> _logLevelsByName;

        private Dictionary<string, LogAction> _logActionsByName;

        #endregion Private Fields

        /**********************************************************************/
        #region Private Types

        private struct LogEntryModel
        {
            public DateTime Created { get; set; }
            public string LevelName { get; set; }
            public string ActionName { get; set; }
            public int? UserId { get; set; }
            public IPAddress SourceAddress { get; set; }
            public object Data { get; set; }
        }

        #endregion Private Types
    }
}
