using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Models.Responses
{
    public abstract class ResponseBase
    {
        /**********************************************************************/
        #region Methods

        public virtual void Validate() { }

        #endregion Methods
    }
}
