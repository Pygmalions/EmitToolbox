using System.Reflection;
using EmitToolbox.Builders;
using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Builders;

[TestFixture, TestOf(typeof(TaskStateMachineBuilder))]
public class TestTaskStateMachineBuilder
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
        asyncBuilder.AwaitResult(
            asyncMethod.Invoke(() => ReturnCompletedTask1()));
        asyncBuilder.AwaitResult(
            asyncMethod.Invoke(() => ReturnDelayedTask1()));

        asyncBuilder.Complete(null);
        
        method.Return(asyncBuilder.Invoke().AsSymbol<Task>());
        
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<Task>>();
        
        Assert.That(functor(), Is.Not.Null);
    }
    
    [Test]
    public async Task AwaitTask_ReturnInt()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        
        var method = type.MethodFactory.Static.DefineFunctor<Task<int>>(
            nameof(AwaitTask_ReturnInt));
        
        var asyncBuilder = method.DefineAsyncStateMachine();
        var asyncMethod = asyncBuilder.Method;
        var symbolNumber1 = asyncBuilder.AwaitResult(
            asyncMethod.Invoke(() => ReturnCompletedTask1()));
        var symbolNumber2 = asyncBuilder.AwaitResult(
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
        var symbolNumber1 = asyncBuilder.AwaitResult(
            asyncMethod.Invoke(() => ReturnCompletedValueTask1()));
        var symbolNumber2 = asyncBuilder.AwaitResult(
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
        var symbolNumber1 = asyncBuilder.AwaitResult(
            asyncMethod.Invoke(() => ReturnCompletedTask1()));
        var symbolNumber2 = asyncBuilder.AwaitResult(
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
        var symbolNumber1 = asyncBuilder.AwaitResult(
            asyncMethod.Invoke(() => ReturnCompletedTask1()));
        var symbolNumber2 = asyncBuilder.AwaitResult(
            asyncMethod.Invoke(() => ReturnDelayedTask1()));
        var symbolNumber3 = asyncBuilder.AwaitResult(argumentNumber);
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
        var symbolNumber1 = asyncBuilder.AwaitResult(
            asyncMethod.Invoke(() => ReturnCompletedValueTask1()));
        var symbolNumber2 = asyncBuilder.AwaitResult(
            asyncMethod.Invoke(() => ReturnCompletedValueTask1()));
        var symbolNumber3 = asyncBuilder.AwaitResult(argumentNumber);
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