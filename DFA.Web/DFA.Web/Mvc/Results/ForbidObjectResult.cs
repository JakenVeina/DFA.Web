using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Mvc
{
    public class ForbidObjectResult : ObjectResult
    {
        /**********************************************************************/
        #region Constructors

        public ForbidObjectResult(object value)
            : base(value)
        {
            StatusCode = StatusCodes.Status403Forbidden;
        }

        #endregion Constructors
    }
}
