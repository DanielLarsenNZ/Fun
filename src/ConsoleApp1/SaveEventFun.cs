using Azure.Messaging.EventHubs;
using Fun;
using Fun.Azure;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SaveEventFun : EventHubCosmosFunBinding<MyEvent, MyDocument>
    {
        public SaveEventFun(EventProcessorClient processor, Container container, FunContext context) : base(processor, container, context)
        {
        }

        public override Task<MyDocument> Run(FunContext context, MyEvent input)
        {
            try
            {
                Console.WriteLine("Run");
                return Task.FromResult(
                    new MyDocument { 
                        Id = Guid.NewGuid().ToString("N"), 
                        MyProperty = input.MyProperty });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //context.Logger.LogError(ex);
                context.PostHealth(FunHealth.Failure(ex));
                throw;
            }
        }
    }
}
