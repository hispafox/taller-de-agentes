using System.Net.Http.Json;
using System.Text.Json;

namespace Incidencias.Pruebas;

public class ListaConPlazoPruebas : PruebaDeApi
{
    private async Task<JsonElement> EnLista(int id, string ruta = "/incidencias")
    {
        var lista = await cliente.GetFromJsonAsync<JsonElement>(ruta);
        return lista.EnumerateArray().First(i => i.GetProperty("id").GetInt32() == id);
    }

    [Fact]
    public async Task C01_Sin_tecnico_ni_plazo_sale_vacio()
    {
        var fila = await EnLista(5);

        Assert.Equal(JsonValueKind.Null, fila.GetProperty("tecnico").ValueKind);
        Assert.False(fila.GetProperty("fueraDePlazo").GetBoolean());
    }

    [Theory]
    [InlineData("2026-11-19", true)]   // C-07: el plazo era ayer
    [InlineData("2026-11-20", false)]  // C-08: el plazo es hoy
    public async Task C07_C08_En_la_lista(string plazo, bool esperado)
    {
        await cliente.PutAsJsonAsync("/incidencias/1/asignacion", new { tecnicoId = 1, plazo });

        Assert.Equal(esperado, (await EnLista(1)).GetProperty("fueraDePlazo").GetBoolean());
    }

    [Fact]
    public async Task C09_El_filtro_deja_solo_las_del_tecnico()
    {
        var lista = await cliente.GetFromJsonAsync<JsonElement>("/incidencias?tecnicoId=1");

        Assert.All(lista.EnumerateArray(), i => Assert.Equal("Rafa", i.GetProperty("tecnico").GetString()));
        Assert.NotEmpty(lista.EnumerateArray());
    }

    [Fact]
    public async Task C12_Una_resuelta_sin_cerrar_sigue_fuera_de_plazo()
    {
        await cliente.PutAsJsonAsync("/incidencias/1/asignacion", new { tecnicoId = 1, plazo = "2026-11-19" });
        await cliente.PostAsync("/incidencias/1/resolver", null);

        Assert.True((await EnLista(1)).GetProperty("fueraDePlazo").GetBoolean());
    }

    [Fact]
    public async Task La_busqueda_trae_tambien_el_tecnico_y_el_plazo()
    {
        await cliente.PutAsJsonAsync("/incidencias/1/asignacion", new { tecnicoId = 1, plazo = "2026-11-19" });

        var fila = await EnLista(1, "/incidencias/buscar?texto=plaza 14");

        Assert.Equal("Rafa", fila.GetProperty("tecnico").GetString());
        Assert.True(fila.GetProperty("fueraDePlazo").GetBoolean());
    }
}
