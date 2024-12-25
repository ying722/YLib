using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;



namespace YLib.GameCore
{
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

        public async Task<T> Spawn<T>(string address) where T : Object
        {
            GameObject resource = await GetResource<GameObject>(address);

            if (resource == null)
            {
                Debug.LogError($"Failed to load resource: {address}");
                return null;
            }

            // 從池中生成物件
            GameObject spawnedObj = GameobjectPool.Spawn(address, resource as GameObject);
            if (spawnedObj == null)
            {
                Debug.LogError($"Failed to spawn object from pool: {address}");
                return null;
            }

            // 檢查是否包含 T 類型的組件
            if (typeof(T) == typeof(GameObject))
            {
                return spawnedObj as T;
            }

            if (resource == null)
            {
                Debug.LogError($"Spawned object does not contain component of type {typeof(T)}: {address}");
                return null;
            }
            
            return spawnedObj.GetComponent<T>();
        }

        public async Task DeSpawn(GameObject obj)
        {
            GameobjectPool.DeSpawn(obj);
        }
    }
}