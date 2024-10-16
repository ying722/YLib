using System.Linq;
using UnityEngine;
using UnityEditor;

namespace YLib.GameCore.Config
{
    /// <summary>
    /// 專案內通用ScriptableObject設定檔，繼承後對腳本按右鍵Create Config會生成在Resources資料夾底下
    /// 此為Singleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseConfig<T> : ScriptableObject where T : ScriptableObject
    {
        
        
        private static T _instance;
        private static object _lock = new object();
        private static bool applicationIsQuitting = false;

        private static string FilePath { get { return DEFAULT_CONFIG_PATH + typeof(T).Name; } }
        
        /// <summary>
        /// 預設Config在Resources資料夾中的讀取位置
        /// </summary>
        private const string DEFAULT_CONFIG_PATH = "Config/";

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("[Config:Instance] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = Resources.Load<T>(FilePath);

                        if (_instance == null)
                            Debug.LogError(FilePath + " is not exists");
                    }
                    //else
                    //{
                    //    Debug.LogWarning("[Config:Instance] Using instance already created: " + typeof(T));
                    //}
                    return _instance;
                }
            }
        }

        public static void CreateConfig()
        {
            string resourcePath = $"Assets/Resources/{typeof(T).Name}.asset";

            // 检查 Resources 文件夹是否存在，不存在则创建
            if (!System.IO.Directory.Exists("Assets/Resources"))
            {
                System.IO.Directory.CreateDirectory("Assets/Resources");
                AssetDatabase.Refresh(); // 刷新资产数据库
                Debug.Log("已创建 Resources 文件夹。");
            }

            // 检查是否已经存在实例
            if (AssetDatabase.LoadAssetAtPath<T>(resourcePath) != null)
            {
                Debug.LogWarning($"{typeof(T).Name} 已经存在于 Resources 文件夹中，无法创建新的实例。");
                return;
            }

            // 创建新的 ScriptableObject 实例
            T config = CreateInstance<T>();

            // 生成资源的唯一路径
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(resourcePath);
            AssetDatabase.CreateAsset(config, assetPathAndName);
            AssetDatabase.SaveAssets();

            // 选择创建的资源
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = config;

            Debug.Log($"创建了新的 {typeof(T).Name}：{assetPathAndName}");
        }
    

        protected virtual void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }
}