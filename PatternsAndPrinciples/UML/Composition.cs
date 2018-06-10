using System.IO;

namespace PatternsAndPinciples.UML
{
    public class InvoiceService
    {
        private readonly PdfConverter _converter = new PdfConverter();

        public Stream GetPdf(dynamic invoice)
        {
            return _converter.GetStream(invoice);
        }
    }

    public class PdfConverter
    {
        public Stream GetStream(dynamic invoice) => Stream.Null;
    }
}