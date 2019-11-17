using System.Collections.Generic;

namespace Algorithms
{
    public interface INode<out TKey, out TValue>
    {
        IEnumerable<INode<TKey, TValue>> Nodes { get; }
        TKey Key { get; }
        TValue Value { get; }
    }
}
