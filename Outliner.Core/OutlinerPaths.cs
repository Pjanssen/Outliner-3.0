using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;
using System.IO;

namespace Outliner
{
public static class OutlinerPaths
{
   private static String GetMaxDirectory(MaxDirectory dir)
   {
      Throw.IfArgumentIsNull(dir, "dir");

      IIPathConfigMgr pathMgr = MaxInterfaces.Global.IPathConfigMgr.PathConfigMgr;
      return pathMgr.GetDir(dir);
   }
   
   private static String configDir;
   public static String ConfigDir
   {
      get
      {
         if (configDir == null)
            configDir = Path.Combine(GetMaxDirectory(MaxDirectory.Plugcfg), "Outliner\\");

         return configDir;
      }
   }

   public static String PluginsDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "Plugins\\"); }
   }


   public static String PresetsDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "Presets\\"); }
   }

   public static String LayoutsDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "Layouts\\"); }
   }

   public static String ContextMenusDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "ContextMenus\\"); }
   }

   public static String FiltersDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "Filters\\"); }
   }

   public static String SortersDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "NodeSorters\\"); }
   }

   public static String StateFile
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "saved_state.xml"); }
   }

   public static String ColorFile
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "colors.xml"); }
   }
}
}
