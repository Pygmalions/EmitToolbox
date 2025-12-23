using EmitToolbox.Symbols;
using JetBrains.Annotations;

namespace EmitToolbox.Builders;

/// <summary>
/// A block builder class to build a try-catch block.
/// </summary>
/// <remarks>
/// Try-cache blocks are protected regions, and they have strict execution transfer rules:
/// <br/> - Normal transfer instructions ('br', 'br_true', 'br_false', etc.)
/// are only allowed to transfer into protected regions and
/// transfer among positions within the same protected region;
/// they are not allowed to transfer out of protected regions.
/// <br/> - Switch transfer instruction ('switch') is only allowed to transfer
/// among positions within the same protected region.
/// </remarks>
[MustDisposeResource]
public class TryCatchBlock: IDisposable
{
    private readonly DynamicFunction _context;

    private bool _disposed;

    private bool _isFinallyDefined;
    
    public TryCatchBlock(DynamicFunction context)
    {
        _context = context;
        _context.Code.BeginExceptionBlock();
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(TryCatchBlock));
        _disposed = true;
        GC.SuppressFinalize(this);
        _context.Code.EndExceptionBlock();
    }

    /// <summary>
    /// Create a catch block for the specified exception type.
    /// </summary>
    /// <param name="exceptionType">Type of the exception to catch.</param>
    /// <param name="exceptionSymbol">Symbol of the caught exception.</param>
    /// <returns>Catch block.</returns>
    [MustDisposeResource]
    public CatchBlock Catch(Type exceptionType, out VariableSymbol exceptionSymbol)
    {
        var block = new CatchBlock(this, exceptionType);
        exceptionSymbol = block.ExceptionSymbol;
        return block;
    }
    
    /// <summary>
    /// Create a catch block for the specified exception type.
    /// </summary>
    /// <typeparam name="TException">Type of the exception to catch.</typeparam>
    /// <param name="exceptionSymbol">Symbol of the caught exception.</param>
    /// <returns>Catch block.</returns>
    [MustDisposeResource]
    public CatchBlock<TException> Catch<TException>(out VariableSymbol<TException> exceptionSymbol) 
        where TException : Exception
    {
        var block = new CatchBlock<TException>(this, typeof(TException));
        exceptionSymbol = block.ExceptionSymbol;
        return block;
    }

    [MustDisposeResource]
    public FinallyBlock Finally()
    {
        if (_isFinallyDefined)
            throw new InvalidOperationException(
                "Cannot define the finally block: it has already been defined in this try-catch block.");
        _isFinallyDefined = true;
        return new FinallyBlock(this);
    }

    [MustDisposeResource]
    public class CatchBlock : IDisposable
    {
        private readonly TryCatchBlock _context;
        
        private bool _disposed;

        internal CatchBlock(TryCatchBlock context, Type exceptionType)
        {
            _context = context;
            context._context.Code.BeginCatchBlock(exceptionType);
            ExceptionSymbol = context._context.Variable(exceptionType);
            ExceptionSymbol.StoreContent();
        }

        public VariableSymbol ExceptionSymbol { get; }

        /// <summary>
        /// Rethrow the caught exception.
        /// </summary>
        public void Rethrow()
        {
            _context._context.Code.Emit(OpCodes.Rethrow);
        }
        
        public void Dispose()
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(CatchBlock));
            _disposed = true;
            // The 'BeginCatchBlock', 'BeginFinallyBlock', etc. methods of 'ILGenerator'
            // will automatically add 'leave' instructions to the end of the block;
            // therefore, we do not need to add 'leave' instructions manually.
            GC.SuppressFinalize(this);
        }
    }

    [MustDisposeResource]
    public class CatchBlock<TException> : CatchBlock
    {
        public new VariableSymbol<TException> ExceptionSymbol
            => field ??= new VariableSymbol<TException>(base.ExceptionSymbol);
        
        internal CatchBlock(TryCatchBlock context, Type exceptionType) : base(context, exceptionType)
        {
        }
    }

    [MustDisposeResource]
    public class FinallyBlock : IDisposable
    {
        private bool _disposed;

        internal FinallyBlock(TryCatchBlock context)
        {
            context._context.Code.BeginFinallyBlock();
        }
        
        public void Dispose()
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(FinallyBlock));
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}

public static class TryCatchBlockExtensions
{
    extension(DynamicFunction function)
    {
        /// <summary>
        /// Create a new try-catch block.
        /// </summary>
        /// <remarks>
        /// Try-cache blocks are protected regions, and they have strict execution transfer rules:
        /// <br/> - Normal transfer instructions ('br', 'br_true', 'br_false', etc.)
        /// are only allowed to transfer into protected regions and
        /// transfer among positions within the same protected region;
        /// they are not allowed to transfer out of protected regions.
        /// <br/> - Switch transfer instruction ('switch') is only allowed to transfer
        /// among positions within the same protected region.
        /// </remarks>
        /// <returns></returns>
        [MustDisposeResource]
        public TryCatchBlock Try() => new(function);
    }
}