using System;

namespace ActorSystemDemo.Messages
{
  public class BookingUnsucessfulMessage
  {
    public BookingUnsucessfulMessage(Guid correlationId, string venueName, string conferenceId, int ticketNumber,
      string attendeeName, string reason)
    {
      Reason = reason;
      CorrelationId = correlationId;
      ConferenceId = conferenceId;
      TicketNumber = ticketNumber;
      VenueName = venueName;
      AttendeeName = attendeeName;
      CreatedDateTIme = DateTime.Now;
    }

    public string Reason { get; private set; }

    public Guid CorrelationId { get; private set; }
    public string ConferenceId { get; private set; }
    public int TicketNumber { get; private set; }
    public string AttendeeName { get; private set; }
    public string VenueName { get; private set; }
    public DateTime CreatedDateTIme { get; private set; }

  }
}