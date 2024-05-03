namespace InterfazDatosSumisuraApi.Models.Habers
{
    public class HabersModels
    {
    }

    public class ResponseGetDataByCrossReference
    {
        public string Item { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public string CrossReference { get; set; }
        public string Marca { get; set; }
        public string Calidad { get; set; }
        public string Ancho { get; set; }
        public decimal Existencia { get; set; }
        public decimal Disponible { get; set; }
        public string Descripcion { get; set; }
        public string Composicion { get; set; }
        public string Proveedor { get; set; }
        public string Origen { get; set; }
        public bool Descuenta { get; set; }
        public bool Activa { get; set; }
        public string Imagen { get; set; }
        public string CodigoViejo { get; set; }
    }
    public class Sku
    {
        public string CrossReference { get; set; }
    }
}
