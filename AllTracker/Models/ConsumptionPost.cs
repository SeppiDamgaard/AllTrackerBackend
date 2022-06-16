using System;

namespace AllTracker.Models
{
    public class ConsumptionPost
    {
        public string Id { get; set; }
        public ConsumptionPost()
        {
            Id = Guid.NewGuid().ToString();
        }

        public User User { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string CronString { get; set; }
        public double FirstIncrement { get; set; }
        public double SecondIncrement { get; set; }
        public double ThirdIncrement { get; set; }
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
