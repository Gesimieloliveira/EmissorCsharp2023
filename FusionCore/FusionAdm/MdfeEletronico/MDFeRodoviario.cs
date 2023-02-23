using System.Collections.Generic;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeRodoviario
    {
        public MDFeRodoviario()
        {
            VeiculoTracao = new MDFeVeiculoTracao();
            VeiculosReboques = new List<MDFeVeiculoReboque>();
            ValesPedagios = new List<MDFeValePedagio>();
            Contratantes = new List<MDFeContratante>();
            Ciots = new List<MDFeCiot>();
        }

        public int MDFeId { get; set; }
        public MDFeEletronico MDFeEletronico { get; set; }
        public string Rntrc { get; set; }
        public string CodigoAgendamentoPorto { get; set; }
        public MDFeVeiculoTracao VeiculoTracao { get; set; }
        public IList<MDFeVeiculoReboque> VeiculosReboques { get; set; }
        public IList<MDFeValePedagio> ValesPedagios { get; set; }
        public IList<MDFeContratante> Contratantes { get; set; }
        public IList<MDFeCiot> Ciots { get; set; }
    }
}