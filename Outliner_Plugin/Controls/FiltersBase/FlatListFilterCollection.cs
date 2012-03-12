namespace Outliner.Controls.FiltersBase
{
    public class FlatListFilterCollection<T> : FilterCollection<T>
    {
        public FlatListFilterCollection() : base() { }
        public FlatListFilterCollection(FilterCollection<T> collection) : base(collection) { }

        protected override FilterResult ShowChildNodes(T data)
        {
           return FilterResult.Hide;
        }
    }
}
