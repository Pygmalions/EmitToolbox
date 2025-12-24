using System.Reflection;
using EmitToolbox.Builders;
using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Builders;

[TestFixture, TestOf(typeof(AsyncStateMachineBuilder))]
public class TestAsyncStateMachineBuilder
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        _assembly.IgnoreVisibilityChecksToAssembly(Assembly.GetExecutingAssembly());
    }
    
    [Test]
    public void BuildStateMachine_DoesNotThrow()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        
        var method = type.MethodFactory.Static.DefineFunctor<Task>(
            nameof(BuildStateMachine_DoesNotThrow));
        
        var asyncBuilder = method.DefineAsyncStateMachine();
        var asyncMethod = asyncBuilder.Method;
        asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnCompletedTask1()));
        asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnDelayedTask1()));

        asyncBuilder.Complete(null);
        
        method.Return(asyncBuilder.Invoke().AsSymbol<Task>());
        
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<Task>>();
        
        Assert.That(functor(), Is.Not.Null);
    }
    
    [Test]
    public async Task ValueTaskMethod_AwaitTask_ReturnInt()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        
        var method = type.MethodFactory.Static.DefineFunctor<ValueTask<int>>(
            nameof(AwaitTask_ReturnInt));
        
        var asyncBuilder = method.DefineAsyncStateMachine();
        var asyncMethod = asyncBuilder.Method;
        var symbolNumber1 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnCompletedTask1()));
        var symbolNumber2 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnDelayedTask1()));
        var result = symbolNumber1 + symbolNumber2;
        
        asyncBuilder.Complete(result);
        
        method.Return(asyncBuilder.Invoke().AsSymbol<ValueTask<int>>());
        
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<ValueTask<int>>>();
        var task = functor();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(await task, Is.EqualTo(2));
        }
    }
    
    [Test]
    public async Task AwaitTask_ReturnInt()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        
        var method = type.MethodFactory.Static.DefineFunctor<Task<int>>(
            nameof(AwaitTask_ReturnInt));
        
        var asyncBuilder = method.DefineAsyncStateMachine();
        var asyncMethod = asyncBuilder.Method;
        var symbolNumber1 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnCompletedTask1()));
        var symbolNumber2 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnDelayedTask1()));
        var result = symbolNumber1 + symbolNumber2;
        
        asyncBuilder.Complete(result);
        
        method.Return(asyncBuilder.Invoke().AsSymbol<Task<int>>());
        
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<Task<int>>>();
        var task = functor();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(task, Is.Not.Null);
            Assert.That(await task, Is.EqualTo(2));
        }
    }
    
    [Test]
    public async Task AwaitValueTask_ReturnInt()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        
        var method = type.MethodFactory.Static.DefineFunctor<Task<int>>(
            nameof(AwaitValueTask_ReturnInt));
        var asyncBuilder = method.DefineAsyncStateMachine();
        var asyncMethod = asyncBuilder.Method;
        var symbolNumber1 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnCompletedValueTask1()));
        var symbolNumber2 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnDelayedValueTask1()));
        var result = symbolNumber1 + symbolNumber2;
        
        asyncBuilder.Complete(result);
        
        method.Return(asyncBuilder.Invoke().AsSymbol<Task<int>>());
        
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<Task<int>>>();
        var task = functor();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(task, Is.Not.Null);
            Assert.That(await task, Is.EqualTo(2));
        }
    }
    
    [Test]
    public async Task AwaitTask_CaptureException()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        
        var method = type.MethodFactory.Static.DefineFunctor<Task<int>>(
            nameof(AwaitTask_ReturnInt));
        
        var asyncBuilder = method.DefineAsyncStateMachine();
        var asyncMethod = asyncBuilder.Method;
        var symbolNumber1 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnCompletedTask1()));
        var symbolNumber2 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ThrowsExceptionTask()));
        var result = symbolNumber1 + symbolNumber2;
        
        asyncBuilder.Complete(result);
        
        method.Return(asyncBuilder.Invoke().AsSymbol<Task<int>>());
        
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<Task<int>>>();
        var task = functor();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(task, Is.Not.Null);
            Assert.ThrowsAsync<Exception>(() => task);
            Assert.That(task.IsFaulted);
            Assert.That(task.Exception, Is.TypeOf<AggregateException>());
        }
    }
    
    [Test]
    public async Task AwaitTask_Capture_ReturnInt()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        
        var method = type.MethodFactory.Static.DefineFunctor<Task<int>>(
            nameof(AwaitTask_Capture_ReturnInt), [typeof(Task<int>)]);
        
        var asyncBuilder = method.DefineAsyncStateMachine();
        var argumentNumber = asyncBuilder
            .Capture(method.Argument<Task<int>>(0));
        var asyncMethod = asyncBuilder.Method;
        var symbolNumber1 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnCompletedTask1()));
        var symbolNumber2 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnDelayedTask1()));
        var symbolNumber3 = asyncBuilder.Await(argumentNumber);
        var result = symbolNumber1 +  symbolNumber2 + symbolNumber3;
        
        asyncBuilder.Complete(result);
        
        method.Return(asyncBuilder.Invoke().AsSymbol<Task<int>>());
        
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<Task<int>, Task<int>>>();
        var source = new TaskCompletionSource<int>();
        var task = functor(source.Task);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(task, Is.Not.Null);
            Assert.That(task.IsCompleted, Is.False);
            source.SetResult(1);
            Assert.That(await task, Is.EqualTo(3));
        }
    }
    
    [Test]
    public async Task AwaitValueTask_Capture_ReturnInt()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        
        var method = type.MethodFactory.Static.DefineFunctor<Task<int>>(
            nameof(AwaitValueTask_Capture_ReturnInt), [typeof(Task<int>)]);

        var asyncBuilder = method.DefineAsyncStateMachine();
        
        var argumentNumber = asyncBuilder
            .Capture(method.Argument<Task<int>>(0));
        var asyncMethod = asyncBuilder.Method;
        var symbolNumber1 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnCompletedValueTask1()));
        var symbolNumber2 = asyncBuilder.Await(
            asyncMethod.Invoke(() => ReturnCompletedValueTask1()));
        var symbolNumber3 = asyncBuilder.Await(argumentNumber);
        var result = symbolNumber1 +  symbolNumber2 + symbolNumber3;
        
        asyncBuilder.Complete(result);
        
        method.Return(asyncBuilder.Invoke().AsSymbol<Task<int>>());
        
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<Task<int>, Task<int>>>();
        var source = new TaskCompletionSource<int>();
        var task = functor(source.Task);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(task, Is.Not.Null);
            Assert.That(task.IsCompleted, Is.False);
            source.SetResult(1);
            Assert.That(await task, Is.EqualTo(3));
        }
    }
    
    private static Task<int> ReturnCompletedTask1()
    {
        return Task.FromResult(1);
    }

    private static async Task<int> ReturnDelayedTask1()
    {
        await Task.Delay(10);
        return 1;
    }
    
    private static ValueTask<int> ReturnCompletedValueTask1()
    {
        return ValueTask.FromResult(1);
    }

    private static async ValueTask<int> ReturnDelayedValueTask1()
    {
        await Task.Delay(10);
        return 1;
    }

    private static Task<int> ThrowsExceptionTask()
    {
        throw new Exception();
    }
}