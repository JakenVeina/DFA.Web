using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFA.Web.Models.Entities
{
    public class Role : EntityBase
    {
        /**********************************************************************/
        #region Properties

        [Required]
        public string Name { get; set; }

        public ICollection<UserRoleMap> UserRoleMaps { get; set; }

        #endregion Properties
    }
}
