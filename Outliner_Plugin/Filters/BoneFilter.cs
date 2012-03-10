using Outliner.Scene;
using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.TreeModes;

namespace Outliner.Filters
{
    public class BoneFilter : NodeFilter<IMaxNodeWrapper>
    {
        override public FilterResult ShowNode(IMaxNodeWrapper data)
        {
           if (data == null)
              return FilterResult.Show;

           if (data.SuperClassID != SClass_ID.Geomobject)
              return FilterResult.Show;

           if (HelperMethods.ClassIDEquals(data.ClassID, BuiltInClassIDA.BONE_OBJ_CLASSID, BuiltInClassIDB.BONE_OBJ_CLASSID))
              return FilterResult.Hide;
           else
              return FilterResult.Show;
        }
    }

}
