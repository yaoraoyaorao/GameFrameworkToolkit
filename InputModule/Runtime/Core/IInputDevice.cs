namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 输入设备接口
    /// </summary>
    public interface IInputDevice
    {
        /// <summary>
        /// 设备是否开启
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 行为列表
        /// </summary>
        public InputAction[] Actions { get; }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Shutdown();

        /// <summary>
        /// 根据行为名获取行为
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public T GetAction<T>(string actionName) where T : class, IInputAction;

        /// <summary>
        /// 根据行为名获取行为
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public IInputAction GetAction(string actionName);
    }
}
