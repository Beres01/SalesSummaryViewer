using Microsoft.AspNetCore.Components;

namespace SalesSummaryViewer.Models
{
    public class SalesSummaryReportDto
    {
        public MarkupString Category { get; set; }
        public decimal TotalUnitsSold { get; set; }
        public decimal TotalManufacturingPrice { get; set; }
        public decimal TotalSalesPrice { get; set; }
    }
}
