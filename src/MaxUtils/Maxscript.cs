using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PJanssen.Outliner.MaxUtils
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
   }
}
