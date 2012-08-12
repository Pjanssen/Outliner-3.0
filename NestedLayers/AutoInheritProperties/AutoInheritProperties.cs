using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.Plugins;
using System.Runtime.InteropServices;
using MaxUtils;

namespace Outliner.LayerTools
{
public static class AutoInheritProperties
{
   private const uint CID_A = 0x1ECA7d7A;
   private const uint CID_B = 0x7B6B1447;
   private static IClass_ID classID;

   public enum NodeLayerProperty : uint
   {
      Color,
      Display,
      GlobalIllumination,
      Motion,
      Render
   }

   private static uint cbKey;
   private static INodeEventCallback cbObject;

   internal static void Start(IGlobal global)
   {
      AutoInheritProperties.classID = global.Class_ID.Create(CID_A, CID_B);

      global.RegisterNotification(layerChanged, null, SystemNotificationCode.NodeLayerChanged);
      
      IISceneEventManager sceneEventMgr = MaxInterfaces.Global.ISceneEventManager;
      cbObject = new LayerCallbacks();
      cbKey = sceneEventMgr.RegisterCallback(cbObject, false, 100, true);
   }

   internal static void Stop(IGlobal global)
   {
      global.UnRegisterNotification(layerChanged, null, SystemNotificationCode.NodeLayerChanged);

      IISceneEventManager sceneEventMgr = MaxInterfaces.Global.ISceneEventManager;
      sceneEventMgr.UnRegisterCallback(cbKey);
      cbObject.Dispose();
      cbObject = null;
   }


   private static void layerChanged(IntPtr param, IntPtr info)
   {
      object callParam = MaxInterfaces.Global.NotifyInfo.Marshal(info).CallParam;
      INodeLayerChangeParams layerChangeParams = callParam as INodeLayerChangeParams;

      IEnumerable<NodeLayerProperty> layerProps = Enum.GetValues(typeof(NodeLayerProperty))
                                                      .Cast<NodeLayerProperty>();

      foreach (NodeLayerProperty prop in layerProps)
      {
         Boolean autoInheritOld = false;
         if (layerChangeParams.OldLayer != null)
            autoInheritOld = GetAutoInherit(layerChangeParams.OldLayer, prop);

         Boolean autoInheritNew = false;
         if (layerChangeParams.NewLayer != null)
            autoInheritNew = GetAutoInherit(layerChangeParams.NewLayer, prop);
         
         if (autoInheritOld || autoInheritNew)
            setNodeInheritProperty(layerChangeParams.Node, prop, true);
      }
   }

   private class LayerCallbacks : INodeEventCallback
   {
      public override void Added(ITab<UIntPtr> nodes)
      {
         IEnumerable<NodeLayerProperty> layerProps = Enum.GetValues(typeof(NodeLayerProperty))
                                                         .Cast<NodeLayerProperty>();

         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            IINodeLayerProperties nodeLayerProps = node.GetInterface(MaxInterfaces.NodeLayerProperties) as IINodeLayerProperties;
            if (nodeLayerProps == null)
               continue;

            String layerName = nodeLayerProps.Layer.Name;
            IILayer layer = MaxInterfaces.IILayerManager.GetLayer(ref layerName);

            if (layer == null)
               continue;

            foreach (NodeLayerProperty prop in layerProps)
            {
               Boolean byLayer = GetAutoInherit(layer, prop);
               if (byLayer)
                  setNodeInheritProperty(node, prop, true);
            }
         }
      }

      public override void CallbackEnd()
      {
         IInterface ip = MaxInterfaces.Global.COREInterface;
         ip.RedrawViews(ip.Time, RedrawFlags.Normal, null);
      }
   }

   private static void setNodeInheritProperty(IINode node, NodeLayerProperty prop, Boolean value)
   {
      IINodeLayerProperties nodeProps = node.GetInterface(MaxInterfaces.NodeLayerProperties) as IINodeLayerProperties;
      if (nodeProps == null)
         return;

      switch (prop)
      {
         case NodeLayerProperty.Color: nodeProps.ColorByLayer = value; break;
         case NodeLayerProperty.Display: nodeProps.DisplayByLayer = value; break;
         case NodeLayerProperty.GlobalIllumination: nodeProps.GlobalIlluminationByLayer = value; break;
         case NodeLayerProperty.Motion: nodeProps.MotionByLayer = value; break;
         case NodeLayerProperty.Render: nodeProps.RenderByLayer = value; break;
      }
   }

   /// <summary>
   /// Gets AutoInherit on a layer for a specific property.
   /// </summary>
   public static Boolean GetAutoInherit(IILayer layer, NodeLayerProperty prop)
   {
      if (layer == null)
         throw new ArgumentNullException("layer");

      IAppDataChunk chunk = layer.GetAppDataChunk(classID, SClass_ID.Gup, (uint)prop);
      return (chunk != null) ? BitConverter.ToBoolean(chunk.Data, 0) : false;
   }

   /// <summary>
   /// Sets AutoInherit on a layer for a specific property.
   /// </summary>
   public static void SetAutoInherit(IILayer layer, NodeLayerProperty prop, Boolean value)
   {
      if (layer == null)
         throw new ArgumentNullException("layer");

      //Set AppDataChunk.
      layer.AddAppDataChunk(classID, SClass_ID.Gup, (uint)prop, BitConverter.GetBytes(value));

      //Update nodes on layer.
      ITab<IINode> nodes = MaxInterfaces.Global.INodeTabNS.Create();
      IILayerProperties layerProperties = MaxInterfaces.IIFPLayerManager.GetLayer(layer.Name);
      layerProperties.Nodes(nodes);

      foreach (IINode node in nodes.ToIEnumerable())
      {
         setNodeInheritProperty(node, prop, value);
      }
   }

   /// <summary>
   /// Removes all AutoInheritProperties AppDataChunks from the scene.
   /// </summary>
   public static void ClearScene()
   {
      IILayerManager layerManager = MaxInterfaces.IILayerManager;
      IEnumerable<NodeLayerProperty> layerProps = Enum.GetValues(typeof(NodeLayerProperty))
                                                      .Cast<NodeLayerProperty>();

      for (int i = 0; i < layerManager.LayerCount; i++)
      {
         IILayer layer = layerManager.GetLayer(i);
         foreach (NodeLayerProperty prop in layerProps)
         {
            layer.RemoveAppDataChunk(classID, SClass_ID.Gup, (uint)prop);
         }
      }
   }
}
}
