using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Servico.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.FusionAdm.Pessoas
{
    public sealed class Cliente : PessoaExtensao
    {
        public int Id { get; set; }
        public bool AplicaLimiteCredito { get; set; }
        public decimal LimiteCredito { get; set; }
        public string Observacao { get; set; }
        public bool SolicitaPedidoNfe { get; set; }

        private Cliente()
        {
            //nhibernate
            Observacao = string.Empty;
        }

        public Cliente(PessoaEntidade pessoa) : this()
        {
            Pessoa = pessoa;
        }

        public decimal ComputaCreditoDisponivel()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var totalDevedor = repositorio.GetTotalEmAbertoFinanceiro(Id);

                return LimiteCredito - totalDevedor;
            }
        }

        public void ChecarLimiteCredito(decimal valorCredito)
        {
            var servico = new LimiteCreditoServico();
            servico.ChecarLimiteCredito(this, valorCredito);
        }

        public void ThrowNaoPossuiEndereco()
        {
            if (Enderecos.Any() == false)
                throw new InvalidOperationException("Cliente não possui endereço, adicionar o endereço antes de selecionar.");
        }

        public PessoaEndereco PriorizarEnderecoEntrega()
        {
            return Enderecos.Any(i => i.Entrega) 
                ? Enderecos.First(i => i.Entrega) 
                : Enderecos.FirstOrDefault();
        }
    }
}
