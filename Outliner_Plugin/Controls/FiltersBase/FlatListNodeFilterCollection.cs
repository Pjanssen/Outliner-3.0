namespace Outliner.Controls.FiltersBase
{
    public class FlatListNodeFilterCollection<T> : NodeFilterCollection<T>
    {
        public FlatListNodeFilterCollection() : base() { }
        public FlatListNodeFilterCollection(NodeFilterCollection<T> collection) : base(collection) { }

        protected override FilterResult ShowChildNodes(T data)
        {
           return FilterResult.Hide;
        }
    }
}
