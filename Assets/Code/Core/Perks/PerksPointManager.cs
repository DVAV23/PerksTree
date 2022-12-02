using System;
using Code.Core.Managers;
using Code.Core.Services;

namespace Code.Core.Perks
{
    public interface IPerksPointManager
    {
        Action PointsValueChanged { get; set; }
        int PerksPointCount { get; }
        void Pay(int price);
        void AddPoints(int value);
    }
    
    public class PerksPointManager: BaseManager, IPerksPointManager
    {
        public const string MANAGER_NAME = nameof(PerksPointManager);
        public Action PointsValueChanged { get; set; }   
        
        private int _perksPointCount;
        
        public override void Init(IServicesAggregator services, Action initializedCb)
        {
            _perksPointCount = 0;
            initializedCb?.Invoke();
            
        }

        private void ChangePointsValue(int value)
        {
            _perksPointCount += value;
            PointsValueChanged?.Invoke();
        }

        #region IPerksPointManager

        void IPerksPointManager.Pay(int price)
        {
            ChangePointsValue(-price);
        }

        void IPerksPointManager.AddPoints(int value)
        {
            ChangePointsValue(value);
        }

        int IPerksPointManager.PerksPointCount => _perksPointCount;
        
        #endregion
    }
}