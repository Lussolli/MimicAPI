using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Repositories.Contracts;

namespace MimicAPI.Repositories
{
    public class PalavraRepository : IPalavraRepository
    {
        private readonly Contexto _contexto;
        public PalavraRepository(Contexto contexto)
        {
            _contexto = contexto;
        }

        public ListaPaginacao<Palavra> ObterPalavras(PalavraUrlQuery query)
        {
            ListaPaginacao<Palavra> lista = new ListaPaginacao<Palavra>();
            IQueryable<Palavra> itens = _contexto.Palavras.AsNoTracking().AsQueryable();
            if (query.Data.HasValue)
            {
                itens = itens.Where(p => p.Criado > query.Data.Value || p.Atualizado > query.Data.Value);
            }

            if (query.NumeroPagina.HasValue)
            {
                int quantidadeTotalRegistros = itens.Count();

                itens = itens.Skip((query.NumeroPagina.Value - 1) * query.QuantidadeRegistros.Value).Take(query.QuantidadeRegistros.Value);
                Paginacao paginacao = new Paginacao();
                paginacao.NumeroPagina = query.NumeroPagina.Value;
                paginacao.RegistrosPorPagina = query.QuantidadeRegistros.Value;
                paginacao.TotalRegistros = quantidadeTotalRegistros;
                paginacao.TotalPaginas = (int)Math.Ceiling((double)quantidadeTotalRegistros / query.QuantidadeRegistros.Value);

                lista.Paginacao = paginacao;
            }

            lista.Resultados.AddRange(itens.ToList());
            return lista;
        }

        public Palavra Obter(int id)
        {
            return _contexto.Palavras.AsNoTracking().FirstOrDefault(p => p.Id == id);
        }

        public void Cadastrar(Palavra palavra)
        {
            _contexto.Palavras.Add(palavra);
            _contexto.SaveChanges();
        }

        public void Atualizar(Palavra palavra)
        {
            _contexto.Palavras.Update(palavra);
            _contexto.SaveChanges();
        }

        public void Deletar(Palavra palavra)
        {
            palavra.Ativo = false;
            _contexto.Palavras.Update(palavra);
            _contexto.SaveChanges();
        }
    }
}
