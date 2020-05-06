// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Main.FuncList
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using DC.IDE.UI.Model;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Telerik.Windows.Controls.Primitives;

namespace DC.IDE.UI.Main
{
    public partial class FuncList : UserControl, IComponentConnector
    {
        public event EventHandler<FuncItem> FuncChanged;

        public FuncList()
        {
            this.InitializeComponent();
        }

        private void RadListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FuncItem addedItem = (FuncItem)e.AddedItems[0];
            if (this.FuncChanged == null)
                return;
            this.FuncChanged((object)this, addedItem);
        }
    }
}
