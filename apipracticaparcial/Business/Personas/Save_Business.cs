using apipracticaparcial.Data;
using apipracticaparcial.Models;
using apipracticaparcial.Resultados.Persona;
using FluentValidation;
using MediatR;
using System.Net;

namespace apipracticaparcial
{
    public class Save_Business
    {
        public class SavePersonaComando : IRequest<ListadoPersonas>
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public int IdCategoria { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<SavePersonaComando>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre de la persona es oblitario");
                RuleFor(x => x.Apellido).NotEmpty().WithMessage("El apellido de la persona es oblitario");
                RuleFor(x => x.IdCategoria).NotEmpty().WithMessage("El idcategoria de la persona es oblitario");
            }
        }

        public class Manejador : IRequestHandler<SavePersonaComando, ListadoPersonas>
        {
            // propiedades
            private readonly ContextDB _contexto;

            private readonly IValidator<SavePersonaComando> _validator;
            //constructor

            public Manejador(ContextDB contexto, IValidator<SavePersonaComando> validator)
            {
                _contexto = contexto;
                _validator = validator;
            }

            public async Task<ListadoPersonas> Handle(SavePersonaComando comando, CancellationToken cancellationToken)
            {
                var result = new ListadoPersonas();

                var validation = await _validator.ValidateAsync(comando);
                if (!validation.IsValid)
                {
                    var errors = string.Join(Environment.NewLine, validation.Errors);
                    result.SetMensajeError(errors, HttpStatusCode.InternalServerError);
                    return result;
                }



                // validaciones TODO

                var persona = new Persona
                {
                    Apellido = comando.Apellido,
                    Nombre = comando.Nombre,
                    FechaAlta = DateTime.Now,
                    IdCategoria = comando.IdCategoria
                };

                await _contexto.Personas.AddAsync(persona);
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
        }
    }
}
