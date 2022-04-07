using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DC.IDE.UI.Converter;
using DC.IDE.UI.UC.Properties;

using PostSharp.Patterns.Model;

using Telerik.Windows.Controls;

namespace DC.IDE.UI.Model.Field
{
    [NotifyPropertyChanged]
    public abstract class FieldItem : ViewModelBase
    {
        /// <summary>
        /// Id
        /// </summary>
        //[Display(Name = "Id", GroupName = "基本设置")]
        //public Guid Id { get; set; }
        /// <summary>
        /// 排序序号
        /// </summary>
        [Display(Name = "排序序号", GroupName = "基本设置")]
        public int SortNum { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        [Display(Name = "名称", GroupName = "基本设置")]
        public string Name { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        [Display(Name = "显示名称", GroupName = "基本设置")]
        public string DisplayName { get; set; }
        /// <summary>
        /// 字段描述
        /// </summary>
        [Display(Name = "字段描述", GroupName = "基本设置")]
        public string Description { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        //[TypeConverter(typeof(FieldTypeToNameConverter))]
        [Display(Name = "字段类型", GroupName = "基本设置")]
        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(UCFieldTypeDropDownSelector), "SelectedItem", Telerik.Windows.Controls.Data.PropertyGrid.EditorStyle.DropDown)]
        public FieldType Type { get; set; }
        [Display(Name = "是否为系统字段", GroupName = "基本设置")]
        public bool IsSystem { get; set; }
        [Display(Name = "是否必填字段", GroupName = "基本设置")]
        public bool IsRequired { get; set; }
        [Display(Name = "是否可搜索字段", GroupName = "基本设置")]
        public bool IsSearchable { get; set; }
        [Display(Name = "是否排序字段", GroupName = "基本设置")]
        public bool IsSortable { get; set; }

        [Display(Name = "表单附加属性", GroupName = "基本设置")]
        public string FormAdditionalAttr{ get; set; }
        [Display(Name = "表单CSS", GroupName = "基本设置")]
        public string FormCSS { get; set; }

        /// <summary>
        /// 字段长度取值范围
        /// </summary>
        [Display(Name = "字段长度取值范围", GroupName = "基本设置")]
        public FieldLength LengthRange { get; set; }
        /// <summary>
        /// 正则校验
        /// </summary>
        [Display(Name = "正则表达式", GroupName = "基本设置")]
        public string Regex { get; set; }
        /// <summary>
        /// 正则校验未通过的提示信息
        /// </summary>
        [Display(Name = "正则错误信息", GroupName = "基本设置")]
        public string RegexErrorMsg { get; set; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        [Display(Name = "是否唯一", GroupName = "基本设置")]
        public bool IsUnique { get; set; }
        /// <summary>
        /// 是否作为基本信息(添加页面显示)
        /// </summary>
        [Display(Name = "是否作为基本信息", GroupName = "基本设置")]
        public bool IsCommonInfo { get; set; }
        ///// <summary>
        ///// 是否正文（投稿）显示（详细页面显示）
        ///// </summary>
        //public bool IsFrontInfo { get; set; }
        /// <summary>
        /// 作为全站搜索
        /// </summary>
        [Display(Name = "是否作为全站搜索", GroupName = "基本设置")]
        public bool IsGloalSearch { get; set; }
        /// <summary>
        /// 作为万能字段的附属字段
        /// </summary>
        [Display(Name = "是否作为万能字段的附属字段", GroupName = "基本设置")]
        public bool IsWithOmni { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "禁用用户组", GroupName = "基本设置")]
        public List<int> DenyGroupList { get; set; }
        [Display(Name = "禁用用户角色", GroupName = "基本设置")]
        public List<int> DenyRoleList { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
        [Display(Name = "该字段的值", GroupName = "基本设置")]
        public object Value { get; set; }
    }
}
