﻿
// dto is data transfer object is responsible for transfer data in and out the apiA

namespace DevCreedMoviesApi.Dtos
{
    public class CreateGenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
