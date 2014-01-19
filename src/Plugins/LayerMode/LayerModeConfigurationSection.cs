using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.Modes.Layer
{
   /// <summary>
   /// Represents the configuration properties for the LayerMode.
   /// </summary>
   public sealed class LayerModeConfigurationSection : ConfigurationSection
   {
      //==========================================================================

      /// <summary>
      /// Gets or sets whether the nodes in a group should be shown in the TreeView.
      /// </summary>
      [ConfigurationProperty("showGroupContents", DefaultValue = true)]
      public bool ShowGroupContents
      {
         get
         {
            return (bool)this["showGroupContents"];
         }
         set
         {
            this["showGroupContents"] = value;
         }
      }

      //==========================================================================
   }
}
