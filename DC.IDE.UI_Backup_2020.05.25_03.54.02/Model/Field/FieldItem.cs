using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public abstract class FieldItem
    {
        /// <summary>
        /// 排序序号
        /// </summary>
        public int SortNum { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public FieldType Type { get; set; }
        public bool IsSystem { get; set; }
        public bool IsRequired { get; set; }
        public bool IsSearchable { get; set; }
        public bool IsSortable { get; set; }

        public string FormAdditionalAttr{ get; set; }
        public string FormCSS { get; set; }

        /// <summary>
        /// 字段长度取值范围
        /// </summary>
        public FieldLength LengthRange { get; set; }
        /// <summary>
        /// 正则校验
        /// </summary>
        public string Regex { get; set; }
        /// <summary>
        /// 正则校验未通过的提示信息
        /// </summary>
        public string RegexErrorMsg { get; set; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool IsUnique { get; set; }
        /// <summary>
        /// 是否作为基本信息(添加页面显示)
        /// </summary>
        public bool IsCommonInfo { get; set; }
        ///// <summary>
        ///// 是否正文（投稿）显示（详细页面显示）
        ///// </summary>
        //public bool IsFrontInfo { get; set; }
        /// <summary>
        /// 作为全站搜索
        /// </summary>
        public bool IsGloalSearch { get; set; }
        /// <summary>
        /// 作为万能字段的附属字段
        /// </summary>
        public bool IsWithOmni { get; set; }
        public List<int> DenyGroupList { get; set; }
        public List<int> DenyRoleList { get; set; }
    }
}
