using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoLojaVirtualLibrary.Models;
using TrabalhoLojaVirtualLibrary.Services;

namespace TrabalhoLojaVirtualAPI.Controllers
{
    [Authorize]
    [Route("api/categoria")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaServices _service;

        public CategoriaController(CategoriaServices service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            var categoria = await _service.DetalheCategoria(id);

            if (categoria is null) return NotFound();

            return Ok(categoria);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Categoria>> GetAllCategorias()
        {
            var categoria = await _service.ListaCategoria();

            if (categoria is null) return NotFound();

            return Ok(categoria);
        }

        [HttpDelete("deletar/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _service.DetalheCategoria(id);
            try
            {
                int resultado = 0;

                if (categoria != null)
                {
                    resultado = await _service.DeletarCategoria(categoria);

                    if (resultado > 0) return NoContent();

                    else throw new Exception("ocorreu um erro ao deletar a categoria");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Não é possível excluir uma categoria com produtos associados.");
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro inesperado.");
            }

        }

        [HttpPost]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([Bind("Descricao")] Categoria categoria)
        {
            if (!ModelState.IsValid) return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Um ou mais erros de validação ocorreram!"
            });

            var result = await _service.CriarCategoria(categoria);

            if (result > 0)
            {
                return CreatedAtAction("Get", new { id = categoria.Id }, categoria);
            }

            return BadRequest();
        }

        [HttpPut("editar/{id:int}")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [Bind("Id,Descricao")] Categoria categoria)
        {
            if (!ModelState.IsValid) return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Um ou mais erros de validação ocorreram!"
            });

            var resultado = 0;

            if (id != categoria.Id)
            {
                return NotFound();
            }

            resultado = await _service.EditarCategoria(categoria);

            if (resultado > 0)
            {
                return NoContent();
            }

            return BadRequest("Ocorreu um erro ao tentar editar a categoria");
        }
    }
}
