using System;
using System.Threading.Tasks;
using ActorSystemDemo.Messages;
using Akka.Actor;
using Akka.Event;

namespace ActorSystemDemo.ActorModel
{
  public class BillingActor : ReceiveActor
  {
    private readonly ILoggingAdapter _logger = Context.GetLogger();

    public BillingActor()
    {
      Configure();
    }

    protected override void PreStart()
    {
      _logger.Info("BillingActor Started");
      base.PreStart();
    }

    public void Configure()
    {
      Receive<GenerateReceiptMessage>
        (message => GenerateReceipt(message.CorrelationId)
          .PipeTo(Sender));
    }

    private async Task<ReceiptMessage> GenerateReceipt(Guid correlationId)
    {
      await Task.Delay(1000);
      return new ReceiptMessage(correlationId);
    }
  }
}