using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YLib.GameCore
{
    public abstract class BaseModule
    {
       public bool IsInited { get; protected set; }

        public virtual void Initialize(params object[] param) { IsInited = true; }
        public virtual void OnUpdate(float delta) { }
        public virtual void OnLogicUpdate(float delta) { }
        public virtual void OnFixedUpdate(float delta) { }
        public virtual void Release(params object[] param) { }
        public virtual void OnApplicationQuit() { }
    }
}