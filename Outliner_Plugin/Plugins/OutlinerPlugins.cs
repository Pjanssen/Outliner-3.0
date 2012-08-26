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
using System.IO;
using Autodesk.Max;
using MaxUtils;

namespace Outliner.Plugins
{
public static class OutlinerPlugins
{
   private const OutlinerPluginType PluginTypeAll = (OutlinerPluginType)0xFF;
   private const OutlinerPluginType PluginTypeNone = (OutlinerPluginType)0x00;
   private static IEnumerable<OutlinerPluginData> plugins;

   private static T GetAttribute<T>(Type sourceType) where T : Attribute
   {
      object[] attributes = sourceType.GetCustomAttributes(typeof(T), false);
      if (attributes != null && attributes.Count() > 0)
         return attributes[0] as T;
      else
         return null;
   }
   private static T GetAttribute<T>(MethodInfo sourceMethod) where T : Attribute
   {
      object[] attributes = sourceMethod.GetCustomAttributes(typeof(T), false);
      if (attributes != null && attributes.Count() > 0)
         return attributes[0] as T;
      else
         return null;
   }

   /// <summary>
   /// The directory from which all plugins are loaded.
   /// </summary>
   public static String PluginDirectory
   {
      get
      {
         IIPathConfigMgr pathMgr = MaxInterfaces.Global.IPathConfigMgr.PathConfigMgr;
         return Path.Combine(pathMgr.GetDir(MaxDirectory.ManagedAssemblies), "OutlinerPlugins");
      }
      
   }

   /// <summary>
   /// The collection of assemblies from which plugins have been loaded.
   /// </summary>
   public static IEnumerable<Assembly> PluginAssemblies
   {
      get
      {
         List<Assembly> assemblies = new List<Assembly>();
            
         //Add own assembly.
         assemblies.Add(Assembly.GetAssembly(typeof(OutlinerPlugins)));

         AssemblyLoader.Loader loader = new AssemblyLoader.Loader();
         //Add plugin assemblies.
         if (Directory.Exists(OutlinerPlugins.PluginDirectory))
         {
            String[] pluginFiles = Directory.GetFiles(OutlinerPlugins.PluginDirectory
                                                      , "*.dll"
                                                      , SearchOption.AllDirectories);
            foreach (String pluginFile in pluginFiles)
            {

               assemblies.Add(Assembly.LoadFile(pluginFile));
            }
         }

         return assemblies;
      }
   }
   
   /// <summary>
   /// (Re)loads all plugins from the <see cref="PluginDirectory"/>.
   /// </summary>
   /// <returns></returns>
   public static IEnumerable<OutlinerPluginData> LoadPlugins()
   {
      List<OutlinerPluginData> plugins = new List<OutlinerPluginData>();

      foreach (Assembly assembly in OutlinerPlugins.PluginAssemblies)
      {
         IEnumerable<Type> pluginClasses = assembly.GetTypes().Where(t => t.IsPublic && (!t.IsAbstract || t.IsSealed));

         foreach (Type pluginClass in pluginClasses)
         {
            OutlinerPluginType pluginType = PluginTypeNone;
            OutlinerPluginAttribute pluginAttr = GetAttribute<OutlinerPluginAttribute>(pluginClass);
            if (pluginAttr != null)
            {
               pluginType = pluginAttr.PluginType;
            }

            if ((pluginType & PluginTypeAll) == 0)
               continue;

            StartPlugin(pluginClass);

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

   private static void StartPlugin(Type pluginClass)
   {
      MethodInfo[] methods = pluginClass.GetMethods(BindingFlags.Static | BindingFlags.Public);
      foreach (MethodInfo method in methods)
      {
         OutlinerPluginStartAttribute initializer = GetAttribute<OutlinerPluginStartAttribute>(method);
         if (initializer != null)
            method.Invoke(null, null);
      }
   }

   /// <summary>
   /// A collection of all loaded plugin metadata.
   /// </summary>
   public static IEnumerable<OutlinerPluginData> Plugins
   {
      get
      {
         if (plugins == null)
            plugins = LoadPlugins();

         return plugins;
      }
   }

   /// <summary>
   /// Gets a collection of plugin metadata for plugins of the given type.
   /// </summary>
   /// <param name="type">The type of plugins to select.</param>
   public static IEnumerable<OutlinerPluginData> GetPluginsByType(OutlinerPluginType type)
   {
      return Plugins.Where(p => (p.PluginType & type) == type);
   }

   /// <summary>
   /// Gets a collection of Filter plugins metadata for filters of the given category.
   /// </summary>
   /// <param name="category">The category of filter to look for.</param>
   internal static IEnumerable<OutlinerPluginData> GetFilterPlugins(FilterCategories category)
   {
      IEnumerable<OutlinerPluginData> pluginTypes = GetPluginsByType(OutlinerPluginType.Filter);
      
      List<OutlinerPluginData> filters = new List<OutlinerPluginData>();
      foreach (OutlinerPluginData plugin in pluginTypes)
      {
         FilterCategoryAttribute categoryAttr = GetAttribute<FilterCategoryAttribute>(plugin.Type);
         if (categoryAttr != null && (categoryAttr.Category & category) == category)
            filters.Add(plugin);
      }

      return filters;
   }

   internal static Type[] GetTreeNodeButtonTypes()
   {
      return GetPluginsByType(OutlinerPluginType.TreeNodeButton).Select(p => p.Type).ToArray();
   }
}
}
