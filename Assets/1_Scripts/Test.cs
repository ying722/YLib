using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using YLib.GameCore;

public class Test : MonoBehaviour
{
    public List<GameObject> usingPrefab;

    private ResourceModule resourceModule;
    // Start is called before the first frame update
    async void Start()
    {
        resourceModule = new ResourceModule();
        // prefab = await resourceModule.GetResource<GameObject>("TestPrefab");
    }

    [Button]
    public async void Spawn()
    {
        GameObject obj = await resourceModule.Spawn("TestPrefab");
        // obj.transform.position = Vector3.zero;
        obj.transform.SetParent(transform,false);
        usingPrefab.Add(obj);
    }

    [Button]
    public async void DeSpawn()
    {
        await resourceModule.DeSpawn(usingPrefab[0]);
        usingPrefab.RemoveAt(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
