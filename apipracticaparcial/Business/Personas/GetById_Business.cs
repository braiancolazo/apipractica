using apipracticaparcial.Data;
using apipracticaparcial.Resultados.Persona;
using FluentValidation;
using MediatR;
using apipracticaparcial.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace apipracticaparcial.Business.Persona
{
    public class GetById_Business
    {
        public class GetPersonaByIdComando : IRequest<ListadoPersonas>
        {
            public int IdPersona { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<GetPersonaByIdComando>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.IdPersona).NotEmpty().WithMessage("El id de la persona es oblitario");
            }
        }

        public class Manejador : IRequestHandler<GetPersonaByIdComando, ListadoPersonas>
        {
            // propiedades
            private readonly ContextDB _contexto;

            private readonly IValidator<GetPersonaByIdComando> _validator;
            //constructor

            public Manejador(ContextDB contexto, IValidator<GetPersonaByIdComando> validator)
            {
                _contexto = contexto;
                _validator = validator;
            }

            public async Task<ListadoPersonas> Handle(GetPersonaByIdComando comando, CancellationToken cancellationToken)
            {
                var result = new ListadoPersonas();

                var validation = await _validator.ValidateAsync(comando);
                if (!validation.IsValid)
                {
                    var errors = string.Join(Environment.NewLine, validation.Errors);
                    result.SetMensajeError(errors, HttpStatusCode.InternalServerError);
                    return result;
                }


                var persona = await _contexto.Personas.Where(c => c.Id == comando.IdPersona).Include(c => c.Categoria).FirstOrDefaultAsync();

                if (persona != null)
                {
                    var itemPersona = new ItemPersona
                    {
                        Id = persona.Id,
                        Apellido = persona.Apellido,
                        Nombre = persona.Nombre,
                        NombreCategoria = persona.Categoria.Nombre
                    };

                    result.ListPersonas.Add(itemPersona);
                    return result;
                }

                var mensajeError = "Persona con " + comando.IdPersona.ToString() + " no encontrada";
                result.SetMensajeError(mensajeError, HttpStatusCode.NotFound);

                return result;
            }


        }
        
    }
}
