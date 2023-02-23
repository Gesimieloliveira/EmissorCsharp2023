using Fusion.Visao.MdfeEletronico.Aba.Model;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.FusionAdm.MdfeEletronico.Validador;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.AssemblyUtils;
using FusionCore.Helpers.AssemblyUtils.Leitura;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using NHibernate.Util;

namespace Fusion.Visao.MdfeEletronico.Builder
{
    public class MDFeBuilder
    {
        private readonly MDFeEletronico _mdfe;

        public MDFeBuilder(MDFeEletronico mdfe)
        {
            _mdfe = mdfe;
            HidrataValores();
        }

        private void HidrataValores()
        {
            if (_mdfe.Id != 0)
            {
                if (_mdfe.Rodoviario == null)
                {
                    _mdfe.Rodoviario = new MDFeRodoviario();
                }
            }
        }

        public void ComCabecalho(AbaCabecalhoMdfeModel model)
        {
            _mdfe.EmissorFiscal = BuscarEmissorFiscal(model.EmissorFiscal);
            _mdfe.TipoEmitente = model.TipoEmitente;
            _mdfe.Modal = model.Modal;
            _mdfe.ProcessoEmissao = MDFeProcessoEmissao.AplicativoContribuiente;
            _mdfe.VersaoAplicativo = AssemblyHelper.LerDoAssemblyPrincipal(new Versao3Digito());
            _mdfe.EstadoCarregamento = model.EstadoCarregamento;
            _mdfe.EstadoDescarregamento = model.EstadoDescarregamento;
            _mdfe.Observacao = model.InformacaoAdicional;
            _mdfe.Emitente.MDFeEletronico = _mdfe;
            _mdfe.Emitente.Empresa = _mdfe.EmissorFiscal.Empresa;
            _mdfe.TipoEmissao = model.TipoEmissao;
            _mdfe.TipoDoTransportador = model.TipoDoTransportador;
            _mdfe.EmissaoEm = model.EmitidaEm;
            _mdfe.PrevisaoInicioViagemEm = model.PrevisaoInicioViagemEm;
            _mdfe.CargaFechada = model.CargaFechada;
            _mdfe.ProdutoPredominante.CepCarregamento = model.CepCarregamento.TrimOrEmpty();
            _mdfe.ProdutoPredominante.CepDescarregamento = model.CepDescarregamento.TrimOrEmpty();
        }

