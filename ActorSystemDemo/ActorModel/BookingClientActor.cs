using ActorSystemDemo.Messages;
using Akka.Actor;
using Akka.Event;

namespace ActorSystemDemo.ActorModel
{
  public class BookingClientActor : ReceiveActor
  {
    private readonly ILoggingAdapter _logger = Context.GetLogger();

    public BookingClientActor()
    {
      Configure();
    }

    protected override void PreStart()
    {
      _logger.Info("BookingClientActor Started");
      base.PreStart();
    }

    public void Configure()
    {
      Receive<BookConferenceMessage>(message =>
      {
        Context.System.ActorSelection("user/" + message.VenueId).Tell(message);
      });

      Receive<BookingSuccessfulMessage>(message =>
      {
        _logger.Info("Conference booking was successful with message {0}", message);
      });

      Receive<BookingUnsucessfulMessage>(message =>
      {
        _logger.Info("Conference booking was unsuccessful with message {0}", message.Reason);
      });

      Receive<CancelBookingMessage>(message =>
      {
        Context.System.ActorSelection("user/" + message.VenueId).Tell(message);
      });

      Receive<CancelBookingSuccessfulMessage>(message =>
      {
        _logger.Info("Conference booking cancelation was successful with message {0}", message);
      });
      Receive<CancelBookingUnsucessfulMessage>(message =>
      {
        _logger.Info("Conference booking cancelation was unsuccessful with message {0}", message.Reason);
      });

    }
  }
}