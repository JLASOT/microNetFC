using Aforo255.Cross.Event.Src.Events;
using MS.AFORO255.Sale.Models;
using System;
using System.Collections.Generic;

namespace MS.AFORO255.Sale.Messages.Events
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
                int customer_id,
                List<SaleDetail> saleDetails
        )
        {
            Sale_ID = sale_id;
            Sale_Date = sale_date;
            Total_Amount = total_amount;
            Customer_Id = customer_id;
            SaleDetails = saleDetails;
        }

        public int Sale_ID { get; set; }
        public DateTime Sale_Date { get; set; }
        public double Total_Amount { get; set; }  
        public int Customer_Id { get; set; }
        public List<SaleDetail> SaleDetails { get; set; }
    }
}
