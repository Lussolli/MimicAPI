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

        [Route("")]
        [HttpGet]
        public ActionResult ObterTodas([FromQuery]PalavraUrlQuery query)
        {
            ListaPaginacao<Palavra> palavras = _repository.ObterPalavras(query);
            if (query.NumeroPagina > palavras.Paginacao.TotalPaginas)
                return NotFound();

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(palavras.Paginacao));
            
            return Ok(palavras.ToList());
        }

        [HttpGet("{id}", Name = "ObterPalavra")]
        public ActionResult Obter(int id)
        {
            Palavra palavra = _repository.Obter(id);

            if (palavra == null)
                return NotFound();

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);
            palavraDTO.Links = new List<LinkDTO>();
            palavraDTO.Links.Add(new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavraDTO.Id }), "GET"));
            palavraDTO.Links.Add(new LinkDTO("update", Url.Link("AtualizarPalavra", new { id = palavraDTO.Id }), "PUT"));
            palavraDTO.Links.Add(new LinkDTO("delete", Url.Link("DeletarPalavra", new { id = palavraDTO.Id }), "DELETE"));

            return Ok(palavraDTO);
        }

        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {
            _repository.Cadastrar(palavra);
            return Created($"/api/palavras/{palavra.Id}", palavra);
        }

        [HttpPut("{id}", Name = "AtualizarPalavra")]
        public ActionResult Atualizar(int id, [FromBody]Palavra palavra)
        {
            Palavra palavraBanco = _repository.Obter(id);

            if (palavraBanco == null)
                return NotFound();

            palavra.Id = id;
            _repository.Atualizar(palavra);

            return Ok();
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
