using System.Collections.Generic;


namespace ToniEmprega.Models
{
    public static class SampleData
    {
        public static List<Offer> GetOffers() => new List<Offer>
        {
            new Offer{ Id=1, Title="Estágio Frontend", Company="Acme SA", Location="Lisboa", Summary="Front-end developer com React.", IsSaved=false, IsPublished=true },
            new Offer{ Id=2, Title="Estágio Backend", Company="Beta Lda", Location="Porto", Summary="C# / ASP.NET Core.", IsSaved=true, IsPublished=true },
            new Offer{ Id=3, Title="Designer", Company="Gamma", Location="Lisboa", Summary="UI/UX designer.", IsSaved=false, IsPublished=false }
        };


        public static List<Application> GetApplications() => new List<Application>
        {
            new Application{ Id=1, OfferId=1, OfferTitle="Estágio Frontend", Status="Submetida" },
            new Application{ Id=2, OfferId=2, OfferTitle="Estágio Backend", Status="Em análise" }
        };
    }
}