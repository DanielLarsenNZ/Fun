using Fun;
using Fun.AspNetCore;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class HelloFun : HttpFunBinding<MyModel, MyModel>
    {
        public HelloFun(FunContext context) : base(context) { }

        public override Task<MyModel> Run(FunContext context, MyModel input, CancellationToken cancellationToken)
        {
            return Task.FromResult(input);
        }
    }
}
