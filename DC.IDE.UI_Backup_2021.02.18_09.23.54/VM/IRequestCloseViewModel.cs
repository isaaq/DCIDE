using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.VM
{
    public interface IRequestCloseViewModel
    {
        event EventHandler RequestClose;
        void Close();
    }
}
