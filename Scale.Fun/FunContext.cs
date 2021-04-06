using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Scale.Fun
{
    public class FunContext
    {
        protected readonly FunController _controller;

        public FunContext(FunController controller, ILogger logger, IConfiguration configuration, ITelemetry telemetry)
        {
            _controller = controller;
            Logger = logger;
            Configuration = configuration;
            Telemetry = telemetry;
        }

        [Obsolete("TODO: Fix logging")]
        public FunContext(FunController controller)
        {
            _controller = controller;
        }

        public virtual Task<FunHealth> Health() => Task.FromResult(FunHealth.Normal());

        public virtual Task ScaleUp(object metadata) => _controller?.ScaleUp();

        public virtual Task ScaleDown(object metadata) => _controller?.ScaleDown();

        public ILogger Logger { get; private set; }

        public ITelemetry Telemetry { get; private set; }

        public IConfiguration Configuration { get; private set; }
    }
}
