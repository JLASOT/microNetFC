using Aforo255.Cross.Event.Src.Events;
using System;

namespace MS.AFORO255.Customer.Messages.Events
{
    public class SaleCreatedEvent: Event
    {
        private object value;

        public SaleCreatedEvent(int sale_ID, object value)
        {
            Sale_ID = sale_ID;
            this.value = value;
        }

        public SaleCreatedEvent(
                int sale_id,
                DateTime sale_date,
                double total_amount,
                int customer_id
        )
        {
            Sale_ID = sale_id;
            Sale_Date = sale_date;
            Total_Amount = total_amount;
            Customer_Id = customer_id;
        }

        public int Sale_ID { get; set; }
        public DateTime Sale_Date { get; set; }
        public double Total_Amount { get; set; }  
        public int Customer_Id { get; set; }

    }
}
