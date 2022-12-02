using System;
using System.Collections.Generic;
using Code.Core.Services;

namespace Code.Data.Reader
{
    public interface IDataProvider
    {
        void GetData(IAssetService assetService,
            Action<Dictionary<string, Dictionary<string, object>>> loadedCb);
    }

    public class DataReader
    {
        public void GetData(IAssetService assetService, Action<Dictionary<string, Dictionary<string, object>>> loadedCb)
        {
            var dataProvider = GetDataProvider();
            dataProvider.GetData(assetService, loadedCb);
        }

        private IDataProvider GetDataProvider()
        {
#if UNITY_EDITOR
            return new EditorDataProvider();
#else
            return new DataProvider();
#endif
        }
    }
}