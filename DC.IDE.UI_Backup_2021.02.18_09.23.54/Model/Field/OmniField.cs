using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class OmniField : FieldItem
    {
        /// <summary>
        /// 表单	
        /// 例如：<input type = 'text' name='info[voteid]' id='voteid' value='{FIELD_VALUE}' style='50' >
        /// </summary>
        public string ElementHtmlTemplate { get; set; }

        public int FieldType { get; set; }

    }
}
