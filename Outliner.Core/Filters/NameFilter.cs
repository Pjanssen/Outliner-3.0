using System;
using System.Text.RegularExpressions;
using Outliner.Scene;
using Outliner.MaxUtils;
using Outliner.Plugins;
using System.Xml.Serialization;

namespace Outliner.Filters
{
[OutlinerPlugin(OutlinerPluginType.Filter)]
[LocalizedDisplayName(typeof(OutlinerResources), "FilterName")]
[FilterCategory(FilterCategories.Hidden)]
public class NameFilter : Filter<IMaxNodeWrapper>
{
   private const String SEARCH_BEGINS_WITH = "^";
   private const String SEARCH_WILDCARD = ".";
   private RegexOptions regExpOptions;
   private String searchString;
   private String origSearchString;
   private Boolean useWildcard;

   public NameFilter() 
   {
      this.searchString = String.Empty;
      this.origSearchString = String.Empty;
      this.regExpOptions = RegexOptions.IgnoreCase;
      this.useWildcard = false;
   }

   [XmlAttribute("searchstring")]
   public String SearchString 
   {
      get 
      {
         return origSearchString;
      }
      set
      {
         ExceptionHelpers.ThrowIfArgumentIsNull(value, "value");

         origSearchString = value;
         if (value == String.Empty)
            searchString = value;
         else
         {
            if (this.UseWildcard || value.Substring(0, 1) == "*")
               searchString = SEARCH_WILDCARD + Regex.Escape(value.Substring(1, value.Length - 1));
            else
               searchString = SEARCH_BEGINS_WITH + Regex.Escape(value);
         }

         this.OnFilterChanged();
      }
   }

   /// <summary>
   /// Gets or sets whether the search should be case sensitive.
   /// If true, a searchstring "S" matches "Sphere", but not "sphere"
   /// </summary>
   [XmlAttribute("casesensitive")]
   public Boolean CaseSensitive 
   {
      get { return regExpOptions == RegexOptions.None; }
      set
      {
         if (value)
            regExpOptions = RegexOptions.None;
         else
            regExpOptions = RegexOptions.IgnoreCase;

         this.OnFilterChanged();
      }
   }

   /// <summary>
   /// Gets or sets whether a wildcard should be prepended to the search string by default.
   /// If true, a searchstring "e" matches "sphere".
   /// </summary>
   [XmlAttribute("usewildcard")]
   public Boolean UseWildcard 
   {
      get { return useWildcard; }
      set
      {
         useWildcard = value;
         this.SearchString = origSearchString;
         this.OnFilterChanged();
      }
   }

   [XmlIgnore]
   public override bool Enabled
   {
      get { return !String.IsNullOrEmpty(searchString); }
      set { }
   }

   protected override Boolean ShowNodeInternal(IMaxNodeWrapper data) 
   {
      if (data == null)
         return false;

      if (String.IsNullOrEmpty(searchString))
         return true;

      return Regex.IsMatch(data.Name, searchString, regExpOptions);
   }
}
}
