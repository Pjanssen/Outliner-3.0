using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.IO;
using System.Reflection;
using Autodesk.Max.Plugins;
using MaxUtils;

namespace Outliner.LayerTools
{
public static class NestedLayers
{
   private static uint CID_A = 0x48197F50;
   private static uint CID_B = 0x9D545B8;
   private static IClass_ID classID;

   public const SystemNotificationCode LayerParented = (SystemNotificationCode)0x00000100;

   private enum SubID : uint
   {
      ParentHandle,
      LayerHierarchy
   }

   public enum LayerProperty : uint
   {
      IsHidden = 10,
      IsFrozen,
      BoxMode,
      XRayMtl,
      Renderable
   }

   private static Dictionary<LayerProperty, Func<Boolean, Boolean, Boolean>> propertyOps =
         new Dictionary<LayerProperty, Func<Boolean, Boolean, Boolean>>() {
            {LayerProperty.IsHidden, Functor.Or},
            {LayerProperty.IsFrozen, Functor.Or},
            {LayerProperty.BoxMode, Functor.Or},
            {LayerProperty.XRayMtl, Functor.Or},
            {LayerProperty.Renderable, Functor.And}
         };


   private static Dictionary<GlobalDelegates.Delegate5, SystemNotificationCode> callbacks =
      new Dictionary<GlobalDelegates.Delegate5, SystemNotificationCode>()
      {
         {NestedLayers.FilePostOpen, SystemNotificationCode.FilePostOpen},
         {NestedLayers.FilePreSave, SystemNotificationCode.FilePreSave},
         {NestedLayers.FilePostMerge, SystemNotificationCode.FilePostMerge}
      };

   internal static void Start(IGlobal global)
   {
      NestedLayers.classID = global.Class_ID.Create(CID_A, CID_B);

      foreach (KeyValuePair<GlobalDelegates.Delegate5, SystemNotificationCode> cb in NestedLayers.callbacks)
      {
         global.RegisterNotification(cb.Key, null, cb.Value);
      }
   }

   internal static void Stop(IGlobal global)
   {
      foreach (KeyValuePair<GlobalDelegates.Delegate5, SystemNotificationCode> cb in NestedLayers.callbacks)
      {
         global.UnRegisterNotification(cb.Key, null, cb.Value);
      }
   }


   private static byte[] getAppData(IAnimatable anim, SubID sbid)
   {
      return NestedLayers.getAppData(anim, (uint)sbid);
   }
   private static byte[] getAppData(IAnimatable anim, LayerProperty sbid)
   {
      return NestedLayers.getAppData(anim, (uint)sbid);
   }
   private static byte[] getAppData(IAnimatable anim, uint sbid)
   {
      IAppDataChunk chunk = anim.GetAppDataChunk(classID, SClass_ID.Gup, sbid);
      return (chunk != null) ? chunk.Data : null;
   }

   private static void setAppData(IAnimatable anim, SubID sbid, byte[] data)
   {
      NestedLayers.setAppData(anim, (uint)sbid, data);
   }
   private static void setAppData(IAnimatable anim, LayerProperty sbid, byte[] data)
   {
      NestedLayers.setAppData(anim, (uint)sbid, data);
   }
   private static void setAppData(IAnimatable anim, uint sbid, byte[] data)
   {
      anim.AddAppDataChunk(classID, SClass_ID.Gup, sbid, data);
   }

   private static void removeAppData(IAnimatable anim, SubID sbid)
   {
      NestedLayers.removeAppData(anim, (uint)sbid);
   }
   private static void removeAppData(IAnimatable anim, LayerProperty sbid)
   {
      NestedLayers.removeAppData(anim, (uint)sbid);
   }
   private static void removeAppData(IAnimatable anim, uint sbid)
   {
      anim.RemoveAppDataChunk(classID, SClass_ID.Gup, sbid);
   }


   /// <summary>
   /// Retrieves the parent of the given layer. Returns null if it is a root layer.
   /// </summary>
   public static IILayer GetParent(IILayer layer)
   {
      if (layer == null)
         return null;

      byte[] data = NestedLayers.getAppData(layer, SubID.ParentHandle);
      if (data == null)
         return null;

      UIntPtr handle = new UIntPtr(BitConverter.ToUInt64(data, 0));
      return MaxInterfaces.Global.Animatable.GetAnimByHandle(handle) as IILayer;
   }

   /// <summary>
   /// Sets the parent of the given layer. Use null to make the layer a root layer.
   /// </summary>
   public static void SetParent(IILayer layer, IILayer parent)
   {
      NestedLayers.SetParent(layer, parent, true);
   }
   private static void SetParent(IILayer layer, IILayer parent, Boolean updateProperties)
   {
      if (layer == null || layer == parent)
         return;
      
      if (parent != null)
      {
         UIntPtr handle = MaxInterfaces.Global.Animatable.GetHandleByAnim(parent);
         byte[] data = BitConverter.GetBytes(handle.ToUInt64());
         NestedLayers.setAppData(layer, SubID.ParentHandle, data);
      }
      else
         NestedLayers.removeAppData(layer, SubID.ParentHandle);

      if (updateProperties)
         NestedLayers.updateProperties(layer);

      MaxInterfaces.Global.BroadcastNotification(LayerParented, layer);
   }

