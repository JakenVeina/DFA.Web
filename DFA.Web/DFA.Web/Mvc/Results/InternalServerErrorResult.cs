using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Mvc
{
    public class InternalServerErrorResult : ObjectResult
    {
        /**********************************************************************/
        #region Constructors

        public InternalServerErrorResult(object value)
            : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }

        #endregion Constructors
    }
}
