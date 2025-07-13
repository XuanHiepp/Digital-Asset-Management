using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database
{
    public class MusicAsset
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }

        /// <summary>
        /// URI to get this resource (from a remote storate, e.g., Azure Blob Storage).
        /// </summary>
        public string Uri { get; set; }
    }
}
