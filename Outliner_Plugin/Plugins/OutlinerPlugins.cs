using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Outliner.Filters;
using Outliner.Scene;
using Outliner.NodeSorters;
using Outliner.Modes;
using System.ComponentModel;
using System.Drawing;

namespace Outliner.Plugins
{
public static class OutlinerPlugins
{
   internal const OutlinerPluginType PluginTypeAll = (OutlinerPluginType)0xFF;
   internal const OutlinerPluginType PluginTypeNone = (OutlinerPluginType)0x00;

   private static T GetAttribute<T>(Type sourceType) where T : Attribute
   {
      object[] attributes = sourceType.GetCustomAttributes(typeof(T), false);
      if (attributes != null && attributes.Count() > 0)
         return attributes[0] as T;
      else
         return null;
   }

   /// <summary>
   /// Returns a list of all plugins in the supplied assemblies.
   /// </summary>
   /// <param name="assembly">The assemblies to look for plugins in.</param>
   /// <returns></returns>
   public static IEnumerable<OutlinerPluginData> GetPlugins(IEnumerable<Assembly> assemblies)
   {
      List<OutlinerPluginData> plugins = new List<OutlinerPluginData>();

      foreach (Assembly assembly in assemblies)
      {
         IEnumerable<Type> pluginClasses = assembly.GetTypes().Where(t => !t.IsAbstract && t.IsPublic);

         foreach (Type pluginClass in pluginClasses)
         {
            OutlinerPluginType pluginType = PluginTypeNone;
            OutlinerPluginAttribute pluginAttr = GetAttribute<OutlinerPluginAttribute>(pluginClass);
            if (pluginAttr != null)
            {
               pluginType = pluginAttr.PluginType;
            }

            if (pluginType == PluginTypeNone)
               continue;

            String name = pluginClass.Name;
            DisplayNameAttribute dispAtrr = GetAttribute<DisplayNameAttribute>(pluginClass);
            if (dispAtrr != null)
               name = dispAtrr.DisplayName;

            Image imgSmall = null;
            Image imgLarge = null;
            LocalizedDisplayImageAttribute imgAttr = GetAttribute<LocalizedDisplayImageAttribute>(pluginClass);
            if (imgAttr != null)
            {
               imgSmall = imgAttr.DisplayImageSmall;
               imgLarge = imgAttr.DisplayImageLarge;
            }

            plugins.Add(new OutlinerPluginData(pluginType, pluginClass, name, imgSmall, imgLarge));
         }
      }

      plugins.Sort((pX, pY) => pX.DisplayName.CompareTo(pY.DisplayName));

      return plugins;
   }

   /// <summary>
   /// Returns a list of all plugins with a certain type in the supplied assemblies.
   /// </summary>
   /// <param name="assembly">The assemblies to look for plugins in.</param>
   /// <param name="baseType">The plugin type to look for.</param>
   /// <returns></returns>
   public static IEnumerable<OutlinerPluginData> GetPlugins(IEnumerable<Assembly> assemblies, OutlinerPluginType pluginType)
   {
      return GetPlugins(assemblies).Where(p => (p.PluginType & pluginType) == pluginType);
   }

   /// <summary>
   /// Returns a list of available plugins in the Outliner assembly.
   /// </summary>
   /// <param name="baseType">The plugin type to look for.</param>
   public static IEnumerable<OutlinerPluginData> GetOwnPlugins(OutlinerPluginType pluginType)
   {
      Assembly assembly = Assembly.GetAssembly(typeof(OutlinerPlugins));
      return GetPlugins(new List<Assembly>(1) { assembly }, pluginType);
   }


   public static IEnumerable<OutlinerPluginData> GetAllPlugins()
   {
      return GetOwnPlugins(PluginTypeAll);
   }

   public static IEnumerable<OutlinerPluginData> GetTreeModePlugins()
   {
      return GetOwnPlugins(OutlinerPluginType.TreeMode);
   }

   public static IEnumerable<OutlinerPluginData> GetSorterPlugins()
   {
      return GetOwnPlugins(OutlinerPluginType.Sorter);
   }

   public static IEnumerable<OutlinerPluginData> GetFilterPlugins()
   {
      return GetOwnPlugins(OutlinerPluginType.Filter);
   }

   public static IEnumerable<OutlinerPluginData> GetFilterPlugins(FilterCategories category)
   {
      IEnumerable<OutlinerPluginData> pluginTypes = OutlinerPlugins.GetFilterPlugins();
      
      List<OutlinerPluginData> filters = new List<OutlinerPluginData>();
      foreach (OutlinerPluginData plugin in pluginTypes)
      {
         FilterCategoryAttribute categoryAttr = GetAttribute<FilterCategoryAttribute>(plugin.Type);
         if (categoryAttr != null && (categoryAttr.Category & category) == category)
            filters.Add(plugin);
      }

      return filters;
   }
}
}
