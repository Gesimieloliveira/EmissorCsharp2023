using System;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionLibrary.VisaoModel;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class CteGridDto : ViewModel
    {
        private bool _isSelecionado;
        public int Id { get; set; }
        public int NumeroDocumento { get; set; }
        public bool Autorizado { get; set; }
        public bool Inutilizado { get; set; }
        public int CodigoAutorizacao { get; set; }
        public decimal ValorServico { get; set; }
        public decimal ValorReceber { get; set; }
        public decimal ValorTotalCarga { get; set; }
        public string NaturezaOperacao { get; set; }
        public string EmitenteNome { get; set; }
        public string TomadorNome { get; set; }
        public string DestinatarioNome { get; set; }
        public string RemetenteNome { get; set; }

        // ReSharper disable once UnusedMember.Global
        public CteStatus Status => GetStatusParaGridModel();

        public short StatusCancelamento { get; set; }
        public string XmlAutorizado { get; set; }

        public bool IsSelecionado
        {
            get => _isSelecionado;
            set
            {
                if (value == _isSelecionado) return;
                _isSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public string Chave { get; set; }
        public string XmlCancelamento { get; set; }

        public DateTime EmissaoEm { get; set; }

        public DateTimeOffset? RecebidoEm { get; set; }

        public DateTime? RecebidoEmDateTime => RecebidoEm?.DateTime;

        private CteStatus GetStatusParaGridModel()
        {
            if (Inutilizado)
            {
                return CteStatus.Inutilizado;
            }

            if (StatusCancelamento == 135 || StatusCancelamento == 136 || StatusCancelamento == 134)
            {
                return CteStatus.Cancelada;
            }

            if (StatusCancelamento == 0 && CodigoAutorizacao == 0)
            {
                return CteStatus.EmDigitacao;
            }

            if (CodigoAutorizacao != 100)
            {
                return CteStatus.Pendente;
            }

            return CteStatus.Autorizado;
        }


        public bool PodeEditar()
        {
            return !Autorizado;
        }

        public bool IsOpcoes()
        {
            return Status == CteStatus.Pendente || Status == CteStatus.Autorizado;
        }

        public bool PermiteExportacao()
        {
            return Status == CteStatus.Autorizado || Status == CteStatus.Cancelada;
        }
    }
}