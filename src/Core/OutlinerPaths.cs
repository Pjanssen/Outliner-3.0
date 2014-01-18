using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Autodesk.Max;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner
{
/// <summary>
/// Provides the locations of various types of files used by the Outliner.
/// </summary>
public static class OutlinerPaths
{
   //==========================================================================

   private static String GetMaxDirectory(MaxDirectory dir)
   {
      Throw.IfNull(dir, "dir");

      IIPathConfigMgr pathMgr = MaxInterfaces.Global.IPathConfigMgr.PathConfigMgr;
      return pathMgr.GetDir(dir);
   }
   
   private static String configDir;

   /// <summary>
   /// Gets the general Configuration directory.
   /// </summary>
   public static String ConfigDir
   {
      get
      {
         if (configDir == null)
            configDir = Path.Combine(GetMaxDirectory(MaxDirectory.Plugcfg), "Outliner\\");

         return configDir;
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets the location of the configuration file.
   /// </summary>
   public static String ConfigurationFile
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "outliner.config"); }
   }

   //==========================================================================

   /// <summary>
   /// Gets the plugins directory.
   /// </summary>
   public static String PluginsDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "Plugins\\"); }
   }

   //==========================================================================

   /// <summary>
   /// Gets the presets directory.
   /// </summary>
   public static String PresetsDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "Presets\\"); }
   }

   //==========================================================================

   /// <summary>
   /// Gets the treeview layouts directory.
   /// </summary>
   public static String LayoutsDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "Layouts\\"); }
   }

   //==========================================================================

   /// <summary>
   /// Gets the context menus directory.
   /// </summary>
   public static String ContextMenusDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "ContextMenus\\"); }
   }

   //==========================================================================

   /// <summary>
   /// Gets the filter configurations directory.
   /// </summary>
   public static String FiltersDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "Filters\\"); }
   }

   //==========================================================================

   /// <summary>
   /// Gets the sorter configurations directory.
   /// </summary>
   public static String SortersDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "NodeSorters\\"); }
   }

   //==========================================================================

   /// <summary>
   /// Gets the color schemes directory.
   /// </summary>
   public static String ColorSchemesDir
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "ColorSchemes"); }
   }

   //==========================================================================

   /// <summary>
   /// Gets the stored state file location.
   /// </summary>
   public static String StateFile
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "saved_state.xml"); }
   }

   //==========================================================================

   /// <summary>
   /// Gets the color file location.
   /// </summary>
   public static String ColorFile
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "colors.xml"); }
   }

   //==========================================================================

   public static String LogFile
   {
      get { return Path.Combine(OutlinerPaths.ConfigDir, "outliner.log"); }
   }

   //==========================================================================
}
}
