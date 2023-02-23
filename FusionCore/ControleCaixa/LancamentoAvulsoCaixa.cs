using System;
using FusionCore.CadastroEmpresa;
using FusionCore.CadastroUsuario;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.ControleCaixa
{
    public class LancamentoAvulsoCaixa : EntidadeBase<Guid>
    {
        private LancamentoAvulsoCaixa()
        {
            DataCriacao = DateTime.Now;
            Motivo = string.Empty;
            Id = Guid.NewGuid();
        }

        public LancamentoAvulsoCaixa(
            IEmpresa empresa,
            IUsuario usuarioCriacao,
            ELocalEventoCaixa localEvento,
            TipoOperacao tipoOperacao,
            TipoLancamentoCaixa tipoLancamentoCaixa,
            string motivo,
            decimal valorOperacao) : this()
        {
            Empresa = empresa;
            UsuarioCriacao = usuarioCriacao;
            TipoOperacao = tipoOperacao;
            TipoLancamentoCaixa = tipoLancamentoCaixa;
            Motivo = motivo;
            ValorOperacao = valorOperacao;
            LocalEvento = localEvento;
        }

        protected override Guid ChaveUnica => Id;
        public Guid Id { get; private set; }
        public IEmpresa Empresa { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public IUsuario UsuarioCriacao { get; private set; }
        public ELocalEventoCaixa LocalEvento { get; private set; }
        public TipoOperacao TipoOperacao { get; private set; }
        public TipoLancamentoCaixa TipoLancamentoCaixa { get; private set; }
        public string Motivo { get; private set; }
        public decimal ValorOperacao { get; private set; }

        public void Alterar(string motivo)
        {
            Motivo = motivo;
        }

        public void AlterarTipo(UsuarioDTO usuario)
        {
            if (usuario.Id != UsuarioCriacao.Id)
            {
                throw new InvalidOperationException("AlterarTipo: usuário diverge do atual");
            }

            UsuarioCriacao = usuario;
        }

        public void AlterarTipo(EmpresaDTO empresa)
        {
            if (empresa.Id != Empresa.Id)
            {
                throw new InvalidOperationException("AlterarTipo: empresa diverge da atual");
            }

            Empresa = empresa;
        }
    }
}