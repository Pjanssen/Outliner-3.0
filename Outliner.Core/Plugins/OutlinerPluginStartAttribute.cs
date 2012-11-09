using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Plugins
{
   /// <summary>Defines a method that is called when the Plugin is loaded.</summary>
   /// <remarks>The method should be public, static.</remarks>
   [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
   public class OutlinerPluginStartAttribute : Attribute
   {
      public OutlinerPluginStartAttribute() { }
   }

   //TODO: implement in OutlinerPLugins
   /// <summary>Defines a method that is called when the Plugin is stopped.</summary>
   /// <remarks>The method should be public, static.</remarks>
   [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
   public class OutlinerPluginStopAttribute : Attribute
   {
      public OutlinerPluginStopAttribute() { }
   }
}
