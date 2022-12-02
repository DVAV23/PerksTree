using System.Collections.Generic;

namespace AStar
{
    public class PFDataContainer<TNode>
    {
        public readonly BinaryHeap<TNode> OpenSet;
        public readonly HashSet<TNode> CloseSet;
        public readonly Dictionary<TNode, TNode> Links;
        public readonly List<TNode> Neighbours;

        public PFDataContainer(int nodesCount)
        {
            OpenSet = new BinaryHeap<TNode>(nodesCount);
            CloseSet = new HashSet<TNode>();
            Links = new Dictionary<TNode, TNode>();
            Neighbours = new List<TNode>();
        }
        
        public void Clear()
        {
            OpenSet.Clear();
            CloseSet.Clear();
            Links.Clear();
            Neighbours.Clear();
        }
    }
}