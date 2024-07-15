namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 输入行为实现
    /// </summary>
    public abstract class InputAction : IInputAction, IReference
    {
        /// <summary>
        /// 行为名
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 行为是否启动
        /// </summary>
        public abstract bool Enable { get; set; }

        /// <summary>
        /// 切换行为数据
        /// </summary>
        /// <param name="actionData"></param>
        public abstract void ChangeActionData(InputActionData actionData);

        /// <summary>
        /// 创建行为
        /// </summary>
        /// <param name="name"></param>
        /// <param name="actionData"></param>
        public abstract void Create(string name, InputActionData actionData);

        /// <summary>
        /// 清空
        /// </summary>
        public virtual void Clear()
        {
           
        }
    }
}
