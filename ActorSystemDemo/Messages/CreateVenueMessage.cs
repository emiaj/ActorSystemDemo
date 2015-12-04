namespace ActorSystemDemo.Messages
{
  public class CreateVenueMessage
  {
    public CreateVenueMessage(string venueId, string venueName)
    {
      VenueId = venueId;
      VenueName = venueName;
    }

    public string VenueId { get; private set; }
    public string VenueName { get; private set; }
  }
}