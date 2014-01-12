using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.NodeSorters;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Controls.Tree.Layout;

namespace PJanssen.Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
[LocalizedDisplayName(typeof(Resources), "Str_FreezeButton")]
public class FreezeButton : NodePropertyButton
{
   public FreezeButton()
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Freeze)) { }

   protected override NodeProperty Property
   {
      get { return NodeProperty.IsFrozen; }
   }

   protected override SetNodePropertyCommand<Boolean> CreateCommand(IEnumerable<IMaxNode> nodes, bool newValue)
   {
      return new FreezeCommand(nodes, newValue);
   }

   protected override string ToolTipEnabled
   {
      get { return Resources.Tooltip_Unfreeze; }
   }

   protected override string ToolTipDisabled
   {
      get { return Resources.Tooltip_Freeze; }
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
