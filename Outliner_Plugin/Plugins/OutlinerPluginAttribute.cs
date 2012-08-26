using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Plugins
{
   /// <summary>
   /// Marks a class as a plugin for the Outliner.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
   sealed public class OutlinerPluginAttribute : Attribute
   {
      public OutlinerPluginType PluginType { get; private set; }

      public OutlinerPluginAttribute(OutlinerPluginType pluginType) 
      {
         this.PluginType = pluginType;
      }
   }

   public enum OutlinerPluginType
   {
      TreeMode       = 0x01,
      Filter         = 0x02,
      NodeSorter     = 0x04,
      TreeNodeButton = 0x08,
      Utility        = 0x10,
      Action         = 0x20
   }
}
