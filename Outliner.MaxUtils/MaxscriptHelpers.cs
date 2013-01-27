using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Outliner.MaxUtils
{
   public static class MaxscriptHelpers
   {
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
      public static void RunResourceScript(Assembly assembly, String res)
      {
         Throw.IfArgumentIsNull(assembly, "assembly");
         Throw.IfArgumentIsNull(res, "res");

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

      public static void WriteToListener(String text)
      {
         MaxInterfaces.Global.TheListener.EditStream.Wputs(text + "\n");
         MaxInterfaces.Global.TheListener.EditStream.Wflush();
      }

      public static void WriteToListener(Object obj)
      {
         MaxscriptHelpers.WriteToListener(obj.ToString());
      }

      public static void WriteToListener(String format, params Object[] args)
      {
         String text = String.Format(CultureInfo.InvariantCulture, format, args);
         MaxscriptHelpers.WriteToListener(text);
      }
   }
}
