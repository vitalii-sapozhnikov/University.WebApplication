using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.AspNetCore.Http;
using Python.Runtime;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using University.Web.Services.Contracts;

namespace University.Web.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly dynamic pke;
        private readonly dynamic extractor;

        public DocumentService()
        {
            // Set the path to the Python runtime DLL
            Python.Runtime.Runtime.PythonDLL = @"C:\Program Files\Python312\python312.dll";

            // Initialize the Python runtime
            PythonEngine.Initialize();

            // Import the 'pke' module and create an instance of MultipartiteRank
            using (Py.GIL())
            {
                pke = Py.Import("pke");
                Console.WriteLine("PKE imported");

                extractor = pke.unsupervised.MultipartiteRank();
                Console.WriteLine("Extractor created");
            }
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
            var keyphrasesList = new string[n];

            using (Py.GIL())
            {
                // Reuse the existing 'pke' module and 'extractor' instance
                Console.WriteLine("Using existing PKE and Extractor");

                extractor.load_document(input: content, language: "uk", normalization: null);
                Console.WriteLine("Document loaded");

                extractor.candidate_selection();
                Console.WriteLine("Candidates selected");

                extractor.candidate_weighting();
                Console.WriteLine("Candidates weighted");

                dynamic keyphrases = extractor.get_n_best(n: n);
                Console.WriteLine("Got n best");

                int i = 0;
                foreach (var keyphrase in keyphrases)
                {
                    keyphrasesList[i++] = (string)keyphrase[0];
                }
            }

            return keyphrasesList;
        }
    }
}
