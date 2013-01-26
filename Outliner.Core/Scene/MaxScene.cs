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
   /// <summary>
   /// The scene's root object.
   /// </summary>
   public static INodeWrapper SceneRoot
   {
      get
      {
         return new INodeWrapper(MaxInterfaces.COREInterface.RootNode);
      }
   }

   /// <summary>
   /// Gets all objects without a parent in the scene.
   /// </summary>
   public static IEnumerable<INodeWrapper> RootObjects
   {
      get
      {
         IINode rootNode = MaxInterfaces.COREInterface.RootNode;
         Int32 nodeCount = rootNode.NumberOfChildren;
         for (int i = 0; i < nodeCount; i++)
         {
            yield return new INodeWrapper(rootNode.GetChildNode(i));
         }
      }
   }

   /// <summary>
   /// Gets all objects in the scene in a flat list.
   /// </summary>
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

   /// <summary>
   /// Gets the number of layers in the scene.
   /// </summary>
   public static Int32 LayerCount
   {
      get
      {
         return MaxInterfaces.IILayerManager.LayerCount;
      }
   }

   /// <summary>
   /// Gets all layers in the scene.
   /// </summary>
   public static IEnumerable<IMaxNode> Layers
   {
      get
      {
         IILayerManager layerManager = MaxInterfaces.IILayerManager;
         int layerCount = layerManager.LayerCount;

         for (int i = 0; i < layerCount; i++)
         {
            IILayer layer = layerManager.GetLayer(i);
            yield return MaxNodeWrapper.Create(layer);
         }
      }
   }
}
}
