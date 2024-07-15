namespace GameFramework.Toolkit.Runtime
{
    public abstract class ProcedureBase : FsmState<ProcedureComponent>
    {
        public override void OnInit(IFsm<ProcedureComponent> fsm)
        {
            base.OnInit(fsm);
        }

        public override void OnEnter(IFsm<ProcedureComponent> fsm)
        {
            base.OnEnter(fsm);
        }

        public override void OnUpdate(IFsm<ProcedureComponent> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        public override void OnLeave(IFsm<ProcedureComponent> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        public override void OnDestroy(IFsm<ProcedureComponent> fsm)
        {
            base.OnDestroy(fsm);
        }
    }

}
