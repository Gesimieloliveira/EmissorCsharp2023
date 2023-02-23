using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionWPF.Financeiro.Contratos.Financeiro;
using FusionLibrary.VisaoModel;

namespace FusionWPF.Parcelamento
{
    public class ParcelamentoContexto : ViewModel
    {
        private readonly IParcelamentoFactory _factory;

        internal ParcelamentoContexto(IParcelamentoFactory factory)
        {
            _factory = factory;
            Parcelas = new ObservableCollection<ParcelaContexto>();
        }

        public bool ParcelasIsEnabled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ObservableCollection<ParcelaContexto> Parcelas
        {
            get => GetValue<ObservableCollection<ParcelaContexto>>();
            set => SetValue(value);
        }

        public int? QuantidadeParcelas
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public decimal ValorParcelar
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public IEnumerable<ITipoDocumento> TiposDocumentos
        {
            get => GetValue<IEnumerable<ITipoDocumento>>();
            set => SetValue(value);
        }

        public ITipoDocumento TipoDocumento
        {
            get => GetValue<ITipoDocumento>();
            set => SetValue(value);
        }

        public event EventHandler<ParcelamentoArgs> ParceladoComSucesso;

        public void CarregarDados()
        {
            using (var repositorio = _factory.CriarRepositorio())
            {
                TiposDocumentos = repositorio.BuscaTiposDocumentos();

                if (TipoDocumento == null)
                {
                    TipoDocumento = TiposDocumentos.FirstOrDefault();
                }
            }
        }

        public void GerarParcelas()
        {
            if (QuantidadeParcelas == null || QuantidadeParcelas <= 0)
            {
                throw new InvalidOperationException("Preciso de uma quantidade de parcelas");
            }

            if (Parcelas.Count == QuantidadeParcelas)
            {
                return;
            }

            Parcelas.Clear();

            var now = DateTime.Now;
            var valorPorParcela = decimal.Round(ValorParcelar / QuantidadeParcelas.Value, 2);
            var totalParcelado = 0.00M;

            for (var i = 1; i <= QuantidadeParcelas; i++)
            {
                var valor = valorPorParcela;
                totalParcelado += valor;

                if (i == QuantidadeParcelas)
                {
                    valor += ValorParcelar - totalParcelado;
                }

                Parcelas.Add(new ParcelaContexto((byte) i, now.AddMonths(i), valor));
            }
        }

        public void ConfirmarParcelamento()
        {
            var acesso = _factory.GetAcessoConcedido();

            if (TipoDocumento == null)
            {
                throw new InvalidOperationException("Preciso de um tipo de documento para as parcelas.");
            }

            if (TipoDocumento.RegistraFinanceiro && !acesso.PossuiFusionGestor)
            {
                throw new InvalidOperationException("Documento escolhido gera financeiro e o recurso não está ativo.");
            }

            var totalParcelado = Parcelas.Sum(i => i.Valor);

            if (totalParcelado != ValorParcelar)
            {
                throw new InvalidOperationException(
                    $"Total das parcelas {totalParcelado:N2} não pode ser diferente de {ValorParcelar:N2}");
            }

            var parcelas = new List<ParcelaGerada>();

            foreach (var i in Parcelas)
            {
                var parcela = i.CriaParcela();

                parcelas.Add(parcela);
            }

            var args = new ParcelamentoArgs(parcelas, TipoDocumento);

            ParceladoComSucesso?.Invoke(this, args);
        }

        public void ComParcelas(IList<ParcelaGerada> parcelas)
        {
            Parcelas.Clear();
            QuantidadeParcelas = parcelas.Count();

            foreach (var parcela in parcelas)
            {
                Parcelas.Add(new ParcelaContexto(parcela.Numero, parcela.Vencimento, parcela.Valor));
            }
        }
    }
}