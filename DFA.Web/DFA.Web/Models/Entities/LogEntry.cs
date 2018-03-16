using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFA.Web.Models.Entities
{
    public class LogEntry : EntityBase
    {
        /**********************************************************************/
        #region Properties

        [Required]
        public DateTime Created { get; set; }

        [Required]
        [ForeignKey(nameof(Level))]
        public int LevelId { get; set; }
        public LogLevel Level { get; set; }

        [Required]
        [ForeignKey(nameof(Action))]
        public int ActionId { get; set; }
        public LogAction Action { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int? UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string SourceAddress { get; set; }

        public string Data { get; set; }

        #endregion Properties
    }
}
