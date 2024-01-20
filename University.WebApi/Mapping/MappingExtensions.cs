using Models.Dtos;
using University.WebApi.Models;

namespace University.WebApi.Mapping
{
    public static class MappingExtensions
    {
        public static Author ToAuthorEntity(this AddAuthorDto authorDto)
        {
            return new Author
            {
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                MiddleName = authorDto.MiddleName
            };
        }

        public static AuthorDto ToAuthorDto(this Author author)
        {
            return new AuthorDto
            {
                Id = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName,
                MiddleName = author.MiddleName
            };
        }

        public static Publication ToPublicationEntity(this UploadPublicationDto publicationDto)
        {
            return new Publication
            {
                Title = publicationDto.Title,
                Abstract = publicationDto.Abstract,
                Description = publicationDto.Description,
                PublicationDate = publicationDto.PublicationDate,
                Keywords = publicationDto.Keywords,
                CloudStorageGuid = publicationDto.CloudStorageId
            };
        }
    }
}
