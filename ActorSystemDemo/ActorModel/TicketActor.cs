using ActorSystemDemo.Messages;
using Akka.Actor;
using Akka.Event;

namespace ActorSystemDemo.ActorModel
{
  public class TicketActor : ReceiveActor
  {
    private readonly int _ticketNumber;
    private readonly string _conferenceId;
    private bool _booked;

    private readonly ILoggingAdapter _logger = Context.GetLogger();

    public TicketActor(int ticketNumber, string conferenceId)
    {
      _ticketNumber = ticketNumber;
      _conferenceId = conferenceId;

      Configure();
    }

    protected override void PreStart()
    {
      _logger.Info("TicketActor #{0} for conference {1} has started", _ticketNumber, _conferenceId);
      base.PreStart();
    }

    public void Configure()
    {
      Receive<BookConferenceMessage>(message =>
      {
        if (_booked)
        {
          Sender.Tell(new BookingUnsucessfulMessage(message.CorrelationId, 
            message.VenueId, message.ConferenceId,
            message.TicketNumber, message.AttendeeName,
            string.Format("Ticket {0} has been already booked!", _ticketNumber)));
        }
        else
        {
          _booked = true;
          Sender.Tell(new BookingSuccessfulMessage(message.CorrelationId, 
            message.VenueId, message.ConferenceId,
            message.TicketNumber, message.AttendeeName));
        }
      });

      Receive<CancelBookingMessage>(message =>
      {
        if (_booked)
        {
          _booked = false;
          _logger.Info(
            "Ticket {0} for conference {1} and venue {2} has become available again!",
            _ticketNumber, _conferenceId, message.VenueId);
        }
        else
        {
          _logger.Warning(
            "Can't cancel a reservation for ticket {0} for conference {1} and venue {2}  because is still available!",
            _ticketNumber, _conferenceId, message.VenueId);
        }
      });
    }
  }
}