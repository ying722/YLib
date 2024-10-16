using System;

namespace YLib.GameCore
{
    public class GameEventDelegate 
    {
        public Delegate Delegate { get; set; }

        public void Invoke<T>(T eventData)
        {
            ((Action<T>)Delegate)?.Invoke(eventData);
        }
    }
}