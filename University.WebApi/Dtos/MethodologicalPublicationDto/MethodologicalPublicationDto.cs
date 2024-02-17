using Models.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.WebApi.Dtos.MethodologicalPublicationDto
{
    public class MethodologicalPublicationDto
    {
        public string? Title { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? CloudStorageGuid { get; set; }
        public string? Abstract { get; set; }
        public string? Description { get; set; }
        public string[]? Keywords { get; set; }
        public bool isPublished { get; set; } = true;
        public int? Volume { get; set; }
        public Language? Language { get; set; }
        public PublicationType? Type { get; set; }
        public int[]? LecturerIds { get; set; }
        public int? PlanId { get; set; }
    }
}
