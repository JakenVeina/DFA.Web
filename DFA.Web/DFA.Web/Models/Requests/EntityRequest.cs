using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using DFA.Web.Models.Validation;

namespace DFA.Web.Models.Requests
{
    public class EntityRequest
    {
        /**********************************************************************/
        #region Properties

        [EntityId]
        public int Id { get; set; }

        #endregion Properties
    }
}
