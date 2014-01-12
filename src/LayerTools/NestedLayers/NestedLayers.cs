using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.IO;
using System.Reflection;
using Autodesk.Max.Plugins;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.LayerTools
{
/// <summary>
/// Provides methods to manage layer nesting.
/// </summary>
public static class NestedLayers
{
   private const uint CID_A = 0x48197F50;
   private const uint CID_B = 0x9D545B8;
   private static IClass_ID classID;
   private const uint PropertySbidOffset = 10;

   private enum SubID : uint
   {
      ParentHandle,
      LayerHierarchy
   }


   private static Dictionary<BooleanNodeProperty, BinaryPredicate<Boolean>> propertyOps =
         new Dictionary<BooleanNodeProperty, BinaryPredicate<Boolean>>() {
            {BooleanNodeProperty.IsHidden, Functor.Or},
            {BooleanNodeProperty.IsFrozen, Functor.Or},
            {BooleanNodeProperty.BoxMode, Functor.Or},
            {BooleanNodeProperty.SeeThrough, Functor.Or},
            {BooleanNodeProperty.Renderable, Functor.And}
         };


   private static Dictionary<GlobalDelegates.Delegate5, SystemNotificationCode> callbacks =
      new Dictionary<GlobalDelegates.Delegate5, SystemNotificationCode>()
      {
         {NestedLayers.FilePostOpen, SystemNotificationCode.FilePostOpen},
         {NestedLayers.FilePreSave, SystemNotificationCode.FilePreSave},
         {NestedLayers.FilePostMerge, SystemNotificationCode.FilePostMerge}
      };

   internal static void Start()
   {
      NestedLayers.classID = MaxInterfaces.Global.Class_ID.Create(CID_A, CID_B);

      foreach (KeyValuePair<GlobalDelegates.Delegate5, SystemNotificationCode> cb in NestedLayers.callbacks)
      {
         MaxInterfaces.Global.RegisterNotification(cb.Key, null, cb.Value);
      }
   }

   internal static void Stop()
   {
      foreach (KeyValuePair<GlobalDelegates.Delegate5, SystemNotificationCode> cb in NestedLayers.callbacks)
      {
         MaxInterfaces.Global.UnRegisterNotification(cb.Key, null, cb.Value);
      }
   }


   private static byte[] getAppData(IAnimatable anim, SubID sbid)
   {
      return NestedLayers.getAppData(anim, (uint)sbid);
   }
   private static byte[] getAppData(IAnimatable anim, BooleanNodeProperty sbid)
   {
      return NestedLayers.getAppData(anim, PropertySbidOffset + (uint)sbid);
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
   private static void setAppData(IAnimatable anim, BooleanNodeProperty sbid, byte[] data)
   {
      NestedLayers.setAppData(anim, PropertySbidOffset + (uint)sbid, data);
   }
   private static void setAppData(IAnimatable anim, uint sbid, byte[] data)
   {
      NestedLayers.removeAppData(anim, sbid);
      anim.AddAppDataChunk(classID, SClass_ID.Gup, sbid, data);
   }

   private static void removeAppData(IAnimatable anim, SubID sbid)
   {
      NestedLayers.removeAppData(anim, (uint)sbid);
   }
   private static void removeAppData(IAnimatable anim, BooleanNodeProperty sbid)
   {
      NestedLayers.removeAppData(anim, PropertySbidOffset + (uint)sbid);
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
      SetLayerParentRestoreObj restoreObj = new SetLayerParentRestoreObj(layer, parent, true);
      if (MaxInterfaces.Global.TheHold.Holding)
      {
         MaxInterfaces.Global.TheHold.Put(restoreObj);
      }
      restoreObj.Redo();
      //NestedLayers.SetParent(layer, parent, true);
   }

   internal static void SetParent(IILayer layer, IILayer parent, Boolean updateProperties)
   {
      if (layer == null || layer == parent)
         return;

      NestedLayers.removeAppData(layer, SubID.ParentHandle);

      if (parent != null)
      {
         UIntPtr handle = MaxInterfaces.Global.Animatable.GetHandleByAnim(parent);
         byte[] data = BitConverter.GetBytes(handle.ToUInt64());
         NestedLayers.setAppData(layer, SubID.ParentHandle, data);
      }

      if (updateProperties)
         NestedLayers.updateProperties(layer, true);

      MaxInterfaces.Global.BroadcastNotification(LayerNotificationCode.LayerParented, layer);
   }

   /// <summary>
   /// Returns a list with all direct children of a layer (non-recursive).
   /// </summary>
   /// <param name="parent">The parent layer to get the childlayers from.</param>
   public static IEnumerable<IILayer> GetChildren(IILayer parent)
   {
      return NestedLayers.GetChildren(parent, false);
   }

   /// <summary>
   /// Returns a list with all children of a layer.
   /// </summary>
   /// <param name="parent">The parent layer to get the childlayers from.</param>
   /// <param name="recursive">Include the entire layer tree.</param>
   public static IEnumerable<IILayer> GetChildren(IILayer parent, Boolean recursive)
   {
      List<IILayer> children = new List<IILayer>();
      IILayerManager layerManager = MaxInterfaces.IILayerManager;

      for (int i = 0; i < layerManager.LayerCount; i++)
      {
         IILayer layer = layerManager.GetLayer(i);
         IILayer layerParent = NestedLayers.GetParent(layer);
         if ((parent == null && layerParent == null) || 
             (parent != null && layerParent != null && layerParent.Handle == parent.Handle))
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
         return NestedLayers.GetChildren(null, false);
      }
   }

   private static Boolean GetLayerProperty(IILayer layer, BooleanNodeProperty property)
   {
      switch (property)
      {
         case BooleanNodeProperty.IsHidden: return layer.IsHidden;
         case BooleanNodeProperty.IsFrozen: return layer.IsFrozen;
         case BooleanNodeProperty.SeeThrough: return layer.XRayMtl;
         case BooleanNodeProperty.BoxMode: return layer.BoxMode;
         case BooleanNodeProperty.BackfaceCull: return layer.BackCull;
         case BooleanNodeProperty.AllEdges: return layer.AllEdges;
         case BooleanNodeProperty.VertexTicks: return layer.VertTicks;
         case BooleanNodeProperty.Trajectory: return layer.Trajectory_;
         case BooleanNodeProperty.IgnoreExtents: return layer.IgnoreExtents;
         case BooleanNodeProperty.FrozenInGray: return layer.ShowFrozenWithMtl;
         case BooleanNodeProperty.Renderable: return layer.Renderable;
         case BooleanNodeProperty.InheritVisibility: return layer.InheritVisibility;
         case BooleanNodeProperty.PrimaryVisibility: return layer.PrimaryVisibility;
         case BooleanNodeProperty.SecondaryVisibility: return layer.SecondaryVisibility;
         case BooleanNodeProperty.ReceiveShadows: return layer.RcvShadows;
         case BooleanNodeProperty.CastShadows: return layer.CastShadows;
         case BooleanNodeProperty.ApplyAtmospherics: return layer.ApplyAtmospherics;
         case BooleanNodeProperty.RenderOccluded: return layer.RenderOccluded;
         default: return false;
      }
   }

   private static void SetLayerProperty(IILayer layer, BooleanNodeProperty property, Boolean value)
   {
      switch (property)
      {
         case BooleanNodeProperty.IsHidden:
            layer.IsHidden = value;
            break;
         case BooleanNodeProperty.IsFrozen:
            layer.IsFrozen = value;
            break;
         case BooleanNodeProperty.SeeThrough:
            layer.XRayMtl = value;
            break;
         case BooleanNodeProperty.BoxMode:
            layer.BoxMode = value;
            break;
         case BooleanNodeProperty.BackfaceCull:
            layer.BackCull = value;
            break;
         case BooleanNodeProperty.AllEdges:
            layer.AllEdges = value;
            break;
         case BooleanNodeProperty.VertexTicks:
            layer.VertTicks = value;
            break;
         case BooleanNodeProperty.Trajectory:
            layer.Trajectory(value, false);
            break;
         case BooleanNodeProperty.IgnoreExtents:
            layer.IgnoreExtents = value;
            break;
         case BooleanNodeProperty.FrozenInGray:
            layer.ShowFrozenWithMtl = value;
            break;
         case BooleanNodeProperty.Renderable:
            layer.Renderable = value;
            break;
         case BooleanNodeProperty.InheritVisibility:
            layer.InheritVisibility = value;
            break;
         case BooleanNodeProperty.PrimaryVisibility:
            layer.PrimaryVisibility = value;
            break;
         case BooleanNodeProperty.SecondaryVisibility:
            layer.SecondaryVisibility = value;
            break;
         case BooleanNodeProperty.ReceiveShadows:
            layer.RcvShadows = value;
            break;
         case BooleanNodeProperty.CastShadows:
            layer.CastShadows = value;
            break;
         case BooleanNodeProperty.ApplyAtmospherics:
            layer.ApplyAtmospherics = value;
            break;
         case BooleanNodeProperty.RenderOccluded:
            layer.RenderOccluded = value;
            break;
         default:
            break;
      }
   }

   /// <summary>
   /// Returns the value of a property on the layer.
   /// It will return the layer's own value, regardless of whether it has been overridden by a parent layer.
   /// </summary>
   public static Boolean GetProperty(IILayer layer, BooleanNodeProperty property)
   {
      byte[] data = NestedLayers.getAppData(layer, property);
      if (data != null)
         return BitConverter.ToBoolean(data, 0);
      else
         return NestedLayers.GetLayerProperty(layer, property);
   }

   /// <summary>
   /// Sets a layer property and propagates it to its children.
   /// </summary>
   public static void SetProperty(IILayer layer, BooleanNodeProperty prop, Boolean value)
   {
      NestedLayers.SetProperty(layer, prop, value, false);
   }

   private static void SetProperty(IILayer layer, BooleanNodeProperty property, Boolean value, Boolean setByParent)
   {
      if (layer == null)
         return;
      
      //Store new value in AppDataChunk.
      if (!setByParent)
         NestedLayers.setAppData(layer, property, BitConverter.GetBytes(value));

      //Set new value based on value and parent value.
      Boolean ownValue = NestedLayers.GetProperty(layer, property);

      Boolean newValue = ownValue;
      IILayer parentLayer = NestedLayers.GetParent(layer);
      if (parentLayer != null)
      {
         Boolean parentValue = NestedLayers.GetLayerProperty(parentLayer, property);

         BinaryPredicate<Boolean> pred = Functor.Or;
         if (!NestedLayers.propertyOps.TryGetValue(property, out pred))
            pred = Functor.Or;
         newValue = pred(ownValue, parentValue);
      }

      NestedLayers.SetLayerProperty(layer, property, newValue);

      //Broadcast notification.
      if (property == BooleanNodeProperty.IsHidden)
      {
         MaxInterfaces.Global.BroadcastNotification(SystemNotificationCode.LayerHiddenStateChanged, layer);
      }
      else if (property == BooleanNodeProperty.IsFrozen)
      {
         MaxInterfaces.Global.BroadcastNotification(SystemNotificationCode.LayerFrozenStateChanged, layer);
      }
      else
      {
         LayerPropertyChangedParam parameters = new LayerPropertyChangedParam(layer, NodeProperties.ToProperty(property));
         MaxInterfaces.Global.BroadcastNotification(LayerNotificationCode.LayerPropertyChanged, parameters);
      }

      //Propagate to children.
      IEnumerable<IILayer> childLayers = NestedLayers.GetChildren(layer, false);
      foreach (IILayer childLayer in childLayers)
      {
         NestedLayers.SetProperty(childLayer, property, value, true);
      }
   }

   /// <summary>
   /// Tests if the given property on the given layer is inherited from a parent layer.
   /// </summary>
   public static Boolean IsPropertyInherited(IILayer layer, BooleanNodeProperty prop)
   {
      if (layer == null)
         return false;

      Boolean ownValue = NestedLayers.GetProperty(layer, prop);
      Boolean actualValue = NestedLayers.GetLayerProperty(layer, prop);

      return (ownValue != actualValue);
   }

   private static void updateProperties(IILayer layer, Boolean recursive)
   {
      IEnumerable<BooleanNodeProperty> layerProps = Enum.GetValues(typeof(BooleanNodeProperty))
                                                        .Cast<BooleanNodeProperty>()
                                                        .Where(p => p != BooleanNodeProperty.None);

      foreach (BooleanNodeProperty prop in layerProps)
      {
         NestedLayers.SetProperty(layer, prop, NestedLayers.GetProperty(layer, prop));
      }

      if (recursive)
      {
         foreach (IILayer child in NestedLayers.GetChildren(layer, false))
            NestedLayers.updateProperties(child, recursive);
      }
   }

   private static void updateProperties()
   {
      foreach (IILayer layer in NestedLayers.RootLayers)
         NestedLayers.updateProperties(layer, true);
   }


   /// <summary>
   /// Removes all NestedLayer AppDataChunks from the scene.
   /// </summary>
   public static void ClearScene()
   {
      IEnumerable<SubID> subIDs = Enum.GetValues(typeof(SubID)).Cast<SubID>();
      IEnumerable<BooleanNodeProperty> layerProps = Enum.GetValues(typeof(BooleanNodeProperty))
                                                        .Cast<BooleanNodeProperty>();

      IILayerManager layerManager = MaxInterfaces.IILayerManager;

      for (int i = 0; i < layerManager.LayerCount; i++)
      {
         IILayer layer = layerManager.GetLayer(i);

         foreach (SubID subId in subIDs)
            NestedLayers.removeAppData(layer, subId);

         foreach (BooleanNodeProperty prop in layerProps)
            NestedLayers.removeAppData(layer, prop);
      }
   }


   #region Store and rebuild layer hierarchy

   private static void FilePostOpen(IntPtr param, IntPtr info)
   {
      NestedLayers.RebuildHierarchy(false);
      NestedLayers.postLoadCleanup();
   }

   private static void FilePreSave(IntPtr param, IntPtr info)
   {
      NestedLayers.StoreHierarchy();
      NestedLayers.preSaveCleanup();
   }

   private static void FilePostMerge(IntPtr param, IntPtr info)
   {
      NestedLayers.RebuildHierarchy(true);
      NestedLayers.postLoadCleanup();
      NestedLayers.updateProperties();
   }


   // Stores the layer hierarchy and properties for each layer, in the LayerHierarchy AppDataChunk.
   // Data format:
   //    Int                  | Bool  | .. | Bool  | String
   //    Number of properties | prop1 | .. | propx | Parent Name
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
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


   // Recreates the ParentHandle and LayerProperties AppDataChunks from the LayerHierarchy.
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
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
      IEnumerable<BooleanNodeProperty> layerProps = Enum.GetValues(typeof(BooleanNodeProperty))
                                                        .Cast<BooleanNodeProperty>();

      //Write property chunk size in bytes.
      //1 byte for each enum value, 1 byte for each property value
      writer.Write((byte)(layerProps.Count() * 2));

      //Write property values.
      foreach (BooleanNodeProperty prop in layerProps)
      {
         writer.Write((Int32)prop);
         writer.Write(NestedLayers.GetProperty(layer, prop));
      }
   }

   private static void readProperties(BinaryReader reader, IILayer layer)
   {
      Type propType = typeof(BooleanNodeProperty);
      int numProps = reader.ReadByte() / 2;
      for (int p = 0; p < numProps; p++)
      {
         Int32 propInt = reader.ReadInt32();
         BooleanNodeProperty prop = (BooleanNodeProperty)Enum.ToObject(propType, propInt);
         Boolean propValue = reader.ReadBoolean();
         NestedLayers.SetProperty(layer, prop, propValue);
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

   #endregion
}
}
