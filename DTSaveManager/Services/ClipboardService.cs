using DTSaveManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DTSaveManager.Services
{
    class ClipboardService : IClipboardService
    {
        public void SetText(string value)
        {
            Clipboard.SetText(value);
        }
    }
}
