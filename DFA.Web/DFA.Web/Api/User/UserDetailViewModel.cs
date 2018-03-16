using System;
using System.ComponentModel.DataAnnotations;

using DFA.Web.Mapping;
using DFA.Web.Models.Entities;

namespace DFA.Web.Api.User
{
    [MapsToAndFromAttribute(typeof(Models.Entities.User))]
    public class UserDetailViewModel
    {
        /**********************************************************************/
        #region Properties

        [Required]
        public string UserName { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime LastActive { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        #endregion Properties
    }
}
