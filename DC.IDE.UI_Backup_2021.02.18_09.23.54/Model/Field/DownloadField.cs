using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class DownloadField : FieldItem
    {
        public string AllowedTypes { get; set; }
        public int UploadNum { get; set; }
        public int DownloadType { get; set; }

    }
}
