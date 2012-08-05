using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.NodeSorters;
using MaxUtils;

namespace Outliner.Controls.Tree.Layout
{
public class FreezeButton : AnimatablePropertyButton
{
   public FreezeButton() { }

   protected override AnimatableProperty Property
   {
      get { return AnimatableProperty.IsFrozen; }
   }

   protected override SetNodePropertyCommand<Boolean> CreateCommand(IEnumerable<IMaxNodeWrapper> nodes, bool newValue)
   {
      return new FreezeCommand(nodes, newValue);
   }

   protected override string ToolTipEnabled
   {
      get { return OutlinerResources.Tooltip_Unfreeze; }
   }

   protected override string ToolTipDisabled
   {
      get { return OutlinerResources.Tooltip_Freeze; }
   }

   protected override System.Drawing.Image ImageEnabled
   {
      get { return OutlinerResources.button_freeze; }
   }
}
}
