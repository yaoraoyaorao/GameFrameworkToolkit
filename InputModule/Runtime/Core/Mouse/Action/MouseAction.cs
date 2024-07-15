namespace GameFramework.Toolkit.Runtime
{


    public abstract class MouseAction : InputAction, IMouseAction
    {
        protected string m_Name;
        protected bool m_Enable;

        /// <summary>
        /// 行为名
        /// </summary>
        public override string Name => m_Name;

        /// <summary>
        /// 行为是否开启
        /// </summary>
        public override bool Enable
        {
            get { return m_Enable; }
            set { m_Enable = value; }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public override void Clear()
        {
            m_Name = "";
        }

        public abstract bool Trigger();
    }

}
