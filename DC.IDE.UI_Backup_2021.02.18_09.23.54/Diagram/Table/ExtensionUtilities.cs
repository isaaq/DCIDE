// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.ExtensionUtilities
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace DC.IDE.UI
{
  public class ExtensionUtilities
  {
    public static string ExecutingAssemblyName
    {
      get
      {
        string fullName = Assembly.GetExecutingAssembly().FullName;
        return fullName.Substring(0, fullName.IndexOf(','));
      }
    }

    public static Stream GetStream(string relativeUri)
    {
      return ExtensionUtilities.GetStream(new Uri(relativeUri, UriKind.RelativeOrAbsolute), ExtensionUtilities.ExecutingAssemblyName);
    }

    public static Stream GetStream(Uri uri)
    {
      return ExtensionUtilities.GetStream(uri, ExtensionUtilities.ExecutingAssemblyName);
    }

    public static BitmapImage GetBitmap(Uri uri, string assemblyName)
    {
      if (uri == (Uri) null)
        return (BitmapImage) null;
      Stream stream = ExtensionUtilities.GetStream(uri, assemblyName);
      if (stream == null)
        return (BitmapImage) null;
      using (stream)
      {
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();
        return bitmapImage;
      }
    }

    public static BitmapImage GetBitmap(string relativeUri, string assemblyName)
    {
      return ExtensionUtilities.GetBitmap(new Uri(relativeUri, UriKind.RelativeOrAbsolute), assemblyName);
    }

    public static BitmapImage GetBitmap(string relativePath)
    {
      return ExtensionUtilities.GetBitmap(new Uri(relativePath, UriKind.RelativeOrAbsolute), ExtensionUtilities.ExecutingAssemblyName);
    }

    public static Stream GetStream(Uri uri, string assemblyName)
    {
      if (!(uri != (Uri) null))
        return (Stream) null;
      return ExtensionUtilities.GetStream(uri.ToString(), assemblyName);
    }

    public static Stream GetStream(string relativeUri, string assemblyName)
    {
      try
      {
        if (Application.Current == null)
          return (Stream) null;
        if (relativeUri.StartsWith("/", StringComparison.Ordinal))
          relativeUri = relativeUri.Remove(0, 1);
        if (assemblyName.ToLower().EndsWith(".dll", StringComparison.Ordinal))
          assemblyName = assemblyName.Replace(".dll", string.Empty);
        StreamResourceInfo streamResourceInfo = Application.GetResourceStream(new Uri(assemblyName + ";component/" + relativeUri, UriKind.Relative)) ?? Application.GetResourceStream(new Uri(relativeUri, UriKind.Relative));
        if (streamResourceInfo != null)
          return streamResourceInfo.Stream;
      }
      catch (Exception ex)
      {
        return (Stream) null;
      }
      return (Stream) null;
    }
  }
}
