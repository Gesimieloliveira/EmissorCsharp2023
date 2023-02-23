using System.Linq;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Fiscal.NF.Componentes;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronico.Emitir.Flyouts.Models
{
    public class BuilderPessoaImportacao
    {
        private readonly PessoaEntidade _pessoa;
        private readonly FlyoutAbaInformacoesImportacaoCteModel _importacaoCteModel;

        public BuilderPessoaImportacao(PessoaEntidade pessoa, FlyoutAbaInformacoesImportacaoCteModel importacaoCteModel)
        {
            _pessoa = pessoa;
            _importacaoCteModel = importacaoCteModel;
        }


        public BuilderPessoaImportacao ConstruirDestinatario()
        {
            if (_importacaoCteModel.DocumentoUnicoDestinatario.Length == 11)
            {
                var cpf = new Cpf(_importacaoCteModel.DocumentoUnicoDestinatario.Trim());
                var documentoRg = _pessoa.Rg;
                var sexo = _pessoa.Sexo;
                var ie = _importacaoCteModel.InscricaoEstadualDestinatario.TrimOrNull();
                var dataNascimento = _pessoa.NascidoEm;

                _pessoa.ComoPessoaFisica(cpf, documentoRg, sexo, new InscricaoEstadual(ie).GetIndicador(), ie, dataNascimento);
            }

            if (_importacaoCteModel.DocumentoUnicoDestinatario.Length == 14)
            {
                var nomeFantasia = _importacaoCteModel.NomeDestinatario.Trim();
                var cnpj = new Cnpj(_importacaoCteModel.DocumentoUnicoDestinatario.TrimOrNull());
                var ie = _importacaoCteModel.InscricaoEstadualDestinatario.TrimOrEmpty();
                var im = _pessoa.InscricaoMunicipal;

                _pessoa.ComoPessoaJuridica(nomeFantasia, cnpj, new InscricaoEstadual(ie).GetIndicador(), ie, im);
            }


            return this;
        }

        public PessoaEndereco ConstruirEnderecoDestinatario()
        {
            var endereco =
                _pessoa.Enderecos?.Where(x =>
                        x.Logradouro.Contains(_importacaoCteModel.DestinatarioLogradouro.ToUpper()))
                    .FirstOrNull() as PessoaEndereco;

            var logradouro = _importacaoCteModel.DestinatarioLogradouro.TrimOrEmpty();
            var numero = _importacaoCteModel.DestinatarioNumero.TrimOrEmpty();
            var bairro = _importacaoCteModel.DestinatarioBairro.TrimOrEmpty();
            var cep = _importacaoCteModel.DestinatarioCep.TrimOrEmpty();
            var cidade = BuscaCidadeOuInsereDestinatario();
            var complemento = _importacaoCteModel.DestinatarioComplemento.TrimOrEmpty();


            if (endereco != null)
            {
                endereco.Logradouro = logradouro;
                endereco.Numero = numero;
                endereco.Bairro = bairro;
                endereco.Cep = cep;
                endereco.Cidade = cidade;
                endereco.Complemento = complemento;
            }

            return endereco ??
                   new PessoaEndereco(logradouro, numero, bairro, cep, cidade)
                   {
                       Complemento = complemento
                   };
        }


        public BuilderPessoaImportacao ConstruirEmitente()
        {
            if (_importacaoCteModel.DocumentoUnico.Length == 11)
            {
                var cpf = new Cpf(_importacaoCteModel.DocumentoUnico.Trim());
                var documentoRg = _pessoa.Rg;
                var sexo = _pessoa.Sexo;
                var ie = _importacaoCteModel.InscricaoEstadual.TrimOrNull();
                var dataNascimento = _pessoa.NascidoEm;

                _pessoa.ComoPessoaFisica(cpf, documentoRg, sexo, new InscricaoEstadual(ie).GetIndicador(), ie, dataNascimento);
            }

            if (_importacaoCteModel.DocumentoUnico.Length == 14)
            {
                var nomeFantasia = _importacaoCteModel.NomeEmitente.Trim();
                var cnpj = new Cnpj(_importacaoCteModel.DocumentoUnico.TrimOrNull());
                var ie = _importacaoCteModel.InscricaoEstadual.Trim();
                var im = _pessoa.InscricaoMunicipal;

                _pessoa.ComoPessoaJuridica(nomeFantasia, cnpj, new InscricaoEstadual(ie).GetIndicador(), ie, im);
            }


            return this;
        }

        public PessoaEndereco ConstruirEnderecoEmitente()
        {
            var endereco =
                _pessoa.Enderecos?.Where(x => x.Logradouro.Contains(_importacaoCteModel.LogradouroEmitente.ToUpper()))
                    .FirstOrNull() as PessoaEndereco;

            var logradouro = _importacaoCteModel.LogradouroEmitente.TrimOrEmpty();
            var numero = _importacaoCteModel.NumeroEmitente.TrimOrEmpty();
            var bairro = _importacaoCteModel.BairroEmitente.TrimOrEmpty();
            var cep = _importacaoCteModel.CepEmitente.TrimOrEmpty();
            var cidade = BuscaCidadeOuInsereEmitente();
            var complemento = _importacaoCteModel.ComplementoEmitente.TrimOrEmpty();


            if (endereco != null)
            {
                endereco.Logradouro = logradouro;
                endereco.Numero = numero;
                endereco.Bairro = bairro;
                endereco.Cep = cep;
                endereco.Cidade = cidade;
                endereco.Complemento = complemento;
            }

            return endereco ??
                   (new PessoaEndereco(logradouro, numero, bairro, cep, cidade)
                   {
                       Complemento = complemento
                   });
        }


        private CidadeDTO BuscaCidadeOuInsereDestinatario()
        {
            var cidade = BuscarCidadePorIbge(int.Parse(_importacaoCteModel.DestinatarioIbgeCidade.ToString()));
            ValidaCidadeDestinatario(cidade);
            cidade = BuscarCidadePorIbge(int.Parse(_importacaoCteModel.DestinatarioIbgeCidade.ToString()));
            return cidade;
        }

        private CidadeDTO BuscaCidadeOuInsereEmitente()
        {
            var cidade = BuscarCidadePorIbge(int.Parse(_importacaoCteModel.IbgeCidadeEmitente.ToString()));
            ValidaCidadeEmitente(cidade);
            cidade = BuscarCidadePorIbge(int.Parse(_importacaoCteModel.IbgeCidadeEmitente.ToString()));
            return cidade;
        }

        private void ValidaCidadeEmitente(CidadeDTO cidade)
        {
            if (cidade != null) return;

            var cidadeNova = new CidadeDTO
            {
                CodigoIbge = int.Parse(_importacaoCteModel.IbgeCidadeEmitente.ToString()),
                Nome = _importacaoCteModel.EmitenteCidade,
                SiglaUf = _importacaoCteModel.EmitenteUf
            };

            InsereCidade(cidadeNova);
        }

        private void ValidaCidadeDestinatario(CidadeDTO cidade)
        {
            if (cidade != null) return;

            var cidadeNova = new CidadeDTO
            {
                CodigoIbge = int.Parse(_importacaoCteModel.DestinatarioIbgeCidade.ToString()),
                Nome = _importacaoCteModel.DestinatarioCidade,
                SiglaUf = _importacaoCteModel.DestinatarioUF
            };

            InsereCidade(cidadeNova);
        }

        private void InsereCidade(CidadeDTO cidadeNova)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCidade = new RepositorioCidade(sessao);

                repositorioCidade.Salvar(cidadeNova);
                transacao.Commit();
            }
        }

        private CidadeDTO BuscarCidadePorIbge(int codigoIbge)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioCidade(sessao).BuscaPeloIbge(codigoIbge);
            }
        }


        public PessoaEntidade Construir()
        {
            return _pessoa;
        }
    }
}