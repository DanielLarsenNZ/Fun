using System;
using System.Threading.Tasks;

namespace Fun
{
    public interface IFunBinding
    {
        Task Bind();
        Task UnBind();
    }
}
