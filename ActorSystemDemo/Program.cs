using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ActorSystemDemo.ActorModel;
using ActorSystemDemo.Messages;
using Akka.Actor;
using Akka.Util.Internal;

namespace ActorSystemDemo
{
  class Program
  {
    static void Main(string[] args)
    {
      ActorSystem actorSystem = ActorSystem.Create("TicketingActorSystem");

      IActorRef bookingClient = actorSystem
        .ActorOf(Props.Create<BookingClientActor>(), "Booking");

      IActorRef billing = actorSystem
        .ActorOf(Props.Create<BillingActor>(), "Billing");

      while (true)
      {
        var command = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(command))
        {
          continue;
        }
        var data = command.Split(',');
        var action = data[0];
        try
        {
          if (action.StartsWith("venue"))
          {
            ExecuteVenueCommand(actorSystem, data[1], data[2]);
          }
          if (action.StartsWith("conference"))
          {
            ExecuteConferenceCommand(actorSystem, data[1], data[2], data[3], int.Parse(data[4]));
          }
          if (action.StartsWith("book"))
          {
            ExecuteBookCommand(bookingClient, data[1], data[2], int.Parse(data[3]), data[4]);
          }
          if (action.StartsWith("cancel"))
          {
            ExecuteCancelBookingCommand(bookingClient, data[1], data[2], int.Parse(data[3]));
          }
        }
        catch (IndexOutOfRangeException)
        {
          Console.WriteLine("Wrong number of parameters, please try again");
        }

        if (action.StartsWith("generate_receipts"))
        {
          ExecuteGenerateReceiptsCommand(actorSystem, billing);
        }
        if (action.StartsWith("clear"))
        {
          Console.Clear();
        }
        if (action.StartsWith("exit"))
        {
          break;
        }
      }
      actorSystem.AwaitTermination(TimeSpan.FromSeconds(1));
    }

    private static void ExecuteVenueCommand(ActorSystem actorSystem,
      string id, string name)
    {
      actorSystem.ActorOf(Props.Create(() => new VenueActor(id, name)), id);
    }

    private static void ExecuteConferenceCommand(ActorSystem actorSystem,
      string venueId, string conferenceId, string conferenceName, int numberOfSeats)
    {
      actorSystem
        .ActorSelection("user/" + venueId)
        .Tell(new CreateConferenceMessage(venueId,
          conferenceId,
          conferenceName,
          numberOfSeats));
    }

    private static void ExecuteBookCommand(IActorRef bookingClient,
      string venueId, string conferenceId, int ticketNumber, string attendeeName)
    {
      bookingClient.Tell(new BookConferenceMessage(venueId,
        conferenceId,
        ticketNumber,
        attendeeName));
    }

    private static void ExecuteCancelBookingCommand(IActorRef bookingClient,
      string venueId, string conferenceId, int ticketNumber)
    {
      bookingClient
        .Tell(new CancelBookingMessage(venueId,
          conferenceId,
          ticketNumber));
    }

    private static void ExecuteGenerateReceiptsCommand(ActorSystem actorSystem,
      IActorRef billing)
    {
      var counter = 0;
      const int max = 20;

      var stopWatch = new Stopwatch();
      stopWatch.Start();

      actorSystem.Log.Info("Started generating receipts at {0}", DateTime.Now);

      Enumerable.Range(1, max).Select(_ => Guid.NewGuid())
        .Select(guid => new GenerateReceiptMessage(guid))
        .ForEach(async (message) =>
        {
          var receipt = await billing.Ask<ReceiptMessage>(message);

          actorSystem.Log.Info("Receipt {0} generated at {1}",
            receipt.CorrelationId,
            receipt.CreatedDateTime);

          if (Interlocked.Increment(ref counter) == max)
          {
            actorSystem.Log.Info("Job finished at {0} in {1}ms",
              DateTime.Now,
              stopWatch.ElapsedMilliseconds);
          }
        });
    }
  }
}