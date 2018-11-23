using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JBA.Models
{
    public class FileHeader
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int HeaderID { get; set; }

        public string Header { get; set; }
        public string HeaderRegex { get; set; }




    }
}