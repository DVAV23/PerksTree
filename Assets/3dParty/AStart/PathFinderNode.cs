namespace AStar
{
    internal struct PathFinderNode<TNode>
    {
        public TNode Node;
        public double Weight;
        public double Heuristic;

        public PathFinderNode(TNode node, double newWeight, double newHeuristic)
        {
            Node = node;
            Weight = newWeight;
            Heuristic = newHeuristic;
        }

        public int CompareTo(ref PathFinderNode<TNode> other)
        {
           return Heuristic.CompareTo(other.Heuristic);
        }
    }
}