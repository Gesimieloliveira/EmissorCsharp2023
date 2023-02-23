using System;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.Financeiro
{
    public sealed class TipoDocumento : EntidadeBase<short>, ITipoDocumento, ISincronizavelAdm
    {
        public TipoDocumento()
        {
            AlteradoEm = DateTime.Now;
            FormaPagamento = FFormaPagamento.Dinheiro;
            RegistraFinanceiro = false;
            EstaAtivo = true;
        }

        public short Id { get; set; }
        protected override short ChaveUnica => Id;
        public string Descricao { get; set; }
        public FFormaPagamento FormaPagamento { get; set; }
        public bool RegistraFinanceiro { get; set; }
        public bool EstaAtivo { get; set; } = true;
        public DateTime? AlteradoEm { get; set; }
        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.TipoDocumento;
    }
}