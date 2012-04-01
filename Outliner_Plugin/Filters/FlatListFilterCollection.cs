namespace Outliner.Filters
{
    public class FlatListFilterCollection<T> : FilterCollection<T>
    {
        public FlatListFilterCollection() : base() { }
        public FlatListFilterCollection(FilterCollection<T> collection) : base(collection) { }

        protected override FilterResults ShowChildNodes(T data)
        {
           return FilterResults.Hide;
        }
    }
}
