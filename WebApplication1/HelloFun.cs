using Scale.Fun;
using System.Threading.Tasks;
using WebApplication1.Scale.Fun;

namespace WebApplication1
{
    public class HelloFun : HttpFun<MyModel, MyModel>
    {
        public HelloFun(FunContext context) : base(context) { }

        public override Task<MyModel> Run(FunContext context, MyModel input)
        {
            return Task.FromResult(input);
        }
    }
}
