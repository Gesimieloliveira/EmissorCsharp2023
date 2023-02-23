using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fusion.Visao.MdfeEletronico.Aba.Model;
using FusionCore.DFe.RegrasNegocios.Chave;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public class MdfeAddDocumentoContexto : ViewModel
    {
        private readonly AbaMdfeCarregamentoModel _carregamentoModel;
        private decimal _valor;

        public MdfeAddDocumentoContexto(AbaMdfeCarregamentoModel carregamentoModel)
        {
            _carregamentoModel = carregamentoModel;
            ProdutosPerigosos = new ObservableCollection<MDFeProdutoPerigoso>();
        }

        public CidadeDTO Municipio
        {
            get => GetValue<CidadeDTO>();
            set => SetValue(value);
        }

        public string Chave
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string SegundoCodigoBarras
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public MDFeProdutoPerigoso ProdutoPerigosoSelecionado
        {
            get => GetValue<MDFeProdutoPerigoso>();
            set => SetValue(value);
        }

        public ObservableCollection<MDFeProdutoPerigoso> ProdutosPerigosos
        {
            get => GetValue<ObservableCollection<MDFeProdutoPerigoso>>();
            set => SetValue(value);
        }

        public decimal Valor
        {
            get => _valor;
            set
            {
                if (value == _valor) return;
                _valor = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<MDFeDescarregamento> Sucesso;

        public void AdicionarProdutoPerigoso(MDFeProdutoPerigoso produto)
        {
            if (ProdutosPerigosos.Any(i => i == produto))
            {
                return;
            }

            ProdutosPerigosos.Add(produto);
        }

        public void RemoverProdutoPerigosoSelecionado()
        {
            ProdutosPerigosos.Remove(ProdutoPerigosoSelecionado);
        }

        public void SalvarAlteracoes()
        {
            if (_carregamentoModel.ChaveDescarregamentoJaInformada(Chave))
            {
                throw  new InvalidOperationException("Essa chave já foi informada!");
            }

            GerarChaveFiscal.ValidarChave(Chave);

            if (Valor < 0)
                throw new InvalidOperationException("O valor não pode ser menor que 0");

            var novo = new MDFeDescarregamento(Municipio, Chave, Valor, SegundoCodigoBarras);

            novo.ThrowExceptionSeInvalido(_carregamentoModel.TipoEmitente);

            foreach (var pp in ProdutosPerigosos)
            {
                novo.AdicionarProdutoPerigoso(pp);
            }

            Sucesso?.Invoke(this, novo);
            Clear();
        }

        private void Clear()
        {
            Chave = string.Empty;
            Valor = 0.0m;
            SegundoCodigoBarras = string.Empty;
            ProdutosPerigosos.Clear();
        }

        public void SetValorTotal(decimal valorTotal)
        {
            Valor = valorTotal;
        }
    }
}