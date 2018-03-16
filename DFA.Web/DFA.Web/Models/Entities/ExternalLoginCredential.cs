using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Models.Entities
{
    public class ExternalLoginCredential
    {
        /**********************************************************************/
        #region Properties

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [ForeignKey(nameof(Policy))]
        public int PolicyId { get; set; }
        public ExternalLoginPolicy Policy { get; set; }

        [Required]
        public string Subject { get; set; }

        #endregion Properties
    }
}
