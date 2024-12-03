#region

using System;
using ThirdParty.Utilities;
using UnityEngine;
using System.Text;
using System.IO;
using Newtonsoft.Json;
#endregion

namespace YLib.GoogleSheet
{
    public static class GoogleSheetService
    {
    #region Public Variables

        public static void LoadDataArray<T>(string url , Action<T[]> complete)
        {
        #if UNITY_EDITOR
            /// <summary>
            /// 獲取 JSON 並轉換為陣列的通用方法
            /// </summary>
            EditorWebRequest.Complete += delegate(string jsonText)
            {
                try
                {
                    var result = JsonHelper.FromJson<T>(jsonText , true);
                    complete.Invoke(result);
                }
                catch (Exception)
                {
                    EditorWebRequest.ClearAction();
                    throw;
                }
            };
            EditorWebRequest.Request(url);
        #endif
        }

        /// <summary>
        /// 獲取原始 JSON 字串
        /// </summary>
        public static void LoadJson(string url, Action<string> complete)
        {
            EditorWebRequest.Complete += delegate (string jsonText)
            {
                try
                {
                    // 直接返回 JSON 字串
                    complete(jsonText);
                }
                catch (Exception)
                {
                    EditorWebRequest.ClearAction();
                    throw;
                }
            };
            EditorWebRequest.Request(url);
        }

        
        public static void LoadCSV(string url, Action<string> complete)
        {
            LoadJson(url, complete => 
            {
                string csvContent = ConvertJsonToCsv(complete);
                Debug.Log($"CSV Content: {csvContent}");
            });
        }

        private static string ConvertJsonToCsv(string json)
        {
            // 使用字典列表解析 JSON
            var entries = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

            // 動態獲取所有的鍵（包含所有語言和 Key）
            HashSet<string> allKeys = new HashSet<string>();
            foreach (var entry in entries)
            {
                foreach (var key in entry.Keys)
                {
                    allKeys.Add(key);
                }
            }

            // 建立 CSV 標頭
            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine(string.Join(",", allKeys));

            // 填入數據
            foreach (var entry in entries)
            {
                List<string> row = new List<string>();
                foreach (var key in allKeys)
                {
                    entry.TryGetValue(key, out string value);
                    row.Add(EscapeCsvField(value));
                }
                csvBuilder.AppendLine(string.Join(",", row));
            }

            return csvBuilder.ToString();
        }

        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return ""; // 空值處理

            // 如果字段中包含逗號或換行符，需要用雙引號括起來
            if (field.Contains(",") || field.Contains("\n"))
                field = $"\"{field.Replace("\"", "\"\"")}\""; // 替換雙引號為雙雙引號

            return field;
        }

    #endregion
    }
}
