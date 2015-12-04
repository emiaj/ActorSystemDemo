namespace ActorSystemDemo.Messages
{
  public class CreateConferenceMessage
  {
    public string VenueId { get; private set; }
    public string ConferenceId { get; private set; }
    public string ConferenceName { get; private set; }
    public int NumberOfSeats { get; private set; }

    public CreateConferenceMessage(string venueId, string conferenceId, string conferenceName, int numberOfSeats)
    {
      VenueId = venueId;
      ConferenceId = conferenceId;
      ConferenceName = conferenceName;
      NumberOfSeats = numberOfSeats;
    }
  }
}