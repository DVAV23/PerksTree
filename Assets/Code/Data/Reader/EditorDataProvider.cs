#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Code.Core.Services;
using UnityEngine;

namespace Code.Data.Reader
{
    /// <summary>
    /// Use only in editor
    /// </summary>
    public class EditorDataProvider : IDataProvider
    {
        private static string _dataFolderPath =
            $"{Application.dataPath}{System.IO.Path.DirectorySeparatorChar}Data{System.IO.Path.DirectorySeparatorChar}";

        public void GetData(IAssetService assetService,
            Action<Dictionary<string, Dictionary<string, object>>> loadedCb)
        {
            var data = GetData();
            loadedCb?.Invoke(data);
        }

        private static Dictionary<string, Dictionary<string, object>> GetData()
        {
            var result = new Dictionary<string, Dictionary<string, object>>();
            if (!System.IO.Directory.Exists(_dataFolderPath))
            {
                Debug.LogError($"Data folder: {_dataFolderPath} not found!");
                return result;
            }

            ProcessDirectory(_dataFolderPath, result);

            return result;
        }

        private static void ProcessDirectory(string path, Dictionary<string, Dictionary<string, object>> container)
        {
            string[] fileEntries = System.IO.Directory.GetFiles(path);
            string[] directoryEntries = System.IO.Directory.GetDirectories(path);
            foreach (var fileEntry in fileEntries)
            {
                if (string.Equals(System.IO.Path.GetExtension(fileEntry), ".json"))
                {
                    ReadFile(fileEntry, container);
                }
            }

            foreach (var directoryPath in directoryEntries)
            {
                ProcessDirectory(directoryPath, container);
            }
        }

        private static void ReadFile(string filePath, Dictionary<string, Dictionary<string, object>> container)
        {
            Dictionary<string, object> dataNode = null;
            using (var sr = new System.IO.StreamReader(filePath))
            {
                var text = sr.ReadToEnd();
                dataNode = (Dictionary<string, object>)fastJSON.JSON.Parse(text);
            }

            var fileName = System.IO.Path.GetFileName(filePath);
            container[fileName] = dataNode;
        }
    }
}
    
#endif