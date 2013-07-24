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
      /// <summary>
      /// Initializes a new instance of the OutlinerPluginStartAttribute class.
      /// </summary>
      public OutlinerPluginStartAttribute() { }
   }

   /// <summary>Defines a method that is called when the Plugin is stopped.</summary>
   /// <remarks>The method should be public, static.</remarks>
   [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
   public class OutlinerPluginStopAttribute : Attribute
   {
      /// <summary>
      /// Initializes a new instance of the OutlinerPluginStopAttribute class.
      /// </summary>
      public OutlinerPluginStopAttribute() { }
   }
}
