using FusionCore.Extencoes;
using FusionCore.FusionAdm.Emissores;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Flags;
using NHibernate.Criterion;
using System;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class EmitenteNfe
    {
        public EmpresaDTO Empresa { get; private set; }
        public byte EmissorId { get; private set; }
        public string Cnpj { get; private set; }
        public string Cpf { get; private set; }
        public RegimeTributario RegimeTributario { get; private set; }
        public string DocumentoUnico => GetDocumentoUncio();
        public string DocumentoUnicoSemZeroAEsquerda => GetDocumentoUnicoSemZeroAEsquerda();

        private string GetDocumentoUnicoSemZeroAEsquerda()
        {
            return Cnpj.IsNotNullOrEmpty() ? Cnpj : Cpf;
        }

        private string GetDocumentoUncio()
        {
            return Cnpj.IsNotNullOrEmpty() ? Cnpj : Cpf.PadLeft(14, '0');
        }

        private EmitenteNfe()
        {
            //nhibernate
        }

        public EmitenteNfe(EmissorFiscal emissorFiscal) : this()
        {
            Empresa = emissorFiscal.Empresa;
            Cnpj = emissorFiscal.Empresa.Cnpj;
            Cpf = emissorFiscal.Empresa.Cpf;
            EmissorId = emissorFiscal.Id;
            RegimeTributario = emissorFiscal.Empresa.RegimeTributario;
        }

        public void AlterarEmissor(EmissorFiscal emissorFiscal)
        {
            Empresa = emissorFiscal.Empresa;
            EmissorId = emissorFiscal.Id;
            Cnpj = Empresa.Cnpj;
            Cpf = Empresa.Cpf;
            RegimeTributario = emissorFiscal.Empresa.RegimeTributario;
        }

        public EmissorFiscal CarregarDadosEmissor(ISessaoManager sessaoManager)
        {
            using (var sessao = sessaoManager.CriaSessao(System.Data.IsolationLevel.ReadCommitted))
            {
                
                return sessao.Get<EmissorFiscal>(EmissorId) 
                    ?? throw new InvalidOperationException($"Erro ao carregar dados do emissor de id {EmissorId}");
            }
        }
    }
}