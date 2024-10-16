using System.Collections.Generic;
using UnityEngine;

namespace YLib.GameCore
{
    public class GameobjectPool
    {
        // 存储可用的游戏对象
        private Queue<GameObject> poolQueue = new Queue<GameObject>();
        
        // 原始的Prefab，池中对象的模板
        private GameObject prefab;
        
        // 父对象，管理池中的对象层级
        private Transform parentTransform;

        private Vector3 initialPosition;
        private Quaternion initialRotation;

        // 构造函数，初始化对象池
        public GameobjectPool(GameObject prefab, int initialSize,Vector3? initialPosition = null, Quaternion? initialRotation = null, Transform parent = null)
        {
            this.prefab = prefab;
            this.parentTransform = parent;
            this.initialPosition = initialPosition ?? Vector3.zero;
            this.initialRotation = initialRotation ?? Quaternion.identity;

            // 预先创建一批对象放入池中
            for (int i = 0; i < initialSize; i++)
            {
                GameObject newObj = GameObject.Instantiate(prefab, this.initialPosition, this.initialRotation, parent);
                newObj.SetActive(false);  // 初始时对象处于禁用状态
                poolQueue.Enqueue(newObj);
            }
        }

        // 从池中获取对象
        public GameObject Get()
        {
            if (poolQueue.Count > 0)
            {
                GameObject obj = poolQueue.Dequeue();
                obj.SetActive(true);  // 激活对象
                return obj;
            }
            else
            {
                // 池中没有可用对象时，创建新的实例
                GameObject newObj = GameObject.Instantiate(prefab, parentTransform);
                return newObj;
            }
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            if(poolQueue.Count > 0)
            {
                GameObject obj = poolQueue.Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                GameObject newObj = GameObject.Instantiate(prefab, position, rotation, parentTransform);
                return newObj;
            }
        }

        public GameObject Get(Vector3 position)
        {
            if(poolQueue.Count > 0)
            {
                GameObject obj = poolQueue.Dequeue();
                obj.transform.position = position;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                GameObject newObj = GameObject.Instantiate(prefab, position, initialRotation, parentTransform);
                return newObj;
            }
        }

        public GameObject Get(Quaternion rotation)
        {
            if(poolQueue.Count > 0)
            {
                GameObject obj = poolQueue.Dequeue();
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                GameObject newObj = GameObject.Instantiate(prefab, initialPosition, initialRotation, parentTransform);
                return newObj;
            }
        }

        

        // 回收对象到池中
        public void Release(GameObject obj,bool resetT = true)
        {
            if (resetT)
            {
                obj.transform.position = initialPosition;
                obj.transform.rotation = initialRotation;
            }
            obj.SetActive(false);  // 禁用对象
            poolQueue.Enqueue(obj);  // 放回池中
        }

        // 返回当前池中的对象数量
        public int Count()
        {
            return poolQueue.Count;
        }
    }
}