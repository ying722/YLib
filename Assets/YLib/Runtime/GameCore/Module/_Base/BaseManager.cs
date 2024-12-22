using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YLib.GameCore
{
    public abstract class BaseManager<T> : MonoSingleton<T> where T : BaseManager<T>
    {
        public abstract void Init();
        public abstract void Release();
    }
}