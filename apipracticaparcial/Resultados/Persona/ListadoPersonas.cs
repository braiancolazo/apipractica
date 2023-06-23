namespace apipracticaparcial.Resultados.Persona
{
    public class ListadoPersonas: ResultadoBase
    {
        public List<ItemPersona> ListPersonas { get; set; } = new List<ItemPersona>();
    }
    public class ItemPersona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreCategoria { get; set; }




    }
}
