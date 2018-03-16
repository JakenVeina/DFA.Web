using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using DFA.Web.Services.Interfaces;

namespace DFA.Web.Services
{
    public class DeferredActionService : IDeferredActionService
    {
        /**********************************************************************/
        #region Constructors

        public DeferredActionService(IHttpContextAccessor httpContextAccessor, IAppAuthenticationService appAuthenticationService, IServiceScopeFactory serviceScopeFactory)
        {
            Contract.Requires(httpContextAccessor != null);
            Contract.Requires(appAuthenticationService != null);
            Contract.Requires(serviceScopeFactory != null);

            _httpContextAccessor = httpContextAccessor;
            _appAuthenticationService = appAuthenticationService;
            _serviceScopeFactory = serviceScopeFactory;

            _httpContextAccessor.HttpContext.Response.OnCompleted(OnHttpResponseCompleted);
        }

        #endregion Constructors

        /**********************************************************************/
        #region IDeferredAsyncActionService

        public void AddAction(Action action)
        {
            Contract.Requires(action != null);

            _actions.Add(serviceProvider =>
            {
                action.Invoke();
                return Task.CompletedTask;
            });
        }

        public void AddAction<TService>(Action<TService> action)
        {
            Contract.Requires(action != null);

            _actions.Add(serviceProvider =>
            {
                action.Invoke(serviceProvider.GetRequiredService<TService>());
                return Task.CompletedTask;
            });
        }

        public void AddAction<TService1, TService2>(Action<TService1, TService2> action)
        {
            Contract.Requires(action != null);

            _actions.Add(serviceProvider =>
            {
                action.Invoke(
                    serviceProvider.GetRequiredService<TService1>(),
                    serviceProvider.GetRequiredService<TService2>());
                return Task.CompletedTask;
            });
        }

        public void AddAction(Func<Task> action)
        {
            Contract.Requires(action != null);

            _actions.Add(serviceProvider =>
                action.Invoke());
        }

        public void AddAction<TService>(Func<TService, Task> action)
        {
            Contract.Requires(action != null);

            _actions.Add(serviceProvider =>
                action.Invoke(serviceProvider.GetRequiredService<TService>()));
        }

        public void AddAction<TService1, TService2>(Func<TService1, TService2, Task> action)
        {
            Contract.Requires(action != null);

            _actions.Add(serviceProvider =>
                action.Invoke(
                    serviceProvider.GetRequiredService<TService1>(),
                    serviceProvider.GetRequiredService<TService2>()));
        }

        #endregion IDeferredAsyncActionService

        /**********************************************************************/
        #region Private Methods

        private Task OnHttpResponseCompleted()
        {
            // TODO: Remove workaround for https://github.com/aspnet/KestrelHttpServer/issues/2035
            // HttpResponse.OnCompleted (for Kestrel) is currently bugged and doesn't run after the response has been sent.
            // To ensure that actions are invoked AFTER the response, we have to push them to another thread.
            // This also means we need to create a new service scope, as the current one will probably be disposed before the actions complete.
            // We also need to manually transfer relevant data from the current scope into the new scope.
            // This is hacky as fuck, so please change as soon as the above bug is fixed.
            Task.Run(async () =>
            {
                var serviceProvider = _serviceScopeFactory.CreateScope().ServiceProvider;

                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                httpContextAccessor.HttpContext = _httpContextAccessor.HttpContext;

                var appAuthenticationService = serviceProvider.GetRequiredService<IAppAuthenticationService>();
                appAuthenticationService.CurrentUserClaims = _appAuthenticationService.CurrentUserClaims;

                foreach (var action in _actions)
                {
                    try
                    {
                        await action.Invoke(serviceProvider);
                    }
                    catch(Exception ex)
                    {
                        var loggingService = serviceProvider.GetRequiredService<ILoggingService>();
                        await loggingService.CreateEntry(Models.LogLevelType.Error, "DeferredActionException", ex);
                    }
                }
            });

            return Task.CompletedTask;
        }

        #endregion Private Methods

        /**********************************************************************/
        #region Private Fields

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IAppAuthenticationService _appAuthenticationService;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly List<Func<IServiceProvider, Task>> _actions
            = new List<Func<IServiceProvider, Task>>();

        #endregion Private Fields
    }
}
