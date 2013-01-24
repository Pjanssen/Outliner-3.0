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

   public static IEnumerable<ILayerWrapper> Layers
   {
      get
      {
         IILayerManager layerManager = MaxInterfaces.IILayerManager;
         int layerCount = layerManager.LayerCount;
         List<ILayerWrapper> layers = new List<ILayerWrapper>(layerCount);

         for (int i = 0; i < layerCount; i++)
         {
            IILayer layer = layerManager.GetLayer(i);
            layers.Add(new ILayerWrapper(layer));
         }

         return layers;
      }
   }

   public static INodeWrapper SceneRoot
   {
      get
      {
         return new INodeWrapper(MaxInterfaces.COREInterface.RootNode);
      }
   }

   public static IEnumerable<INodeWrapper> RootObjects
   {
      get
      {
         IINode rootNode = MaxInterfaces.COREInterface.RootNode;
         Int32 nodeCount = rootNode.NumberOfChildren;
         List<INodeWrapper> nodes = new List<INodeWrapper>(nodeCount);

         for (int i = 0; i < nodeCount; i++)
         {
            nodes.Add(new INodeWrapper(rootNode.GetChildNode(i)));
         }

         return nodes;
      }
   }

   public static IEnumerable<INodeWrapper> AllObjects
   {
      get
      {
         return GetChildObjects(MaxInterfaces.COREInterface.RootNode);
      }
   }

   private static IEnumerable<INodeWrapper> GetChildObjects(IINode node)
   {
      for (int i = 0; i < node.NumberOfChildren; i++)
      {
         IINode childNode = node.GetChildNode(i);
         yield return new INodeWrapper(childNode);

         foreach (INodeWrapper child in GetChildObjects(childNode))
         {
            yield return child;
         }
      }
   }
}
}
