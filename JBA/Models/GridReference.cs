using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JBA.Models
{
    public class GridReference
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GridReferenceID { get; set; }

        [Required]
        [RegularExpression(@"^[\d]+")]
        public int XRef { get; set; }

        [Required]
        [RegularExpression(@"^[\d]+")]
        public int YRef { get; set; }

        public virtual DataCollection DataCollection { get; set; }
        public virtual ICollection<GridReferenceData> GridReferenceDataCollection { get; set; }
    }
}