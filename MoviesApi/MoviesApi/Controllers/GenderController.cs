using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Common.Messages;
using MoviesApi.Common.RoutesName;
using MoviesApi.Context;
using MoviesApi.DTOs.Gender;
using MoviesApi.Entities;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/gender")]
    public class GenderController : CustomBaseController
    {
        public GenderController(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }

        [HttpGet]
        public async Task<ActionResult<List<GenderDto>>> Get(CancellationToken token)
        {
            return await Get<Gender, GenderDto>(token);
        }

        [HttpGet("{id:int}", Name = RoutesName.GetGenderById)]
        public async Task<ActionResult<GenderDto>> Get(int id, CancellationToken token)
        {
            return await GetById<Gender, GenderDto>(id, NotFoundMessages.EntityNotFound, token);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateGenderDto createGenderDto, CancellationToken token)
        {
            return await Post<Gender, CreateGenderDto>(createGenderDto, RoutesName.GetGenderById,
                OkMessages.GenderCreated, token);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CreateGenderDto createGenderDto, CancellationToken token)
        {
            return await Put<Gender, CreateGenderDto>(id, createGenderDto, OkMessages.GenderModify, token);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken token)
        {
            return await Delete<Gender>(id, OkMessages.DeletedGenre, NotFoundMessages.GenreNotExist, token);
        }
    }
}