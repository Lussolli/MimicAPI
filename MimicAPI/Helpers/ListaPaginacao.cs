using System.Collections.Generic;
using MimicAPI.Models.DTO;

namespace MimicAPI.Helpers
{
    public class ListaPaginacao<T>
    {
        public List<T> Resultados { get; set; } = new List<T>();
        public Paginacao Paginacao { get; set; }
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}