using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DC.IDE.UI.Model.Field;
using MongoDB.Bson;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using DCIDE.UI.VM;
using System.Windows.Media;
using PostSharp.Aspects.Advices;
using System.Windows.Controls;
using System.Dynamic;

namespace DC.IDE.UI.Util
{
    public static class CommonHelper
    {
        public static ObjectId ToObjectId(this object str)
        {
            return new ObjectId(str.ToString());
        }

        public static IEnumerable<ObjectId> ToObjectId(this IEnumerable<object> objs)
        {
            return objs.Select(s => s.ToObjectId());
        }

        public static FieldItem FillFieldItem(this FieldItem fi, BsonDocument doc)
        {
            if (doc == null) return null;
            var type = fi.GetType();

            foreach (var ele in doc)
            {
                var name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(ele.Name);
                var propertyInfo = type.GetProperty(name);
                if (propertyInfo != null)
                {
                    if(name == "Type")
                        propertyInfo.SetValue(fi, (FieldType)ele.Value.AsInt32, null);
                    else
                        propertyInfo.SetValue(fi, ele.Value.AsString, null);
                }
            }
            return fi;
        }

        public static Notifier GetNotify()
        {
            Notifier notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            return notifier;
        }
        public static void ChangePropVM(this UserControl uc, object item)
        {
            var parent = (FrameworkElement)VisualTreeHelper.GetParent(uc);
            if (parent != null)
            {
                var parentdc = (VMMain)parent.DataContext;
                parentdc.PropList = item;
                parentdc.OnPropChange();
            }
        }

    }
}
