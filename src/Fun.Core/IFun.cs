using System;
using System.Threading.Tasks;

namespace Fun
{
    public interface IFun<TInput, TOutput> : IFun
    {
        Task<TOutput> Run(FunContext context, TInput input);

        //Task Bind(Func<FunContext, TInput, Task<TOutput>> fun);
    }

    public interface IFun 
    {
        Task Bind();
        Task UnBind();
    }
}
