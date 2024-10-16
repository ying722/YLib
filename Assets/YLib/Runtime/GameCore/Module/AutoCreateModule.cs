using System;

namespace YLib.GameCore
{
    /// <summary>
    /// 標記是否在遊戲啟動時自動加入Module
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AutoCreateModule : System.Attribute { }
}