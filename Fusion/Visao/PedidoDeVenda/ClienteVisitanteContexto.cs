using System;
using FusionCore.FusionAdm.PedidoVenda;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda
{
    public class ClienteVisitanteContexto : ViewModel
    {
        public string Nome
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public event EventHandler<Visitante> FinalizadoSucesso;

        public void ConfirmarVisitante()
        {
            var visitante = new Visitante
            {
                Nome = Nome
            };

            FinalizadoSucesso?.Invoke(this, visitante);
        }

        public void Update(Visitante visitante)
        {
            Nome = visitante.Nome;
        }
    }
}