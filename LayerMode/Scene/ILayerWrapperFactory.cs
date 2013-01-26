using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Plugins;

namespace Outliner.Scene
{
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public class ILayerWrapperFactory : IMaxNodeFactory
   {
      public IMaxNode CreateMaxNode(object baseNode)
      {
         IILayer ilayer = baseNode as IILayer;
         if (ilayer != null)
            return new ILayerWrapper(ilayer);

         IILayerProperties ilayerProperties = baseNode as IILayerProperties;
         if (ilayerProperties != null)
            return new ILayerWrapper(ilayerProperties);

         return null;
      }

      [OutlinerPluginStart]
      public static void RegisterFactory()
      {
         MaxNodeWrapper.RegisterMaxNodeFactory(new ILayerWrapperFactory());
      }
   }
}
