using System.Reflection;

namespace EmitToolbox.Framework.Test;

public class ProxyGeneratorTest
{
    [Test]
    public void BuildProxyTest()
    {
        var generator = new ProxyGenerator(Assembly.GetExecutingAssembly().GetName().Name!);
        Assert.DoesNotThrow(delegate
        {
            var proxyClass = generator.Create(typeof(SampleProxyObject));
            var proxyObject = (SampleProxyObject)Activator.CreateInstance(proxyClass)!;
            Assert.That(proxyObject.Add(1, 1), Is.EqualTo(2));
        });
    }

    [Test]
    public void BeforeInvokingTest()
    {
        var generator = new ProxyGenerator(Assembly.GetExecutingAssembly().GetName().Name!);
        generator.Handlers.AddLast(new SampleBeforeProxyHandler());
        
        Assert.DoesNotThrow(delegate
        {
            var proxyClass = generator.Create(typeof(SampleProxyObject));
            var proxyObject = (SampleProxyObject)Activator.CreateInstance(proxyClass)!;
            var result = proxyObject.Add(1, 1);
            Assert.That(result, Is.EqualTo(6));
        });
    }
    
    [Test]
    public void AfterInvokingTest()
    {
        var generator = new ProxyGenerator(Assembly.GetExecutingAssembly().GetName().Name!);
        generator.Handlers.AddLast(new SampleAfterProxyHandler());
        
        Assert.DoesNotThrow(delegate
        {
            var proxyClass = generator.Create(typeof(SampleProxyObject));
            var proxyObject = (SampleProxyObject)Activator.CreateInstance(proxyClass)!;
            var result = proxyObject.Add(1, 1);
            Assert.That(result, Is.EqualTo(3));
        });
    }
}