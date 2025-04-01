using SalesSummaryViewer.Models;
using System.Globalization;

namespace SalesSummaryViewer.DAL
{
    public class SalesSummaryRepository : ISalesSummaryRepository
    {
        public async Task<List<SalesSummary>> Get()
        {
            List<SalesSummary> sales = new List<SalesSummary>();
            var lines = await File.ReadAllLinesAsync("~/../Data/Data.csv");
            foreach (var line in lines.Skip(1))
            {
                var salesItem = line.Split(',');

                try
                {
                    var cleanedManufacturingPrice = salesItem[5]?.Remove(0, 1) ?? "0.0";
                    var cleanedSalePrice = salesItem[6].Remove(0, 1);
                    var dateTokens = salesItem[7].Split('/');
                    var saleDate = new DateOnly(int.Parse(dateTokens[2]), int.Parse(dateTokens[0]), int.Parse(dateTokens[1]));
                    var salesRecord = new SalesSummary()
                    {
                        Segment = salesItem[0],
                        Country = salesItem[1],
                        Product = salesItem[2],
                        Discount = salesItem[3],
                        UnitsSold = Decimal.Parse(salesItem[4].Trim()),
                        ManufacturingPrice = Decimal.Parse(cleanedManufacturingPrice),
                        SalePrice = Decimal.Parse(cleanedSalePrice),
                        Date = saleDate
                    };
                    sales.Add(salesRecord);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return sales;
        }
    }
}
