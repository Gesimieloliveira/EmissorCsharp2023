using System;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Financeiro
{
    public class DocumentoReceberLancamento : EntidadeBase<int>
    {
        public DocumentoReceberLancamento()
        {
            CriadoEm = DateTime.Now;
        }

        public int Id { get; set; }
        protected override int ChaveUnica => Id;
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public TipoLancamento TipoLancamento { get; set; }
        public DocumentoReceber DocumentoReceber { get; set; }
        public DateTime? DataEstorno { get; set; }
        public UsuarioDTO UsuarioCriacao { get; set; }
        public bool Estornado { get; set; }
        public string TipoLancamentoTexto { get; set; }
        public DateTime? CriadoEm { get; set; }
        public bool FoiAlterado { get; private set; }
        public UsuarioDTO UsuarioEstorno { get; private set; }
        public bool Cancelado { get; private set; }
        public ETipoRecebimento? TipoRecebimento { get; set; }

        public string DescricaoLancamento 
        {
            get
            {
                return TipoLancamento == TipoLancamento.Pagamento
                    ? $"{TipoLancamento.GetDescription()} - {TipoRecebimento?.GetDescription()}"
                    : TipoLancamento.GetDescription();
            }
        }

        public void Estornar(UsuarioDTO usuario)
        {
            if (Estornado)
            {
                throw new InvalidOperationException("Lançamento já foi estornado");
            }

            Estornado = true;
            DataEstorno = DateTime.Now;
            UsuarioEstorno = usuario;

            FoiAlterado = true;
        }

        public void Cancelar(UsuarioDTO usuario)
        {
            Cancelado = true;
            FoiAlterado = true;
        }
    }
}