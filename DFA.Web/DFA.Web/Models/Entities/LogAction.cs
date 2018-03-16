using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Models.Entities
{
    public class LogAction : EntityBase
    {
        /**********************************************************************/
        #region Properties

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        #endregion Properties
    }
}
