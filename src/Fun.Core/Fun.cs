using System;
using System.Threading.Tasks;

namespace Fun
{
    public abstract class Fun<TInput, TOutput> : Fun, IFun<TInput, TOutput>
    {
        public Fun(FunContext context) : base(context) { }

        public abstract Task<TOutput> Run(FunContext context, TInput input);
    }

    public abstract class Fun : IFun
    {
        public Fun(FunContext context) => _context = context;

        protected readonly FunContext _context;

        public abstract Task Bind();

        public virtual Task UnBind() => Task.CompletedTask;
    }
}
