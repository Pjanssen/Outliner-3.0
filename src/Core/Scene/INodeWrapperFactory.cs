using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Plugins;
using PJanssen;

namespace Outliner.Scene
{
   /// <summary>
   /// A factory class which creates an INodeWrapper for an INode object.
   /// </summary>
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public class INodeWrapperFactory : IMaxNodeFactory
   {
      public IMaxNode CreateMaxNode(object baseNode)
      {
         Throw.IfNull(baseNode, "baseNode");

         IINode inode = baseNode as IINode;
         if (inode != null)
            return new INodeWrapper(inode);

         return null;
      }

      [OutlinerPluginStart]
      public static void RegisterFactory()
      {
         MaxNodeWrapper.RegisterMaxNodeFactory(new INodeWrapperFactory());
      }
   }
}
