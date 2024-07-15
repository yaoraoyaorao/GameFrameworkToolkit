namespace GameFramework.Toolkit.Runtime
{


    public abstract class MouseAction : InputAction, IMouseAction
    {
        protected string m_Name;
        protected bool m_Enable;

        /// <summary>
        /// ��Ϊ��
        /// </summary>
        public override string Name => m_Name;

        /// <summary>
        /// ��Ϊ�Ƿ���
        /// </summary>
        public override bool Enable
        {
            get { return m_Enable; }
            set { m_Enable = value; }
        }

        /// <summary>
        /// ���
        /// </summary>
        public override void Clear()
        {
            m_Name = "";
        }

        public abstract bool Trigger();
    }

}
