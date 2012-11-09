using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.MaxUtils;
using Outliner.Commands;
using Outliner.Scene;

namespace Outliner.Controls.ContextMenu
{
public class NodePropertyMenuItem : MenuItemData
{
   public NodePropertyMenuItem() : base() 
   {
      this.Property = BooleanNodeProperty.None;
   }

   public NodePropertyMenuItem( String text, String image, Type resType
                              , BooleanNodeProperty property) : base(text, image, resType)
   {
      this.Property = property;
   }

   [XmlAttribute("property")]
   [DefaultValue(BooleanNodeProperty.None)]
   public BooleanNodeProperty Property { get; set; }

   protected override Boolean Enabled( Outliner.Controls.Tree.TreeNode clickedTn
                                     , IEnumerable<IMaxNodeWrapper> context)
   {
      return !context.All(n => n.IsPropertyInherited(this.Property));
   }

   protected override bool Checked( Outliner.Controls.Tree.TreeNode clickedTn
                                  , IEnumerable<Scene.IMaxNodeWrapper> context)
   {
      return context.Any(n => n.GetProperty(this.Property));
   }

   public override void OnClick( Outliner.Controls.Tree.TreeNode clickedTn
                               , IEnumerable<Scene.IMaxNodeWrapper> context)
   {
      Boolean newValue = !this.Checked(clickedTn, context);
      NodeProperty prop = NodePropertyHelpers.ToProperty(this.Property);
      SetNodePropertyCommand<Boolean> cmd = new SetNodePropertyCommand<Boolean>(context, prop, newValue);
      cmd.Execute(true);
   }
}
}
