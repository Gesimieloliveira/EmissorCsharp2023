using System;
using FusionCore.CadastroUsuario;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.ControleCaixa
{
    public class CaixaIndividual : EntidadeBase<Guid>
    {
        private CaixaIndividual()
        {
            Id = Guid.NewGuid();
            EstadoAtual = EEstadoCaixa.Aberto;
        }

        public CaixaIndividual(
            IUsuario usuario,
            DateTime abertura,
            decimal saldoInicial,
            ELocalEventoCaixa local,
            byte? terminalId = null) : this()
        {
            if (local == ELocalEventoCaixa.Gestao && terminalId != null)
            {
                throw new InvalidOperationException("Caixa deste local não pode ter Terminal");
            }

            if (local == ELocalEventoCaixa.Terminal && terminalId == null)
            {
                throw new InvalidOperationException("Caixa de terminal precisa que informe o terminal");
            }

            Usuario = usuario;
            SaldoInicial = saldoInicial;
            DataAbertura = abertura;
            LocalEvento = local;
            TerminalId = terminalId;
        }

        protected override Guid ChaveUnica => Id;
        public Guid Id { get; private set; }
        public IUsuario Usuario { get; private set; }
        public EEstadoCaixa EstadoAtual { get; private set; }
        public ELocalEventoCaixa LocalEvento { get; private set; }
        public byte? TerminalId { get; private set; }
        public DateTime DataAbertura { get; private set; }
        public decimal SaldoInicial { get; private set; }
        public DateTime? DataFechamento { get; private set; }
        public decimal SaldoCalculado { get; private set; }
        public decimal SaldoInformado { get; private set; }

        public void FecharCaixa(decimal saldoEsperadoDinheiro, decimal saldoEmDinheiro)
        {
            if (EstadoAtual != EEstadoCaixa.Aberto)
            {
                throw new InvalidOperationException("Caixa precisa estar Aberto para ser Fechado!");
            }

            SaldoInformado = saldoEmDinheiro;
            SaldoCalculado = saldoEsperadoDinheiro;
            DataFechamento = DateTime.Now;
            EstadoAtual = EEstadoCaixa.Fechado;
        }

        public void AlterarTipo(UsuarioDTO usuario)
        {
            if (usuario.Id != Usuario.Id)
            {
                throw new InvalidOperationException($"AlterarTipo: Usuário precisa ser o mesmo");
            }

            Usuario = usuario;
        }
    }
}