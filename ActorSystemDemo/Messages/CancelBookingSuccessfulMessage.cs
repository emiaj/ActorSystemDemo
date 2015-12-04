using System;

namespace ActorSystemDemo.Messages
{
  public class CancelBookingSuccessfulMessage
  {
    public CancelBookingSuccessfulMessage(Guid correlationId, string venueName, string conferenceId, int ticketNumber)
    {
      ConferenceId = conferenceId;
      TicketNumber = ticketNumber;
      VenueName = venueName;
      CorrelationId = correlationId;
      CreatedDateTime = DateTime.Now;
    }

    public Guid CorrelationId { get; private set; }
    public string ConferenceId { get; private set; }
    public int TicketNumber { get; private set; }
    public string VenueName { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
  }
}