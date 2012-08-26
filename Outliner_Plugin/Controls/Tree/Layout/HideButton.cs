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
public class HideButton : AnimatablePropertyButton
{
   public HideButton() 
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Hide)) { }

   protected override AnimatableProperty Property
   {
      get { return AnimatableProperty.IsHidden; }
   }

   protected override SetNodePropertyCommand<Boolean> CreateCommand(IEnumerable<IMaxNodeWrapper> nodes, bool newValue)
   {
      return new HideCommand(nodes, newValue);
   }

   protected override string ToolTipEnabled
   {
      get { return OutlinerResources.Tooltip_Unhide; }
   }

   protected override string ToolTipDisabled
   {
      get { return OutlinerResources.Tooltip_Hide; }
   }

   public override TreeNodeLayoutItem Copy()
   {
      HideButton newItem = new HideButton();

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
