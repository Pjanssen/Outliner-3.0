using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Scene
{
public static class MaxScene
{
   public static Int32 LayerCount
   {
      get
      {
         return MaxInterfaces.IILayerManager.LayerCount;
      }
   }

   public static IEnumerable<IILayerWrapper> Layers
   {
      get
      {
         IILayerManager layerManager = MaxInterfaces.IILayerManager;
         int layerCount = layerManager.LayerCount;
         List<IILayerWrapper> layers = new List<IILayerWrapper>(layerCount);

         for (int i = 0; i < layerCount; i++)
         {
            IILayer layer = layerManager.GetLayer(i);
            layers.Add(new IILayerWrapper(layer));
         }

         return layers;
      }
   }

   public static IEnumerable<IINodeWrapper> RootObjects
   {
      get
      {
         IINode rootNode = MaxInterfaces.COREInterface.RootNode;
         Int32 nodeCount = rootNode.NumberOfChildren;
         List<IINodeWrapper> nodes = new List<IINodeWrapper>(nodeCount);

         for (int i = 0; i < nodeCount; i++)
         {
            nodes.Add(new IINodeWrapper(rootNode.GetChildNode(i)));
         }

         return nodes;
      }
   }

   public static IEnumerable<IINodeWrapper> Objects
   {
      get
      {
         throw new NotImplementedException();
      }
   }

   public static IEnumerable<SelectionSetWrapper> SelectionSets
   {
      get
      {
         throw new NotImplementedException();
      }
   }
}
}
