using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Outliner.Controls
{
   public class OutlinerColorTable : ProfessionalColorTable
   {
      public override Color ButtonCheckedHighlight
      {
         get { return Color.FromArgb(46, 56, 103); }
      }
      public override Color ButtonCheckedHighlightBorder
      {
         get { return Color.FromArgb(46, 56, 103); }
      }
      public override Color ButtonCheckedGradientBegin
      {
         get { return Color.FromArgb(136, 174, 225); }
      }
      public override Color ButtonCheckedGradientMiddle
      {
         get { return Color.FromArgb(108, 139, 183); }
      }
      public override Color ButtonCheckedGradientEnd
      {
         get { return Color.FromArgb(79, 103, 140); }
      }

      public override Color ButtonSelectedBorder
      {
         get { return Color.FromArgb(43, 43, 43); }
      }
      public override Color ButtonSelectedHighlightBorder
      {
         get { return Color.FromArgb(46, 56, 103); }
      }
      public override Color ButtonSelectedGradientBegin
      {
         get { return Color.FromArgb(141, 141, 141); }
      }
      public override Color ButtonSelectedGradientMiddle
      {
         get { return Color.FromArgb(124, 124, 124); }
      }
      public override Color ButtonSelectedGradientEnd
      {
         get { return Color.FromArgb(107, 107, 107); }
      }

      public override Color ButtonPressedBorder
      {
         get { return Color.FromArgb(46, 56, 103); }
      }
      public override Color ButtonPressedHighlightBorder
      {
         get { return Color.FromArgb(46, 56, 103); }
      }
      public override Color ButtonPressedGradientBegin
      {
         get { return Color.FromArgb(96, 96, 96); }
      }
      public override Color ButtonPressedGradientMiddle
      {
         get { return Color.FromArgb(79, 79, 79); }
      }
      public override Color ButtonPressedGradientEnd
      {
         get { return Color.FromArgb(62, 62, 62); }
      }

      public override Color MenuBorder
      {
         get { return Color.FromArgb(24, 24, 24); }
      }
      public override Color ToolStripBorder
      {
         get { return Color.FromArgb(24, 24, 24); }
      }
      public override Color ToolStripDropDownBackground
      {
         get { return Color.FromArgb(93, 93, 93); }
      }

      public override Color SeparatorLight
      {
         get { return Color.Empty; }
      }
      public override Color SeparatorDark
      {
         get { return Color.FromArgb(55, 55, 55); }
      }
   }
}
