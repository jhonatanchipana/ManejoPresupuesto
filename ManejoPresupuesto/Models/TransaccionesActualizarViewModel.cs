namespace ManejoPresupuesto.Models
{
    public class TransaccionesActualizarViewModel: TransaccionCreacionViewModel
    {
        public int CuentaAnteriorid { get; set; }
        public decimal MontoAnterior { get; set; }  
        public string UrlRetorno { get; set; }  

    }
}
