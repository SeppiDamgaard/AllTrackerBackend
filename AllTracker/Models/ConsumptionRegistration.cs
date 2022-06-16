using System;

namespace AllTracker.Models
{
    public class ConsumptionRegistration
    {
        public string Id { get; set; }
        public ConsumptionRegistration()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ConsumptionPost Post { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
