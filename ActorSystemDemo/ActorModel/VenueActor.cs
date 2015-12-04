using System.Collections.Generic;
using ActorSystemDemo.Messages;
using Akka.Actor;
using Akka.Event;

namespace ActorSystemDemo.ActorModel
{
  public class VenueActor : ReceiveActor
  {
    private readonly IDictionary<string, IActorRef> _conferences = new Dictionary<string, IActorRef>();
    private readonly ILoggingAdapter _logger = Context.GetLogger();

    public VenueActor(string id, string name)
    {
      Id = id;
      Name = name;

      Configure();
    }

    public string Id { get; private set; }
    public string Name { get; }

    protected override void PreStart()
    {
      _logger.Info("VenueActor {0} has started", Name);
      base.PreStart();
    }

    public void Configure()
    {
      Receive<CreateConferenceMessage>(message =>
      {
        if (_conferences.ContainsKey(message.ConferenceId))
        {
          _logger.Info("That conference has been created already!");
        }
        else
        {
          _conferences[message.ConferenceId] = Context.ActorOf(Props.Create(() =>
            new ConferenceActor(message.ConferenceId,
              message.ConferenceName,
              message.NumberOfSeats)), message.ConferenceId);
        }
      });

      Receive<BookConferenceMessage>(message =>
      {
        if (_conferences.ContainsKey(message.ConferenceId))
        {
          _conferences[message.ConferenceId].Forward(message);
        }
        else
        {
          Sender.Tell(new BookingUnsucessfulMessage(message.CorrelationId,
            message.VenueId, message.ConferenceId, message.TicketNumber,
            message.AttendeeName, "Conference not found!"));
        }
      });


      Receive<CancelBookingMessage>(message =>
      {
        if (_conferences.ContainsKey(message.ConferenceId))
        {
          _conferences[message.ConferenceId].Forward(message);
        }
        else
        {
          Sender.Tell(new CancelBookingUnsucessfulMessage(message.CorrelationId, message.VenueId, message.ConferenceId,
            message.TicketNumber, "Conference not found!"));
        }
      });

    }
  }
}