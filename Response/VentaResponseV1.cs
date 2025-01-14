namespace GatewayAPI.Response
{
    public class VentaResponseV1
    {
        public int IdVenta { get; set; }
        public string NumeroFactura { get; set; }

        public int Total { get; set; }
        public int IdCliente { get; set; }
        public int IdVendedor { get; set; }
    }
}
