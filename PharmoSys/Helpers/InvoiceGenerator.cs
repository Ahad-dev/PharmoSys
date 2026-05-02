using PharmoSys.Core.Models;
using PharmoSys.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;

namespace PharmoSys.Helpers
{
    public static class InvoiceGenerator
    {
        public static string GeneratePdf(IEnumerable<CartItem> cartItems, decimal totalAmount, string cashierName)
        {
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PharmoSys_Invoices");
            Directory.CreateDirectory(folderPath);

            var invoiceNumber = $"INV-{DateTime.Now:yyyyMMddHHmmss}";
            var filePath = Path.Combine(folderPath, $"{invoiceNumber}.pdf");

            var settings = SettingsService.LoadSettings();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily(Fonts.Arial));

                    page.Header().Element(c => ComposeHeader(c, invoiceNumber, cashierName, settings));
                    page.Content().Element(c => ComposeContent(c, cartItems, totalAmount));
                    page.Footer().Element(ComposeFooter);
                });
            })
            .GeneratePdf(filePath);

            return filePath;
        }

        private static void ComposeHeader(IContainer container, string invoiceNumber, string cashierName, StoreSettings settings)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(settings.StoreName).FontSize(24).SemiBold().FontColor(Colors.Teal.Medium);
                    column.Item().Text(settings.StoreAddress);
                    column.Item().Text($"Phone: {settings.StoreContact}");
                });

                row.ConstantItem(150).Column(column =>
                {
                    column.Item().Text($"Invoice: {invoiceNumber}").SemiBold();
                    column.Item().Text($"Date: {DateTime.Now:dd MMM yyyy HH:mm}");
                    column.Item().Text($"Cashier: {cashierName}");
                });
            });
        }

        private static void ComposeContent(IContainer container, IEnumerable<CartItem> items, decimal total)
        {
            container.PaddingVertical(1, Unit.Centimetre).Column(column =>
            {
                column.Spacing(5);

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30);
                        columns.RelativeColumn();
                        columns.ConstantColumn(80);
                        columns.ConstantColumn(80);
                        columns.ConstantColumn(100);
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("#");
                        header.Cell().Element(CellStyle).Text("Product");
                        header.Cell().Element(CellStyle).AlignRight().Text("Unit Price");
                        header.Cell().Element(CellStyle).AlignRight().Text("Qty");
                        header.Cell().Element(CellStyle).AlignRight().Text("Total");

                        static IContainer CellStyle(IContainer c) => c.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    });

                    // Rows
                    int index = 1;
                    foreach (var item in items)
                    {
                        table.Cell().Element(CellStyle).Text(index.ToString());
                        table.Cell().Element(CellStyle).Text(item.Name);
                        table.Cell().Element(CellStyle).AlignRight().Text($"PKR {item.Price:N2}");
                        table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
                        table.Cell().Element(CellStyle).AlignRight().Text($"PKR {item.Subtotal:N2}");

                        static IContainer CellStyle(IContainer c) => c.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                        index++;
                    }
                });

                // Total
                column.Item().PaddingTop(15).AlignRight().Text($"Grand Total: PKR {total:N2}").FontSize(16).SemiBold();
            });
        }

        private static void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(x =>
            {
                x.Span("Thank you for your business! - PharmoSys");
            });
        }
    }
}
