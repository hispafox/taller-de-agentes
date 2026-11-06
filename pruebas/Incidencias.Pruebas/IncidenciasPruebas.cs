using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Incidencias.Pruebas;

public class IncidenciasPruebas : PruebaDeApi
{
    private const int Abierta = 1;

    private async Task<int> IdDe(string titulo)
    {
        var lista = await cliente.GetFromJsonAsync<JsonElement>("/incidencias");
        return lista.EnumerateArray()
            .First(i => i.GetProperty("titulo").GetString()!.StartsWith(titulo))
            .GetProperty("id").GetInt32();
    }

    [Fact]
    public async Task La_incidencia_uno_de_la_carga_esta_abierta()
    {
        var incidencia = await cliente.GetFromJsonAsync<JsonElement>("/incidencias/1");

        Assert.Equal("Fuga en el garaje, plaza 14", incidencia.GetProperty("titulo").GetString());
        Assert.Equal("Abierta", incidencia.GetProperty("estado").GetString());
    }

    [Fact]
    public async Task Abrir_una_incidencia_la_deja_abierta_con_la_fecha_del_reloj()
    {
        var respuesta = await cliente.PostAsJsonAsync("/incidencias",
            new { titulo = "Timbre del 2.º B", descripcion = (string?)null, observaciones = (string?)null, prioridad = "Alta", zonaId = 1 });

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
        var creada = await respuesta.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Abierta", creada.GetProperty("estado").GetString());
        Assert.Equal("Alta", creada.GetProperty("prioridad").GetString());
        Assert.Equal("Norte", creada.GetProperty("zona").GetString());
        Assert.StartsWith("2026-11-20", creada.GetProperty("fechaApertura").GetString());
    }

    [Fact]
    public async Task Sin_titulo_da_400()
    {
        var respuesta = await cliente.PostAsJsonAsync("/incidencias",
            new { titulo = " ", descripcion = (string?)null, observaciones = (string?)null, prioridad = "Normal", zonaId = (int?)null });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task Un_cuerpo_que_no_se_puede_leer_da_400_y_no_500()
    {
        var respuesta = await cliente.PostAsJsonAsync("/incidencias",
            new { titulo = "Grifo", descripcion = (string?)null, observaciones = (string?)null, prioridad = "Inventada", zonaId = (int?)null });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        Assert.Equal("application/problem+json", respuesta.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Una_zona_que_no_existe_da_400_con_problem_details()
    {
        var respuesta = await cliente.PostAsJsonAsync("/incidencias",
            new { titulo = "Grifo del cuarto de basuras", descripcion = (string?)null, observaciones = (string?)null, prioridad = "Normal", zonaId = 99 });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        Assert.Equal("application/problem+json", respuesta.Content.Headers.ContentType?.MediaType);
        var error = await respuesta.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("La zona no existe", error.GetProperty("title").GetString());
        Assert.Equal(400, error.GetProperty("status").GetInt32());
        Assert.Equal("No hay ninguna zona con el identificador 99.", error.GetProperty("detail").GetString());
    }

    [Fact]
    public async Task Una_zona_que_no_existe_no_cambia_la_incidencia_al_editar()
    {
        var antes = await cliente.GetStringAsync($"/incidencias/{Abierta}");

        var respuesta = await cliente.PutAsJsonAsync($"/incidencias/{Abierta}",
            new { titulo = "Otro título", descripcion = (string?)null, observaciones = (string?)null, prioridad = "Normal", zonaId = 99 });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        Assert.Equal(antes, await cliente.GetStringAsync($"/incidencias/{Abierta}"));
    }

    [Fact]
    public async Task Una_incidencia_que_no_existe_da_404()
    {
        var respuesta = await cliente.GetAsync("/incidencias/9999");

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
    }

    [Fact]
    public async Task Editar_cambia_lo_guardado()
    {
        await cliente.PutAsJsonAsync($"/incidencias/{Abierta}",
            new { titulo = "Fuga en el garaje", descripcion = (string?)null, observaciones = "Van dos", prioridad = "Alta", zonaId = 2 });

        var leida = await cliente.GetFromJsonAsync<JsonElement>($"/incidencias/{Abierta}");
        Assert.Equal("Fuga en el garaje", leida.GetProperty("titulo").GetString());
        Assert.Equal("Van dos", leida.GetProperty("observaciones").GetString());
        Assert.Equal("Alta", leida.GetProperty("prioridad").GetString());
    }

    [Fact]
    public async Task Se_resuelve_se_cierra_y_se_reabre()
    {
        var resuelta = await cliente.PostAsync($"/incidencias/{Abierta}/resolver", null);
        Assert.Equal(HttpStatusCode.OK, resuelta.StatusCode);

        var cerrada = await cliente.PostAsync($"/incidencias/{Abierta}/cerrar", null);
        var json = await cerrada.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Cerrada", json.GetProperty("estado").GetString());
        Assert.StartsWith("2026-11-20", json.GetProperty("fechaCierre").GetString());

        var reabierta = await cliente.PostAsync($"/incidencias/{Abierta}/reabrir", null);
        json = await reabierta.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Abierta", json.GetProperty("estado").GetString());
        Assert.Equal(JsonValueKind.Null, json.GetProperty("fechaCierre").ValueKind);
    }

    [Fact]
    public async Task Una_incidencia_abierta_no_se_puede_cerrar_sin_resolver()
    {
        var respuesta = await cliente.PostAsync($"/incidencias/{Abierta}/cerrar", null);

        Assert.Equal(HttpStatusCode.Conflict, respuesta.StatusCode);
    }

    [Fact]
    public async Task Una_incidencia_cerrada_no_se_edita()
    {
        var id = await IdDe("Caldera con error");

        var respuesta = await cliente.PutAsJsonAsync($"/incidencias/{id}",
            new { titulo = "Otro título", descripcion = (string?)null, observaciones = (string?)null, prioridad = "Normal", zonaId = (int?)null });

        Assert.Equal(HttpStatusCode.Conflict, respuesta.StatusCode);
    }

    [Theory]
    [InlineData("estado=Cerrada", "Caldera con error E04", "Gotera en el ático")]
    [InlineData("prioridad=Alta", "Gotera en el ático", "Antena sin señal")]
    [InlineData("zonaId=3", "Gotera en el ático", "Antena sin señal")]
    public async Task La_lista_se_filtra(string filtro, string sale, string noSale)
    {
        var titulos = await Titulos($"/incidencias?{filtro}");

        Assert.Contains(sale, titulos);
        Assert.DoesNotContain(noSale, titulos);
    }

    [Fact]
    public async Task La_busqueda_mira_el_titulo_y_la_descripcion()
    {
        Assert.Contains("Antena sin señal", await Titulos("/incidencias/buscar?texto=antena"));
        Assert.Contains("Barandilla suelta en la escalera", await Titulos("/incidencias/buscar?texto=soldador"));
    }

    [Fact]
    public async Task Las_zonas_salen_por_orden_de_nombre()
    {
        var zonas = await cliente.GetFromJsonAsync<JsonElement>("/zonas");

        Assert.Equal(["Centro", "Norte", "Sur"], zonas.EnumerateArray().Select(z => z.GetProperty("nombre").GetString()));
    }

    private async Task<List<string?>> Titulos(string ruta)
    {
        var lista = await cliente.GetFromJsonAsync<JsonElement>(ruta);
        return lista.EnumerateArray().Select(i => i.GetProperty("titulo").GetString()).ToList();
    }
}
