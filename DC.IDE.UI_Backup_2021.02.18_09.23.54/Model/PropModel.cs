using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Patterns.Model;

namespace DC.IDE.UI.Model
{
	public class PropModel : DynamicObject, INotifyPropertyChanged
	{

		readonly IDictionary<string, object> data;

		public PropModel()
		{
			this.data = new Dictionary<string, object>();
		}

		public PropModel(IDictionary<string, object> source)
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

		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
