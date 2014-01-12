using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PJanssen.Outliner
{
   public class OutlinerVersion
   {
      public int Major
      {
         get;
         set;
      }

      public int Minor
      {
         get;
         set;
      }

      public int Build
      {
         get;
         set;
      }

      public int Revision
      {
         get;
         set;
      }

      public bool IsBeta
      {
         get;
         set;
      }

      public OutlinerVersion() { }

      public OutlinerVersion(int major, int minor, int build, int revision)
      {
         this.Major = major;
         this.Minor = minor;
         this.Build = build;
         this.Revision = revision;
      }
   }
}
