using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace Outliner.Plugins
{
public class OutlinerActionConverter : TypeConverter
{
   public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
   {
      List<String> names = new List<string>() { "None" };
      names.AddRange(OutlinerActions.ActionNames);

      return new StandardValuesCollection(names);
   }

   public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
   {
      return false;
   }

   public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
   {
      return true;
   }
}

public class OutlinerPredicateConverter : TypeConverter
{
   public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
   {
      List<String> names = new List<string>() { "None" };
      names.AddRange(OutlinerActions.PredicateNames);

      return new StandardValuesCollection(names);
   }

   public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
   {
      return false;
   }

   public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
   {
      return true;
   }
}
}
