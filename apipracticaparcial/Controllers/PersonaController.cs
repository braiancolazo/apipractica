using apipracticaparcial.Business.Persona;
using apipracticaparcial.Business.Personas;
using apipracticaparcial.comando;
using apipracticaparcial.Data;
using apipracticaparcial.Models;
using apipracticaparcial.Resultados.Persona;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace apipracticaparcial.Controllers
{
    [ApiController]
    public class PersonaController : Controller
    {
        private readonly ContextDB _contexto;
        private readonly IMediator _mediator;

        public PersonaController(ContextDB contexto, IMediator mediator)
        {
            _contexto = contexto;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("api/personas/getPersonas")]
        public async Task<ListadoPersonas> GetPersonas()
        {
            var result = new ListadoPersonas();
            var personas = await _contexto.Personas.ToListAsync();

            if (personas != null)
            {
                foreach (var item in personas)
                {
                    var itemPersona = new ItemPersona();
                    itemPersona.Id = item.Id;
                    itemPersona.Apellido = item.Apellido;
                    itemPersona.Nombre = item.Nombre;

                    result.ListPersonas.Add(itemPersona);
                }

                return result;
            }

            var mensajeError = "Personas no encontradas";
            result.SetMensajeError(mensajeError, HttpStatusCode.NotFound);
            return result;
        }

        [HttpGet]
        [Route("api/personas/getPersonaById/{id}")]
        public async Task<ListadoPersonas> GetPersonaById(int id)
        {
            return await _mediator.Send(new GetById_Business.GetPersonaByIdComando
            {
                IdPersona = id
            });
        }
        [HttpPost]
        [Route("api/personas/postNuevaPersona")]
        public async Task<ListadoPersonas> PostNuevaPersona([FromBody] NuevaPersona comando)
        {
            return await _mediator.Send(new Save_Business.SavePersonaComando
            {
                Nombre = comando.Nombre,
                Apellido = comando.Apellido,
                IdCategoria = comando.IdCategoria,
            });

        }
        [HttpPut]
        [Route("api/personas/putPersona")]
        public async Task<ListadoPersonas> PutPersona([FromBody] UpdatePersona comando)
        {
            return await _mediator.Send(new Update_Business.UpdatePersonaComando
            {
                Id=comando.Id,
                Nombre = comando.Nombre,
                Apellido = comando.Apellido,
                IdCategoria = comando.IdCategoria,
            });
        }



    }
}
