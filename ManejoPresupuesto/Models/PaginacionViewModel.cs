using DocumentFormat.OpenXml.Drawing.Charts;

namespace ManejoPresupuesto.Models
{
    public class PaginacionViewModel
    {
        public int Pagina { get; set; } = 1;
        public int recordsPorPagina = 10;
        public readonly int CantidadMaximaRecordsPorPagina = 50;

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = (value > CantidadMaximaRecordsPorPagina) 
                    ? CantidadMaximaRecordsPorPagina : value;
            }
        }

        public int RecordsAsaltar => recordsPorPagina * (Pagina - 1);
    }
}
