using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace PJanssen.Outliner
{
   [DataContract(Name = "OutlinerVersion", Namespace = "http://outliner.pjanssen.nl/")]
   [Serializable]
   public class OutlinerVersion : IComparable, IComparable<OutlinerVersion>
   {
      //==========================================================================

      public OutlinerVersion() 
         : this(1, 0, 0, ReleaseStage.Release) 
      { }

      public OutlinerVersion(int major, int minor, int build)
         : this(major, minor, build, ReleaseStage.Release)
      { }

      public OutlinerVersion(int major, int minor, int build, ReleaseStage stage)
      {
         this.Major = major;
         this.Minor = minor;
         this.Build = build;
         this.Stage = stage;
      }

      //==========================================================================

      public static OutlinerVersion GetCurrentVersion()
      {
         Assembly assembly = Assembly.GetAssembly(typeof(OutlinerInstallation));
         Version version = assembly.GetName().Version;

         ReleaseStage releaseStage = ReleaseStage.Release;
         object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), false);
         if (attributes.Length > 0)
         {
            AssemblyConfigurationAttribute configAttr = (AssemblyConfigurationAttribute)attributes[0];
            Enum.TryParse<ReleaseStage>(configAttr.Configuration, out releaseStage);
         }

         return new OutlinerVersion(version.Major, version.Minor, version.Build, releaseStage);
      }

      //==========================================================================

      [DataMember(IsRequired = true, Order = 0)]
      public int Major
      {
         get;
         set;
      }

      [DataMember(IsRequired = true, Order = 1)]
      public int Minor
      {
         get;
         set;
      }

      [DataMember(IsRequired = true, Order = 2)]
      public int Build
      {
         get;
         set;
      }

      [DataMember(IsRequired = true, Order = 3)]
      public ReleaseStage Stage
      {
         get;
         set;
      }

      //==========================================================================

      public int CompareTo(object obj)
      {
         return CompareTo(obj as OutlinerVersion);
      }

      public int CompareTo(OutlinerVersion other)
      {
         if (other == null)
            return 1;

         int diff = this.Major.CompareTo(other.Major);
         if (diff != 0)
            return diff;

         diff = this.Minor.CompareTo(other.Minor);
         if (diff != 0)
            return diff;

         diff = this.Build.CompareTo(other.Build);
         if (diff != 0)
            return diff;

         diff = this.Stage.CompareTo(other.Stage);
         if (diff != 0)
            return diff;

         return 0;
      }

      //==========================================================================

      public override bool Equals(object obj)
      {
         if (obj == null)
            return false;

         return this.CompareTo(obj) == 0;
      }

      public override int GetHashCode()
      {
         return base.GetHashCode() ^ this.Major.GetHashCode()
                                   ^ this.Minor.GetHashCode()
                                   ^ this.Build.GetHashCode()
                                   ^ this.Stage.GetHashCode();
      }

      //==========================================================================

      public static bool operator ==(OutlinerVersion versionX, OutlinerVersion versionY)
      {
         if (object.ReferenceEquals(versionX, versionY))
            return true;

         if ((object)versionX == null || (object)versionY == null)
            return false;

         return versionX.Equals(versionY);
      }

      public static bool operator !=(OutlinerVersion versionX, OutlinerVersion versionY)
      {
         return !(versionX == versionY);
      }

      public static bool operator <(OutlinerVersion versionX, OutlinerVersion versionY)
      {
         return versionX.CompareTo(versionY) == -1;
      }

      public static bool operator <=(OutlinerVersion versionX, OutlinerVersion versionY)
      {
         return versionX.CompareTo(versionY) < 1;
      }

      public static bool operator >(OutlinerVersion versionX, OutlinerVersion versionY)
      {
         return versionX.CompareTo(versionY) == 1;
      }

      public static bool operator >=(OutlinerVersion versionX, OutlinerVersion versionY)
      {
         return versionX.CompareTo(versionY) > -1;
      }

      //==========================================================================

      public override string ToString()
      {
         string stage = "";
         switch (this.Stage)
         {
            case ReleaseStage.Alpha:
               stage = " alpha";
               break;
            case ReleaseStage.Beta:
               stage = " beta";
               break;
            default:
               stage = "";
               break;
         }

         return string.Format("{0}.{1}.{2}{3}", this.Major, this.Minor, this.Build, stage);
      }

      //==========================================================================
   }
}
