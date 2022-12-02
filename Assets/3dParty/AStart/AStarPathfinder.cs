using System;
using System.Collections.Generic;

namespace AStar
{
    public static class Pathfinder
    {
        public static bool TryFindPath<TNode>(
            TNode start, 
            TNode destination,
            PFDataContainer<TNode> dataContainer,
            Func<TNode, TNode, double> getMoveCost,
            Func<TNode, TNode, double> getDistance,
            Action<TNode, List<TNode>> populateNeighbours,
            Func<TNode, TNode, bool> canMoveToNode,
            out List<TNode> path)
        {
            if (start == null || destination == null)
            {
                path = null;
                return false;
            }

            dataContainer.Clear();
            dataContainer.OpenSet.Enqueue(new PathFinderNode<TNode>(start, 0, 0));

            if (!FindPath(dataContainer, destination, getMoveCost, getDistance, populateNeighbours, canMoveToNode))
            {
                path = new List<TNode>();
                return false;
            }

            var links = dataContainer.Links;
            path = new List<TNode> { destination };
            while (links.TryGetValue(destination, out destination))
            {
                path.Add(destination);
            }
            path.Reverse();
            return true;
        }

        private static bool FindPath<TNode>(
            PFDataContainer<TNode> dataContainer, 
            TNode destination,
            Func<TNode, TNode, double> getMoveCost,
            Func<TNode, TNode, double> getEstimate,
            Action<TNode, List<TNode>> populateNeighbours,
            Func<TNode, TNode, bool> canMoveToNode)
        {
            var closeSet = dataContainer.CloseSet;
            var openSet = dataContainer.OpenSet;

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                closeSet.Add(current.Node);

                if (current.Node.Equals(destination))
                {
                    return true;
                }

                GenerateNodesFromNeighbours(current,
                    destination,
                    dataContainer,
                    getMoveCost,
                    getEstimate,
                    populateNeighbours,
                    canMoveToNode);
            }
            return false;
        }

        private static void GenerateNodesFromNeighbours<TNode>(
            PathFinderNode<TNode> current, 
            TNode destination,
            PFDataContainer<TNode> dataContainer,
            Func<TNode, TNode, double> getMoveCost,
            Func<TNode, TNode, double> getEstimate,
            Action<TNode, List<TNode>> populateNeighbours,
            Func<TNode, TNode, bool> canMoveToNode)
        {

            var closeSet = dataContainer.CloseSet;
            var openSet = dataContainer.OpenSet;
            var links = dataContainer.Links;
            var neighbours = dataContainer.Neighbours;
            
            var currentNode = current.Node;
            populateNeighbours(current.Node, neighbours);
            foreach (var n in neighbours)
            {
                if (closeSet.Contains(n))
                {
                    continue;
                }
                
                if (!canMoveToNode(n, currentNode))
                {
                    closeSet.Add(n);
                    continue;
                }

                var d = getMoveCost(n, currentNode);

                var weight = d + current.Weight;
                
                var newPathNode = new PathFinderNode<TNode>(n, weight, weight + getEstimate(n, destination));
                if (openSet.ProcessNode(ref newPathNode.Node, ref newPathNode))
                {
                    links[newPathNode.Node] = currentNode;
                }
            }
        }
    }
}