using Models.Models;

namespace University.WebApi.Dtos
{
    public class AddPublicationToPlanDto
    {
        public string Title { get; set; }
        public int Volume { get; set; }
        public Language Language { get; set; }
        public PublicationType Type { get; set; }
        public int[] LecturerIds { get; set; }
    }
}
