using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.PropertyGrid;

namespace DC.IDE.UI.UC.Properties
{
    public class PropertyGridDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item as PropertyDefinition != null && (item as PropertyDefinition).SourceProperty.PropertyType == typeof(Int32))
            {
                return IntegerPropertyDataTemplate;
            }
            if (item as PropertyDefinition != null && (item as PropertyDefinition).SourceProperty.Name == "LastName")
            {
                return LastNameDataTemplate;
            }
            return null;
        }

        public DataTemplate IntegerPropertyDataTemplate { get; set; }
        public DataTemplate LastNameDataTemplate { get; set; }
    }
}
