using System;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 状态机状态基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FsmState<T> where T : class
    {
        public FsmState()
        {

        }

        public virtual void OnInit(IFsm<T> fsm) { }

        public virtual void OnEnter(IFsm<T> fsm) { }

        public virtual void OnUpdate(IFsm<T> fsm, float elapseSeconds, float realElapseSeconds) { }
    
        public virtual void OnLeave(IFsm<T> fsm, bool isShutdown) { }

        public virtual void OnDestroy(IFsm<T> fsm) { }

        protected void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new Exception("FSM is invalid.");
            }

            fsmImplement.ChangeState<TState>();
        }

        protected void ChangeState(IFsm<T> fsm, Type stateType)
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new Exception("FSM is invalid.");
            }

            if (stateType == null)
            {
                throw new Exception("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new Exception(string.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            fsmImplement.ChangeState(stateType);
        }
    }
}
