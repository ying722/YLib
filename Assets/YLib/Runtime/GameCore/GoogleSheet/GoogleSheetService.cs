#region

using System;
using ThirdParty.Utilities;
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

        private string ConvertJsonToCsv(string json)
        {
            // 解析 JSON
            LocalizationEntry[] entries = JsonUtility.FromJson<Wrapper>($"{{\"entries\":{json}}}").entries;

            // 建立 CSV 標頭
            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Key,English,Chinese (Taiwan),Chinese (Simplified),Japanese,Korean");

            // 填入數據
            foreach (var entry in entries)
            {
                csvBuilder.AppendLine(
                    $"{EscapeCsvField(entry.Key)},{EscapeCsvField(entry.English)},{EscapeCsvField(entry.Chinese_Taiwan)},{EscapeCsvField(entry.Chinese_Simplified)},{EscapeCsvField(entry.Japanese)},{EscapeCsvField(entry.Korean)}"
                );
            }

            return csvBuilder.ToString();
        }

        private string EscapeCsvField(string field)
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
