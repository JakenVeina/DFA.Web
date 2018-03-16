using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFA.Web.Models.Entities
{
    public class User : EntityBase
    {
        /**********************************************************************/
        #region Properties

        [Required]
        public string Name { get; set; }

        [Required]
        [ForeignKey(nameof(LoginCredential))]
        public int? LoginCredentialId { get; set; }

        public LoginCredential LoginCredential { get; set; }

        public ICollection<ExternalLoginCredential> ExternalLoginCredentials { get; set; }

        [Required]
        public int LoginNonce { get; set; }

        public ICollection<UserRoleMap> UserRoleMaps { get; set; }

        [Required]
        public string EMailAddress { get; set; }

        [Required]
        public bool IsEMailAddressPublic { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset Created { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset LastActive { get; set; }

        #endregion Properties
    }
}
