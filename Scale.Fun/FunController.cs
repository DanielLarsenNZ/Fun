using System.Threading.Tasks;

namespace Scale.Fun
{
    public class FunController
    {
        public virtual Task ScaleUp() => Task.CompletedTask;
        public virtual Task ScaleDown() => Task.CompletedTask;
    }
}
