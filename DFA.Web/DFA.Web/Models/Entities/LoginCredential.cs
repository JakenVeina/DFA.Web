using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFA.Web.Models.Entities
{
    public class LoginCredential : EntityBase
    {
        /**********************************************************************/
        #region Properties

        [Required]
        public string Algorithm { get; set; }

        [Required]
        public int Iterations { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string Hash { get; set; }

        [Required]
        public bool IsEMailAddressVerified { get; set; }

        [Required]
        public int SuccessiveFailedLoginCount { get; set; }

        public DateTime? LockoutEnd { get; set; }

        #endregion Properties
    }
}
