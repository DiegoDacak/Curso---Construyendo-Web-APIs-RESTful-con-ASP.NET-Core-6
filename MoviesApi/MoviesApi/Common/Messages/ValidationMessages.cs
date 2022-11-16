using System.Collections.Generic;

namespace MoviesApi.Common.Messages
{
    public static class ValidationMessages
    {
        public static string MaxSize(int maxSize)
        {
            return $"El peso del archivo no debe ser mayor a {maxSize} mb";
        }

        public static string ArchiveValidTypes(List<string> validTypes)
        {
            return $"El tipo de archivo debe ser uno de los siguientes: {string.Join(", ", validTypes)}";
        }
    }
}