using System.Collections.Generic;
using System.Linq;
using ActorSystemDemo.Messages;
using Akka.Actor;
using Akka.Event;

namespace ActorSystemDemo.ActorModel
{
  public class ConferenceActor : ReceiveActor
  {
    private readonly ILoggingAdapter _logger = Context.GetLogger();

    private readonly string _conferenceId;
    private readonly string _conferenceName;
    private readonly int _numberOfSeats;

    private readonly IDictionary<int, IActorRef> _tickets;

    public ConferenceActor(string conferenceId,
      string conferenceName, int numberOfSeats)
    {
      _conferenceId = conferenceId;
      _conferenceName = conferenceName;
      _numberOfSeats = numberOfSeats;

      _tickets = Enumerable.Range(1, _numberOfSeats)
        .ToDictionary(
          ticketNumber => ticketNumber,
          ticketNumber => Context
            .ActorOf(Props.Create(() =>
              new TicketActor(ticketNumber, _conferenceId)), ticketNumber.ToString()));


      Configure();
    }

    protected override void PreStart()
    {
      _logger.Info("ConferenceActor {0} with {1} tickets has started", _conferenceName, _numberOfSeats);
      base.PreStart();
    }

    public void Configure()
    {
      Receive<BookConferenceMessage>(message =>
      {
        if (_tickets.ContainsKey(message.TicketNumber))
        {
          _tickets[message.TicketNumber].Forward(message);
        }
        else
        {
          Sender.Tell(new BookingUnsucessfulMessage(message.CorrelationId,
            message.VenueId, message.ConferenceId, message.TicketNumber,
            message.AttendeeName, "Ticket not found!"));
        }
      });

      Receive<CancelBookingMessage>(message =>
      {
        if (_tickets.ContainsKey(message.TicketNumber))
        {
          _tickets[message.TicketNumber].Forward(message);
        }
        else
        {
          Sender.Tell(new CancelBookingUnsucessfulMessage(message.CorrelationId, 
            message.VenueId, message.ConferenceId,
            message.TicketNumber, "Ticket not found!"));
        }
      });

    }
  }
}