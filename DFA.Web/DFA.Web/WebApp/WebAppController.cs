using System;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DFA.Web.WebApp
{
    [Route("")]
    public class WebAppController : Controller
    {
        [HttpGet]
        public IActionResult Index()
            => View();

        [HttpGet("[action]")]
        public IActionResult Login([FromBody] string returnUrl)
            => View();
    }
}
