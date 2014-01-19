using PJanssen.Outliner.Controls.Tree;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PJanssen.Outliner.Configuration
{
   public sealed class TreeViewConfigurationSection : ConfigurationSection
   {
      //==========================================================================

      [ConfigurationProperty("dragDropButton", DefaultValue=MouseButtons.Left)]
      public MouseButtons DragDropMouseButton
      {
         get
         {
            return (MouseButtons)this["dragDropButton"];
         }
         set
         {
            this["dragDropButton"] = value;
         }
      }

      //==========================================================================

      [ConfigurationProperty("doubleClickAction", DefaultValue = TreeNodeDoubleClickAction.Rename)]
      public TreeNodeDoubleClickAction DoubleClickAction
      {
         get
         {
            return (TreeNodeDoubleClickAction)this["doubleClickAction"];
         }
         set
         {
            this["doubleClickAction"] = value;
         }
      }
      
      //==========================================================================

      [ConfigurationProperty("scrollToSelection", DefaultValue = true)]
      public bool ScrollToSelection
      {
         get
         {
            return (bool)this["scrollToSelection"];
         }
         set
         {
            this["scrollToSelection"] = value;
         }
      }

      //==========================================================================

      [ConfigurationProperty("autoExpandSelectionParents", DefaultValue = true)]
      public bool AutoExpandSelectionParents
      {
         get
         {
            return (bool)this["autoExpandSelectionParents"];
         }
         set
         {
            this["autoExpandSelectionParents"] = value;
         }
      }

      //==========================================================================

      [ConfigurationProperty("collapseAutoExpandedParents", DefaultValue = true)]
      public bool CollapseAutoExpandedParents
      {
         get
         {
            return (bool)this["collapseAutoExpandedParents"];
         }
         set
         {
            this["collapseAutoExpandedParents"] = value;
         }
      }
      
      //==========================================================================
   }
}
