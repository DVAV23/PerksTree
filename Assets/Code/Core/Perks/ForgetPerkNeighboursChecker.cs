using System;
using System.Collections.Generic;
using AStar;

namespace Code.Core.Perks
{
    public struct ForgetPerkNeighboursChecker
    {
        private readonly IPerksManager _perksManager;
        private readonly string _perkIdToCheck;
        private readonly string _basePerkId;
        private readonly PFDataContainer<string> _pfDataContainer;

        public ForgetPerkNeighboursChecker(IPerksManager perksManager, string perkIdToCheck, string basePerkId)
        {
            _perksManager = perksManager;
            _perkIdToCheck = perkIdToCheck;
            _basePerkId = basePerkId;
            _pfDataContainer = new PFDataContainer<string>(perksManager.Perks.Count);
        }

        public bool Check()
        {
            var perkInstance = _perksManager.PerksById[_perkIdToCheck];
            foreach (var perkId in perkInstance.NeighbourPerks)
            {
                var neighbourPerkInstance = _perksManager.PerksById[perkId];
                if (neighbourPerkInstance.IsBase || !neighbourPerkInstance.IsLearnt)
                {
                    continue;
                }

                var hasPath = Pathfinder.TryFindPath(
                    perkId,
                    _basePerkId,
                    _pfDataContainer,
                    GetMoveCost,
                    GetDistance,
                    PopulateNeighbours,
                    CanMoveToNode,
                    out _);
                if (!hasPath)
                {
                    return false;
                }
            }
            
            return true;
        }

        private bool CanMoveToNode(string from, string to)
        {
            var toPerkInstance = _perksManager.PerksById[to];
            if (!toPerkInstance.IsLearnt)
            {
                return false;
            }

            var fromPerkInstance = _perksManager.PerksById[from];
            return fromPerkInstance.NeighbourPerks.Contains(to);
        }

        private void PopulateNeighbours(string currentPerkId, List<string> neighbours)
        {
            neighbours.Clear();
            var neighbourPerks = _perksManager.PerksById[currentPerkId].NeighbourPerks;
            foreach (var perkId in neighbourPerks)
            {
                if (perkId.Equals(_perkIdToCheck))
                {
                    continue;
                }

                neighbours.Add(perkId);
            }
        }

        private double GetDistance(string from, string to)
        {
            var positionTo = _perksManager.PerksById[to].ViewData.Position;
            var positionFrom = _perksManager.PerksById[from].ViewData.Position;
            return Math.Abs(positionTo.X - positionFrom.X) + Math.Abs(positionTo.Y - positionFrom.Y);
        }

        private double GetMoveCost(string from, string to)
        {
            return GetDistance(from, to);
        }
    }
}