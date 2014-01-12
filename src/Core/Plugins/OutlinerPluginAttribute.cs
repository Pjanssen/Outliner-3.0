using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.Plugins
{
   /// <summary>
   /// Marks a class as a plugin for the Outliner.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
   sealed public class OutlinerPluginAttribute : Attribute
   {
      /// <summary>
      /// The type of plugin.
      /// </summary>
      public OutlinerPluginType PluginType { get; private set; }

      /// <summary>
      /// Initializes a new instance of the OutlinerPluginAttribute class.
      /// </summary>
      /// <param name="pluginType">The type of plugin.</param>
      public OutlinerPluginAttribute(OutlinerPluginType pluginType) 
      {
         this.PluginType = pluginType;
      }
   }

   /// <summary>
   /// Defines the type of Outliner plugin.
   /// </summary>
   [Flags]
   public enum OutlinerPluginType
   {
      /// <summary>
      /// A plugin which controls how the treeview should be filled and updated.
      /// </summary>
      /// <remarks>The plugin class must inherit from TreeMode.</remarks>
      TreeMode             = 0x01,

      /// <summary>
      /// A plugin which controls whether a node in the tree should be shown or hidden.
      /// </summary>
      /// <remarks>The plugin class must inherit from Filter&lt;IMaxNode&gt;.</remarks>
      Filter               = 0x02,

      /// <summary>
      /// A plugin which sorts the nodes in the treeview.
      /// </summary>
      /// <remarks>The plugin class must inherit from NodeSorter.</remarks>
      NodeSorter           = 0x04,

      /// <summary>
      /// A plugin which displays an item in the treeview.
      /// </summary>
      /// <remarks>The plugin class must inherit from TreeNodeButton.</remarks>
      TreeNodeButton       = 0x08,

      /// <summary>
      /// A generic utlity plugin, which can be used to execute a function when the
      /// plugin assembly is loaded or unloaded.
      /// </summary>
      Utility              = 0x10,
      
      /// <summary>
      /// A model for a ContextMenuItem.
      /// </summary>
      ContextMenuItemModel = 0x20,
      
      /// <summary>
      /// A plugin class containing OutlinerActions and OutlinerPredicates.
      /// </summary>
      ActionProvider       = 0x40
   }
}
