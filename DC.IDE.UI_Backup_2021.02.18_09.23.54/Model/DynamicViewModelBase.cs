using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using PostSharp.Patterns.Model;

namespace DC.IDE.UI.Model
{
    [NotifyPropertyChanged]
    public class DynamicViewModelBase : DynamicObject, INotifyPropertyChanged
    {
        protected readonly IDictionary<string, object> data;

        public DynamicViewModelBase()
        {
            this.data = new Dictionary<string, object>();
        }

        public DynamicViewModelBase(IDictionary<string, object> source)
        {
            this.data = source;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.data.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;

            return true;
        }

        public object this[string columnName]
        {
            get
            {
                if (this.data.ContainsKey(columnName))
                {
                    return this.data[columnName];
                }

                return null;
            }
            set
            {
                if (!this.data.ContainsKey(columnName))
                {
                    this.data.Add(columnName, value);

                    this.OnPropertyChanged(columnName);
                }
                else
                {
                    if (this.data[columnName] != value)
                    {
                        this.data[columnName] = value;

                        this.OnPropertyChanged(columnName);
                    }
                }
            }
        }

        //
        // 摘要:
        //     Raised when a property on this object has a new value.
        public event PropertyChangedEventHandler PropertyChanged;

        //
        // 摘要:
        //     Warns the developer if this object does not have a public property with the specified
        //     name. This method does not exist in a Release build.
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        protected void VerifyPropertyName(string propertyName)
        {
            _ = (GetType().GetProperty(propertyName) == null);
        }

        //
        // 摘要:
        //     Invokes the specified action on the UI thread.
        //
        // 参数:
        //   action:
        //     An Action to be invoked on the UI thread.
        public static void InvokeOnUIThread(Action action)
        {
            Dispatcher dispatcher = (Application.Current != null && Application.Current.Dispatcher != null) ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.BeginInvoke(action);
            }
        }

        //
        // 摘要:
        //     Performs application-defined tasks associated with freeing, releasing, or resetting
        //     unmanaged resources.
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        //
        // 摘要:
        //     Raises this object's Telerik.Windows.Controls.ViewModelBase.PropertyChanged event.
        //     This method uses CallerMemberName attribute to identify the source property when
        //     called without parameter.
        //
        // 参数:
        //   propertyName:
        //     The property that has a new value.
        protected internal void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }

        //
        // 摘要:
        //     Raises this object's Telerik.Windows.Controls.ViewModelBase.PropertyChanged event.
        //
        // 参数:
        //   propertyName:
        //     The property that has a new value.
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                propertyChanged(this, e);
            }
        }

        //
        // 摘要:
        //     Raises this object's Telerik.Windows.Controls.ViewModelBase.PropertyChanged event.
        //
        // 参数:
        //   propertyExpression:
        //     A MemberExpression, containing the property that value changed.
        //
        // 言论：
        //     Use the following syntax: this.OnPropertyChanged(() => this.MyProperty); instead
        //     of: this.OnPropertyChanged("MyProperty");.
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            OnPropertyChanged(((MemberExpression)propertyExpression.Body).Member.Name);
        }

        //
        // 摘要:
        //     Releases unmanaged and - optionally - managed resources.
        //
        // 参数:
        //   disposing:
        //     true to release both managed and unmanaged resources. false to release only unmanaged
        //     resources.
        protected virtual void Dispose(bool disposing)
        {
        }
    }

}
