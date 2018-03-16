using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Mvc
{
    public class BadGatewayObjectResult : ObjectResult
    {
        /**********************************************************************/
        #region Constructors

        public BadGatewayObjectResult(object value)
            : base(value)
        {
            StatusCode = StatusCodes.Status502BadGateway;
        }

        #endregion Constructors
    }
}
