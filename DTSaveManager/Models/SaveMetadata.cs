using System.Collections.Generic;
using System.Linq;

namespace DTSaveManager.Models
{
    public class SaveMetadata
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public bool Active { get; set; }
    }
}