   /// <summary>
   /// Returns a list with all children of a layer.
   /// </summary>
   /// <param name="parent">The parent layer to get the childlayers from.</param>
   /// <param name="recursive">Include the entire layer tree.</param>
   public static List<IILayer> GetChildren(IILayer parent, Boolean recursive)
   {
      List<IILayer> children = new List<IILayer>();
      IILayerManager layerManager = MaxInterfaces.IILayerManager;

      for (int i = 0; i < layerManager.LayerCount; i++)
      {
         IILayer layer = layerManager.GetLayer(i);
         if (NestedLayers.GetParent(layer) == parent)
         {
            children.Add(layer);
            if (recursive)
               children.AddRange(NestedLayers.GetChildren(layer, recursive));
         }
      }

      return children;
   }

   /// <summary>
   /// Tests if a layer is a root layer (i.e. it has no parent layer).
   /// </summary>
   public static Boolean IsRootLayer(IILayer layer)
   {
      return NestedLayers.GetParent(layer) == null;
   }

   /// <summary>
   /// Gets all root layers.
   /// </summary>
   public static IEnumerable<IILayer> RootLayers
   {
      get
      {
         IILayerManager layerManager = MaxInterfaces.IILayerManager;
         List<IILayer> rootLayers = new List<IILayer>(layerManager.LayerCount);
         for (int i = 0; i < layerManager.LayerCount; i++)
         {
            rootLayers.Add(layerManager.GetLayer(i));
         }

         return rootLayers.Where(l => NestedLayers.IsRootLayer(l));
      }
   }

   /// <summary>
   /// Returns the value of a property on the layer.
   /// It will return the layer's own value, regardless of whether it has been overridden by a parent layer.
   /// </summary>
   public static Boolean GetProperty(IILayer layer, LayerProperty prop)
   {
      String propName = Enum.GetName(typeof(LayerProperty), prop);
      PropertyInfo propInfo = typeof(IILayer).GetProperty(propName);
      Boolean ownValue = (Boolean)propInfo.GetValue(layer, null);

      byte[] data = NestedLayers.getAppData(layer, prop);
      if (data != null)
         return BitConverter.ToBoolean(data, 0);
      else
         return ownValue;
   }

   /// <summary>
   /// Sets a layer property and propagates it to its children.
   /// </summary>
   public static void SetProperty(IILayer layer, LayerProperty prop, Boolean value)
   {
      NestedLayers.SetProperty(layer, prop, value, false);
   }

   private static void SetProperty(IILayer layer, LayerProperty prop, Boolean value, Boolean setByParent)
   {
      if (layer == null)
         return;
      
      //Store new value in AppDataChunk.
      String propName = Enum.GetName(typeof(LayerProperty), prop);
      PropertyInfo propInfo = typeof(IILayer).GetProperty(propName);

      if (!setByParent)
         NestedLayers.setAppData(layer, prop, BitConverter.GetBytes(value));

      //Set new value based on value and parent value.
      Boolean ownValue = NestedLayers.GetProperty(layer, prop);
      Boolean parentValue = false;

      IILayer parentLayer = NestedLayers.GetParent(layer);
      if (parentLayer != null)
      {
         parentValue = (Boolean)propInfo.GetValue(parentLayer, null);
      }

      Func<Boolean, Boolean, Boolean> op = Functor.Or;
      NestedLayers.propertyOps.TryGetValue(prop, out op);

      propInfo.SetValue(layer, op(parentValue, ownValue), null);
      //propInfo.SetValue(layer, (parentValue | ownValue), null);

      //Propagate to children.
      List<IILayer> childLayers = NestedLayers.GetChildren(layer, false);
      foreach (IILayer childLayer in childLayers)
      {
         NestedLayers.SetProperty(childLayer, prop, value, true);
      }
   }


   private static void updateProperties(IILayer layer)
   {
      IEnumerable<LayerProperty> layerProps = Enum.GetValues(typeof(LayerProperty))
                                                  .Cast<LayerProperty>();

      IILayer parent = NestedLayers.GetParent(layer);

      foreach (LayerProperty prop in layerProps)
      {
         NestedLayers.SetProperty(layer, prop, NestedLayers.GetProperty(layer, prop));
      }
   }

   private static void updateProperties()
   {
      IEnumerable<IILayer> rootLayers = NestedLayers.RootLayers;
      rootLayers.ForEach(l => NestedLayers.updateProperties(l));
   }


   /// <summary>
   /// Removes all NestedLayer AppDataChunks from the scene.
   /// </summary>
   public static void ClearScene()
   {
      IEnumerable<SubID> subIDs = Enum.GetValues(typeof(SubID)).Cast<SubID>();
      IEnumerable<LayerProperty> layerProps = Enum.GetValues(typeof(LayerProperty))
                                                  .Cast<LayerProperty>();

      IILayerManager layerManager = MaxInterfaces.IILayerManager;

      for (int i = 0; i < layerManager.LayerCount; i++)
      {
         IILayer layer = layerManager.GetLayer(i);

         foreach (SubID subId in subIDs)
            NestedLayers.removeAppData(layer, subId);

         foreach (LayerProperty prop in layerProps)
            NestedLayers.removeAppData(layer, prop);
      }
   }

  

