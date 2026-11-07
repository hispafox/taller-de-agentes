using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Incidencias.Pruebas;

public class AsignacionPruebas : PruebaDeApi
{
    [Fact]
    public async Task C02_Al_volver_a_leerla_tiene_el_tecnico_y_el_plazo()
    {
        await cliente.PutAsJsonAsync("/incidencias/5/asignacion", new { tecnicoId = 1, plazo = "2026-11-27" });

        var leida = await cliente.GetFromJsonAsync<JsonElement>("/incidencias/5");
        Assert.Equal("Rafa", leida.GetProperty("tecnico").GetProperty("nombre").GetString());
        Assert.Equal("2026-11-27", leida.GetProperty("plazo").GetString());
    }

    [Fact]
    public async Task C03_El_tecnico_y_el_plazo_se_cambian()
    {
        await cliente.PutAsJsonAsync("/incidencias/1/asignacion", new { tecnicoId = 2, plazo = "2026-12-01" });

        var leida = await cliente.GetFromJsonAsync<JsonElement>("/incidencias/1");
        Assert.Equal("Javi", leida.GetProperty("tecnico").GetProperty("nombre").GetString());
        Assert.Equal("2026-12-01", leida.GetProperty("plazo").GetString());
    }

    [Fact]
    public async Task C04_Un_tecnico_que_no_existe_da_error()
    {
        var respuesta = await cliente.PutAsJsonAsync("/incidencias/1/asignacion",
            new { tecnicoId = 999, plazo = "2026-12-01" });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task C05_Al_cerrarla_conserva_su_tecnico()
    {
        await cliente.PostAsync("/incidencias/1/resolver", null);
        await cliente.PostAsync("/incidencias/1/cerrar", null);

        var leida = await cliente.GetFromJsonAsync<JsonElement>("/incidencias/1");
        Assert.Equal("Rafa", leida.GetProperty("tecnico").GetProperty("nombre").GetString());
    }

    [Fact]
    public async Task C10_Una_incidencia_cerrada_no_se_asigna()
    {
        await cliente.PostAsync("/incidencias/1/resolver", null);
        await cliente.PostAsync("/incidencias/1/cerrar", null);

        var respuesta = await cliente.PutAsJsonAsync("/incidencias/1/asignacion",
            new { tecnicoId = 2, plazo = "2026-12-01" });

        Assert.Equal(HttpStatusCode.Conflict, respuesta.StatusCode);
    }

    [Fact]
    public async Task C11_Un_plazo_que_ya_ha_pasado_se_guarda()
    {
        var respuesta = await cliente.PutAsJsonAsync("/incidencias/1/asignacion",
            new { tecnicoId = 1, plazo = "2026-11-01" });

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
    }

    [Theory]
    [InlineData("2026-11-19", true)]   // C-07: el plazo era ayer
    [InlineData("2026-11-20", false)]  // C-08: el plazo es hoy
    public async Task C07_C08_Fuera_de_plazo_depende_del_dia(string plazo, bool esperado)
    {
        var respuesta = await cliente.PutAsJsonAsync("/incidencias/1/asignacion",
            new { tecnicoId = 1, plazo });

        var json = await respuesta.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(esperado, json.GetProperty("fueraDePlazo").GetBoolean());
    }

    [Theory]
    [InlineData("""{ "tecnicoId": 1 }""")]                                   // falta plazo
    [InlineData("""{ "plazo": "2026-11-27" }""")]                            // falta tecnicoId
    [InlineData("""{ "tecnicoId": 1, "plazo": "27/11/2026" }""")]            // no es una fecha
    [InlineData("""{ "tecnicoId": 1, "plazo": "2026-11-27", "responsable": 2 }""")]  // campo sin acordar
    public async Task Contrato_Un_cuerpo_que_no_lo_cumple_da_400_y_no_cambia_nada(string cuerpo)
    {
        var antes = await cliente.GetStringAsync("/incidencias/1");
        var contenido = new StringContent(cuerpo, Encoding.UTF8, "application/json");

        var respuesta = await cliente.PutAsync("/incidencias/1/asignacion", contenido);

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        Assert.Equal(antes, await cliente.GetStringAsync("/incidencias/1"));
    }

    [Fact]
    public async Task Contrato_La_respuesta_trae_lo_acordado()
    {
        var respuesta = await cliente.PutAsJsonAsync("/incidencias/1/asignacion",
            new { tecnicoId = 1, plazo = "2026-11-27" });

        var json = await respuesta.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("2026-11-27", json.GetProperty("plazo").GetString());
        Assert.Equal("Rafa", json.GetProperty("tecnico").GetProperty("nombre").GetString());
        Assert.Equal(JsonValueKind.False, json.GetProperty("fueraDePlazo").ValueKind);
    }
}
