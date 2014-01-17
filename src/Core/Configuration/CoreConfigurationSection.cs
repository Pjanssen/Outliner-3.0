using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.Configuration
{
   public class CoreConfigurationSection : ConfigurationSection
   {
      //==========================================================================

      public CoreConfigurationSection()
      {
         ColorScheme = GetDefaultColorScheme();
      }

      //==========================================================================

      private static String GetDefaultColorScheme()
      {
         if (MaxUtils.MaxInterfaces.ColorThemeLightActive)
            return "light.xml";
         else
            return "dark.xml";
      }

      //==========================================================================

      [ConfigurationProperty("colorScheme")]
      public string ColorScheme
      {
         get
         {
            return (string)this["colorScheme"];
         }
         set
         {
            this["colorScheme"] = value;
         }
      }

      //==========================================================================
   }
}
