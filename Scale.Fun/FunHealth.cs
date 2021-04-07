namespace Fun
{
    public class FunHealth
    {
        public FunHealth(FunHealthStatuses mode) => Mode = mode;
        public FunHealthStatuses Mode { get; set; }
        public static FunHealth Normal() => new(FunHealthStatuses.Normal);
        public static FunHealth Degraded() => new(FunHealthStatuses.Degraded);
        public static FunHealth Failure() => new(FunHealthStatuses.Failure);
    }

    public enum FunHealthStatuses
    {
        Normal = 0,
        Degraded = 1,
        Failure = 2
    }
}
