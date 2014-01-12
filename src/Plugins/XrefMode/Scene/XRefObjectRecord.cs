using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Max;
using Autodesk.Max.IXRefItem;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Scene
{
   public class XRefObjectRecord : MaxNodeWrapper, IXRefRecord
   {
      private IIObjXRefRecord record;

      public XRefObjectRecord(IIObjXRefRecord record)
      {
         this.record = record;
      }

      public override object BaseObject
      {
         get { return this.record; }
      }

      #region IXRef implementation

      public Boolean Enabled
      {
         get { return this.record.IsEnabled_; }
         set { this.record.SetEnable(value); }
      }

      public Boolean AutoUpdate
      {
         get { return this.record.IsAutoUpdate; }
         set { this.record.SetAutoUpdate(value); }
      }

      public void Update()
      {
         Boolean updateResult = this.record.Update;
      }

      public String Filename
      {
         get { return record.SrcFile.FullFilePath; }
      }

      #endregion

      #region Equality

      public override bool Equals(object obj)
      {
         XRefObjectRecord otherRecord = obj as XRefObjectRecord;
         return otherRecord != null && this.record.Equals(otherRecord);
      }

      public override int GetHashCode()
      {
         return this.record.GetHashCode();
      }

      #endregion

      #region Type
      
      protected override MaxNodeType MaxNodeType
      {
         get { return MaxNodeType.XRefRecord; }
      }

      #endregion

      #region Name

      public override string Name
      {
         get { return Path.GetFileName(this.record.SrcFile.FileName); }
         set { }
      }

      public override bool CanEditName
      {
         get { return false; }
      }

      #endregion

      #region ChildNodes

      public override int ChildNodeCount
      {
         get
         {
            return (int)record.ItemCount(XRefItemType.Object);
         }
      }

      public override IEnumerable<object> ChildBaseObjects
      {
         get
         {
            ITab<IReferenceTarget> tab = MaxInterfaces.Global.Tab.Create<IReferenceTarget>();
            this.record.GetItems(XRefItemType.Object, tab);
            return IINodes.ITabToIEnumerable(tab);
         }
      }

      #endregion

      #region Nodeproperties

      public override bool GetNodeProperty(BooleanNodeProperty property)
      {
         return this.ChildNodes.All(n => n.GetNodeProperty(property));
      }

      public override void SetNodeProperty(BooleanNodeProperty property, bool value)
      {
         this.ChildNodes.ForEach(n => n.SetNodeProperty(property, value));
      }

      #endregion

      #region Delete

      public override bool CanDelete
      {
         get { return true; }
      }

      public override void Delete()
      {
         MaxInterfaces.Global.IObjXRefManager8.Instance.RemoveRecordFromScene(this.record);
         MaxInterfaces.Global.BroadcastNotification(XRefNotificationCodes.XRefObjectRecordDeleted, this.record);
         this.record = null;
      }

      #endregion

      public override string ImageKey
      {
         get
         {
            return "xref_object";
         }
      }
   }
}
