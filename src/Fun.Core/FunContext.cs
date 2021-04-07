using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Fun
{
    public class FunContext
    {
        protected readonly FunController _controller;
        protected FunHealth _health = FunHealth.Normal();

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

        public virtual Task<FunHealth> GetHealth() => Task.FromResult(_health);

        /// <summary>
        /// Post a health notification to the Context
        /// </summary>
        /// <param name="health"></param>
        /// <remarks>This method must not block or wait for IO. Use a background queue or worker.</remarks>
        public virtual void PostHealth(FunHealth health) => _health = health;

        /// <remarks>This method must not block or wait for IO. Use a background queue or worker.</remarks>
        public virtual void ScaleUp(object metadata) => _controller?.ScaleUp();

        /// <remarks>This method must not block or wait for IO. Use a background queue or worker.</remarks>
        public virtual void ScaleDown(object metadata) => _controller?.ScaleDown();

        public ILogger Logger { get; private set; }

        public ITelemetry Telemetry { get; private set; }

        public IConfiguration Configuration { get; private set; }
    }
}
