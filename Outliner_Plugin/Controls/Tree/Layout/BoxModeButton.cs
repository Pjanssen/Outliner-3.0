using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Controls.Tree.Layout
{
   public class BoxModeButton : AnimatablePropertyButton
   {
      public BoxModeButton() 
         : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.BoxMode)) { }

      protected override MaxUtils.AnimatableProperty Property
      {
         get { return MaxUtils.AnimatableProperty.BoxMode; }
      }

      protected override string ToolTipEnabled
      {
         get { return OutlinerResources.Tooltip_BoxMode; }
      }

      //TODO: Test if this override is needed?
      protected override System.Drawing.Image ImageEnabled
      {
         get { return OutlinerResources.button_boxmode; }
      }

      public override TreeNodeLayoutItem Copy()
      {
         BoxModeButton newItem = new BoxModeButton();

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
