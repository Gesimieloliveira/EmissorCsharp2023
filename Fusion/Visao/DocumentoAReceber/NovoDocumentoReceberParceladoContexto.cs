using System;
using System.Collections.Generic;
using System.Data;
using Fusion.Sessao;
using FusionCore.CadastroEmpresa;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Financeiro.Servicos;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionLibrary.VisaoModel;
using FusionWPF.Parcelamento;
using NHibernate.Util;

namespace Fusion.Visao.DocumentoAReceber
{
    public class NovoDocumentoReceberParceladoContexto : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema;

        public NovoDocumentoReceberParceladoContexto(SessaoSistema sessaoSistema)
        {
            _sessaoSistema = sessaoSistema;
            Parcelas = new List<ParcelaGerada>();
            DataEmissao = DateTime.Now;
        }

        public IEmpresa EmpresaSelecionada
        {
            get => GetValue<IEmpresa>();
            set => SetValue(value);
        }

        public Cliente ClienteSelecionado
        {
            get => GetValue<Cliente>();
            set => SetValue(value);
        }

        public string Descricao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime? DataEmissao
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public decimal Valor
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public IEnumerable<ParcelaGerada> Parcelas
        {
            get => GetValue<IEnumerable<ParcelaGerada>>();
            private set => SetValue(value);
        }

        public ITipoDocumento TipoDocumento
        {
            get => GetValue<ITipoDocumento>();
            private set => SetValue(value);
        }

        public void ComParcelas(ParcelamentoArgs args)
        {
            Parcelas = args.Parcelas;
            TipoDocumento = args.TipoDocumento;
        }

        public void GerarDocumentosParcelados()
        {
            ThrowExceptionSeInvalido();

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var servico = new ServicoCadastroDeDocumento(sessao);
                var malote = servico.CriarMalote(OrigemDocumento.Manual, _sessaoSistema.UsuarioLogado);

                foreach (var pa in Parcelas)
                {
                    var doc = servico.CriarNovo(
                        _sessaoSistema.UsuarioLogado,
                        EmpresaSelecionada,
                        ClienteSelecionado,
                        pa.Valor,
                        DataEmissao.Value,
                        pa.Vencimento,
                        TipoDocumento,
                        pa.Numero,
                        Descricao
                    );

                    doc.AnexarMalote(malote);
                }

                servico.PersistirMalote(malote);
                sessao.Transaction.Commit();
            }
        }

        private void ThrowExceptionSeInvalido()
        {
            if (EmpresaSelecionada == null) 
                throw  new InvalidOperationException("Preciso que escolha uma Empresa!");

            if (ClienteSelecionado == null)
                throw new InvalidOperationException("Preciso que escolha um Cliente");

            if (DataEmissao == null)
                throw new InvalidOperationException("Preciso que informe a Data de Emissão");

            if (DataEmissao.Value.Date > DateTime.Today)
                throw new InvalidOperationException("Data Emissão não pode ser uma data do futuro");
            
            if (DataEmissao.Value < DateTime.Now.AddDays(-30)) 
                throw new InvalidOperationException("Data Emissão não pode ser menor que 30 dias");

            if (!Parcelas.Any())
                throw new InvalidOperationException("Preciso que defina o parcelamento");
        }
    }
}