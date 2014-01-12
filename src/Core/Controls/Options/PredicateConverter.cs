using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Controls.Options
{
   public class PredicateConverter : StringConverter
   {
      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
      {
         if (sourceType == typeof(string))
            return true;

         return base.CanConvertFrom(context, sourceType);
      }
      
      public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
      {
         return Functor.PredicateFromString(value as String);
      }

      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
      {
         if (destinationType == typeof(BinaryPredicate<Boolean>))
            return true;

         return base.CanConvertTo(context, destinationType);
      }

      public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
      {
         BinaryPredicate<Boolean> predicate = value as BinaryPredicate<Boolean>;

         if (destinationType == typeof(String) &&  predicate != null)
            return Functor.PredicateToString<Boolean>(predicate);

         return base.ConvertTo(context, culture, value, destinationType);
      }

      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
         return true;
      }

      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
         return new StandardValuesCollection(new string[] { "Or", "And" });
      }
   }
}
