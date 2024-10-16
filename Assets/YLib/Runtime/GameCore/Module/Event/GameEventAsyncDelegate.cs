using System;
using System.Threading.Tasks;

namespace YLib.GameCore
{
    public class GameEventAsyncDelegate
    {
        public Delegate Delegate { get; set; }

        public async Task Invoke<T1>(T1 eventData)
        {
            await ((Func<T1, Task>)Delegate)?.Invoke(eventData);
        }
    }
}
