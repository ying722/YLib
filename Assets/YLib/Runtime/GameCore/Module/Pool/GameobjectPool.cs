using System.Collections.Generic;
using UnityEngine;

namespace YLib.GameCore
{
    public class GameobjectPool
    {
        private static GameObject poolObject;

        // 存储可用的游戏对象
        private static Dictionary<string,PoolObject> poolQueue = new Dictionary<string, PoolObject>();
        
        // 原始的Prefab，池中对象的模板
        private GameObject prefab;
        
        // 父对象，管理池中的对象层级
        private Transform parentTransform;

        private Vector3 initialPosition;
        private Quaternion initialRotation;

        // 构造函数，初始化对象池
        // public GameobjectPool(GameObject prefab, int initialSize,Vector3? initialPosition = null, Quaternion? initialRotation = null, Transform parent = null)
        // {
        //     this.prefab = prefab;
        //     this.parentTransform = parent;
        //     this.initialPosition = initialPosition ?? Vector3.zero;
        //     this.initialRotation = initialRotation ?? Quaternion.identity;

        //     // 预先创建一批对象放入池中
        //     for (int i = 0; i < initialSize; i++)
        //     {
        //         GameObject newObj = GameObject.Instantiate(prefab, this.initialPosition, this.initialRotation, parent);
        //         newObj.SetActive(false);  // 初始时对象处于禁用状态
        //         poolQueue.Enqueue(newObj);
        //     }
        // }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void CreatePoolObject()
        {
            if(poolObject == null)
            {
                poolObject = new GameObject("[ Pool ]");
                Object.DontDestroyOnLoad(poolObject);
            }
        }

        public static GameObject Spawn(string poolName,GameObject prefab)
        {
            if(poolQueue.ContainsKey(poolName) && poolQueue[poolName].queue.Count > 0)
            {
                GameObject obj = poolQueue[poolName].queue.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                if(!poolQueue.ContainsKey(poolName))
                {
                    GameObject newType = new GameObject($"[ {poolName} ]");
                    newType.transform.SetParent(poolObject.transform);
                    GameObject newObj = GameObject.Instantiate(prefab, newType.transform);
                    newObj.name = poolName;
                    poolQueue.Add(poolName, new PoolObject(newType.transform, new Queue<GameObject>()));
                    // poolQueue[prefab.name].queue.Enqueue(newObj);
                    newObj.SetActive(true);
                    return newObj;
                }
                else
                {
                    GameObject newObj = GameObject.Instantiate(prefab, poolQueue[poolName].poolParent);
                    newObj.name = poolName;
                    newObj.SetActive(true);
                    return newObj;
                }
            }
        }

        public static void DeSpawn(GameObject obj)
        {
            string key = obj.name;
            if(poolQueue.ContainsKey(key))
            {
                obj.transform.SetParent(poolQueue[key].poolParent);
                obj.SetActive(false);
                
                poolQueue[key].queue.Enqueue(obj);
            }
            else
            {
                Debug.LogError("The object is not in the pool.");
            }
        }

        // 从池中获取对象
        // public GameObject Get(GameObject prefab)
        // {
        //     string key = prefab.name;
        //     if (poolQueue.Count > 0)
        //     {
        //         GameObject obj = poolQueue[key].Dequeue();
        //         obj.SetActive(true);  // 激活对象
        //         return obj;
        //     }
        //     else
        //     {
        //         // 池中没有可用对象时，创建新的实例
        //         GameObject newObj = GameObject.Instantiate(prefab, parentTransform);
        //         return newObj;
        //     }
        // }

        // public GameObject Get(Vector3 position, Quaternion rotation)
        // {
        //     if(poolQueue.Count > 0)
        //     {
        //         GameObject obj = poolQueue.Dequeue();
        //         obj.transform.position = position;
        //         obj.transform.rotation = rotation;
        //         obj.SetActive(true);
        //         return obj;
        //     }
        //     else
        //     {
        //         GameObject newObj = GameObject.Instantiate(prefab, position, rotation, parentTransform);
        //         return newObj;
        //     }
        // }

        // public GameObject Get(Vector3 position)
        // {
        //     if(poolQueue.Count > 0)
        //     {
        //         GameObject obj = poolQueue.Dequeue();
        //         obj.transform.position = position;
        //         obj.SetActive(true);
        //         return obj;
        //     }
        //     else
        //     {
        //         GameObject newObj = GameObject.Instantiate(prefab, position, initialRotation, parentTransform);
        //         return newObj;
        //     }
        // }

        // public GameObject Get(Quaternion rotation)
        // {
        //     if(poolQueue.Count > 0)
        //     {
        //         GameObject obj = poolQueue.Dequeue();
        //         obj.transform.rotation = rotation;
        //         obj.SetActive(true);
        //         return obj;
        //     }
        //     else
        //     {
        //         GameObject newObj = GameObject.Instantiate(prefab, initialPosition, initialRotation, parentTransform);
        //         return newObj;
        //     }
        // }

        

        // 回收对象到池中
        // public void Release(GameObject obj,bool resetT = true)
        // {
        //     if (resetT)
        //     {
        //         obj.transform.position = initialPosition;
        //         obj.transform.rotation = initialRotation;
        //     }
        //     obj.SetActive(false);  // 禁用对象
        //     poolQueue.Enqueue(obj);  // 放回池中
        // }

        // 返回当前池中的对象数量
        // public int Count()
        // {
        //     return poolQueue.Count;
        // }

        public class PoolObject
        {
            public Transform poolParent;
            public Queue<GameObject> queue;
        
            public PoolObject(Transform poolParent, Queue<GameObject> queue)
            {
                this.poolParent = poolParent;
                this.queue = queue;
            }
        }
    }
}