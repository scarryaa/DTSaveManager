using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTSaveManager.Services
{
    static class NavigationService
    {
        public static void Navigate(string website)
        {
            System.Diagnostics.Process.Start(website);
        }
    }
}
