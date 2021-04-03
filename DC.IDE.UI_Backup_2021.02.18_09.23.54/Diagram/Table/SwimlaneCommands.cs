// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.SwimlaneCommands
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System.Windows.Input;

namespace DC.IDE.UI.Diagram.Table
{
  public static class SwimlaneCommands
  {
    private static RoutedUICommand addCommand;
    private static RoutedUICommand removeCommand;

    public static RoutedUICommand RemoveCommand
    {
      get
      {
        if (SwimlaneCommands.removeCommand == null)
          SwimlaneCommands.removeCommand = new RoutedUICommand("Remove Command", nameof (RemoveCommand), typeof (SwimlaneCommands));
        return SwimlaneCommands.removeCommand;
      }
    }

    public static RoutedUICommand AddCommand
    {
      get
      {
        if (SwimlaneCommands.addCommand == null)
          SwimlaneCommands.addCommand = new RoutedUICommand("Add Command", nameof (AddCommand), typeof (SwimlaneCommands));
        return SwimlaneCommands.addCommand;
      }
    }
  }
}
