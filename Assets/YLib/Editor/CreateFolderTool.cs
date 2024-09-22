using UnityEditor;
using UnityEngine;

public class CreateFolderTool
{
    [MenuItem("Tools/Create Multiple Folders")]
    private static void CreateMultipleFolders()
    {
        // 定義要創建的資料夾名稱
        string[] folderNames = { "0_Scenes","1_Scripts","2_Prefabs","3_Datas","4_Images","5_UI","6_Animations","7_Audios","8_Videos","9_Materials","10_Shaders","11_Fonts","12_Editor" }; // 自定義資料夾名稱

        foreach (string folderName in folderNames)
        {
            string path = $"Assets/{folderName}";

            // 如果資料夾已經存在，則顯示提示
            if (AssetDatabase.IsValidFolder(path))
            {
                Debug.Log($"Folder already exists: {path}");
                continue;
            }

            // 創建資料夾
            AssetDatabase.CreateFolder("Assets", folderName);
            Debug.Log($"Created folder: {path}");
        }

        AssetDatabase.Refresh(); // 刷新資產數據庫以顯示新資料夾
    }
}
