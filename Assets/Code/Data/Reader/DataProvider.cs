using System;
using System.Collections.Generic;
using Code.Core.Services;

namespace Code.Data.Reader
{
    public class DataProvider : IDataProvider
    {
        public void GetData(IAssetService assetService,
            Action<Dictionary<string, Dictionary<string, object>>> loadedCb)
        {
            assetService.GetTextAssetAsync(Code.Data.Perks.PerksDataContainer.DATA_FILE_NAME, (textAsset) =>
            {
                var dataNode = (Dictionary<string, object>)fastJSON.JSON.Parse(textAsset.text);
                var result = new Dictionary<string, Dictionary<string, object>>
                {
                    { Code.Data.Perks.PerksDataContainer.DATA_FILE_NAME, dataNode }
                };
                loadedCb?.Invoke(result);
            });
        }
    }
}