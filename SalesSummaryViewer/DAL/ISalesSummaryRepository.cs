using SalesSummaryViewer.Models;

namespace SalesSummaryViewer.DAL
{
    public interface ISalesSummaryRepository
    {
        Task<List<SalesSummary>> Get();
    }
}
