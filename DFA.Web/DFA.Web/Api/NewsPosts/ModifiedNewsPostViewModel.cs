using System.ComponentModel.DataAnnotations;

using DFA.Web.Mapping;
using DFA.Web.Models.Entities;

namespace DFA.Web.Api.NewsPosts
{
    [MapsTo(typeof(NewsPost))]
    public class ModifiedNewsPostViewModel
    {
        /**********************************************************************/
        #region Properties

        [Required]
        [MinLength(1)]
        public string Message { get; set; }

        #endregion Properties
    }
}
