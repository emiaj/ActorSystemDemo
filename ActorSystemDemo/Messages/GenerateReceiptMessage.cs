using System;

namespace ActorSystemDemo.Messages
{
  public class GenerateReceiptMessage
  {
    public GenerateReceiptMessage(Guid correlationId)
    {
      CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; private set; }
  }
}