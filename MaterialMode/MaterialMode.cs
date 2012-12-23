using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using Outliner.Modes;
using Outliner.Controls.Tree;

namespace Outliner.MaterialMode
{
   [OutlinerPlugin(OutlinerPluginType.TreeMode)]
   [LocalizedDisplayName(typeof(Resources), "Mode_Materials")]
   public class MaterialMode : TreeMode
   {
      public MaterialMode(TreeView tree) : base(tree) 
      {
         //this.PermanentFilters.Add(new UnassignedMaterialFilter());
      }

      protected override void FillTree()
      {
         throw new NotImplementedException();
      }
   }
}
