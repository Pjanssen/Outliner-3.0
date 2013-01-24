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

   [Flags]
   public enum OutlinerPluginType
   {
      /// <summary>
      /// A plugin which controls how the treeview should be filled and updated.
      /// The plugin class must inherit from TreeMode.
      /// </summary>
      TreeMode             = 0x01,

      /// <summary>
      /// A plugin which controls whether a node in the tree should be shown or hidden.
      /// The plugin class must inherit from Filter<IMaxNode>.
      /// </summary>
      Filter               = 0x02,

      /// <summary>
      /// A plugin which sorts the nodes in the treeview. The plugin class must inherit
      /// from NodeSorter.
      /// </summary>
      NodeSorter           = 0x04,

      /// <summary>
      /// A plugin which displays an item in the treeview. The plugin class must inherit
      /// from TreeNodeButton.
      /// </summary>
      TreeNodeButton       = 0x08,

      /// <summary>
      /// A generic utlity plugin, which can be used to execute a function when the
      /// plugin assembly is loaded or unloaded.
      /// </summary>
      Utility              = 0x10,
      

      ContextMenuItemModel = 0x20,
      
      /// <summary>
      /// A plugin class containing OutlinerActions and OutlinerPredicates.
      /// </summary>
      ActionProvider       = 0x40
   }
}
