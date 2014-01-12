using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Commands
{
   /// <summary>
   /// Freezes or unfreezes nodes in the 3dsmax scene.
   /// </summary>
   public class FreezeCommand : SetNodePropertyCommand<Boolean>
   {
      /// <summary>
      /// Initializes a new instance of the FreezeCommand class.
      /// </summary>
      /// <param name="nodes">The nodes to freeze or unfreeze.</param>
      /// <param name="newValue">The new freeze value.</param>
      public FreezeCommand(IEnumerable<IMaxNode> nodes, Boolean newValue)
         : base(nodes, NodeProperty.IsFrozen, newValue) { }

      public override string Description
      {
         get { return OutlinerResources.Command_Freeze; }
      }
   }
}
