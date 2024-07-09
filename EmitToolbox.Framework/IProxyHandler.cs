using EmitToolbox.Framework.Contexts;

namespace EmitToolbox.Framework;

public interface IProxyHandler
{
    Task Process(ClassContext context);
}