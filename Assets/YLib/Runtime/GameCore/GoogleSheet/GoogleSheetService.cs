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

    #endregion
    }
}
