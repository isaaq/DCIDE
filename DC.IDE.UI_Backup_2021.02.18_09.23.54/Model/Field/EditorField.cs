using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class EditorField : FieldItem
    {
        public int Style { get; set; }
        public string DefaultValue { get; set; }
        public int DefaultHeight { get; set; }
        /// <summary>
        /// 是否启用关联链接
        /// </summary>
        public bool IsOpenAssociationLink { get; set; }
        public bool IsSaveRemoteImage { get; set; }

    }
}
