using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JBA.Models
{
    public class GridReferenceData
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GridReferenceDataID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Value { get; set; }

        public virtual GridReference GridReference { get; set; }
    }
}