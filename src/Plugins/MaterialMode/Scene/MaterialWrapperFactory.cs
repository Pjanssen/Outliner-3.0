using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Scene
{
   /// <summary>
   /// A factory class which creates an IMaxNode for an IMtl object.
   /// </summary>
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public class MaterialWrapperFactory : IMaxNodeFactory
   {
      /// <summary>
      /// Creates an IMaxNode for an IMtl object.
      /// </summary>
      /// <param name="baseNode">The 3dsMax node to create an IMaxNode for.</param>
      public IMaxNode CreateMaxNode(object baseNode)
      {
         IMtl mtl = baseNode as IMtl;
         if (mtl != null)
            return new MaterialWrapper(mtl);

         ITexmap texmap = baseNode as ITexmap;
         if (texmap != null)
            return new TextureMapWrapper(texmap);

         return null;
      }

      /// <summary>
      /// Registers the factory in the Outliner plugin system.
      /// </summary>
      [OutlinerPluginStart]
      public static void RegisterFactory()
      {
         MaxNodeWrapper.RegisterMaxNodeFactory(new MaterialWrapperFactory());
      }
   }
}
