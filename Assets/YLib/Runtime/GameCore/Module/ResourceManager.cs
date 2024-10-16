using System.IO;
using UnityEngine;

public static class ResourceManager
{
    // 加載 Resources 文件夾中的資源
    public static T LoadFromResources<T>(string path) where T : Object
    {
        T resource = Resources.Load<T>(path);
        if (resource == null)
        {
            Debug.LogError($"ResourceManager Error: Failed to load resource at path: {path}");
        }
        return resource;
    }

    // 加載 JSON 文件
    public static T LoadFromJson<T>(string name,string path = "")
    {
        if(path == "")
        {
            path = Application.streamingAssetsPath;
        }

        string fullPath = Path.Combine(path, name);
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"ResourceManager Error: JSON file not found at path: {fullPath}");
            return default(T);
        }

        string jsonContent = File.ReadAllText(fullPath);
        return JsonUtility.FromJson<T>(jsonContent);
    }

    // 加載設定檔 (以假設的 Config 為例)
    public static T LoadConfig<T>(string path) where T : ScriptableObject
    {
        T config = Resources.Load<T>(path);
        if (config == null)
        {
            Debug.LogError($"ResourceManager Error: Failed to load config at path: {path}");
        }
        return config;
    }
}
