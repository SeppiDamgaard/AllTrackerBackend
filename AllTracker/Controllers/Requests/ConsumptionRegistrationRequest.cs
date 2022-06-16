using System;

namespace AllTracker.Controllers.Requests
{
    public class ConsumptionRegistrationRequest
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
