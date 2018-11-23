using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JBA.Models
{
    public class DataCollection
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DataCollectionID { get; set; }

        [Required]
        public string Description { get; set; }

        public byte[] CollectionFile { get; set; }

        public virtual ICollection<GridReference> GridReferences { get; set; }
    }
}