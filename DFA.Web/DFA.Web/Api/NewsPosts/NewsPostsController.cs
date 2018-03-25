using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AutoMapper.QueryableExtensions;

using DFA.Web.Api.Events;
using DFA.Web.Mapping;
using DFA.Web.Models;
using DFA.Web.Models.Criteria;
using DFA.Web.Models.Entities;
using DFA.Web.Models.Requests;
using DFA.Web.Models.ViewModels;
using DFA.Web.Services.Interfaces;

namespace DFA.Web.Api.NewsPosts
{
    public class NewsPostsController : ApiControllerBase
    {
        /**********************************************************************/
        #region Constructors

        public NewsPostsController(
            DFAWebDataContext dataContext,
            IApiEventsService apiEventsService,
            IDeferredActionService deferredActionService,
            ILoggingService loggingService,
            IAppAuthenticationService appAuthenticationService)
            : base(dataContext, apiEventsService, deferredActionService, loggingService)
        {
            Contract.Requires(appAuthenticationService != null);

            AppAuthenticationService = appAuthenticationService;
        }

        #endregion Constructors

        /**********************************************************************/
        #region Methods

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] DataPageRequest request)
        {
            Contract.Requires(request != null);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.LastRowIndex > 10)
                request.LastRowIndex = 10;

            var userId = AppAuthenticationService.CurrentUserClaims?.Id;

            var viewModelBuilder = new DataPageViewModelBuilder<NewsPostViewModel>()
            {
                Query = DataContext.NewsPosts
                    .Include(x => x.UnreadNewsPostNotices
                        .Where(y => y.UserId == userId))
                    .ProjectTo<NewsPostViewModel>(),
                Sort = query => query.OrderByDescending(x => x.Created)
            };

            return Ok(await viewModelBuilder.Build(request));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromRoute] EntityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = AppAuthenticationService.CurrentUserClaims?.Id;

            var newsPost = await DataContext.NewsPosts
                .Include(x => x.UnreadNewsPostNotices
                    .Where(y => y.UserId == userId))
                .ProjectTo<NewsPostViewModel>()
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (newsPost == null)
                return NotFound();

            return Ok(newsPost);
        }

        [HttpPut]
        [Authorize(Roles = nameof(AppRoles.AppAdministrator))]
        public async Task<IActionResult> Put([FromBody] ModifiedNewsPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newsPost = viewModel.MapTo<NewsPost>();
            newsPost.CreatedById = AppAuthenticationService.CurrentUserClaims.Id;

            await DataContext.NewsPosts.AddAsync(newsPost);

            var unreadNewsPostNotices = await DataContext
                .Users
                .Select(x => new UnreadNewsPostNotice()
                {
                    UserId = x.Id,
                    NewsPostId = newsPost.Id
                })
                .ToArrayAsync();

            await DataContext.UnreadNewsPostNotices.AddRangeAsync(unreadNewsPostNotices);
            newsPost.UnreadNewsPostNotices = unreadNewsPostNotices;

            await DataContext.SaveChangesAsync();

            RaiseApiEvent("created", newsPost.MapTo<NewsPostEventViewModel>());

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(AppRoles.AppAdministrator))]
        public async Task<IActionResult> Put([FromRoute] EntityRequest request, [FromBody] ModifiedNewsPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newsPost = await DataContext.NewsPosts
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (newsPost == null)
                return NotFound();

            newsPost.MapFrom(viewModel);

            DataContext.NewsPosts.Update(newsPost);
            DataContext.SaveChanges();

            var newsPostEventViewModel = newsPost.MapTo<NewsPostEventViewModel>();
            RaiseApiEvent("modified", newsPostEventViewModel);
            RaiseApiEvent($"{request.Id}/modified", newsPostEventViewModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(AppRoles.AppAdministrator))]
        public async Task<IActionResult> Delete([FromRoute] EntityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newsPost = await DataContext.NewsPosts
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (newsPost == null)
                return NotFound();

            DataContext.NewsPosts.Remove(newsPost);
            await DataContext.SaveChangesAsync();

            var newsPostEventViewModel = newsPost.MapTo<NewsPostEventViewModel>();
            RaiseApiEvent("modified", newsPostEventViewModel);
            RaiseApiEvent($"{request.Id}/modified", newsPostEventViewModel);

            return Ok();
        }

        [HttpGet("unread")]
        [AllowAnonymous]
        public async Task<IActionResult> Unread([FromQuery] DataPageRequest request)
        {
            Contract.Requires(request != null);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.PageSize > 10)
                request.PageSize = 10;

            var viewModelBuilder = new DataPageViewModelBuilder<NewsPostViewModel>();

            if (AppAuthenticationService.CurrentUserClaims != null)
            {
                viewModelBuilder.Query = DataContext.NewsPosts
                    .Include(x => x.UnreadNewsPostNotices
                        .Where(y => y.UserId == AppAuthenticationService.CurrentUserClaims.Id))
                    .ProjectTo<NewsPostViewModel>();

                viewModelBuilder.Sort = query => query.OrderByDescending(x => x.Created);
            }

            return Ok(viewModelBuilder.Build(request));
        }

        [HttpGet("unread/count")]
        [AllowAnonymous]
        public async Task<IActionResult> UnreadCount()
            => Ok(new CountViewModel()
            {
                Count = (AppAuthenticationService.CurrentUserClaims == null)
                    ? 0
                    : await DataContext.UnreadNewsPostNotices
                        .Where(x => x.UserId == AppAuthenticationService.CurrentUserClaims.Id)
                        .Select(x => x.NewsPost)
                        .CountAsync()
            });

        [HttpPost("created/subscribe")]
        [HttpPost("modified/subscribe")]
        [HttpPost("deleted/subscribe")]
        [HttpPost("{id}/modified/subscribe")]
        [HttpPost("{id}/deleted/subscribe")]
        [AllowAnonymous]
        public Task<IActionResult> Subscribe([FromBody] ApiEventsSubscriptionRequest request)
            => AddApiEventSubscription(request);

        [HttpPost("created/unsubscribe")]
        [HttpPost("modified/unsubscribe")]
        [HttpPost("deleted/unsubscribe")]
        [HttpPost("{id}/modified/unsubscribe")]
        [HttpPost("{id}/deleted/unsubscribe")]
        [AllowAnonymous]
        public Task<IActionResult> Unsubscribe([FromBody] ApiEventsSubscriptionRequest request)
            => RemoveApiEventSubscription(request);

        #endregion Methods

        /**********************************************************************/
        #region Protected Properties

        internal protected IAppAuthenticationService AppAuthenticationService { get; }

        #endregion Protected Properties
    }
}