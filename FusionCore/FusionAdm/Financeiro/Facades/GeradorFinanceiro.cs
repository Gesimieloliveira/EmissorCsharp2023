using System;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using static FusionCore.Core.Flags.TipoOperacao;
using static FusionCore.FusionAdm.Financeiro.Flags.Situacao;

namespace FusionCore.FusionAdm.Financeiro.Facades
{
    public static class GeradorFinanceiro
    {
        public static void Gerar(ISession sessao, Nfeletronica nfe, UsuarioDTO usuario)
        {
            if (nfe.TipoOperacao == Entrada)
            {
                return;
            }

            var malote = Malote.Cria(OrigemDocumento.Nfe, nfe.UuidVenda, usuario, 0.00M);

            foreach (var pg in nfe.Pagamentos)
            {
                if (pg.TipoDocumento?.RegistraFinanceiro != true)
                {
                    continue;
                }

                foreach (var parcela in pg.Parcelas)
                {
                    var doc = new DocumentoReceber
                    {
                        ValorDocumento = parcela.Valor,
                        ValorOriginal = parcela.Valor,
                        Descricao = $"NF-E ID {nfe.Id}",
                        EmitidoEm = DateTime.Now,
                        Parcela = parcela.Numero,
                        Vencimento = parcela.Vencimento,
                        Malote = malote,
                        Situacao = Aberto,
                        Empresa = nfe.Emitente.Empresa,
                        Cliente = nfe.Destinatario.FindCliente(),
                        NumeroDocumento = nfe.NumeroDocumento.ToString(),
                        TipoDocumento = pg.TipoDocumento,
                        UsuarioCriacao = usuario
                    };

                    malote.DocumentosReceber.Add(doc);
                }
            }

            var repositorioNfe = new RepositorioNfe(sessao);
            var repositorioMalote = new RepositorioMalote(sessao);

            nfe.Malote = malote;

            repositorioMalote.Persiste(malote);
            repositorioNfe.SalvarAlteracoes(nfe);
        }
    }
}
