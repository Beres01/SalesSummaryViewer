using Microsoft.AspNetCore.Components;
using SalesSummaryViewer.DAL;
using SalesSummaryViewer.Models;

namespace SalesSummaryViewer.Components.Pages
{
    public partial class Home
    {
        private bool IsSegmentSelected { get; set; }
        private bool IsCountrySelected { get; set; }
        private bool IsProductSelected { get; set; }
        private bool IsDiscountSelected { get; set; }
        private string SegmentButtonColor => IsSegmentSelected ? "SlateGray" : "#f0f0f0";
        private string SegmentCountryColor => IsCountrySelected ? "SlateGray" : "#f0f0f0";
        private string SegmentProductColor => IsProductSelected ? "SlateGray" : "#f0f0f0";
        private string SegmentDiscountColor => IsDiscountSelected ? "SlateGray" : "#f0f0f0";
        private string SegmentButtonStyle => $"background-color: {SegmentButtonColor}";
        private string SegmentCountryStyle => $"background-color: {SegmentCountryColor}";
        private string SegmentProductStyle => $"background-color: {SegmentProductColor}";
        private string SegmentDiscountStyle => $"background-color: {SegmentDiscountColor}";
        private bool IsNotYetImplemented { get; set; }

        private List<SalesSummary> SalesSummaries { get; set; }
        private List<SalesSummaryReportDto> SalesSummariesReport { get; set; } = new List<SalesSummaryReportDto>();

        [Inject]
        ISalesSummaryRepository SalesSummaryRepository { get; set; }

        public void ToggleSegmentSelected()
        {
            IsSegmentSelected = !IsSegmentSelected;
            StateHasChanged();
        }

        public void ToggleCountrySelected()
        {
            IsCountrySelected = !IsCountrySelected;
            StateHasChanged();
        }

        public void ToggleProductSelected()
        {
            IsProductSelected = !IsProductSelected;
            StateHasChanged();
        }

        public void ToggleDiscountSelected()
        {
            IsDiscountSelected = !IsDiscountSelected;
            StateHasChanged();
        }

        public async Task GenerateSalesSummary()
        {
            SalesSummariesReport.Clear();
            SalesSummaries = await SalesSummaryRepository.Get();
            await GenerateReportData();
        }

        public async Task GenerateReportData()
        {            
            if (!IsSegmentSelected && !IsCountrySelected && !IsProductSelected && !IsDiscountSelected)
            {
                return;
            }

            IsNotYetImplemented = false;

            if (IsSingleSelection())
            {
                if (IsSegmentSelected && !IsCountrySelected && !IsProductSelected && !IsDiscountSelected)
                {
                    SetSalesSummary("Segment");
                }
                else if (!IsSegmentSelected && IsCountrySelected && !IsProductSelected && !IsDiscountSelected)
                {
                    SetSalesSummary("Country");
                }
                else if (!IsSegmentSelected && !IsCountrySelected && IsProductSelected && !IsDiscountSelected)
                {
                    SetSalesSummary("Product");
                }
                else if (!IsSegmentSelected && !IsCountrySelected && !IsProductSelected && IsDiscountSelected)
                {
                    SetSalesSummary("Country");
                }
            }
            else if(IsDoubleSelection())
            {
                if (IsSegmentSelected && IsCountrySelected && !IsProductSelected && !IsDiscountSelected)
                {
                    SetSalesSummary("Segment", "Country");
                }
                else if (IsSegmentSelected && !IsCountrySelected && IsProductSelected && !IsDiscountSelected)
                {
                    SetSalesSummary("Segment", "Product");
                }
                else if (IsSegmentSelected && !IsCountrySelected && !IsProductSelected && IsDiscountSelected)
                {
                    SetSalesSummary("Segment", "Discount");
                }
                else if (!IsSegmentSelected && IsCountrySelected && IsProductSelected && !IsDiscountSelected)
                {
                    SetSalesSummary("Country", "Product");
                }
                else if (!IsSegmentSelected && IsCountrySelected && !IsProductSelected && IsDiscountSelected)
                {
                    SetSalesSummary("Country", "Discount");
                }
                else if (!IsSegmentSelected && !IsCountrySelected && IsProductSelected && IsDiscountSelected)
                {
                    SetSalesSummary("Product", "Discount");
                }
            }
            else if (IsMoreThanImplementedForSelection())
            {
                IsNotYetImplemented = true;
                StateHasChanged();
            }
        }

