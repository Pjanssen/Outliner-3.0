   using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Scene
{
   /// <summary>
   /// A factory class which creates an IMaxNode for an ILayer or ILayerProperties object.
   /// </summary>
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public class ILayerWrapperFactory : IMaxNodeFactory
   {
      /// <summary>
      /// Creates an IMaxNode for an ILayer object.
      /// </summary>
      /// <param name="baseNode">The 3dsMax node to create an IMaxNode for.</param>
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

      /// <summary>
      /// Registers the factory in the Outliner plugin system.
      /// </summary>
      [OutlinerPluginStart]
      public static void RegisterFactory()
      {
         MaxNodeWrapper.RegisterMaxNodeFactory(new ILayerWrapperFactory());
      }
   }
}
