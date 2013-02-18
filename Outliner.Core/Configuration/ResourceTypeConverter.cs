using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Outliner.Plugins;
using System.Reflection;
using System.Resources;

namespace Outliner.Configuration
{
internal class ResourceTypeConverter : TypeListConverter
{
   public ResourceTypeConverter() : base(GetResourceTypes()) { }

   private static Type[] GetResourceTypes()
   {
      return OutlinerPlugins.PluginAssemblies
                            .SelectMany(a => a.GetTypes())
                            .Where(t => t.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.NonPublic) != null)
                            .ToArray();
   }
}
}
