using System;
using System.Threading.Tasks;

namespace Fun
{
    public abstract class FunBinding : IFunBinding
    {        
        public FunBinding(FunContext context) => _context = context;

        protected readonly FunContext _context;

        public abstract Task Bind();

        public virtual Task UnBind() => Task.CompletedTask;
    }
}
