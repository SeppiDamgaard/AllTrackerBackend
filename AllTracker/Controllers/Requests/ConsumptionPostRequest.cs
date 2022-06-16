namespace AllTracker.Controllers.Requests
{
    public class ConsumptionPostRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string CronString { get; set; }
        public double FirstIncrement { get; set; }
        public double SecondIncrement { get; set; }
        public double ThirdIncrement { get; set; }
    }
}
