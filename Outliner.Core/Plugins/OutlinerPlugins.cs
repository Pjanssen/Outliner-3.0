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
using Outliner.MaxUtils;

namespace Outliner.Plugins
{
public static class OutlinerPlugins
{
   internal const OutlinerPluginType PluginTypeAll = (OutlinerPluginType)0xFF;
   internal const OutlinerPluginType PluginTypeNone = (OutlinerPluginType)0x00;
   private const String PluginExtension = "dll";
   private const String PluginSearchPattern = "*." + PluginExtension;

   private static IEnumerable<Assembly> pluginAssemblies;
   private static IEnumerable<OutlinerPluginData> plugins;


   /// <summary>
   /// (Re)loads all plugins from the PluginDirectory.
   /// </summary>
   internal static IEnumerable<OutlinerPluginData> LoadPlugins()
   {
      OutlinerPlugins.plugins = PluginAssemblies.SelectMany(a => LoadPlugins(a))
                                                .ToList();

      return OutlinerPlugins.plugins;
   }

   private static IEnumerable<OutlinerPluginData> LoadPlugins(Assembly pluginAssembly)
   {
      AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

      IEnumerable<OutlinerPluginData> plugins = pluginAssembly.GetExportedTypes()
                                                              .Where(t => t.IsPublic && (!t.IsAbstract || t.IsSealed))
                                                              .Where(t => t.HasAttribute<OutlinerPluginAttribute>())
                                                              .Select(t => new OutlinerPluginData(t));

      StartPlugins(plugins);

      return plugins;
   }

   private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
   {
      return OutlinerPlugins.PluginAssemblies.FirstOrDefault(a => a.FullName == args.Name);
   }

   /// <summary>
   /// Calls all methods in plugins marked with the OutlinerPluginStart attribute.
   /// </summary>
   internal static void StartPlugins(IEnumerable<OutlinerPluginData> plugins)
   {
      if (plugins != null)
         plugins.ForEach(p => InvokePluginMethod<OutlinerPluginStartAttribute>(p.Type));
   }

   /// <summary>
   /// Calls all methods in plugins marked with the OutlinerPluginStop attribute.
   /// </summary>
   internal static void StopPlugins()
   {
      if (plugins != null)
         plugins.ForEach(p => InvokePluginMethod<OutlinerPluginStopAttribute>(p.Type));
   }

   private static void InvokePluginMethod<T>(Type pluginClass) where T : Attribute
   {
      String pluginClassName = pluginClass.Name;
      pluginClass.GetMethods(BindingFlags.Static | BindingFlags.Public)
                 .Where(m => m.HasAttribute<T>())
                 .ForEach(m => m.Invoke(null, null));
   }
   

   /// <summary>
   /// The collection of assemblies from which plugins have been loaded.
   /// </summary>
   public static IEnumerable<Assembly> PluginAssemblies
   {
      get
      {
         if (pluginAssemblies == null)
         {
            List<Assembly> assemblies = new List<Assembly>();

            //Add own assembly.
            assemblies.Add(Assembly.GetAssembly(typeof(OutlinerPlugins)));

            //Add plugin assemblies.
            String pluginDir = OutlinerPaths.PluginsDir;
            if (Directory.Exists(pluginDir))
            {
               String[] pluginFiles = Directory.GetFiles( pluginDir
                                                        , OutlinerPlugins.PluginSearchPattern
                                                        , SearchOption.AllDirectories);
               foreach (String pluginFile in pluginFiles)
               {
                  assemblies.Add(Assembly.LoadFile(pluginFile));
               }
            }

            pluginAssemblies = assemblies;
         }

         return pluginAssemblies;
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
            OutlinerPlugins.LoadPlugins();

         return plugins;
      }
   }


   /// <summary>
   /// Gets a collection of plugin metadata objects for plugins of the given type.
   /// </summary>
   /// <param name="type">The type of plugins to select.</param>
   public static IEnumerable<OutlinerPluginData> GetPlugins(OutlinerPluginType type)
   {
      return Plugins.Where(p => (p.PluginType & type) != 0);
   }

   /// <summary>
   /// Gets a collection plugin metadata objects contained in the given assembly.
   /// Note that the assembly has to be loaded by the plugin system first.
   /// </summary>
   public static IEnumerable<OutlinerPluginData> GetPlugins(Assembly pluginAssembly)
   {
      return Plugins.Where(p => p.Type.Assembly == pluginAssembly);
   }

   /// <summary>
   /// Gets the plugin metadata for the plugin with the given type.
   /// </summary>
   public static OutlinerPluginData GetPlugin(Type pluginType)
   {
      return Plugins.FirstOrDefault(p => p.Type == pluginType);
   }

   /// <summary>
   /// Gets the Type of the supplied plugin and typename.
   /// </summary>
   public static Type GetPluginType(OutlinerPluginType pluginType, String typeName)
   {
      IEnumerable<OutlinerPluginData> plugins = OutlinerPlugins.GetPlugins(pluginType);
      OutlinerPluginData plugin = plugins.FirstOrDefault(p => p.Type.Name == typeName || p.Type.FullName == typeName);
      return (plugin != null) ? plugin.Type : null;
   }

   /// <summary>
   /// Gets an array of Types that can be serialized.
   /// </summary>
   public static Type[] GetSerializableTypes()
   {
      return OutlinerPlugins.GetPlugins( OutlinerPluginType.Filter
                                       | OutlinerPluginType.NodeSorter
                                       | OutlinerPluginType.TreeNodeButton
                                       | OutlinerPluginType.ContextMenuItemModel)
                            .Select(p => p.Type)
                            .ToArray();
   }
}
}
