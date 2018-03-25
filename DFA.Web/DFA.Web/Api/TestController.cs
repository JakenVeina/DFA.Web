using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using DFA.Common.Extensions;

using DFA.Web.Api.Events;
using DFA.Web.Models;
using DFA.Web.Models.Entities;
using DFA.Web.Models.ViewModels;
using DFA.Web.Filters;
using DFA.Web.Services.Interfaces;

namespace DFA.Web.Api
{
    public class ViewModelA
    {
        public string PropertyA { get; set; }
    }

    public class ViewModelB
    {
        public string PropertyB { get; set; }
    }

    public class ViewModelAB
    {
        public ViewModelA ViewModelA { get; set; }
        public ViewModelB ViewModelB { get; set; }
    }

    public class TestController : ApiControllerBase
    {
        /**********************************************************************/
        #region Constructors

        public TestController(
            DFAWebDataContext dataContext,
            IApiEventsService apiEventsService,
            IDeferredActionService deferredActionService,
            ILoggingService loggingService)
            : base(dataContext, apiEventsService, deferredActionService, loggingService)
        {
        }

        #endregion Constructors

        /**********************************************************************/
        #region Methods

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var x = ControllerContext.ActionDescriptor.AttributeRouteInfo.Template;

            return Ok();
        }

        [HttpPut]
        [AllowAnonymous]
        public IActionResult Put()
        {
            return Ok();
        }

        [HttpPost("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(int id)
        {
            RaiseApiEvent("somethinghappened", new { id = id });
            RaiseApiEvent($"{id}/somethinghappened", new { id = id });

            return Ok();
        }

        [HttpDelete]
        [AllowAnonymous]
        public IActionResult Delete()
        {
            return Ok();
        }

        [HttpPost("somethinghappened/subscribe")]
        [HttpPost("{id}/somethinghappened/subscribe")]
        [AllowAnonymous]
        public Task<IActionResult> Subscribe([FromBody] ApiEventsSubscriptionRequest request)
            => AddApiEventSubscription(request);

        [HttpPost("somethinghappened/unsubscribe")]
        [HttpPost("{id}/somethinghappened/unsubscribe")]
        [AllowAnonymous]
        public Task<IActionResult> Unsubscribe([FromBody] ApiEventsSubscriptionRequest request)
            => RemoveApiEventSubscription(request);

        #endregion Methods
    }
}