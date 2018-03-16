using DFA.Web.Mapping;
using DFA.Web.Models.Entities;

namespace DFA.Web.Api.User
{
    [MapsFrom(typeof(Models.Entities.User))]
    public class UserViewModel
    {
        /**********************************************************************/
        #region Properties

        public int Id { get; set; }

        public string UserName { get; set; }

        #endregion Properties
    }
}
