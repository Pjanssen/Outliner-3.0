using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Scene
{
   public static class IMaxNodeExtensions
   {
      /// <summary>
      /// Extracts the base objects from a list of IMaxNodes.
      /// </summary>
      public static IEnumerable<Object> GetBaseObjects(this IEnumerable<IMaxNode> nodes)
      {
         return nodes.Select(n => n.BaseObject);
      }

      /// <summary>
      /// Converts an IEnumerable of IMaxNodes to an IINodeTab.
      /// </summary>
      public static IINodeTab ToIINodeTab(this IEnumerable<IMaxNode> nodes)
      {
         return IINodeHelpers.ToIINodeTab(nodes.GetBaseObjects());
      }

      /// <summary>
      /// Returns true if the supplied node is a selected node, or a parent of a selected node.
      /// </summary>
      public static Boolean IsParentOfSelected(this IMaxNode node)
      {
         Throw.IfArgumentIsNull(node, "node");

         if (node.IsSelected)
            return true;

         foreach (IMaxNode child in node.ChildNodes)
         {
            if (child.IsSelected || child.IsParentOfSelected())
               return true;
         }

         return false;
      }
   }
}
