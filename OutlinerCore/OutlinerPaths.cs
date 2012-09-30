using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using MaxUtils;
using System.IO;

namespace Outliner
{
public static class OutlinerPaths
{
   private static String GetMaxDirectory(MaxDirectory dir)
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(dir, "dir");

      IIPathConfigMgr pathMgr = MaxInterfaces.Global.IPathConfigMgr.PathConfigMgr;
      return pathMgr.GetDir(dir);
   }

   private static String plugins;
   public static String Plugins
   {
      get
      {
         if (plugins == null)
         {
            plugins = Path.Combine( GetMaxDirectory(MaxDirectory.ManagedAssemblies)
                                  , "OutlinerPlugins");
         }
         return plugins;
      }
   }

   private static String presets;
   public static String Presets
   {
      get
      {
         if (presets == null)
         {
            presets = Path.Combine( GetMaxDirectory(MaxDirectory.ManagedAssemblies)
                                  , "OutlinerPresets");
         }
         return presets;
      }
   }
}
}