   /// <summary>
   /// Stores the layer hierarchy and properties for each layer, in the LayerHierarchy AppDataChunk.
   /// </summary>
   /// <remarks>
   /// Data format:
   /// Int                  | Bool  | .. | Bool  | String
   /// Number of properties | prop1 | .. | propx | Parent Name
   /// </remarks>
   private static void StoreHierarchy()
   {
      IILayerManager layerManager = MaxInterfaces.IILayerManager;

      for (int i = 0; i < layerManager.LayerCount; i++)
      {
         IILayer layer = layerManager.GetLayer(i);

         if (NestedLayers.IsRootLayer(layer))
            continue;

         using (MemoryStream stream = new MemoryStream())
         using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode))
         {
            //Write layer properties.
            writeProperties(writer, layer);

            //Iterate over all parent layers and store names + properties.
            IILayer parentLayer = NestedLayers.GetParent(layer);
            while (parentLayer != null)
            {
               writer.Write(parentLayer.Name);
               writeProperties(writer, parentLayer);

               parentLayer = NestedLayers.GetParent(parentLayer);
            }

            //Store the data in an AppDataChunk.
            writer.Flush();
            NestedLayers.setAppData(layer, SubID.LayerHierarchy, stream.ToArray());
         }
      }
   }


   /// <summary>
   /// Recreates the ParentHandle and LayerProperties AppDataChunks from the LayerHierarchy.
   /// </summary>
   private static void RebuildHierarchy(Boolean recursive)
   {
      IILayerManager layerManager = MaxInterfaces.IILayerManager;

      for (int i = 0; i < layerManager.LayerCount; i++)
      {
         IILayer layer = layerManager.GetLayer(i);

         byte[] data = NestedLayers.getAppData(layer, SubID.LayerHierarchy);
         if (data == null)
            continue;

         using (MemoryStream stream = new MemoryStream(data))
         using (BinaryReader reader = new BinaryReader(stream, Encoding.Unicode))
         {
            //Restore own properties.
            readProperties(reader, layer);

            //Rebuild hierarchy.
            IILayer currentLayer = layer;
            while (reader.PeekChar() != -1)
            {
               String parentLayerName = reader.ReadString();
               IILayer parentLayer = layerManager.GetLayer(ref parentLayerName);

               if (parentLayer == null)
                  parentLayer = layerManager.CreateLayer(ref parentLayerName);

               NestedLayers.SetParent(currentLayer, parentLayer, false);

               if (reader.PeekChar() != -1)
                  readProperties(reader, parentLayer);

               currentLayer = parentLayer;

               if (!recursive)
                  break;
            }
         }
      }
   }

   private static void writeProperties(BinaryWriter writer, IILayer layer)
   {
      IEnumerable<LayerProperty> layerProps = Enum.GetValues(typeof(LayerProperty))
                                                  .Cast<LayerProperty>();

      writer.Write(layerProps.Count());
      foreach (LayerProperty prop in layerProps)
      {
         writer.Write(NestedLayers.GetProperty(layer, prop));
      }
   }

   private static void readProperties(BinaryReader reader, IILayer layer)
   {
      List<LayerProperty> layerProps = Enum.GetValues(typeof(LayerProperty))
                                           .Cast<LayerProperty>()
                                           .ToList();

      Int32 numProps = reader.ReadInt32();
      for (int p = 0; p < numProps; p++)
      {
         Boolean propValue = reader.ReadBoolean();
         NestedLayers.SetProperty(layer, layerProps[p], propValue);
      }
   }

  

   private static void preSaveCleanup()
   {
      IILayerManager layerManager = MaxInterfaces.IILayerManager;

      for (int i = 0; i < layerManager.LayerCount; i++)
      {
         IILayer layer = layerManager.GetLayer(i);
         NestedLayers.removeAppData(layer, SubID.ParentHandle);
      }
   }

   private static void postLoadCleanup()
   {
      IILayerManager layerManager = MaxInterfaces.IILayerManager;

      for (int i = 0; i < layerManager.LayerCount; i++)
      {
         IILayer layer = layerManager.GetLayer(i);
         NestedLayers.removeAppData(layer, SubID.LayerHierarchy);
      }
   }




   internal static void FilePostOpen(IntPtr param, IntPtr info)
   {
      NestedLayers.RebuildHierarchy(false);
      NestedLayers.postLoadCleanup();
   }

   internal static void FilePreSave(IntPtr param, IntPtr info)
   {
      NestedLayers.StoreHierarchy();
      NestedLayers.preSaveCleanup();
   }

   internal static void FilePostMerge(IntPtr param, IntPtr info)
   {
      NestedLayers.RebuildHierarchy(true);
      NestedLayers.postLoadCleanup();
      NestedLayers.updateProperties();
   }

   
}
}
