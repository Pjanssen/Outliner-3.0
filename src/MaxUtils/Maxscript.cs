using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PJanssen;

namespace Outliner.MaxUtils
{
   /// <summary>
   /// Provides methods for common maxscript operations.
   /// </summary>
   public static class Maxscript
   {
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
      public static void RunResourceScript(Assembly assembly, String res)
      {
         Throw.IfNull(assembly, "assembly");
         Throw.IfNull(res, "res");

         String script = String.Empty;

         using (Stream stream = assembly.GetManifestResourceStream(res))
         using (StreamReader sr = new StreamReader(stream))
         {
            script = sr.ReadToEnd();
         }

         if (!String.IsNullOrWhiteSpace(script))
         {
            MaxInterfaces.Global.ExecuteMAXScriptScript(script, true, null);
         }
      }

      /// <summary>
      /// Writes a message to the maxscript listener.
      /// </summary>
      /// <param name="text">The text to write.</param>
      public static void WriteToListener(String text)
      {
         MaxInterfaces.Global.TheListener.EditStream.Wputs(text + "\n");
         MaxInterfaces.Global.TheListener.EditStream.Wflush();
      }

      /// <summary>
      /// Writes the string representation of an object to the maxscript listener.
      /// </summary>
      /// <param name="obj">The object to write.</param>
      public static void WriteToListener(Object obj)
      {
         Maxscript.WriteToListener(obj.ToString());
      }

      /// <summary>
      /// Writes a message to the maxscript listener using String.Format.
      /// </summary>
      /// <param name="format">The format string.</param>
      /// <param name="args">The objects to format.</param>
      public static void WriteToListener(String format, params Object[] args)
      {
         String text = String.Format(CultureInfo.InvariantCulture, format, args);
         Maxscript.WriteToListener(text);
      }
   }
}
