namespace ToniEmprega.Models
{
    public class HomeViewModel
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public SnapshotViewModel Snapshot { get; set; }
        public List<Offer> Offers { get; set; }
        public List<Application> Applications { get; set; }
    }


    public class SnapshotViewModel
    {
        public int MyApplications { get; set; }
        public int MatchingOffers { get; set; }
        public int PendingDocuments { get; set; }
    }


    public class Offer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Summary { get; set; }
        public bool IsSaved { get; set; }
        public bool IsPublished { get; set; }
    }


    public class Application
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public string OfferTitle { get; set; }
        public string Status { get; set; }
    }
}