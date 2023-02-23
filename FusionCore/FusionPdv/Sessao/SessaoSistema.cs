using System;
using System.Collections.Generic;
using FusionCore.FusionPdv.Configuracoes;
using FusionCore.FusionPdv.Ecf;
using FusionCore.FusionPdv.Financeiro;
using FusionCore.FusionPdv.Models;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Seguranca.Licenciamento.Dominio;

namespace FusionCore.FusionPdv.Sessao
{
    public static class SessaoSistema
    {
        public static UsuarioPdvDt UsuarioLogado { get; set; }
        public static DateTime OnPoucoPapel { get; set; } = DateTime.Now;
        public static EntidadeTef ConfigTef { get; set; }
        public static BalancaPdv ConfiguracoesBalanca { get; set; }
        public static IList<FormaPagamento> FormasPagamentoEcf { get; set; }
        public static IList<FormaPagamentoEcfDt> FormasPagamentoInterno { get; set; }
        public static IList<Aliquota> AliquotasDoEcf { get; set; }
        public static EmpresaDt Empresa { get; set; }
        public static ConfiguracaoFinanceiroPdv ConfiguracoesFinanceiro { get; set; }
        public static ConfiguracaoFrenteCaixaPdv LogoCaixa { get; set; }
        public static AcessoConcedido AcessoConcedido { get; set; }
        public static string MensagemErroAcesso { get; set; }
        public static object EstadoEcf { get; set; }
    }
}