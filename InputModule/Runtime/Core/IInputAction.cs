namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 输入行为接口
    /// </summary>
    public interface IInputAction
    {
        /// <summary>
        /// 行为名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="name"></param>
        /// <param name="actionData"></param>
        public void Create(string name, InputActionData actionData);

        /// <summary>
        /// 改变行为数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="actionData"></param>
        public void ChangeActionData(InputActionData actionData);
    }
}
