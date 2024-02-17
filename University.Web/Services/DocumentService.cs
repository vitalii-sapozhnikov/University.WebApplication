using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using University.Web.Services.Contracts;

namespace University.Web.Services
{
    public class DocumentService : IDocumentService
    {
        public DocumentService()
        {
            
        }

        public async Task<StringBuilder> GetContentAsync(IFormFile file)
        {
            StringBuilder textContent = new StringBuilder();

            using (Stream stream = file.OpenReadStream())
            {
                using (PdfReader pdfReader = new PdfReader(stream))
                {
                    using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                    {
                        for (int pageNumber = 1; pageNumber <= pdfDocument.GetNumberOfPages(); pageNumber++)
                        {
                            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                            string pageText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(pageNumber), strategy);
                            textContent.Append(pageText);
                        }
                    }
                }
            }

            return textContent;
        }

        public async Task<string[]> GetKeyWordsAsync(string content, int n)
        {
            return new string[1];
        }
    }
}
