using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TrabalhoLojaVirtualLibrary.Models;
using TrabalhoLojaVirtualLibrary.Services;

namespace LinkBuyApi.Controllers
{
    [Authorize]
    [Route("api/produto")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoServices produtoServices;
        private readonly VendedorServices vendedorServices;
        private readonly CategoriaServices categoriaServices;

        public ProdutoController(ProdutoServices produtoServices, VendedorServices vendedorServices, CategoriaServices categoriaServices)
        {
            this.produtoServices = produtoServices;
            this.vendedorServices = vendedorServices;
            this.categoriaServices = categoriaServices;
        }

        [AllowAnonymous]
        [HttpGet("listar-produtos-cadastrados")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produto>> GetAllProdutos()
        {

            var produto = await produtoServices.ObterTodosAsync();

            if (produto is null) return NotFound();

            return Ok(produto);
        }

        [SwaggerIgnore]
        public async Task<Vendedor> VerificarVendedor()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var vendedor = await vendedorServices.ObterPorLoginAsync(userIdString);

            return vendedor;
        }

        [AllowAnonymous]
        [HttpGet("todos-produtos")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produto>> GetAllProdutosVendedor()
        {

            var vendedor = await VerificarVendedor();

            if (vendedor == null) return BadRequest("Faça a autenticação para prosseguir");

            var produto = await produtoServices.ObterPorVendedorAsync(vendedor.Id);

            if (produto is null) return NotFound();

            return Ok(produto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            var vendedor = await VerificarVendedor();

            if (vendedor == null) return BadRequest("Faça a autenticação para prosseguir");

            var produto = await produtoServices.ObterProdutoDoVendedor(id, vendedor.Id);

            if (produto is null) return NotFound();

            return Ok(produto);
        }

        [HttpGet("buscar/{CategoriaId:int}")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produto>> GetProdutoByCategoria(int CategoriaId)
        {
            var vendedor = await VerificarVendedor();

            if (vendedor == null) return BadRequest("Faça a autenticação para prosseguir");

            var produto = await produtoServices.ObterPorCategoriaDoVendedorAsync(CategoriaId, vendedor.Id);

            if (produto is null || produto.Count() <= 0) return NotFound("Nenhum produto encontrado com a categoria infomada");

            return Ok(produto);
        }


        [HttpPost("novo")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] InserirNovoProduto produto)
        {

            if (!ModelState.IsValid) return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Um ou mais erros de validação ocorreram!"
            });

            var vendedor = await VerificarVendedor();

            if (vendedor == null) return BadRequest("Faça a autenticação para prosseguir");

            var categoria = await categoriaServices.DetalheCategoria(produto.CategoriaId);

            if (categoria == null) return NotFound("Categoria não encontrada");

            string nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(produto.ImagemUpload.FileName);

            await produtoServices.SalvarImagemAsync(produto.ImagemUpload, nomeArquivo);

            Produto inserir = new Produto
            {
                CategoriaId = produto.CategoriaId,
                VendedorId = vendedor.Id,
                Descricao = produto.Descricao,
                Estoque = produto.Estoque,
                Imagem = nomeArquivo,
                Valor = produto.Valor
            };

            var result = await produtoServices.AdicionarAsync(inserir);

            if (result > 0)
            {

                return CreatedAtAction("Get", new { id = inserir.Id }, inserir);
            }

            return BadRequest("ocorreu um erro ao criar o produto");
        }


        [HttpDelete("deletar/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var vendedor = await VerificarVendedor();

                if (vendedor == null) return BadRequest("Faça a autenticação para prosseguir");

                var produto = await produtoServices.ObterProdutoDoVendedor(id, vendedor.Id);

                if (produto == null)
                {
                    return NotFound("Produto não encontrado");
                }

                var result = await produtoServices.RemoverAsync(produto);

                if (result > 0)
                {
                    await produtoServices.ExcluirImagemAsync(produto.Imagem);
                    return NoContent();
                }

                return BadRequest("Ocorreu um erro ao deletar o produto");
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao deletar o produto");
            }
        }


        [HttpPut("editar/{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Put(int id, [FromForm] InserirNovoProduto produtoInsert)
        {

            if (!ModelState.IsValid) return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Um ou mais erros de validação ocorreram!"
            });

            var vendedor = await VerificarVendedor();

            var categoria = await categoriaServices.DetalheCategoria(produtoInsert.CategoriaId);

            if (categoria == null) return NotFound("Categoria não encontrada");

            var produtoEdit = await produtoServices.ObterDetalhesAsync(id);

            if (produtoEdit == null) return NotFound("Produto não encontrado");

            await produtoServices.ExcluirImagemAsync(produtoEdit.Imagem);

            string nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(produtoInsert.ImagemUpload.FileName);

            await produtoServices.SalvarImagemAsync(produtoInsert.ImagemUpload, nomeArquivo);

            var produto = new Produto()
            {
                Id = id,
                Descricao = produtoInsert.Descricao,
                Estoque = produtoInsert.Estoque,
                Valor = produtoInsert.Valor,
                Imagem = nomeArquivo,
                VendedorId = vendedor.Id,
                CategoriaId = produtoInsert.CategoriaId,
            };



            var result = await produtoServices.AtualizarAsync(produto);

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest("Ocorreu um erro ao tentar editar o produto");
        }


    }
}