//using GatewayAPI.Models;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

namespace GatewayAPI.Models
{
    public class VentasClientesAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            //throw new NotImplementedException();

            // La información aquí llega en un JSON
            var ventasContent = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
            var clientesContent = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();

            // JSON => List<OBJ>
            var ventas = JsonConvert.DeserializeObject<List<VentaResponseV1>>(ventasContent);
            var clientes = JsonConvert.DeserializeObject<List<ClienteResponseV1>>(clientesContent);

            // Usar join de LINQ con expresiones lambda
            var ventasConClientes = ventas.Join(
                clientes,
                venta => venta.IdCliente,
                cliente => cliente.IdCliente,
                (venta, cliente) => new
                {
                    IdVenta = venta.IdVenta,
                    NumeroFactura = venta.NumeroFactura,
                    Total = venta.Total,
                    IdCliente = cliente.IdCliente,
                    Nombres = cliente.Nombres,
                    Apellidos = cliente.Apellidos
                }).ToList();

            // Convertir de Lista de objetos => JSON
            var jsonResult = JsonConvert.SerializeObject(ventasConClientes);

            var stringContent = new StringContent(jsonResult);
            return new DownstreamResponse(stringContent, System.Net.HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "application/json");


        }
    }
}
