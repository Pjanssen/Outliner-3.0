using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner
{
   public class OutlinerVersionConverter : StringConverter
   {
      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
      {
         return destinationType == typeof(string);
      }

      public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
      {
         if (destinationType != typeof(string))
            throw new NotSupportedException();

         OutlinerVersion version = value as OutlinerVersion;
         if (version == null)
            return null;

         return version.ToString();
      }

      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
      {
         return sourceType == typeof(string);
      }

      public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
      {
         string version = value as string;
         if (version == null)
            return null;

         string[] parts = version.Split(new char[] { '.', ' ' });

         int major = int.Parse(parts[0]);
         int minor = int.Parse(parts[1]);
         int build = int.Parse(parts[2]);
         ReleaseStage stage = ReleaseStage.Release;

         if (parts.Length > 3 && parts[3].Length > 0)
         {
            string stageStr = parts[3].Substring(0, 1).ToUpper() + parts[3].Substring(1);
            stage = (ReleaseStage)Enum.Parse(typeof(ReleaseStage), stageStr);
         }

         return new OutlinerVersion(major, minor, build, stage);
      }
   }
}
