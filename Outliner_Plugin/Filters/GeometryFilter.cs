using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;
using Outliner.TreeModes;

namespace Outliner.Filters
{
public class GeometryFilter : NodeFilter<IMaxNodeWrapper>
{
   override public FilterResult ShowNode(IMaxNodeWrapper data)
   {
      if (data.SuperClassID != SClass_ID.Geomobject)
         return FilterResult.Show;

      //TODO: Check particle classes.
      if (HelperMethods.ClassIDEquals(data.ClassID, BuiltInClassIDA.BONE_OBJ_CLASSID, BuiltInClassIDB.BONE_OBJ_CLASSID))
         return FilterResult.Show;
      else
         return FilterResult.Hide;

      /*
       if (!(n is OutlinerObject))
           return FilterResult.Show;

       OutlinerObject o = (OutlinerObject)n;

       if (o.SuperClass != MaxTypes.Geometry)
           return FilterResult.Show;

       if (o.Class == MaxTypes.Bone || o.Class == MaxTypes.Biped || o.Class == MaxTypes.PfSource || 
           o.Class == MaxTypes.PCloud || o.Class == MaxTypes.PArray || o.Class == MaxTypes.PBlizzard ||
           o.Class == MaxTypes.PSpray || o.Class == MaxTypes.PSuperSpray || o.Class == MaxTypes.PSnow)
       {
           return FilterResult.Show;
       }
       else
           return FilterResult.Hide;
       */
   }
}
}
