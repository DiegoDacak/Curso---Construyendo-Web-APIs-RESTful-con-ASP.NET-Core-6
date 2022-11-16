using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(x => x.Libros, opt => opt.MapFrom(MapAutoresDTOLibros));
            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones
                    => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDTO>();
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();
            CreateMap<Libro, LibroDTConAutores>()
                .ForMember(dto => dto.Autores, opt => opt.MapFrom(MapLibroDTOAutores));
            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
        }

        private List<LibroDTO> MapAutoresDTOLibros(Autor autor, AutorDTO autorDto)
        {
            var result = new List<LibroDTO>();
            if (autor.AutoresLibros == null)
            {
                return result;
            }

            foreach (var autorLibro in autor.AutoresLibros)
            {
                result.Add(new LibroDTO()
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Title
                });
            }
            
            return result;
        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDto)
        {
            var resultado = new List<AutorDTO>();
            if (libro.AutoresLibros == null)
            {
                return resultado;
            }

            foreach (var autorLibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });
            }
            return resultado;
        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDto, Libro libro)
        {
            var resultado = new List<AutorLibro>();
            if (libroCreacionDto.AutoresIds == null)
            {
                return resultado;
            }

            foreach (var autorId in libroCreacionDto.AutoresIds)
            {
                resultado.Add(new AutorLibro() {AutorId = autorId});
            }

            return resultado;
        }
    }
}