        private void SetSalesSummary(string categoryName)
        {
            if (IsSingleSelection())
            {
                if (IsSegmentSelected)
                {
                    SalesSummariesReport = (from s in SalesSummaries
                                            group s by s.Segment into g
                                            select new SalesSummaryReportDto()
                                            {
                                                Category = (MarkupString)$"<span>{g.Key}</span>",
                                                TotalManufacturingPrice = g.Sum(c => c.ManufacturingPrice),
                                                TotalUnitsSold = g.Sum(c => c.UnitsSold),
                                                TotalSalesPrice = g.Sum(c => c.SalePrice)
                                            }).ToList();
                }
                else if (IsCountrySelected)
                {
                    SalesSummariesReport = (from s in SalesSummaries
                                            group s by s.Country into g
                                            select new SalesSummaryReportDto()
                                            {
                                                Category = (MarkupString)$"<span>{g.Key}</span>",
                                                TotalManufacturingPrice = g.Sum(c => c.ManufacturingPrice),
                                                TotalUnitsSold = g.Sum(c => c.UnitsSold),
                                                TotalSalesPrice = g.Sum(c => c.SalePrice)
                                            }).ToList();
                }
                else if (IsProductSelected)
                {
                    SalesSummariesReport = (from s in SalesSummaries
                                            group s by s.Country into g
                                            select new SalesSummaryReportDto()
                                            {
                                                Category = (MarkupString)$"<span>{g.Key}</span>",
                                                TotalManufacturingPrice = g.Sum(c => c.ManufacturingPrice),
                                                TotalUnitsSold = g.Sum(c => c.UnitsSold),
                                                TotalSalesPrice = g.Sum(c => c.SalePrice)
                                            }).ToList();
                }
                else if (IsDiscountSelected)
                {
                    SalesSummariesReport = (from s in SalesSummaries
                                            group s by s.Discount into g
                                            select new SalesSummaryReportDto()
                                            {
                                                Category = (MarkupString)$"<span>{g.Key}</span>",
                                                TotalManufacturingPrice = g.Sum(c => c.ManufacturingPrice),
                                                TotalUnitsSold = g.Sum(c => c.UnitsSold),
                                                TotalSalesPrice = g.Sum(c => c.SalePrice)
                                            }).ToList();
                }

                StateHasChanged();
            }           
        }

