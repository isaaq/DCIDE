using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DC.IDE.UI.Model.Webpart;
using DC.IDE.UI.Util;
using MongoDB.Bson;
using PostSharp.Patterns.Model;


using Telerik.Windows.Controls;

namespace DC.IDE.UI.Model
{
    public abstract class BaseWebpartPropModel : DynamicViewModelBase
    {
        #region 静态属性

        [Display(Name = "名称", GroupName = "基本设置")]
        public string Name { get; set; }
        [Display(Name = "分类", GroupName = "基本设置")]
        public string Category { get; set; }

        public int DynamicPropertiesCount { get { return this.data.Count; } }

        public string ManagePageContent { get; set; }
        public string MainContent { get; set; }

        /// <summary>
        /// 工具栏按钮
        /// </summary>
        public IList<ToolbarButton> toolbarButtonList { get; set; }
        public BsonArray DataSources { get; set; }

        #endregion



        /// <summary>
        /// 构建动态属性
        /// </summary>
        protected virtual void BuildDynamicProperties()
        {

        }

        private void LoadData()
        {
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var t_c = m.GetTable("sys_webpart_categories");
            var categories = t_c.FindAll();
        }



        public BaseWebpartPropModel()
        {
            BuildDynamicProperties();
            LoadData();
        }
    }
}
