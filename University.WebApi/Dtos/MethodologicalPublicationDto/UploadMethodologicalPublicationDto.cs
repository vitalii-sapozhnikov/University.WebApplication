using Microsoft.AspNetCore.Http;
using Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.WebApi.Dtos.MethodologicalPublicationDto
{
    public class UploadMethodologicalPublicationDto
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Description { get; set; }
        public string[] Keywords { get; set; }
        public int[] AuthorIds { get; set; }
        public int[] DisciplineIds { get; set; }
        public PublicationType PublicationType { get; set; }
        public DateTime PublicationDate { get; set; }
        public string CloudStorageId { get; set; }
        public int Volume { get; set; }
        public Language Language { get; set; }
    }
}
