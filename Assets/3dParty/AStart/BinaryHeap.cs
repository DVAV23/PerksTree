using System.Collections.Generic;

namespace AStar
{
    public class BinaryHeap<TNode>
    {
        private readonly Dictionary<TNode, int> _map;
        private readonly PathFinderNode<TNode>[] _collection;
        private int _count;
        
        public BinaryHeap(int capacity)
        {
            _collection = new PathFinderNode<TNode>[capacity];
            _map = new Dictionary<TNode, int>(capacity);
        }
        
        public int Count => _count;

        internal void Enqueue(PathFinderNode<TNode> item)
        {
            _collection[_count] = item;
            _count++;
            int i = _count - 1;
            _map[item.Node] = i;
            while(i > 0)
            {
                int j = (i - 1) / 2;
                
                if (_collection[i].CompareTo(ref _collection[j]) > 0)
                    break;

                Swap(i, j);
                i = j;
            }
        }

        internal PathFinderNode<TNode> Dequeue()
        {
            if (_count == 0) return default;
            
            var result = _collection[0];
            RemoveRoot();
            _map.Remove(result.Node);
            return result;
        }

        public void Clear()
        {
            _count = 0;
            _map.Clear();
        }

        internal bool ProcessNode(ref TNode position, ref PathFinderNode<TNode> newNode)
        {
            if (!_map.TryGetValue(position, out int existingNodeIndex))
            {
                Enqueue(newNode);
                return true;
            }
            
            ref var existingNode = ref _collection[existingNodeIndex];
            if (newNode.Heuristic < existingNode.Heuristic)
            {
                _collection[existingNodeIndex] = newNode;
                return true;
            }
            
            return false;
        }
        
        private void RemoveRoot()
        {
            _collection[0] = _collection[_count - 1];
            _map[_collection[0].Node] = 0;
            _count--;

            var i = 0;
            while(true)
            {
                int largest = LargestIndex(i);
                if (largest == i)
                    return;

                Swap(i, largest);
                i = largest;
            }
        }

        private void Swap(int i, int j)
        {
            (_collection[i], _collection[j]) = (_collection[j], _collection[i]);
            _map[_collection[i].Node] = i;
            _map[_collection[j].Node] = j;
        }

        private int LargestIndex(int i)
        {
            int leftInd = 2 * i + 1;
            int rightInd = 2 * i + 2;
            int largest = i;

            if (leftInd < _count && _collection[leftInd].CompareTo(ref _collection[largest]) < 0)
            {
                largest = leftInd;
            }

            if (rightInd < _count && _collection[rightInd].CompareTo(ref _collection[largest]) < 0)
            {
                largest = rightInd;
            }
            
            return largest;
        }
    }
}