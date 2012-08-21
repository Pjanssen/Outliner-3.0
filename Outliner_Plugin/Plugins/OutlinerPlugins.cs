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

namespace Outliner.Plugins
{
public struct OutlinerPluginData
{
   String displayName;
   Type type;
   
   public String DisplayName { get { return displayName; } }
   public Type Type { get { return type; } }
   
   public OutlinerPluginData(String displayName, Type type)
   {
      this.displayName = displayName;
      this.type = type;
   }
}

public static class OutlinerPlugins
{
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
         IEnumerable<Type> pluginTypes = assembly.GetTypes().Where(t => !t.IsAbstract && t.IsPublic);

         foreach (Type pluginType in pluginTypes)
         {
            object[] attributes = pluginType.GetCustomAttributes(typeof(OutlinerPluginAttribute), false);
            if (attributes == null || attributes.Count() == 0)
               continue;

            String name = pluginType.Name;
            attributes = pluginType.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Count() > 0)
               name = ((DisplayNameAttribute)attributes[0]).DisplayName;

            plugins.Add(new OutlinerPluginData(name, pluginType));
         }
      }

      plugins.Sort((pX, pY) => pX.DisplayName.CompareTo(pY.DisplayName));

      return plugins;
   }

   /// <summary>
   /// Returns a list of all plugins with a certain base type in the supplied assemblies.
   /// </summary>
   /// <param name="assembly">The assemblies to look for plugins in.</param>
   /// <param name="baseType">The base plugin type to look for.</param>
   /// <returns></returns>
   public static IEnumerable<OutlinerPluginData> GetPlugins(IEnumerable<Assembly> assemblies, Type baseType)
   {
      return GetPlugins(assemblies).Where(p => p.Type.IsSubclassOf(baseType));
   }

   /// <summary>
   /// Returns a list of available plugins in the Outliner assembly.
   /// </summary>
   /// <param name="baseType">The base plugin type to look for.</param>
   /// <returns></returns>
   public static IEnumerable<OutlinerPluginData> GetOwnPlugins(Type baseType)
   {
      Assembly assembly = Assembly.GetAssembly(typeof(OutlinerPlugins));
      return GetPlugins(new List<Assembly>(1) { assembly }, baseType);
   }

   public static IEnumerable<OutlinerPluginData> GetTreeModePlugins()
   {
      return GetOwnPlugins(typeof(TreeMode));
   }

   public static IEnumerable<OutlinerPluginData> GetSorterPlugins()
   {
      return GetOwnPlugins(typeof(NodeSorter));
   }

   public static IEnumerable<OutlinerPluginData> GetFilterPlugins()
   {
      return GetOwnPlugins(typeof(Filter<IMaxNodeWrapper>));
   }

   public static IEnumerable<OutlinerPluginData> GetFilterPlugins(FilterCategories category)
   {
      IEnumerable<OutlinerPluginData> pluginTypes = OutlinerPlugins.GetFilterPlugins();
      
      List<OutlinerPluginData> filters = new List<OutlinerPluginData>();
      foreach (OutlinerPluginData plugin in pluginTypes)
      {
         object[] attributes = plugin.Type.GetCustomAttributes(typeof(FilterCategoryAttribute), false);
         if (attributes.Count() > 0)
         {
            FilterCategories filterCategory = ((FilterCategoryAttribute)attributes[0]).Category;
            if ((filterCategory & category) == category)
            {
               filters.Add(plugin);
            }
         }
      }

      return filters;
   }
}
}
