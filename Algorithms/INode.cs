using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    public interface INode<TKey, TValue>
    {
        IEnumerable<INode<TKey, TValue>> Nodes { get; }
        TKey Key { get; }
        TValue Value { get; }
    }
}
