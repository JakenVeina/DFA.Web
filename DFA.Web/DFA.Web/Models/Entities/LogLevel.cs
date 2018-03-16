using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace DFA.Web.Models.Entities
{
    public class LogLevel : EntityBase
    {
        /**********************************************************************/
        #region Properties

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        #endregion Properties
    }
}
