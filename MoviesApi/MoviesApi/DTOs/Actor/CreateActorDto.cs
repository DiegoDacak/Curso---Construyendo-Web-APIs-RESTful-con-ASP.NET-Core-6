using Microsoft.AspNetCore.Http;
using MoviesApi.Common.Enum;
using MoviesApi.Validations;

namespace MoviesApi.DTOs.Actor
{
    public class CreateActorDto : ActorPatchDto
    {
        [ArchiveTypeValidation(archiveTypeGroup: ArchiveTypeGroup.Image)]
        [ArchiveSizeValidation(maxSizeMb: 4)]
        public IFormFile Photo { get; set; }
    }
}