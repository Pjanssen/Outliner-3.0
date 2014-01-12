using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Max;
using Autodesk.Max.MaxSDK.AssetManagement;
using ManagedServices;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Scene
{
   public class XRefSceneRecord : MaxNodeWrapper, IXRefRecord
   {
      private int index;
      private IINode sceneRoot;
      public IAssetUser Asset { get; private set; }

      public XRefSceneRecord(IAssetUser asset, int index)
      {
         this.index = index;
         this.Asset = asset;
         this.sceneRoot = MaxInterfaces.COREInterface.RootNode;
      }

      public override object BaseObject
      {
         get { return this.Asset; }
      }

      public override bool IsValid
      {
         get
         {
            return this.Index >= 0;
         }
      }

      #region XrefScene specific

      public int Index
      {
         get
         {
            return index;
            //for (int i = 0; i < sceneRoot.XRefFileCount; i++)
            //{
            //   if (sceneRoot.GetXRefFile(i).IdAsString == this.Asset.IdAsString)
            //      return i;
            //}

            //return -1;
         }
      }

      public String FileName
      {
         get { return this.Asset.FileName; }
      }

      /// <summary>
      /// Tests if the XrefScene has all the given flags set.
      /// </summary>
      public Boolean HasFlags(XRefSceneFlags flags)
      {
         if (!this.IsValid)
            return false;

         return ((XRefSceneFlags)this.sceneRoot.GetXRefFlags(this.Index) & flags) == flags;
      }

      /// <summary>
      /// Sets the given flags on the XrefScene.
      /// </summary>
      public void SetFlags(XRefSceneFlags flags)
      {
         int index = this.Index;
         if (index >= 0)
         {
            this.sceneRoot.SetXRefFlags(index, (uint)flags, true);
            this.sceneRoot.FlagXrefChanged(index);
            this.sceneRoot.ReloadXRef(index);
            MaxInterfaces.Global.BroadcastNotification(XRefNotificationCodes.XRefSceneFlagsChanged, this.BaseObject);
         }
      }

      /// <summary>
      /// Removes the given flags from the XrefScene.
      /// </summary>
      public void RemoveFlags(XRefSceneFlags flags)
      {
         int index = this.Index;
         if (index >= 0)
         {
            this.sceneRoot.SetXRefFlags(index, (uint)flags, false);
            this.sceneRoot.FlagXrefChanged(index);
            this.sceneRoot.ReloadXRef(index);
            MaxInterfaces.Global.BroadcastNotification(XRefNotificationCodes.XRefSceneFlagsChanged, this.BaseObject);
         }
      }

      #endregion


      #region IXRefRecord implementation

      public Boolean Enabled
      {
         get
         {
            return !this.HasFlags(XRefSceneFlags.Disabled);
         }
         set
         {
            if (value)
               this.RemoveFlags(XRefSceneFlags.Disabled);
            else
               this.SetFlags(XRefSceneFlags.Disabled);
         }
      }

      public Boolean AutoUpdate
      {
         get
         {
            return this.HasFlags(XRefSceneFlags.AutoUpdate);
         }
         set
         {
            if (value)
               this.SetFlags(XRefSceneFlags.AutoUpdate);
            else
               this.RemoveFlags(XRefSceneFlags.AutoUpdate);
         }
      }

      public void Update()
      {
         this.sceneRoot.ReloadXRef(this.Index);
      }

      public String Filename
      {
         get { return Asset.FullFilePath; }
      }

      #endregion


      #region Equality

      public override bool Equals(object obj)
      {
         XRefSceneRecord xrefScene = obj as XRefSceneRecord;
         return xrefScene != null && xrefScene.Asset.Handle == this.Asset.Handle;
      }

      public override int GetHashCode()
      {
         return this.Asset.GetHashCode();
      }

      #endregion


      #region Delete

      public override bool CanDelete
      {
         get { return true; }
      }

      public override void Delete()
      {
         this.sceneRoot.DeleteXRefFile(this.Index);
         MaxInterfaces.Global.BroadcastNotification(XRefNotificationCodes.XRefSceneDeleted, this.BaseObject);
      }

      #endregion


      #region Type
      
      protected override MaxNodeType MaxNodeType
      {
         get { return MaxNodeType.XRefSceneRecord; }
      }

      #endregion


      #region Name

      public override string Name
      {
         get { return Path.GetFileName(this.Asset.FileName); }
         set { }
      }

      public override bool CanEditName
      {
         get { return false; }
      }

      #endregion


      #region Node Properties

      public override bool GetNodeProperty(BooleanNodeProperty property)
      {
         if (property == BooleanNodeProperty.IsHidden)
            return this.HasFlags(XRefSceneFlags.Hidden);
         if (property == BooleanNodeProperty.BoxMode)
            return this.HasFlags(XRefSceneFlags.BoxMode);
         
         return base.GetNodeProperty(property);
      }

      public override void SetNodeProperty(BooleanNodeProperty property, bool value)
      {
         XRefSceneFlags flags = 0;
         if (property == BooleanNodeProperty.IsHidden)
            flags = XRefSceneFlags.Hidden;
         else if (property == BooleanNodeProperty.BoxMode)
            flags = XRefSceneFlags.BoxMode;

         if (flags != 0)
         {
            if (value)
               this.SetFlags(flags);
            else
               this.RemoveFlags(flags);
         }
         else
         {
            base.SetNodeProperty(property, value);
         }
      }

      #endregion

      public override string ImageKey
      {
         get
         {
            return "xref_scene";
         }
      }
   }
}
