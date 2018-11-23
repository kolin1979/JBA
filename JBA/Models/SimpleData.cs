using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JBA.Models
{
    public class SimpleData
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DataID { get; set; }

        [RegularExpression(@"^[\d]+")]
        public int XRef { get; set; }

        [RegularExpression(@"^[\d]+")]
        public int YRef { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int Value { get; set; }      
    }
}