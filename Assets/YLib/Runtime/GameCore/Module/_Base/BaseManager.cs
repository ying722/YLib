using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YLib.GameCore
{
    public interface IBaseManager
    {
        void Init();
        void Release();
    }

    public abstract class BaseManager<T> : MonoSingleton<T>,IBaseManager where T : BaseManager<T>
    {
        public abstract void Init();
        public abstract void Release();
    }
}