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
      TreeMode = 0x01,
      Filter   = 0x02,
      Sorter   = 0x04
   }
}
