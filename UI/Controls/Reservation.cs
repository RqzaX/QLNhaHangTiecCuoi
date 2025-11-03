using System;

namespace UI.Controls
{
    // Reservation data class
    public class Reservation
    {
        public string Code { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string Phone { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public string TableName { get; set; } = "";
        public string Area { get; set; } = "";
        public int Guests { get; set; }
        public string Status { get; set; } = "";
        public string Note { get; set; } = "";
        public decimal Deposit { get; set; }
    }

    // Event args for reservation events
    public class ReservationEventArgs : EventArgs
    {
        public Reservation Reservation { get; }
        public ReservationEventArgs(Reservation reservation)
        {
            Reservation = reservation;
        }
    }
}

