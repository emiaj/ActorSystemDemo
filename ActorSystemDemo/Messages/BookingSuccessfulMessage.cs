using System;

namespace ActorSystemDemo.Messages
{
  public class BookingSuccessfulMessage
  {
    public BookingSuccessfulMessage(Guid correlationId, string venueName, string conferenceId, int ticketNumber,
      string atttendeeName)
    {
      ConferenceId = conferenceId;
      TicketNumber = ticketNumber;
      AtttendeeName = atttendeeName;
      VenueName = venueName;
      CorrelationId = correlationId;
      CreatedDateTime = DateTime.Now;
    }

    public Guid CorrelationId { get; private set; }
    public string ConferenceId { get; private set; }
    public int TicketNumber { get; private set; }
    public string AtttendeeName { get; private set; }
    public string VenueName { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
  }
}