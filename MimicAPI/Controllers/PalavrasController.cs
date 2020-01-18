using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Models.DTO;
using MimicAPI.Repositories.Contracts;
using Newtonsoft.Json;

namespace MimicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PalavrasController : ControllerBase
    {
        private readonly IPalavraRepository _repository;
        private readonly IMapper _mapper;

        public PalavrasController(IPalavraRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("", Name = "ObterTodas")]
        public ActionResult ObterTodas([FromQuery]PalavraUrlQuery query)
        {
            ListaPaginacao<Palavra> palavras = _repository.ObterPalavras(query);
            if (palavras.Resultados.Count == 0)
                return NotFound();
            
            ListaPaginacao<PalavraDTO> lista = CriarLinkListaPalavraDTO(query, palavras);
            return Ok(lista);
        }

        private ListaPaginacao<PalavraDTO> CriarLinkListaPalavraDTO(PalavraUrlQuery query, ListaPaginacao<Palavra> palavras)
        {
            ListaPaginacao<PalavraDTO> lista = _mapper.Map<ListaPaginacao<Palavra>, ListaPaginacao<PalavraDTO>>(palavras);
            foreach (var palavra in lista.Resultados)
                palavra.Links.Add(new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavra.Id }), "GET"));

            lista.Links.Add(new LinkDTO("self", Url.Link("ObterTodas", query), "GET"));

            if (lista.Paginacao != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(lista.Paginacao));

                if (query.NumeroPagina + 1 <= lista.Paginacao.TotalPaginas)
                {
                    var queryString = new PalavraUrlQuery()
                    {
                        NumeroPagina = query.NumeroPagina + 1,
                        QuantidadeRegistros = query.QuantidadeRegistros,
                        Data = query.Data
                    };
                    lista.Links.Add(new LinkDTO("next", Url.Link("ObterTodas", queryString), "GET"));
                }

                if (query.NumeroPagina - 1 > 0)
                {
                    var queryString = new PalavraUrlQuery()
                    {
                        NumeroPagina = query.NumeroPagina - 1,
                        QuantidadeRegistros = query.QuantidadeRegistros,
                        Data = query.Data
                    };
                    lista.Links.Add(new LinkDTO("prev", Url.Link("ObterTodas", queryString), "GET"));
                }
            }

            return lista;
        }

        [HttpGet("{id}", Name = "ObterPalavra")]
        public ActionResult Obter(int id)
        {
            Palavra palavra = _repository.Obter(id);

            if (palavra == null)
                return NotFound();

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);
            palavraDTO.Links.Add(new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavraDTO.Id }), "GET"));
            palavraDTO.Links.Add(new LinkDTO("update", Url.Link("AtualizarPalavra", new { id = palavraDTO.Id }), "PUT"));
            palavraDTO.Links.Add(new LinkDTO("delete", Url.Link("DeletarPalavra", new { id = palavraDTO.Id }), "DELETE"));

            return Ok(palavraDTO);
        }

        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {
            _repository.Cadastrar(palavra);
            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);
            palavraDTO.Links.Add(
                new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavraDTO.Id }), "GET")
            );
            return Created($"/api/palavras/{palavra.Id}", palavraDTO);
        }

        [HttpPut("{id}", Name = "AtualizarPalavra")]
        public ActionResult Atualizar(int id, [FromBody]Palavra palavra)
        {
            Palavra palavraBanco = _repository.Obter(id);

            if (palavraBanco == null)
                return NotFound();

            palavra.Id = id;
            _repository.Atualizar(palavra);
            
            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);
            palavraDTO.Links.Add(
                new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavraDTO.Id }), "GET")
            );

            return Ok(palavraDTO);
        }

        [HttpDelete("{id}", Name = "DeletarPalavra")]
        public ActionResult Deletar(int id)
        {
            Palavra palavra = _repository.Obter(id);

            if (palavra == null)
                return NotFound();
            
            _repository.Deletar(palavra);
            return NoContent();
        }
    }
}
