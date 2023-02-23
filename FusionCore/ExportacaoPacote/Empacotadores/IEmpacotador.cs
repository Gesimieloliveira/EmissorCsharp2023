using System.Collections.Generic;
using System.IO;

namespace FusionCore.ExportacaoPacote.Empacotadores
{
    public interface IEmpacotador
    {
        void ComEnvelopes(IList<IEnvelope> envelopes);
        FileInfo GeraPacote();
    }
}