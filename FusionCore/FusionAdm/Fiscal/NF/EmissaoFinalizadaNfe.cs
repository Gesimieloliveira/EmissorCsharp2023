using System;
using System.Linq;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Autorizacao;
using FusionCore.Helpers.DocumentoXml;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NFe.Classes;
using NFe.Classes.Protocolo;
using NFe.Classes.Servicos.Autorizacao;

#pragma warning disable 649
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class EmissaoFinalizadaNfe : Entidade
    {
        private int _nfeId;

        private EmissaoFinalizadaNfe()
        {
            //nhibernate
            //use static methods for factory
        }

        public Nfeletronica Nfe { get; private set; }
        public EmpresaDTO Empresa { get; private set; }
        public string Cnpj { get; private set; }
        public TipoEmissao TipoEmissao { get; private set; }
        public TipoAmbiente TipoAmbiente { get; private set; }
        public ChaveSefaz Chave { get; private set; }
        public string Protocolo { get; private set; }
        public string DigestValue { get; private set; }
        public DateTime RecebidoEm { get; private set; }
        public string XmlAutorizado { get; private set; }
        public bool IsDenegado { get; private set; }
        protected override int ReferenciaUnica => _nfeId;

        public static EmissaoFinalizadaNfe CriarFinalizacao(Nfeletronica nfe, EmissaoNfe emissao)
        {
            var finalizacao = new EmissaoFinalizadaNfe
            {
                Nfe = nfe,
                Empresa = emissao.Empresa,
                Cnpj = emissao.Cnpj,
                Cpf = emissao.Cpf,
                TipoEmissao = emissao.TipoEmissao,
                TipoAmbiente = emissao.Ambiente,
                Chave = emissao.Chave,
                Protocolo = emissao.GetProtocolo(),
                DigestValue = emissao.GetDigestValue(),
                RecebidoEm = emissao.GetDataRecebimento(),
                IsDenegado = emissao.IsDenegadoUsoDaEmissao()
            };

            finalizacao.CriarXmlNotaAutorizada(emissao);

            return finalizacao;
        }

        public string Cpf { get; private set; }

        private void CriarXmlNotaAutorizada(EmissaoNfe emissao)
        {
            try
            {
                var xmlHelper = new XmlHelper(emissao.XmlAutorizacao);
                var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();
                var verAplic = xmlHelper.GetValueFromElement("verAplic", "infProt").GetValueOrDefault<string>();
                var xMotivo = xmlHelper.GetValueFromElement("xMotivo", "infProt").GetValueOrDefault<string>();

                var envNfe = FuncoesXml.XmlStringParaClasse<enviNFe3>(emissao.XmlEnvio);
                var nfe = envNfe.NFe.FirstOrDefault();

                var nfeProc = new nfeProc
                {
                    versao = emissao.VersaoDocumento,
                    NFe = nfe,
                    protNFe = new protNFe
                    {
                        versao = emissao.VersaoDocumento,
                        infProt = new infProt
                        {
                            tpAmb = emissao.Ambiente.ToZeus(),
                            chNFe = Chave.Chave,
                            dhRecbto = emissao.GetDataRecebimento(),
                            nProt = emissao.GetProtocolo(),
                            digVal = emissao.GetDigestValue(),
                            cStat = status,
                            verAplic = verAplic,
                            xMotivo = xMotivo
                        }
                    }
                };

                XmlAutorizado = FuncoesXml.ClasseParaXmlString(nfeProc);
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Falha ao criar XML da Nota Autorizada: " + ex.Message, ex);
            }
        }
    }
}