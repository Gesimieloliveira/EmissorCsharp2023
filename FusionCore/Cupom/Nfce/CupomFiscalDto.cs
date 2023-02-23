using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Base;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionLibrary.VisaoModel;
using JetBrains.Annotations;

namespace FusionCore.Cupom.Nfce
{
    public class CupomFiscalDto : EntidadeBase<int>, INotifyPropertyChanged
    {
        private bool _isSelecionado;

        public CupomFiscalDto()
        {
        }

        public int Id { get; set; }
        public Status Status { get; set; }
        public int NumeroFiscal { get; set; }
        public string NomeEmpresa { get; set; }
        public string Chave { get; set; }
        public short SerieFiscal { get; set; }
        public DateTime EmitidaEm { get; set; }
        public decimal Total { get; set; }
        public bool Denegada { get; set; }
        public string NomeCliente { get; set; }
        public bool EmissaoAutorizada { get; set; }
        public DateTime CriadoEm { get; set; }
        public CupomSituacao CupomSituacao { get; private set; }

        public bool IsSelecionado
        {
            get => _isSelecionado;
            set
            {
                _isSelecionado = value;
                OnPropertyChanged();
            }
        }


        protected override int ChaveUnica => Id;
        public string SituacaoInformativa => CupomSituacao.GetDescription();
        public bool IsAutorizada => CupomSituacao == CupomSituacao.Autorizado;
        public bool IsCancelada => CupomSituacao == CupomSituacao.Cancelado;
        public bool IsPendente => CupomSituacao == CupomSituacao.AutorizadoOffline;
        public bool IsDenegada => CupomSituacao == CupomSituacao.Denegada;
        public bool IsRejeicaoOffline => CupomSituacao == CupomSituacao.RejeicaoOffline;
        public bool IsRejeicao => CupomSituacao == CupomSituacao.Rejeicao;
        public SituacaoFiscal SituacaoFiscal { get; set; }

        public void ResolveCupomSituacaoNfce()
        {
            switch (Status)
            {
                case Status.Cancelada:
                    CupomSituacao = CupomSituacao.Cancelado;
                    break;
                case Status.Transmitida:
                    CupomSituacao = CupomSituacao.Autorizado;
                    break;
                case Status.PendenteOffline:
                    CupomSituacao = CupomSituacao.AutorizadoOffline;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Denegada)
                CupomSituacao = CupomSituacao.Denegada;
        }

        public void ResolveCupomSituacao()
        {
            IsFaturamento = true;
            switch (SituacaoFiscal)
            {
                case SituacaoFiscal.Aberta:
                    CupomSituacao = CupomSituacao.Aberta;
                    break;
                case SituacaoFiscal.Autorizada:
                    CupomSituacao = CupomSituacao.Autorizado;
                    EmissaoAutorizada = true;
                    break;
                case SituacaoFiscal.AutorizadaSemInternet:
                    CupomSituacao = CupomSituacao.AutorizadoOffline;
                    break;
                case SituacaoFiscal.Cancelado:
                    CupomSituacao = CupomSituacao.Cancelado;
                    break;
                case SituacaoFiscal.AutorizadaDenegada:
                    CupomSituacao = CupomSituacao.Denegada;
                    Denegada = true;
                    break;
                case SituacaoFiscal.RejeicaoOffline:
                    CupomSituacao = CupomSituacao.RejeicaoOffline;
                    break;
                case SituacaoFiscal.Rejeicao:
                    CupomSituacao = CupomSituacao.Rejeicao;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsFaturamento { get; private set; }
        public bool IsAberto => CupomSituacao == CupomSituacao.Aberta;
        public bool IsContingencia => CupomSituacao == CupomSituacao.AutorizadoOffline;
        public bool IsPodeAutorizar => IsRejeicaoOffline || IsAberto || IsRejeicao;
        public int VendaId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
