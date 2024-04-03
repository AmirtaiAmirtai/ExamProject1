namespace ExamProject1.Models
{
    public class Sale
    {

        public required int Id { get; set; }
        public required string LeadId { get; set; }
        public required string SellerId { get; set; }
        public required DateTime AgreementDate { get; set; }
    }
}
