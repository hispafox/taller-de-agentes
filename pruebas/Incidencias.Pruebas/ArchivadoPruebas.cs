using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Incidencias.Pruebas;

public class ArchivadoPruebas : PruebaDeApi
{
    private async Task<int> IdDe(string titulo, string ruta = "/incidencias")
    {
        var lista = await cliente.GetFromJsonAsync<JsonElement>(ruta);
        return lista.EnumerateArray()
            .First(i => i.GetProperty("titulo").GetString()!.StartsWith(titulo))
            .GetProperty("id").GetInt32();
    }

    private async Task<int> ArchivarCerradas()
    {
        var respuesta = await cliente.PostAsync("/incidencias/archivar-cerradas", null);
        var json = await respuesta.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("archivadas").GetInt32();
    }

    [Fact]
    public async Task Se_archivan_las_cerradas_hace_mas_de_90_dias()
    {
        Assert.Equal(1, await ArchivarCerradas());

        var cerradas = await cliente.GetStringAsync("/incidencias?estado=Cerrada");
        Assert.DoesNotContain("Luz de escalera fundida", cerradas);
        Assert.Contains("Puerta de garaje no cierra", cerradas);
    }

    [Fact]
    public async Task A_los_90_dias_justos_se_archiva_y_uno_antes_no()
    {
        var id = await IdDe("Puerta de garaje");   // cerrada hace 68 días

        fabrica.Reloj.Advance(TimeSpan.FromDays(21));
        await ArchivarCerradas();
        Assert.False((await cliente.GetFromJsonAsync<JsonElement>($"/incidencias/{id}")).GetProperty("archivada").GetBoolean());

        fabrica.Reloj.Advance(TimeSpan.FromDays(1));
        await ArchivarCerradas();
        Assert.True((await cliente.GetFromJsonAsync<JsonElement>($"/incidencias/{id}")).GetProperty("archivada").GetBoolean());
    }

    [Fact]
    public async Task Una_archivada_sigue_saliendo_en_la_busqueda()
    {
        await ArchivarCerradas();

        var encontradas = await cliente.GetStringAsync("/incidencias/buscar?texto=escalera");

        Assert.Contains("Luz de escalera fundida", encontradas);
    }

    [Fact]
    public async Task Al_reabrirla_deja_de_estar_archivada()
    {
        var id = await IdDe("Luz de escalera");
        await ArchivarCerradas();

        var respuesta = await cliente.PostAsync($"/incidencias/{id}/reabrir", null);

        var json = await respuesta.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Abierta", json.GetProperty("estado").GetString());
        Assert.False(json.GetProperty("archivada").GetBoolean());
    }

    [Fact]
    public async Task A_mano_se_archiva_una_cerrada()
    {
        var id = await IdDe("Caldera con error");

        var respuesta = await cliente.PostAsync($"/incidencias/{id}/archivar", null);

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        Assert.DoesNotContain("Caldera con error", await cliente.GetStringAsync("/incidencias?estado=Cerrada"));
    }

    [Fact]
    public async Task Lo_que_no_esta_cerrado_no_se_archiva()
    {
        var respuesta = await cliente.PostAsync("/incidencias/1/archivar", null);

        Assert.Equal(HttpStatusCode.Conflict, respuesta.StatusCode);
    }
}