        private void SetSalesSummary(string categoryName, string secondCategoryName)
        {
            if (IsDoubleSelection())
            {
                if (IsSegmentSelected && IsCountrySelected)
                {
                    SalesSummariesReport = (from s in SalesSummaries
                                            group s by new { s.Segment, s.Country } into g
                                            select new SalesSummaryReportDto()
                                            {
                                                Category = (MarkupString)$"<dl><dt>Segment</dt><dd>{g.Key.Segment}</dd><dt>Country</dt><dd>{g.Key.Country}</dd></dl>",
                                                TotalManufacturingPrice = g.Sum(c => c.ManufacturingPrice),
                                                TotalUnitsSold = g.Sum(c => c.UnitsSold),
                                                TotalSalesPrice = g.Sum(c => c.SalePrice)
                                            }).ToList();
                }
                else if(IsSegmentSelected && IsProductSelected)
                {
                    SalesSummariesReport = (from s in SalesSummaries
                                            group s by new { s.Segment, s.Product } into g
                                            select new SalesSummaryReportDto()
                                            {
                                                Category = (MarkupString)$"<dl><dt>Segment</dt><dd>{g.Key.Segment}</dd><dt>Product</dt><dd>{g.Key.Product}</dd></dl>",
                                                TotalManufacturingPrice = g.Sum(c => c.ManufacturingPrice),
                                                TotalUnitsSold = g.Sum(c => c.UnitsSold),
                                                TotalSalesPrice = g.Sum(c => c.SalePrice)
                                            }).ToList();
                }
                else if (IsSegmentSelected && IsDiscountSelected)
                {
                    SalesSummariesReport = (from s in SalesSummaries
                                            group s by new { s.Segment, s.Discount } into g
                                            select new SalesSummaryReportDto()
                                            {
                                                Category = (MarkupString)$"<dl><dt>Segment</dt><dd>{g.Key.Segment}</dd><dt>Discount</dt><dd>{g.Key.Discount}</dd></dl>",
                                                TotalManufacturingPrice = g.Sum(c => c.ManufacturingPrice),
                                                TotalUnitsSold = g.Sum(c => c.UnitsSold),
                                                TotalSalesPrice = g.Sum(c => c.SalePrice)
                                            }).ToList();
                }
                else if(IsCountrySelected && IsProductSelected)
                {
                    SalesSummariesReport = (from s in SalesSummaries
                                            group s by new { s.Country, s.Product } into g
                                            select new SalesSummaryReportDto()
                                            {
                                                Category = (MarkupString)$"<dl><dt>Country</dt><dd>{g.Key.Country}</dd><dt>Product</dt><dd>{g.Key.Product}</dd></dl>",
                                                TotalManufacturingPrice = g.Sum(c => c.ManufacturingPrice),
                                                TotalUnitsSold = g.Sum(c => c.UnitsSold),
                                                TotalSalesPrice = g.Sum(c => c.SalePrice)
                                            }).ToList();
                }
                else if (IsCountrySelected && IsDiscountSelected)
                {
                    SalesSummariesReport = (from s in SalesSummaries
                                            group s by new { s.Country, s.Discount } into g
                                            select new SalesSummaryReportDto()
                                            {
                                                Category = (MarkupString)$"<dl><dt>Country</dt><dd>{g.Key.Country}</dd><dt>Discount</dt><dd>{g.Key.Discount}</dd></dl>",
                                                TotalManufacturingPrice = g.Sum(c => c.ManufacturingPrice),
                                                TotalUnitsSold = g.Sum(c => c.UnitsSold),
                                                TotalSalesPrice = g.Sum(c => c.SalePrice)
                                            }).ToList();
                }
                else if(IsProductSelected && IsDiscountSelected)
                {
                    SalesSummariesReport = (from s in SalesSummaries
                                            group s by new { s.Product, s.Discount } into g
                                            select new SalesSummaryReportDto()
                                            {
                                                Category = (MarkupString)$"<dl><dt>Product</dt><dd>{g.Key.Product}</dd><dt>Discount</dt><dd>{g.Key.Discount}</dd></dl>",
                                                TotalManufacturingPrice = g.Sum(c => c.ManufacturingPrice),
                                                TotalUnitsSold = g.Sum(c => c.UnitsSold),
                                                TotalSalesPrice = g.Sum(c => c.SalePrice)
                                            }).ToList();
                }

                StateHasChanged();
            }
        }

        private bool IsSingleSelection()
        {
            if((IsSegmentSelected && !IsCountrySelected && !IsProductSelected && !IsDiscountSelected) ||
                (!IsSegmentSelected && IsCountrySelected && !IsProductSelected && !IsDiscountSelected) ||
                (!IsSegmentSelected && !IsCountrySelected && IsProductSelected && !IsDiscountSelected) ||
                (!IsSegmentSelected && !IsCountrySelected && !IsProductSelected && IsDiscountSelected))
            {
                return true;
            }

            return false;
        }

        private bool IsDoubleSelection()
        {
            if ((IsSegmentSelected && IsCountrySelected && !IsProductSelected && !IsDiscountSelected) ||
                (IsSegmentSelected && !IsCountrySelected && IsProductSelected && !IsDiscountSelected) ||
                (IsSegmentSelected && !IsCountrySelected && !IsProductSelected && IsDiscountSelected) ||
                (!IsSegmentSelected && IsCountrySelected && IsProductSelected && !IsDiscountSelected) ||
                (!IsSegmentSelected && IsCountrySelected && !IsProductSelected && IsDiscountSelected) ||
                (!IsSegmentSelected && !IsCountrySelected && IsProductSelected && IsDiscountSelected) ||
                (!IsSegmentSelected && IsCountrySelected && !IsProductSelected && IsDiscountSelected))
            {
                return true;
            }

            return false;
        }

        private bool IsMoreThanImplementedForSelection()
        {
            if( (IsSegmentSelected || IsCountrySelected || IsProductSelected || IsDiscountSelected) && !IsSingleSelection() && !IsDoubleSelection())
            {
                return true;
            }

            return false;
        }

    }
}
