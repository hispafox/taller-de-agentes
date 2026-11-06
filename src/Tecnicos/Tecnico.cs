namespace GestorIncidencias;

public class Tecnico
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public bool Activo { get; set; } = true;
}
