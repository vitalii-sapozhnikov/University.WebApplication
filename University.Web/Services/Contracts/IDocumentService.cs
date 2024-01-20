using System.Text;

namespace University.Web.Services.Contracts
{
    public interface IDocumentService
    {
        Task<StringBuilder> GetContentAsync(IFormFile file);
        Task<string[]> GetKeyWordsAsync(string content, int n);
    }
}
