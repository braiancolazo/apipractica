using apipracticaparcial.Data;
using apipracticaparcial.Resultados.Persona;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace apipracticaparcial.Business.Personas
{
    public class Update_Business
    {
        public class UpdatePersonaComando : IRequest<ListadoPersonas>
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public int IdCategoria { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<UpdatePersonaComando>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("El Id de la persona es oblitario");
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre de la persona es oblitario");
                RuleFor(x => x.Apellido).NotEmpty().WithMessage("El apellido de la persona es oblitario");
                RuleFor(x => x.IdCategoria).NotEmpty().WithMessage("El idcategoria de la persona es oblitario");
            }
        }

        public class Manejador : IRequestHandler<UpdatePersonaComando, ListadoPersonas>
        {
            // propiedades
            private readonly ContextDB _contexto;

            private readonly IValidator<UpdatePersonaComando> _validator;
            //constructor

            public Manejador(ContextDB contexto, IValidator<UpdatePersonaComando> validator)
            {
                _contexto = contexto;
                _validator = validator;
            }

            public async Task<ListadoPersonas> Handle(UpdatePersonaComando comando, CancellationToken cancellationToken)
            {
                var result = new ListadoPersonas();
                // validaciones TODO

                var persona = await _contexto.Personas.FirstOrDefaultAsync(c => c.Id == comando.Id);

                if (persona != null)
                {
                    persona.Nombre = comando.Nombre;
                    persona.Apellido = comando.Apellido;
                    persona.IdCategoria = comando.IdCategoria;
                    persona.FechaModificacion = DateTime.Now;

                    _contexto.Update(persona);
                    await _contexto.SaveChangesAsync();

                    var personaItem = new ItemPersona
                    {
                        Apellido = persona.Apellido,
                        Nombre = persona.Nombre,
                        Id = persona.Id
                    };

                    result.ListPersonas.Add(personaItem);

                    return result;
                }
                else
                {
                    result.SetMensajeError("persona no encontrada", HttpStatusCode.NotFound);
                    return result;
                }
            }
        }
    }
}
