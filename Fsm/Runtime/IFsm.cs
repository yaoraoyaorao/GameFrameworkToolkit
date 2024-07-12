using System;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// ×´Ì¬»ú½Ó¿Ú
    /// </summary>
    public interface IFsm<T> where T : class
    {
        public string Name { get; }
        public T Owner { get; }
        public int FsmStateCount { get; }
        public bool IsRunning { get; }
        public bool IsDestroyed { get; }
        public FsmState<T> CurrentState { get; }
        public float CurrentStateTime { get; }
        public void Start<TState>() where TState : FsmState<T>;
        public void Start(Type stateType);
        public bool HasState<TState>() where TState : FsmState<T>;
        public bool HasState(Type stateType);
        public TState GetState<TState>() where TState : FsmState<T>;
        public FsmState<T> GetState(Type stateType);
        public FsmState<T>[] GetAllStates();
        public bool HasData(string name);
        public TData GetData<TData>(string name) where TData : Variable;
        public Variable GetData(string name);
        void SetData<TData>(string name, TData data) where TData : Variable;
        void SetData(string name, Variable data);
        bool RemoveData(string name);
    }

}
