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
   public FreezeButton() 
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Freeze)) { }

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


   public override TreeNodeLayoutItem Copy()
   {
      FreezeButton newItem = new FreezeButton();

      newItem.PaddingLeft = this.PaddingLeft;
      newItem.PaddingRight = this.PaddingRight;
      newItem.VisibleTypes = this.VisibleTypes;
      newItem.InvertBehavior = this.InvertBehavior;
      newItem.imageDisabled = this.imageDisabled;
      newItem.imageDisabled_Filtered = this.imageDisabled_Filtered;
      newItem.imageEnabled = this.imageEnabled;
      newItem.imageEnabled_Filtered = this.imageEnabled_Filtered;

      return newItem;
   }
}
}
