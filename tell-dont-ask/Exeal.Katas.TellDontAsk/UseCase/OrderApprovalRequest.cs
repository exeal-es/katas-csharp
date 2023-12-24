namespace Exeal.Katas.TellDontAsk.UseCase
{
    public class OrderApprovalRequest
    {
        public int OrderId { get; set; }
        public bool Approved { get; set; }
    }
}