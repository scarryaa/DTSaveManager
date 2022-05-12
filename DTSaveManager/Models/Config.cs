using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTSaveManager.Models
{
    public class Config
    {
        public string DemonTurfDirectory { get; set; }
        public string NeonSplashDirectory { get; set; }
        public bool? NeonSplashDisabled { get; set; }
        public string ActiveTheme { get; set; }
        public string DtActiveFile { get; set; }
        public string NeonSplashActiveFile { get; set; }
        public List<string> LockedFiles { get; set; }
    }
}
