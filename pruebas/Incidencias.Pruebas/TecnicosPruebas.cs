using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Incidencias.Pruebas;

public class TecnicosPruebas : PruebaDeApi
{
    [Fact]
    public async Task La_lista_sale_por_orden_de_nombre()
    {
        var tecnicos = await cliente.GetFromJsonAsync<JsonElement>("/tecnicos");

        Assert.Equal(["Javi", "Manolo", "Óscar", "Rafa"], tecnicos.EnumerateArray().Select(t => t.GetProperty("nombre").GetString()));
    }

    [Fact]
    public async Task Rafa_es_el_tecnico_uno()
    {
        var tecnicos = await cliente.GetFromJsonAsync<JsonElement>("/tecnicos");

        Assert.Equal(1, tecnicos.EnumerateArray().First(t => t.GetProperty("nombre").GetString() == "Rafa").GetProperty("id").GetInt32());
    }

    [Fact]
    public async Task Se_da_de_alta_un_tecnico()
    {
        var respuesta = await cliente.PostAsJsonAsync("/tecnicos", new { nombre = "Elena" });

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
        Assert.Contains("Elena", await cliente.GetStringAsync("/tecnicos"));
    }

    [Fact]
    public async Task Sin_nombre_da_400()
    {
        var respuesta = await cliente.PostAsJsonAsync("/tecnicos", new { nombre = " " });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }
}
