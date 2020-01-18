# MimicAPI

API sendo desenvolvida com ASP.NET Core 3.1 no curso da Udemy de [**ASP.NET Core - Web API**](https://www.udemy.com/course/aspnet-core-2-web-api/).

## Iniciar o projeto

----

Clone o projeto na sua máquina e execute o comando `dotnet ef database update` para aplicar as *migrations*.

O projeto utiliza o SGBD SQLite.

O banco de dados MimicAPI.db fica dentro da pasta *Database*.

Execute o comando `dotnet run` no terminal para executar a API.

## Rotas

---

### Palavras

Obter todas: /api/v1.0/palavras [**GET**]

Obter uma palavra pelo id: /api/v1.0/palavras/{id} [**GET**]

Cadastrar: /api/v1.0/palavras [**POST**]

Passando no body:

``` JSON
{
    "nome": "Leão",
    "pontuacao": 1
}
```

Atualizar: /api/v1.0/palavras/{id} [**PUT**]

Passando no body:

``` JSON
{
	"id": 1,
	"nome": "Leão",
	"pontuacao": 10,
	"ativo": true
}
```

Deletar: /api/v1.1/palavras/{id} [**DELETE**]
