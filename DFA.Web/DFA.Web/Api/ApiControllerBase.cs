using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

using DFA.Common.Extensions;

using DFA.Web.Api.Events;
using DFA.Web.Filters;
using DFA.Web.Models;
using DFA.Web.Models.Entities;
using DFA.Web.Services.Interfaces;

namespace DFA.Web.Api
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExceptionHandler]
    [RequireHttps]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
        /**********************************************************************/
        #region Constructors

        public ApiControllerBase(DFAWebDataContext dataContext, IApiEventsService apiEventsService, IDeferredActionService deferredActionService, ILoggingService loggingService)
        {
            Contract.Requires(dataContext != null);
            Contract.Requires(apiEventsService != null);
            Contract.Requires(deferredActionService != null);
            Contract.Requires(loggingService != null);

            DataContext = dataContext;
            ApiEventsService = apiEventsService;
            DeferredActionService = deferredActionService;
            LoggingService = loggingService;
        }

        #endregion Constructors

        /**********************************************************************/
        #region Protected Properties

        private static string _eventPrefix;
        internal protected virtual string EventPrefix
        {
            get
            {
                if(_eventPrefix == null)
                    _eventPrefix = GetType()
                        .GetCustomAttributes(true)
                        .OfType<RouteAttribute>()
                        .First()
                        .Template;

                return _eventPrefix;
            }
        }


        internal protected DFAWebDataContext DataContext { get; }

        internal protected IApiEventsService ApiEventsService { get; }

        internal protected IDeferredActionService DeferredActionService { get; }

        internal protected ILoggingService LoggingService { get; }

        #endregion Protected Properties

        /**********************************************************************/
        #region Protected Methods

        internal protected void RaiseApiEvent(string name, object data)
            => DeferredActionService.AddAction(() =>
                ApiEventsService.RaiseEvent(
                      AttributeRouteModel.ReplaceTokens(
                          AttributeRouteModel.CombineTemplates(EventPrefix, name),
                          ControllerContext.ActionDescriptor.RouteValues),
                      data));

        internal protected async Task<IActionResult> AddApiEventSubscription(ApiEventsSubscriptionRequest request, string suffix = "/subscribe")
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var name = HttpContext.Request.Path.Value;

            Contract.Requires(name.EndsWith(suffix));

            name = name.Substring(0, (name.Length - suffix.Length));
            if (name.StartsWith('/'))
                name = name.Substring(1);

            if (!(await ApiEventsService.AddSubscription(request.SubscriptionToken, name)))
                return BadRequest(new InvalidSubscriptionTokenViewModel(request.SubscriptionToken));

            return Ok();
        }

        internal protected async Task<IActionResult> RemoveApiEventSubscription(ApiEventsSubscriptionRequest request, string suffix = "/unsubscribe")
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var name = HttpContext.Request.Path.Value;

            Contract.Requires(name.EndsWith(suffix));

            name = name.Substring(0, (name.Length - suffix.Length));
            if (name.StartsWith('/'))
                name = name.Substring(1);

            if (!(await ApiEventsService.RemoveSubscription(request.SubscriptionToken, name)))
                return BadRequest(new InvalidSubscriptionTokenViewModel(request.SubscriptionToken));

            return Ok();
        }

        internal protected virtual IActionResult Forbid(object error)
            => new ForbidObjectResult(error);

        internal protected virtual IActionResult BadGateway(object error)
            => new BadGatewayObjectResult(error);

        internal protected virtual IActionResult InternalServerError(object error)
            => new InternalServerErrorResult(error);

        internal protected void DeferEntityUpdate<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression) where TEntity : class
            => DeferDataContextAction(async dataContext
                => dataContext.UpdateEntityProperty(entity, propertyExpression));

        internal protected void DeferDataContextAction(Func<DFAWebDataContext, Task> action)
            => DeferredActionService.AddAction<DFAWebDataContext>(dataContext
                => action.Invoke(dataContext));

        internal protected Task<LogEntry> CreateLogEntry(LogLevelType logLevelType, string actionName, object data = null, [CallerMemberName] string callerName = null)
            => LoggingService.CreateEntry(logLevelType, GetFullActionName(actionName, callerName), data);

        internal protected void DeferLogEntry(LogLevelType logLevelType, string actionName, object data = null, [CallerMemberName] string callerName = null)
            => DeferredActionService.AddAction<ILoggingService>(loggingService
                => loggingService.CreateEntry(logLevelType, GetFullActionName(actionName, callerName), data));

        #endregion Protected Methods

        /**********************************************************************/
        #region Private Methods

        private string GetFullActionName(string actionName, string callerName)
            => $"{GetType().Name}.{callerName}.{actionName}";

        #endregion Private Methods
    }
}