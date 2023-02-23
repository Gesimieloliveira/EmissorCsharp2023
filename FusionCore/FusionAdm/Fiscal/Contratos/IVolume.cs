using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.NF;

namespace FusionCore.FusionAdm.Fiscal.Contratos
{
    public interface IVolume
    {
        int Id { get; set; }
        Nfeletronica Nfe { get; set; }
        int Quantidade { get; set; }
        decimal PesoBruto { get; set; }
        decimal PesoLiquido { get; set; }
        string Especie { get; set; }
        string Numeracao { get; set; }
        string Marca { get; set; }
        IList<ILacre> Lacres { get; set; } 
         
    }
}