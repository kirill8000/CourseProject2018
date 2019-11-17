using System;
using System.Collections;
using System.Collections.Generic;

namespace Algorithms
{
    public class Map<TKey, TValue> : IEnumerable<TValue> where TKey : IComparable<TKey>
    {
        private Node _root;
        private Node _cash;

        public IEnumerable<INode<TKey, TValue>> Root => new List<INode<TKey, TValue>> {_root};

        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (var value in Traverse())
                {
                    yield return value.Key;
                }
            }
        }

        private class Node : INode<TKey, TValue>
        {
            public bool IsRed = true;

            public IEnumerable<INode<TKey, TValue>> Nodes
            {
                get
                {
                    if (Left != null)
                    {
                        yield return Left;
                    }

                    if (Right != null)
                    {
                        yield return Right;
                    }
                }
            }

            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public Node Left;
            public Node Right;
            public int N = 1;

            public Node(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        private int GetSize(Node x)
        {
            if (x == null)
            {
                return 0;
            }

            return x.N;
        }

        private Node RotateLeft(Node h)
        {
            Node x = h.Right;
            h.Right = x.Left;
            x.Left = h;
            x.IsRed = x.Left.IsRed;
            x.Left.IsRed = true;
            x.N = h.N;
            h.N = GetSize(h.Left) + GetSize(h.Right) + 1;
            return x;
        }

        private Node RotateRight(Node h)
        {
            Node x = h.Left;
            h.Left = x.Right;
            x.Right = h;
            x.IsRed = x.Right.IsRed;
            x.Right.IsRed = true;
            x.N = h.N;
            h.N = GetSize(h.Left) + GetSize(h.Right) + 1;
            return x;
        }

        public bool IsEmpty => _root == null;

        public int Size => GetSize(_root);

        private IEnumerable<Node> Traverse()
        {
            int state = 0;
            var s = new Stack<Node>();
            var curr = _root;
            while (state != 3)
            {
                switch (state)
                {
                    case 0:
                        if (curr.Left != null)
                        {
                            s.Push(curr);
                            curr = curr.Left;
                        }
                        else
                        {
                            state = 1;
                        }

                        break;
                    case 1:
                        yield return curr;
                        if (curr.Right != null)
                        {
                            s.Push(curr);
                            curr = curr.Right;
                            state = 0;
                        }
                        else
                        {
                            state = 2;
                        }

                        break;
                    case 2:
                        if (s.Count == 0)
                        {
                            state = 3;
                        }
                        else if (s.Peek().Right == curr)
                        {
                            curr = s.Pop();
                        }
                        else
                        {
                            curr = s.Pop();
                            state = 1;
                        }

                        break;
                }
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (var node in Traverse())
            {
                yield return node.Value;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Node Add(Node h, TKey key, TValue value)
        {
            if (h == null)
            {
                return new Node(key, value);
            }

            var cmp = key.CompareTo(h.Key);
            if (cmp < 0)
            {
                h.Left = Add(h.Left, key, value);
            }
            else if (cmp > 0)
            {
                h.Right = Add(h.Right, key, value);
            }
            else
            {
                h.Value = value;
            }

            if (IsRed(h.Right))
            {
                h = RotateLeft(h);
            }

            if (IsRed(h.Left) && IsRed(h.Left.Left))
            {
                h = RotateRight(h);
            }

            if (IsRed(h.Left) && IsRed(h.Right))
            {
                FlipColors(h);
            }

            h.N = GetSize(h.Left) + GetSize(h.Right) + 1;
            return h;
        }


        private void FlipColors(Node h)
        {
            h.IsRed = !h.IsRed;
            h.Left.IsRed = !h.Left.IsRed;
            h.Right.IsRed = !h.Right.IsRed;
        }

        private bool IsRed(Node h)
        {
            if (h == null)
                return false;
            return h.IsRed;
        }

        public void Add(TKey key, TValue value)
        {
            _root = Add(_root, key, value);
            _root.IsRed = false;
        }

        public bool Contains(TKey key)
        {
            return TryGetValue(key, out _);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue value))
                {
                    return value;
                }

                throw new KeyNotFoundException();
            }
        }

        private bool TryGetValue(TKey key, Node node, out Node value)
        {
            if (node == null)
            {
                value = null;
                return false;
            }

            var cmp = key.CompareTo(node.Key);
            if (cmp < 0)
            {
                return TryGetValue(key, node.Left, out value);
            }
            else if (cmp > 0)
            {
                return TryGetValue(key, node.Right, out value);
            }
            else
            {
                value = node;
                return true;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_cash != null && key.CompareTo(_cash.Key) == 0)
            {
                value = _cash.Value;
                return true;
            }

            var result = TryGetValue(key, _root, out var n);
            _cash = n;
            if (result)
            {
                value = n.Value;
            }
            else
            {
                value = default(TValue);
            }

            return result;
        }

        public TKey Min()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Empty collection");
            }
            return Min(_root);
        }

        private TKey Min(Node n)
        {
            if (n.Left == null)
                return n.Key;
            return Min(n.Left);
        }

        public TKey Select(int keyRank)
        {
            return Select(_root, keyRank).Key;
        }
        private Node Select(Node x, int keyRank)
        {
            if (x == null) return null;
            int t = GetSize(x.Left);
            if (t > keyRank)
            {
                return Select(x.Left, keyRank);
            }
            else if (t < keyRank)
            {
                return Select(x.Right, keyRank - t - 1);
            }
            else
            {
                return x;
            }
        }


        public int Rank(TKey key)
        {
            return Rank(key, _root);
        }

        private int Rank(TKey key, Node x)
        {
            if (x == null)
            {
                return 0;
            }

            int cmp = key.CompareTo(x.Key);
            if (cmp < 0)
            {
                return Rank(key, x.Left);
            }
            else if (cmp > 0)
            {
                return 1 + GetSize(x.Left) + Rank(key, x.Right);
            }
            else
            {
                return GetSize(x.Left);
            }
        }

        public TKey Max()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Empty collection");
            }
            return Max(_root);
        }

        private TKey Max(Node n)
        {
            if (n.Right == null)
                return n.Key;
            return Max(n.Right);
        }

        private Node MoveRedLeft(Node h)
        {
            FlipColors(h);
            if (IsRed(h.Right.Left))
            {
                h.Right = RotateRight(h.Right);
                h = RotateLeft(h);
                FlipColors(h);
            }

            return h;
        }

        private Node MoveRedRight(Node h)
        {
            FlipColors(h);
            if (IsRed(h.Left.Left))
            {
                h = RotateRight(h);
                FlipColors(h);
            }

            return h;
        }

        private Node Balance(Node h)
        {
            if (IsRed(h.Right))
            {
                h = RotateLeft(h);
            }

            if (IsRed(h.Left) && IsRed(h.Left.Left)) h = RotateRight(h);
            if (IsRed(h.Left) && IsRed(h.Right)) FlipColors(h);

            h.N = GetSize(h.Left) + GetSize(h.Right) + 1;
            return h;
        }

        public void DeleteMax()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Empty collection");
            }
            _root = DeleteMax(_root);
            if (!IsEmpty)
            {
                _root.IsRed = false;
            }
        }

        private Node DeleteMax(Node h)
        {
            if (IsRed(h.Left))
            {
                h = RotateRight(h);
            }

            if (h.Right == null)
            {
                return null;
            }

            if (!IsRed(h.Right) && !IsRed(h.Right.Left))
            {
                h = MoveRedRight(h);
            }

            h.Right = DeleteMax(h.Right);
            return Balance(h);
        }

        public void DeleteMin()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Empty collection");
            }
            _root = DeleteMin(_root);
            if (!IsEmpty)
            {
                _root.IsRed = false;
            }
        }

        private Node DeleteMin(Node h)
        {
            if (h.Left == null)
            {
                return null;
            }

            if (!IsRed(h.Left) && !IsRed(h.Left.Left))
            {
                h = MoveRedLeft(h);
            }

            h.Left = DeleteMin(h.Left);
            return Balance(h);
        }

        public void Delete(TKey key)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Empty collection");
            }
            _root = Delete(_root, key);
            if (!IsEmpty)
            {
                _root.IsRed = false;
            }
        }

        private Node Delete(Node h, TKey key)
        {
            if (key.CompareTo(h.Key) < 0)
            {
                if (!IsRed(h.Left) && !IsRed(h.Left.Left))
                {
                    h = MoveRedLeft(h);
                }

                h.Left = Delete(h.Left, key);
            }
            else
            {
                if (IsRed(h.Left))
                {
                    h = RotateRight(h);
                }

                if (key.CompareTo(h.Key) == 0 && (h.Right == null))
                    return null;

                if (!IsRed(h.Right) && !IsRed(h.Right.Left))
                {
                    h = MoveRedRight(h);
                }

                if (key.CompareTo(h.Key) == 0)
                {
                    h.Key = Min(h.Right);
                    TryGetValue(h.Key, h.Right, out var node);
                    h.Value = node.Value;
                    h.Right = DeleteMin(h.Right);
                }
                else
                {
                    h.Right = Delete(h.Right, key);
                }
            }

            return Balance(h);
        }

        public IEnumerable<TKey> GetBreadthFirstSearchEnumerator()
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(_root);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current.Left != null)
                {
                    queue.Enqueue(current.Left);
                }

                if (current.Right != null)
                {
                    queue.Enqueue(current.Right);
                }

                yield return current.Key;
            }
        }
    }
}
