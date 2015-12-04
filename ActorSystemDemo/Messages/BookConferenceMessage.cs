using System;

namespace ActorSystemDemo.Messages
{
  public class BookConferenceMessage
  {
    public BookConferenceMessage(string venueId, string conferenceId, int ticketNumber, string attendeeName)
    {
      ConferenceId = conferenceId;
      TicketNumber = ticketNumber;
      AttendeeName = attendeeName;
      VenueId = venueId;

      CorrelationId = Guid.NewGuid();
      CreatedDateTime = DateTime.Now;
    }

    public Guid CorrelationId { get; private set; }
    public string ConferenceId { get; private set; }
    public int TicketNumber { get; private set; }
    public string VenueId { get; private set; }
    public string AttendeeName { get; private set; }

    public DateTime CreatedDateTime { get; private set; }
  }
}