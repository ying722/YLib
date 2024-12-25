using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using YLib.GameCore;

public class Test : MonoBehaviour
{
    public List<AudioSource> usingPrefab;

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
        AudioSource obj = await resourceModule.Spawn<AudioSource>("TestPrefab");
        GameObject go = await resourceModule.Spawn<GameObject>("TestPrefab");
        PlayableDirector pd = await resourceModule.Spawn<PlayableDirector>("TestPrefab");
        // obj.transform.position = Vector3.zero;
        obj.transform.SetParent(transform,false);
        usingPrefab.Add(obj);
    }

    [Button]
    public async void DeSpawn()
    {
        // await resourceModule.DeSpawn(usingPrefab[0]);
        // usingPrefab.RemoveAt(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
