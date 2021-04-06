//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Scale.Fun
//{
//    public abstract class FunBinding<TInput, TOutput> : FunBinding
//    {
//        protected Func<FunContext, TInput, Task<TOutput>> _fun;
//        protected readonly FunContext _context;

//        public FunBinding(FunContext context)
//        {
//            _context = context;
//        }

//        public Task Bind(Func<FunContext, TInput, Task<TOutput>> fun) => Task.FromResult(_fun = fun);
//    }

//    public abstract class FunBinding { }
//}
