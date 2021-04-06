//using Scale.Fun;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyFun
//{
//    public class EventToCosmosFun : FunContext
//    {
//        private readonly EventProcessorClient _processor;
//        private readonly Container _container;
//        private bool _unbinding = false;

//        public EventFun(EventProcessorClient processor, Container container)
//        {
//            _processor = processor;
//            _container = container;
//        }

//        public override async Task Bind()
//        {
//            // Register handlers for processing events and handling errors
//            _processor.ProcessEventAsync += new(ProcessEventArgs eventArgs)
//            {
//                if (args.CancellationToken.IsCancellationRequested)
//                {
//                    return Task.CompletedTask;
//                }

//                try
//                {
//                    await _container.CreateItemAsync(
//                        Run<MyEvent, MyDocument>(
//                            JsonConvert.DeserializeObject<MyEvent>(
//                                Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()))));

//                    await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
//                    Health.Ok();
//                }
//                catch (OutOfMemoryException ex)
//                {
//                    context.Logger.LogError(ex);
//                    ScaleUp(ex);
//                }
//                catch (Exception ex)
//                {
//                    // TODO: retry logic
//                    context.Logger.LogError(ex);

//                    // Circuit breaker
//                    if (attempts > 3) Health.Error(ex);
//                    else Health.Warning(ex);
//                }
//           };

//        _processor.ProcessErrorAsync += new(ProcessErrorEventArgs eventArgs) 
//        {
//            context.Logger.LogError(eventArgs.Exception);
//            // Circuit breaker
//            if (attempts > 3) Health.Error(ex);
//            else Health.Warning(ex);
//        };

//        // Start the processing
//        await _processor.StartProcessingAsync();

//            while (!_unbinding) Task.Delay(1);
//    }

//    public async Task UnBind()
//    {
//        _unbinding = true;
//        await _processor.StopProcessingAsync();
//    }

//    public async Task<FunHealth> Health() => Health;

//    }
//}