        private EmissorFiscal BuscarEmissorFiscal(EmissorFiscalComboBox modelEmissorFiscal)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioEmissorFiscal(sessao).GetPeloId(modelEmissorFiscal.Id);
            }
        }

        public MDFeEletronico Construir()
        {
            return _mdfe;
        }

        public void ComCarregamentoDescarregamento(AbaMdfeCarregamentoModel model)
        {
            _mdfe.UnidadeMedida = model.UnidadeMedida;
            _mdfe.PesoBrutoCarga = model.PesoBrutoCarga;
            _mdfe.ValorTotalCarga = model.ValorTotalCarga;

            PreencherMunicipioCarregamento(model);
            PreencherPercurso(model);
            PreencherLacre(model);
            PreencherSeguroCarga(model);
        }

        private void PreencherMunicipioCarregamento(AbaMdfeCarregamentoModel model)
        {
            _mdfe.MunicipioCarregamentos.Clear();

            model.ListaMunicipioCarregamento.ForEach(mc =>
            {
                _mdfe.MunicipioCarregamentos.Add(new MDFeMunicipioCarregamento
                {
                    Id = mc.MunicipioCarregamento.Id,
                    Cidade = mc.Cidade,
                    MDFeEletronico = _mdfe
                });
            });
        }

        private void PreencherLacre(AbaMdfeCarregamentoModel model)
        {
            _mdfe.Lacres.Clear();
            model.ListaLacres.ForEach(l =>
            {
                _mdfe.Lacres.Add(new MDFeLacre
                {
                    Id = l.Lacre.Id,
                    MDFeEletronico = _mdfe,
                    Numero = l.Numero
                });
            });
        }

        private void PreencherSeguroCarga(AbaMdfeCarregamentoModel model)
        {
            _mdfe.SeguroCargas.Clear();
            model.ListaSeguroCarga.ForEach(s =>
            {
                var seguro = new MDFeSeguroCarga
                {
                    NumeroApolice = s.NumeroApolice,
                    CnpjResponsavel = s.CnpjResponsavel,
                    CnpjSeguradora = s.CnpjSeguradora,
                    CpfResponsavel = s.CpfResponsavel,
                    MDFeEletronico = _mdfe,
                    NomeSeguradora = s.NomeSeguradora,
                    Responsavel = s.ResponsavelSeguro
                };

                s.Averbacoes.ForEach(x =>
                {
                    seguro.Averbacoes.Add(x);
                });

                _mdfe.SeguroCargas.Add(seguro);
            });
        }

        private void PreencherPercurso(AbaMdfeCarregamentoModel model)
        {
            _mdfe.Percursos.Clear();
            model.ListaPercurso.ForEach(p =>
            {
                _mdfe.Percursos.Add(new MDFePercurso
                {
                    Id = p.Percurso.Id,
                    MDFeEletronico = _mdfe,
                    Estado = p.EstadoUf
                });
            });
        }

        public void ComRodoviario(AbaRodoviarioMdfeModel model)
        {
            var rntrc = model.Rntrc == "ISENTO" ? "" : model.Rntrc ?? string.Empty;

            _mdfe.Rodoviario.MDFeEletronico = _mdfe;
            _mdfe.Rodoviario.CodigoAgendamentoPorto = model.CodigoAgendamentoPorto;
            _mdfe.Rodoviario.Rntrc = rntrc;
            _mdfe.ProdutoPredominante.CodigoBarras = model.CodigoBarrasProdutoPredominante;
            _mdfe.ProdutoPredominante.Nome = model.NomeProdutoPredominante;
            _mdfe.ProdutoPredominante.TipoCarga = model.TipoCarga;
            _mdfe.ProdutoPredominante.Ncm = model.NcmProdutoPredominante;
            _mdfe.CategoriaComercialVeiculo = model.CategoriaComercialVeiculo;

            MDFeRodoviarioValidador.Checar(_mdfe.Rodoviario);

            PreencheVeiculoTracao(model);
            PreencheVeiculoReboque(model);
            PreencheValePedagio(model);
            PreencherContratante(model);
            PreencherCiot(model);
            PreencherInformacaoPagamento(model);
        }

        private void PreencherInformacaoPagamento(AbaRodoviarioMdfeModel model)
        {
            _mdfe.InformacaoPagamentos.Clear();

            model.ListarPagamentos.ForEach(p =>
            {
                _mdfe.InformacaoPagamentos.Add(p);
            });
        }

        private void PreencherCiot(AbaRodoviarioMdfeModel model)
        {
            _mdfe.Rodoviario.Ciots.Clear();

            model.ListaCiot.ForEach(c =>
            {
                _mdfe.Rodoviario.Ciots.Add(new MDFeCiot
                {
                    DocumentoUnico = c.MDFeCiot.DocumentoUnico,
                    Rodoviario = c.MDFeCiot.Rodoviario,
                    Id = c.MDFeCiot.Id,
                    Ciot = c.MDFeCiot.Ciot
                });
            });
        }

        private void PreencherContratante(AbaRodoviarioMdfeModel model)
        {
            _mdfe.Rodoviario.Contratantes.Clear();

            model.ListaContratante.ForEach(c =>
            {
                _mdfe.Rodoviario.Contratantes.Add(new MDFeContratante
                {
                    Id = c.Contratante.Id,
                    PessoaEntidade = c.Contratante.PessoaEntidade,
                    Rodoviario = c.Contratante.Rodoviario
                });
            });
        }

        private void PreencheValePedagio(AbaRodoviarioMdfeModel model)
        {
            _mdfe.Rodoviario.ValesPedagios.Clear();

            model.ListaValePedagio.ForEach(vp =>
            {
                _mdfe.Rodoviario.ValesPedagios.Add(new MDFeValePedagio
                {
                    Id = vp.MDFeValePedagio.Id,
                    Rodoviario = _mdfe.Rodoviario,
                    CnpjEmpresaFornecedora = vp.CnpjEmpresaFornecedora,
                    CnpjResponsavelPagamento = vp.CnpjResponsavel,
                    NumeroComprovante = vp.NumeroCompra,
                    Valor = vp.Valor,
                    CpfResponsavel = vp.CpfResponsavel
                });
            });
        }

        private void PreencheVeiculoReboque(AbaRodoviarioMdfeModel model)
        {
            _mdfe.Rodoviario.VeiculosReboques.Clear();

            model.ListaVeiculoReboque.ForEach(vr =>
            {
                _mdfe.Rodoviario.VeiculosReboques.Add(new MDFeVeiculoReboque
                {
                    Id = vr.MFDeVeiculoReboque.Id,
                    Veiculo = vr.Veiculo,
                    Rodoviario = _mdfe.Rodoviario
                });
            });
        }

        private void PreencheVeiculoTracao(AbaRodoviarioMdfeModel model)
        {
            _mdfe.Rodoviario.VeiculoTracao = new MDFeVeiculoTracao();

            model.ListaVeiculoTracao.ForEach(vt =>
            {
                _mdfe.Rodoviario.VeiculoTracao.RodoviarioId = vt.MDFeVeiculoTracao.RodoviarioId;
                _mdfe.Rodoviario.VeiculoTracao.Veiculo = vt.Veiculo;
                _mdfe.Rodoviario.VeiculoTracao.Rodoviario = _mdfe.Rodoviario;
            });

            model.ListaCondutor.ForEach(c =>
            {
                _mdfe.Rodoviario.VeiculoTracao.Condutores.Add(new MDFeCondutor
                {
                    Id = c.MDFeCondutor.Id,
                    VeiculoTracao = _mdfe.Rodoviario.VeiculoTracao,
                    Condutor = c.Condutor
                });
            });
        }

        public void ComVeiculoTracao(MDFeVeiculoTracao mdfeVeiculoTracao)
        {
            _mdfe.Rodoviario.VeiculoTracao = mdfeVeiculoTracao;
        }

        public void ComMunicipioCarregamento(MDFeMunicipioCarregamento carregamento)
        {
            _mdfe.MunicipioCarregamentos.Add(carregamento);
        }

        public void ComLacre(MDFeLacre lacre)
        {
            _mdfe.Lacres.Add(lacre);
        }

        public void ComPercurso(MDFePercurso percurso)
        {
            _mdfe.Percursos.Add(percurso);
        }

        public void ComSeguro(MDFeSeguroCarga seguroCarga)
        {
            _mdfe.SeguroCargas.Add(seguroCarga);
        }
    }
}