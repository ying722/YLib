using System.Linq;
using UnityEngine;
#if UNITY_ADDRESSABLE
using UnityEngine.AddressableAssets;
#endif

namespace GameCore.Exposed
{
    /// <summary>
    /// 類似Odin的GolbalConfig，但是加入Adressable功能，沒匯入依舊使用普通的Resources.Load()
    /// </summary>
    public abstract class BaseReleaseConfig<T> : ScriptableObject where T : ScriptableObject
    {
        const string resourcePath = "Assets/ScriptableObjects/Resources";
        const string addressableResourcesPath = "Assets/ScriptableObjects/AddressableResources";
        static T _instance = null;

        static bool _isRoad = false;
        public static bool IsRoad
        {
            get { return _isRoad; }
        }

        public static async void LoadAssetAsync()
        {
#if UNITY_ADDRESSABLE
        if(!_isRoad)
        {
            _instance = await Addressables.LoadAssetAsync<T>(typeof(T).Name).Task;
            _isRoad = true;
            if(!_instance)
                Debug.Log(typeof(T).Name + "not Loaded");
        }
#else
            Debug.LogError("Addressable system not Import!");
#endif
        }

        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
    //編輯器內讀取檔案方式
            if (!_instance) _instance = ResourcesLoad();
#if UNITY_ADDRESSABLE
            if (!_instance) _instance = EditorAssetDatabaseLoad();
#endif
#else
                //輸出後讀取檔案方式
                if (!_instance) _instance = ResourcesLoadwithDebug();
#if UNITY_ADDRESSABLE
            if (!_instance && !_isRoad)
            {
                Debug.Log("Not Asset ids not load");
                LoadAssetAsync();
            }
#endif
#endif

                //編輯器下如果沒有檔案時自動建立
#if UNITY_EDITOR
            if (!_instance)
            {
                string path = resourcePath;
#if UNITY_ADDRESSABLE
                path = GetAddressablePath();
#else
                path = GetResourcePath();
#endif
                Debug.Log("<color=red>自動建立 " + typeof (T) + $" 於 {path} </color>");
                T asset = ScriptableObject.CreateInstance<T>();
                string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).Name + ".asset");
                UnityEditor.AssetDatabase.CreateAsset(asset, assetPathAndName);
                UnityEditor.AssetDatabase.SaveAssets();

                _instance = asset;

                return asset;
            }
#endif
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
#if UNITY_EDITOR
    private static string GetAddressablePath()
    {
        string path = addressableResourcesPath;
        if(!UnityEditor.AssetDatabase.IsValidFolder(addressableResourcesPath))
        {
            UnityEditor.AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
            UnityEditor.AssetDatabase.CreateFolder("Assets/ScriptableObjects", "AddressableResources");
        }  
        return path;

    }
    private static string GetResourcePath()
    {
        string path = resourcePath;
        if(!UnityEditor.AssetDatabase.IsValidFolder(resourcePath))
        {
            UnityEditor.AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
            UnityEditor.AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Resources");
        } 
        return path;
    }
    private static T EditorAssetDatabaseLoad()
    {
        T results = null;
        string[] folders = new string[]{ addressableResourcesPath };
        string[] guids1 = UnityEditor.AssetDatabase.FindAssets(typeof(T).Name, folders);

        string path = "";
        for(int index = 0; index < guids1.Length; index++)
        {
            path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids1[index]);
            results = (T)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T));
            if(results != null){ break;}
        }

        if(!results) Debug.Log("<color=red>AssetDatabaseLoad 抓不到檔案 " + typeof (T) + $" 於 {addressableResourcesPath} </color>");

        return results;
    }
#endif
        private static T ResourcesLoadwithDebug()
        {
            T results = ResourcesLoad();
            if (!results) Debug.Log("<color=red>Resources.LoadAll() 抓不到檔案 " + typeof(T) + "</color>");
            return results;
        }

        private static T ResourcesLoad()
        {
            T results = null;
            results = Resources.LoadAll<T>("").FirstOrDefault();
            return results;
        }
    }
}
