using System;

namespace ActorSystemDemo.Messages
{
  public class CancelBookingMessage
  {
    public CancelBookingMessage(string venueId, string conferenceId, int ticketNumber)
    {
      ConferenceId = conferenceId;
      TicketNumber = ticketNumber;
      VenueId = venueId;

      CorrelationId = Guid.NewGuid();
      CreatedDateTime = DateTime.Now;
    }

    public Guid CorrelationId { get; private set; }
    public string ConferenceId { get; private set; }
    public int TicketNumber { get; private set; }
    public string VenueId { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
  }
}