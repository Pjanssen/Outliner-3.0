using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.Configuration
{
   /// <summary>
   /// Defines the UI categories for FilterConfigurations.
   /// </summary>
   [Flags]
   public enum FilterCategory
   {
      /// <summary>
      /// FilterConfigurations with the Hidden category will not be shown in the UI.
      /// </summary>
      Hidden     = 0x00,

      /// <summary>
      /// Defines that the filter will operate on the node's type.
      /// </summary>
      Classes    = 0x01,

      /// <summary>
      /// Defines that the filter will operate on one or more property of the node.
      /// </summary>
      Properties = 0x02,

      /// <summary>
      /// A custom filter, implemented as a plugin by the user.
      /// </summary>
      Custom     = 0x04,

      /// <summary>
      /// All filter categories.
      /// </summary>
      All        = 0xFF
   }
}
