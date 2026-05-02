using ClosedXML.Excel;
using PharmoSys.Data.Entities;
using PharmoSys.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;

namespace PharmoSys.Helpers
{
    public static class ReportExportGenerator
    {
        public static string GenerateExcelReport(List<SaleEntity> sales, DateTime start, DateTime end)
        {
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PharmoSys_Reports");
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, $"SalesReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sales Report");

                // Header
                worksheet.Cell(1, 1).Value = "PharmoSys Sales Report";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Range(1, 1, 1, 6).Merge();

                worksheet.Cell(2, 1).Value = $"Period: {start:dd MMM yyyy} to {end:dd MMM yyyy}";
                worksheet.Range(2, 1, 2, 6).Merge();

                // Columns
                var headers = new[] { "Sale ID", "Date", "Cashier", "Product", "Qty", "Price", "Subtotal" };
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(4, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.Teal;
                    cell.Style.Font.FontColor = XLColor.White;
                }

                // Data
                int row = 5;
                decimal grandTotal = 0;

                foreach (var sale in sales)
                {
                    foreach (var item in sale.SaleItems)
                    {
                        worksheet.Cell(row, 1).Value = sale.SaleId;
                        worksheet.Cell(row, 2).Value = sale.SaleDate.ToString("yyyy-MM-dd HH:mm");
                        worksheet.Cell(row, 3).Value = sale.User?.Username ?? "Unknown";
                        worksheet.Cell(row, 4).Value = item.Product?.Name ?? "Unknown";
                        worksheet.Cell(row, 5).Value = item.Quantity;
                        worksheet.Cell(row, 6).Value = item.Price;
                        worksheet.Cell(row, 7).Value = item.Subtotal;
                        
                        row++;
                    }
                    grandTotal += sale.FinalAmount;
                }

                // Grand Total
                worksheet.Cell(row + 1, 6).Value = "Grand Total:";
                worksheet.Cell(row + 1, 6).Style.Font.Bold = true;
                worksheet.Cell(row + 1, 7).Value = grandTotal;
                worksheet.Cell(row + 1, 7).Style.Font.Bold = true;

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(filePath);
            }

            return filePath;
        }

        public static string GeneratePdfReport(List<SaleEntity> sales, DateTime start, DateTime end)
        {
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PharmoSys_Reports");
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, $"SalesReport_{DateTime.Now:yyyyMMddHHmmss}.pdf");

            var settings = SettingsService.LoadSettings();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                    page.Header().Element(c =>
                    {
                        c.Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text(settings.StoreName).FontSize(24).SemiBold().FontColor(Colors.Teal.Medium);
                                column.Item().Text(settings.StoreAddress);
                                column.Item().Text($"Phone: {settings.StoreContact}");
                            });

                            row.ConstantItem(150).Column(column =>
                            {
                                column.Item().Text("Detailed Sales Report").FontSize(14).SemiBold();
                                column.Item().Text($"From: {start:dd MMM yyyy}");
                                column.Item().Text($"To: {end:dd MMM yyyy}");
                            });
                        });
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                    {
                        column.Spacing(5);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40); // ID
                                columns.ConstantColumn(90); // Date
                                columns.RelativeColumn();   // Product
                                columns.ConstantColumn(40); // Qty
                                columns.ConstantColumn(60); // Price
                                columns.ConstantColumn(70); // Subtotal
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("ID");
                                header.Cell().Element(CellStyle).Text("Date");
                                header.Cell().Element(CellStyle).Text("Product");
                                header.Cell().Element(CellStyle).AlignRight().Text("Qty");
                                header.Cell().Element(CellStyle).AlignRight().Text("Price");
                                header.Cell().Element(CellStyle).AlignRight().Text("Total");

                                static IContainer CellStyle(IContainer c) => c.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                            });

                            decimal grandTotal = 0;

                            foreach (var sale in sales)
                            {
                                foreach (var item in sale.SaleItems)
                                {
                                    table.Cell().Element(CellStyle).Text(sale.SaleId.ToString());
                                    table.Cell().Element(CellStyle).Text(sale.SaleDate.ToString("dd/MM HH:mm"));
                                    table.Cell().Element(CellStyle).Text(item.Product?.Name ?? "Unknown");
                                    table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
                                    table.Cell().Element(CellStyle).AlignRight().Text(item.Price.ToString("N2"));
                                    table.Cell().Element(CellStyle).AlignRight().Text(item.Subtotal.ToString("N2"));

                                    static IContainer CellStyle(IContainer c) => c.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(3);
                                }
                                grandTotal += sale.FinalAmount;
                            }

                            // Empty spacer row before grand total
                            table.Cell().ColumnSpan(6).PaddingTop(10);
                            
                            table.Cell().ColumnSpan(5).AlignRight().Text("GRAND TOTAL:").SemiBold().FontSize(12);
                            table.Cell().AlignRight().Text($"PKR {grandTotal:N2}").SemiBold().FontSize(12);
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
                });
            })
            .GeneratePdf(filePath);

            return filePath;
        }
    }
}
