using System;
using System.Collections.Generic;
using System.Text;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionLibrary.VisaoModel;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class MdfeGridDto : ViewModel
    {
        public int Id { get; set; }
        public DateTime DataEmissao { get; set; }
        public int NumeroDocumento { get; set; }
        public MDFeStatus Status { get; set; }
        public MDFeTipoEmitente TipoEmitente { get; set; }
        public string UFCarregamento { get; set; }
        public string UFDescarregamento { get; set; }
        public decimal ValorTotalCarga { get; set; }
        public decimal PesoBrutoTotalCarga { get; set; }
        public int QuantidadeCTe { get; set; }
        public int QuantidadeNFe { get; set; }
        public string Chave { get; set; }
        public string XmlAutorizado { get; set; }
        public string XmlCancelamento { get; set; }
        public string NomeEmitente { get; set; }
        public string NomeVeiculoTracao { get; set; }
        public string PlacaVeiculoTracao { get; set; }
        public IList<string> MotoristasNomes { get; set; } = new List<string>();

        public string ObterTodosMotoristas => ObterNomesMotoristas();

        public bool AparecerChaveCopiar => ObterStatusMDFe();

        private bool ObterStatusMDFe()
        {
            return Status == MDFeStatus.Autorizado || Status == MDFeStatus.Cancelada ||
                   Status == MDFeStatus.ConsultaProcessamento
                   || Status == MDFeStatus.Encerrada;
        }

        private string ObterNomesMotoristas()
        {
            var nomes = new StringBuilder();

            if (MotoristasNomes.Count == 0) return nomes.ToString();

            foreach (var motoristasNome in MotoristasNomes)
            {
                nomes.Append(motoristasNome);
                nomes.Append(MotoristasNomes.Count > 1 ? " ; " : string.Empty);
            }

            return nomes.ToString();
        }

        public int StatusCancelamento => Status == MDFeStatus.Cancelada ? 135 : 0;

        public bool IsSelecionado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string NomeMotorista { get; set; }
    }
}