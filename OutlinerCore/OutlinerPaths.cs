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

   private static String pluginsDir;
   public static String PluginsDir
   {
      get
      {
         if (pluginsDir == null)
         {
            pluginsDir = Path.Combine( GetMaxDirectory(MaxDirectory.ManagedAssemblies)
                                     , "OutlinerPlugins");
         }
         return pluginsDir;
      }
   }

   private static String configDir;
   public static String ConfigDir
   {
      get
      {
         if (configDir == null)
            configDir = Path.Combine(GetMaxDirectory(MaxDirectory.Plugcfg), "Outliner");

         return configDir;
      }
   }

   public static String PresetsDir
   {
      get
      {
         return Path.Combine(OutlinerPaths.ConfigDir, "Presets");
      }
   }

   public static String StateFile
   {
      get
      {
         return Path.Combine(OutlinerPaths.ConfigDir, "saved_state.xml");
      }
   }

   public static String ColorFile
   {
      get
      {
         return Path.Combine(OutlinerPaths.ConfigDir, "colors.xml");
      }
   }
}
}
