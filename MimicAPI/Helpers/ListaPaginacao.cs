using System.Collections.Generic;

namespace MimicAPI.Helpers
{
    public class ListaPaginacao<T> : List<T>
    {
        public Paginacao Paginacao { get; set; }
    }
}