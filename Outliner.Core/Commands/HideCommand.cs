using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
   /// <summary>
   /// Hides or unhides nodes in the 3dsmax scene.
   /// </summary>
   public class HideCommand : SetNodePropertyCommand<Boolean>
   {
      /// <summary>
      /// Initializes a new instance of the HideCommand class.
      /// </summary>
      /// <param name="nodes">The IMaxNodes to hide or unhide.</param>
      /// <param name="newValue">The new hidden value.</param>
      public HideCommand(IEnumerable<IMaxNode> nodes, Boolean newValue) 
         : base(nodes, NodeProperty.IsHidden, newValue) { }

      public override string Description
      {
         get { return OutlinerResources.Command_Hide; }
      }
   }
}
