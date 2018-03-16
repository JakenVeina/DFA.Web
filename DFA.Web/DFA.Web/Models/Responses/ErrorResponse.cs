using System.Diagnostics.Contracts;

namespace DFA.Web.Models.Responses
{
    public class ErrorResponse : ResponseBase
    {
        /**********************************************************************/
        #region Properties

        public string Error { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region ResponseBase

        public override void Validate()
        {
            Contract.Requires(Error != null);

            base.Validate();
        }

        #endregion ResponseBase
    }
}
