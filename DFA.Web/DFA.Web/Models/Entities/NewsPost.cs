using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFA.Web.Models.Entities
{
    public class NewsPost : EntityBase
    {
        /**********************************************************************/
        #region Properties

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        [Required]
        [ForeignKey(nameof(CreatedBy))]
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public ICollection<UnreadNewsPostNotice> UnreadNewsPostNotices { get; set; }

        #endregion Properties
    }
}
