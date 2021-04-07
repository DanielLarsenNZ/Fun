using System;

namespace Fun
{
    public class FunHealth
    {
        public FunHealth(FunHealthStatuses mode) => Mode = mode;
        
        public FunHealth(FunHealthStatuses mode, Exception ex)
        {
            Mode = mode;
            LastException = ex;
        }

        public FunHealthStatuses Mode { get; private set; }

        public Exception LastException { get; private set; }

        public static FunHealth Normal() => new(FunHealthStatuses.Normal);
        
        public static FunHealth Degraded() => new(FunHealthStatuses.Degraded);
        
        public static FunHealth Degraded(Exception ex) => new(FunHealthStatuses.Degraded, ex);
        
        public static FunHealth Failure() => new(FunHealthStatuses.Failure);
        
        public static FunHealth Failure(Exception ex) => new(FunHealthStatuses.Failure, ex);
    }

    public enum FunHealthStatuses
    {
        Normal = 0,
        Degraded = 1,
        Failure = 2
    }
}
