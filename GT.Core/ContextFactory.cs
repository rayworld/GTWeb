using System.Runtime.Remoting.Messaging;

namespace GT.Core
{
    /// <summary>
    /// 数据上下文工厂
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前线程的数据上下文
        /// </summary>
        /// <returns>数据上下文</returns>
        public static GTContext CurrentContext()
        {
            if (!(CallContext.GetData("GTContext") is GTContext _nContext))
            {
                _nContext = new GTContext();
                CallContext.SetData("GTContext", _nContext);
            }
            return _nContext;
        }
    }
}