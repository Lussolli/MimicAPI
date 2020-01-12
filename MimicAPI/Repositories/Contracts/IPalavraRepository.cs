using System.Collections.Generic;
using MimicAPI.Helpers;
using MimicAPI.Models;

namespace MimicAPI.Repositories.Contracts
{
    public interface IPalavraRepository
    {
        ListaPaginacao<Palavra> ObterPalavras(PalavraUrlQuery query);
        Palavra Obter(int id);
        void Cadastrar(Palavra palavra);
        void Atualizar(Palavra palavra);
        void Deletar(Palavra palavra);
    }
}
