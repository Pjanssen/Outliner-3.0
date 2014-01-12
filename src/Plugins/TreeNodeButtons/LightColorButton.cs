using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;

namespace PJanssen.Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
[LocalizedDisplayName(typeof(Resources), "Str_LightColorButton")]
public class LightColorButton : ColorButton
{
   public LightColorButton()
   {
      this.ButtonWidth = 12;
      this.ButtonHeight = 8;
   }

   public override TreeNodeLayoutItem Copy()
   {
      LightColorButton newItem = new LightColorButton();

      newItem.PaddingLeft = this.PaddingLeft;
      newItem.PaddingRight = this.PaddingRight;
      newItem.VisibleTypes = this.VisibleTypes;
      newItem.ButtonHeight = this.ButtonHeight;
      newItem.ButtonWidth = this.ButtonWidth;

      return newItem;
   }

   protected override Color GetNodeColor(IMaxNode node)
   {
      INodeWrapper inode = node as INodeWrapper;
      if (inode == null)
         return Color.Empty;

      ILightObject light = inode.INode.ObjectRef as ILightObject;
      if (light == null)
         return Color.Empty;
   
      return Colors.FromMaxColor(light.GetRGBColor(0, MaxInterfaces.IntervalForever));
   }

   protected override void PreviewNodeColor(IEnumerable<IMaxNode> maxNodes, Color color)
   {
      IPoint3 colorPoint3 = MaxInterfaces.Global.Point3.Create(color.B / 255f, color.G / 255f, color.R / 255f);

      foreach (INodeWrapper node in maxNodes.OfType<INodeWrapper>())
      {
         ILightObject light = node.INode.ObjectRef as ILightObject;
         if (light == null)
            continue;

         light.SetRGBColor(0, colorPoint3);
      }
   }

   protected override void CommitNodeColor(IEnumerable<IMaxNode> maxNodes, Color color)
   {
      SetLightColorCommand cmd = new SetLightColorCommand(maxNodes, color);
      cmd.Execute();
   }
}
}
