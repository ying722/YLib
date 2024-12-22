using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YLib.GameCore
{
    public abstract class BaseManager : MonoSingleton<BaseManager>
    {
        public abstract void Init();
        public abstract void Release();
    }
}