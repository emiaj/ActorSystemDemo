using System;

namespace ActorSystemDemo.Messages
{
  public class ReceiptMessage
  {
    public ReceiptMessage(Guid correlationId)
    {
      CorrelationId = correlationId;
      CreatedDateTime = DateTime.Now;
    }

    public Guid CorrelationId { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
  }
}