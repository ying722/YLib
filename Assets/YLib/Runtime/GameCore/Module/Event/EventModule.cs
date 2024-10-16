using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YLib.GameCore
{
    /// <summary>
    /// 事件管理模組
    ///
    /// </summary>
    [AutoCreateModule]
    public class EventModule : BaseModule
    {
        private Dictionary<Type, GameEventDelegate> _gameEvents = new Dictionary<Type, GameEventDelegate>();
        private Dictionary<Type, GameEventAsyncDelegate> _gameAsyncEvents = new Dictionary<Type, GameEventAsyncDelegate>();

        #region Sync Event.
        public interface ISyncSturct { }

        public void AddListener<T>(Action<T> handler) where T : struct, ISyncSturct
        {
            if (!_gameEvents.TryGetValue(typeof(T), out GameEventDelegate gameEventDelegate))
            {
                gameEventDelegate = new GameEventDelegate
                {
                    Delegate = handler
                };
                _gameEvents.Add(typeof(T), gameEventDelegate);
            }
            else
                gameEventDelegate.Delegate = Delegate.Combine(gameEventDelegate.Delegate, handler);
        }

        public void RemoveListener<T>(Action<T> handler) where T : struct, ISyncSturct
        {
            GameEventDelegate gameEventDelegate;
            if (_gameEvents.TryGetValue(typeof(T), out gameEventDelegate))
            {
                gameEventDelegate.Delegate = Delegate.Remove(gameEventDelegate.Delegate, handler);
            }
        }

        public void NotifyEvent<T>(T eventData = default) where T : struct, ISyncSturct
        {
            GameEventDelegate gameEventDelegate;
            if (_gameEvents.TryGetValue(typeof(T), out gameEventDelegate))
            {
                gameEventDelegate.Invoke(eventData);
            }
        }
        #endregion

        #region Async Event.
        public interface IAsyncSturct { }

        public void AddAsyncListener<T>(Func<T, Task> handler) where T : struct, IAsyncSturct
        {
            if (!_gameAsyncEvents.TryGetValue(typeof(T), out GameEventAsyncDelegate gameEventAsyncDelegate))
            {
                gameEventAsyncDelegate = new GameEventAsyncDelegate
                {
                    Delegate = handler
                };
                _gameAsyncEvents.Add(typeof(T), gameEventAsyncDelegate);
            }
            else
                gameEventAsyncDelegate.Delegate = Delegate.Combine(gameEventAsyncDelegate.Delegate, handler);
        }

        public void RemoveAsyncListener<T>(Func<T, Task> handler) where T : struct, IAsyncSturct
        {
            GameEventAsyncDelegate gameEventAsyncDelegate;
            if (_gameAsyncEvents.TryGetValue(typeof(T), out gameEventAsyncDelegate))
            {
                gameEventAsyncDelegate.Delegate = Delegate.Remove(gameEventAsyncDelegate.Delegate, handler);
            }
        }

        public async Task NotifyAsyncEvent<T>(T eventData = default) where T : struct, IAsyncSturct
        {
            if (_gameAsyncEvents.TryGetValue(typeof(T), out GameEventAsyncDelegate gameEventAsyncDelegate))
            {
                await gameEventAsyncDelegate.Invoke(eventData);
            }
        }
        #endregion
    }
}