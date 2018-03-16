using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFA.Web.Models.Entities
{
    public class UnreadNewsPostNotice
    {
        /**********************************************************************/
        #region Properties

        [Required]
        [ForeignKey(nameof(NewsPost))]
        public int NewsPostId { get; set; }
        public NewsPost NewsPost { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        #endregion Properties
    }
}
