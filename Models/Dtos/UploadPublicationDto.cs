using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class UploadPublicationDto
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Description { get; set; }
        public string[] Keywords { get; set; }
        public int[] AuthorIds { get; set; }
        public DateTime PublicationDate { get; set; }
        public string CloudStorageId { get; set; }
    }
}
