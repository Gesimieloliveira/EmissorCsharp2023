using System;
using System.IO;
using Fusion.FastReport.Relatorios;
using FusionCore.Relatorios;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Relatorios
{
    public class CadastroFastReportContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private RelatorioProprio _relatorio;

        public CadastroFastReportContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public string Descricao
        {
            get => GetValue<string>();
            set => SetValue(value?.Trim());
        }

        public string Grupo
        {
            get => GetValue<string>();
            set => SetValue(value?.Trim());
        }

        public string ArquivoFrx
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool IsEdicao
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public event EventHandler<RelatorioProprio> SalvouComSucesso;

        public void SalvarAlteracoes()
        {
            if (string.IsNullOrWhiteSpace(Descricao))
            {
                throw new InvalidOperationException("Preciso de uma descrição para o relatório");
            }

            if (string.IsNullOrWhiteSpace(Grupo))
            {
                throw new InvalidOperationException("Preciso de um grupo para o relatório");
            }

            if (_relatorio == null)
            {
                var template = CriaTemplate();
                _relatorio = new RelatorioProprio(Descricao, Grupo, template);
            }
            else
            {
                _relatorio.AlterarInformacoes(Descricao, Grupo);
            }

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioRelatorio(sessao);
                repositorio.SalvarRelatorio(_relatorio);

                transacao.Commit();
            }

            SalvouComSucesso?.Invoke(this, _relatorio);
        }

        private Template CriaTemplate()
        {
            if (File.Exists(ArquivoFrx))
            {
                return new Template(File.ReadAllBytes(ArquivoFrx));
            }

            var fornecedor = new FornecedorTemplate(_sessaoManager);
            var empty = fornecedor.ObtemBytesFrx("FrEmptyTemplate.frx");

            return new Template(empty);
        }

        public void Edicao(RelatorioProprio relatorio)
        {
            Descricao = relatorio.Descricao;
            Grupo = relatorio.Grupo;
            IsEdicao = true;

            _relatorio = relatorio;
        }
    }
}