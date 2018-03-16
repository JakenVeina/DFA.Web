using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Mvc
{
    public class BadGatewayResult : StatusCodeResult
    {
        /**********************************************************************/
        #region Constructors

        public BadGatewayResult()
            : base(StatusCodes.Status502BadGateway) { }

        #endregion Constructors
    }
}
