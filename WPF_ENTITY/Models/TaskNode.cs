using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPF_ENTITY.Models
{
    public class TaskNode
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Column("DateStart", TypeName = "DateTime2")]
        public DateTime? DateStart { get; set; }
        [Column("DateFinish", TypeName = "DateTime2")]
        public DateTime? DateFinish { get; set; }
        [Column("DateCreate", TypeName = "DateTime2")]
        public DateTime? DateCreate { get; set; }     
       
        [DefaultValue(false)]
        [Column("IsStart", TypeName = "bit")]
        public bool IsStart { get; set; }

        [DefaultValue(false)]
        [Column("IsFinish", TypeName = "bit")]
        public bool IsFinish { get; set; }
        [DefaultValue(false)]
        [Column("IsProgress", TypeName = "bit")]
        public bool IsProgress { get; set; }

    }
}
