using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.Plugins;

namespace PJanssen.Outliner.LayerTools
{
   internal class SetLayerParentRestoreObj : RestoreObj
   {
      private IILayer layer;
      private IILayer parent;
      private Boolean updateProperties;
      private IILayer prevParent;

      public SetLayerParentRestoreObj(IILayer layer, IILayer parent, Boolean updateProperties)
      {
         Throw.IfNull(layer, "layer");

         this.layer = layer;
         this.parent = parent;
         this.updateProperties = updateProperties;
      }

      public override void Redo()
      {
         this.prevParent = NestedLayers.GetParent(layer);
         NestedLayers.SetParent(this.layer, this.parent, this.updateProperties);
      }

      public override void Restore(bool isUndo)
      {
         NestedLayers.SetParent(this.layer, this.prevParent, this.updateProperties);
      }
   }
}
