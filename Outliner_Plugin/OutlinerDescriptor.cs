using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.Plugins;

namespace Outliner
{
   public class OutlinerDescriptor : ClassDesc2
   {
      public static OutlinerGUP Instance { get; private set; }
      public IClass_ID classID { get; private set; }

      public OutlinerDescriptor(IGlobal global)
      {
         classID = global.Class_ID.Create(0x2af116d5, 0x45710572);
      }

      public override string Category 
      {
         get { return "Outliner_Plugin"; }
      }

      public override IClass_ID ClassID 
      {
         get { return this.classID; }
      }

      public override string ClassName 
      {
         get { return "Outliner_Plugin"; }
      }

      public override object Create(bool loading) 
      {
         OutlinerDescriptor.Instance = new OutlinerGUP();
         return OutlinerDescriptor.Instance;
      }

      public override bool IsPublic 
      {
         get { return true; }
      }

      public override SClass_ID SuperClassID
      {
         get { return Autodesk.Max.SClass_ID.Gup; }
      }
   }
}
