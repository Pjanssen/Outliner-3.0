using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.Filters
{
   public class SuperClassConverter : StringConverter
   {
      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
      {
         if (sourceType == typeof(String))
            return true;

         return base.CanConvertFrom(context, sourceType);
      }

      public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
      {
         return Enum.Parse(typeof(SClass_ID), (string)value);
      }

      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
         return true;
      }

      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
         return true;
      }

      public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
         List<String> names = new List<String>(Enum.GetNames(typeof(SClass_ID)));
         names.Sort();

         List<SClass_ID> sortedClassIDs = new List<SClass_ID>(names.Count);
         foreach (String name in names)
         {
            sortedClassIDs.Add((SClass_ID)Enum.Parse(typeof(SClass_ID), name));
         }
         return new StandardValuesCollection(sortedClassIDs);
      }
   }
}
