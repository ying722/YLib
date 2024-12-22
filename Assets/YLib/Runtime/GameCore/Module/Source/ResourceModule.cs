using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YLib.GameCore;   
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceModule : BaseModule
{
    /// <summary>
    /// 獲取 Addressables 系統中的資源。
    /// </summary>
    /// <typeparam name="T">資源的類型</typeparam>
    /// <param name="address">資源的 Addressables 地址</param>
    /// <returns>返回資源對象</returns>
    public async Task<T> GetResource<T>(string address) where T : Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Resource loaded successfully: {address}");
            return handle.Result;
        }
        else
        {
            Debug.LogError($"Failed to load resource at address: {address}");
            return null;
        }
    }

    public async Task<GameObject> Spawn(string address)
    {
        GameObject obj = await GetResource<GameObject>(address);
        
        return GameobjectPool.Spawn(address,obj);
    }

    public async Task DeSpawn(GameObject obj)
    {
        GameobjectPool.DeSpawn(obj);
    }
}
