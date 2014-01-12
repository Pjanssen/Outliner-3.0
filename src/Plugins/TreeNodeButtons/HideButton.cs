using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.NodeSorters;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
public class HideButton : NodePropertyButton
{
   public HideButton()
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Hide)) { }

   protected override NodeProperty Property
   {
      get { return NodeProperty.IsHidden; }
   }

   protected override SetNodePropertyCommand<Boolean> CreateCommand(IEnumerable<IMaxNode> nodes, bool newValue)
   {
      return new HideCommand(nodes, newValue);
   }

   protected override string ToolTipEnabled
   {
      get { return Resources.Tooltip_Unhide; }
   }

   protected override string ToolTipDisabled
   {
      get { return Resources.Tooltip_Hide; }
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
