using System.Collections.Generic;
using System.Linq;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Financeiro.Repositorios;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.FusionAdm.Financeiro.Servicos
{
    public class ServicoDocumentoReceber
    {
        private readonly ServicoRegistroDeCaixa _servicoCaixa;
        private readonly RepositorioDocumentoReceber _repositorio;

        public ServicoDocumentoReceber(ISession session, ServicoRegistroDeCaixa servicoCaixa)
        {
            _servicoCaixa = servicoCaixa;
            _repositorio = new RepositorioDocumentoReceber(session);
        }

        public void ComputarJuros(IEnumerable<DocumentoReceber> documentos, UsuarioDTO usuario)
        {
            foreach (var d in documentos)
            {
                d.GerarJuros(usuario);

                _repositorio.SalvarAlteracoes(d);
            }
        }

        public void ComputarDesconto(
            IList<DocumentoReceber> documentos,
            decimal valorDesconto,
            UsuarioDTO usuario)
        {
            if (valorDesconto == 0)
            {
                return;
            }

            var totalRestante = documentos.Sum(i => i.ValorRestanteCorrigido);
            var percentual = valorDesconto / totalRestante * 100;
            var descontoAplicado = 0.00M;

            for (var i = 0; i < documentos.Count; i++)
            {
                var item = documentos[i];
                var desconto = decimal.Round(item.ValorRestanteCorrigido * percentual / 100, 2);
                var diferencaFinal = 0.00M;

                descontoAplicado += desconto;

                if (i == documentos.Count - 1)
                {
                    diferencaFinal = valorDesconto - descontoAplicado;
                }

                item.FornecerDesconto(desconto + diferencaFinal, usuario);

                _repositorio.SalvarAlteracoes(item);
            }
        }

        public void ReceberGrupoDeDocumentos(IEnumerable<DocumentoReceber> documentos, decimal totalSerRecebido, UsuarioDTO usuario, ETipoRecebimento tipoRecebimento)
        {
            var valorRestanteParaReceber = totalSerRecebido;

            foreach (var documento in documentos.OrderBy(i => i.Vencimento))
            {
                if (valorRestanteParaReceber == 0)
                {
                    break;
                }

                var valorRecebido = documento.ValorRestante;

                if (valorRestanteParaReceber < documento.ValorRestante)
                {
                    valorRecebido = valorRestanteParaReceber;
                }

                var lancamento = documento.AdicionarRecebimento(valorRecebido, usuario, tipoRecebimento);

                _servicoCaixa.RegistrarRecebimento(lancamento);
                _repositorio.SalvarAlteracoes(documento);

                valorRestanteParaReceber -= valorRecebido;
            }
        }

        public void EstornarUltimoLancamento(DocumentoReceber documento, UsuarioDTO usuario)
        {
            var estorno = documento.EstonarUltimoLancamento(usuario);

            if (estorno.TipoLancamento == TipoLancamento.Pagamento)
            {
                _servicoCaixa.RegistrarEstornoRecebimento(estorno);
            }

            _repositorio.SalvarAlteracoes(documento);
        }

        public void FazerCancelamento(DocumentoReceber documento, UsuarioDTO usuario)
        {
            documento.CancelarDocumento(usuario);

            foreach (var i in documento.Lancamentos)
            {
                if (i.TipoLancamento == TipoLancamento.Pagamento && i.Cancelado)
                {
                    _servicoCaixa.RegistrarEstornoRecebimento(i);
                }
            }
            
            _repositorio.SalvarAlteracoes(documento);
        }
    }
}