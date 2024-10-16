namespace TTT
{
    /// <summary>
    /// 单例创建器
    /// </summary>
    public class SingletonBase<T> where T : SingletonBase<T>, new()
    {
        private static T _instance;
        private static readonly object syslock = new object();
        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                
                lock (syslock)
                {
                    _instance = new T();
                    return _instance;
                }
            }
        }

    }
}

