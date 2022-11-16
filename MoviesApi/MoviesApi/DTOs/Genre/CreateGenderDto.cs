﻿using System.ComponentModel.DataAnnotations;
using MoviesApi.Common.Messages;

namespace MoviesApi.DTOs.Genre
{
    public class CreateGenderDto
    {
        [Required]
        [StringLength(40, ErrorMessage = ErrorMessages.StringLenght)]
        public string Name { get; set; }
    }
}