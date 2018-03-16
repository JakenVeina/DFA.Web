//using System.Diagnostics.Contracts;
//using System.Threading.Tasks;

//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//using DFA.Web.Mapping;
//using DFA.Web.Models;
//using DFA.Web.Models.ViewModels;

//namespace DFA.Web.Api.User
//{
//    public class UserController : ApiControllerBase
//    {
//        /**********************************************************************/
//        #region Constructors

//        public UserController(UserManager<User> userManager)
//        {
//            Contract.Requires(userManager != null);

//            _userManager = userManager;

//            Contract.Ensures(_userManager != null);
//        }

//        #endregion Constructors

//        /**********************************************************************/
//        #region Methods

//        [HttpGet]
//        public async Task<IActionResult> Get()
//            => Ok((await _userManager.GetUserAsync(User)).MapTo<UserViewModel>());

//        [HttpGet("[action]")]
//        public async Task<IActionResult> Detail()
//            => Ok((await _userManager.GetUserAsync(User)).MapTo<UserDetailViewModel>());

//        #endregion Methods

//        /**********************************************************************/
//        #region Private Fields

//        private readonly UserManager<User> _userManager;

//        #endregion Private Fields
//    }
//}
