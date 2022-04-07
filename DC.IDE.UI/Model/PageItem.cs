using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using PostSharp.Patterns.Model;
using Telerik.Windows.Controls;

namespace DC.IDE.UI.Model
{
    [NotifyPropertyChanged]
    public class PageItem : ViewModelBase
    {
        public string Title { get; set; }
        public ObjectId Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
    }
}
