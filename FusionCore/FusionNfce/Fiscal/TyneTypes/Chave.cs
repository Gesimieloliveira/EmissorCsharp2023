using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Helpers.CloneSerializer;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class Chave : ICloneable
    {
        private byte _codigoIbgeUf;
        private DateTime _anoMes;
        private string _cnpj;
        private CnpjEmitente _cnpjTyne;
        private ModeloDocumento _modeloDocumento;
        private short _serie;
        private Serie _serieTyne;
        private long _numeroFiscal;
        private NumeroFiscal _numeroFiscalTyne;
        private TipoEmissao _formaEmissao;
        private int _codigoNumerico;
        private CodigoNumerico _codigoNumericoTyne;
        private int _digitoVerificador;
        private DigitoVerificador _digitoVerificadorTyne;

        private Chave() { }

        private Chave(byte codigoIbgeUf, DateTime anoMes, CnpjEmitente cnpjTyne, ModeloDocumento modeloDocumento, Serie serieTyne, NumeroFiscal numeroFiscalTyne, TipoEmissao formaEmissao, CodigoNumerico codigoNumericoTyne, DigitoVerificador digitoVerificadorTyne)
        {
            _codigoIbgeUf = codigoIbgeUf;
            _anoMes = anoMes;
            _cnpjTyne = cnpjTyne;
            _cnpj = _cnpjTyne.Valor;
            _modeloDocumento = modeloDocumento;
            _serieTyne = serieTyne;
            _serie = _serieTyne.Valor;
            _numeroFiscalTyne = numeroFiscalTyne;
            _numeroFiscal = _numeroFiscalTyne.Valor; 
            _formaEmissao = formaEmissao;
            _codigoNumericoTyne = codigoNumericoTyne;
            _codigoNumerico = _codigoNumericoTyne.Valor;
            _digitoVerificadorTyne = digitoVerificadorTyne;
            _digitoVerificador = _digitoVerificadorTyne.Valor;
        }


        public byte CodigoIbgeUf => _codigoIbgeUf;
        public DateTime AnoMes => _anoMes;
        public CnpjEmitente Cnpj => (_cnpjTyne ?? (_cnpjTyne = new CnpjEmitente(_cnpj)));
        public ModeloDocumento ModeloDocumento => _modeloDocumento;
        public Serie Serie => (_serieTyne ?? (_serieTyne = new Serie(_serie)));
        public NumeroFiscal NumeroFiscal => (_numeroFiscalTyne ?? (_numeroFiscalTyne = new NumeroFiscal(_numeroFiscal)));
        public TipoEmissao FormaEmissao => _formaEmissao;
        public CodigoNumerico CodigoNumerico => (_codigoNumericoTyne ?? (_codigoNumericoTyne = new CodigoNumerico(_codigoNumerico)));
        public DigitoVerificador DigitoVerificador => (_digitoVerificadorTyne ?? (_digitoVerificadorTyne = new DigitoVerificador(_digitoVerificador)));



        public Builder ToBuilder()
        {
            return new Builder(this);
        }

        public sealed class Builder
        {
            private byte CodigoIbgeUf { get; set; }
            private DateTime AnoMes { get; set; }
            private CnpjEmitente Cnpj { get; set; }
            private ModeloDocumento ModeloDocumento { get; set; }
            private Serie Serie { get; set; }
            private NumeroFiscal NumeroFiscal { get; set; }
            private TipoEmissao FormaEmissao { get; set; }
            private CodigoNumerico CodigoNumerico { get; set; }
            private DigitoVerificador DigitoVerificador { get; set; }

            public Builder() { }

            public Builder(Chave chave)
            {
                var chaveClone = chave.CopiaProfunda();

                CodigoIbgeUf = chaveClone.CodigoIbgeUf;
                AnoMes = chaveClone.AnoMes;
                Cnpj = chaveClone.Cnpj;
                ModeloDocumento = chaveClone.ModeloDocumento;
                Serie = chaveClone.Serie;
                NumeroFiscal = chaveClone.NumeroFiscal;
                FormaEmissao = chaveClone.FormaEmissao;
                CodigoNumerico = chaveClone.CodigoNumerico;
                DigitoVerificador = chaveClone.DigitoVerificador;
            }

            public Builder ComCodigoIbgeUf(byte codigoIbgeUf)
            {
                CodigoIbgeUf = codigoIbgeUf;
                return this;
            }

            public Builder ComAnoMes(DateTime anoMes)
            {
                AnoMes = anoMes;
                return this;
            }

            public Builder ComCnpjEmitente(CnpjEmitente cnpjEmitente)
            {
                Cnpj = cnpjEmitente;
                return this;
            }

            public Builder ComModeloDocumento(ModeloDocumento modeloDocumento)
            {
                ModeloDocumento = modeloDocumento;
                return this;
            }

            public Builder ComSerie(Serie serie)
            {
                Serie = serie;
                return this;
            }

            public Builder ComNumeroFiscal(NumeroFiscal numeroFiscal)
            {
                NumeroFiscal = numeroFiscal;
                return this;
            }

            public Builder ComFormaEmissao(TipoEmissao formaEmissao)
            {
                FormaEmissao = formaEmissao;
                return this;
            }

            public Builder ComCodigoNumerico(CodigoNumerico codigoNumerico)
            {
                CodigoNumerico = codigoNumerico;
                return this;
            }

            public Builder ComDigitoVerificador(DigitoVerificador digitoVerificador)
            {
                DigitoVerificador = digitoVerificador;
                return this;
            }

            public static implicit operator Chave(Builder builder)
            {
                var chave = new Chave(builder.CodigoIbgeUf, builder.AnoMes, builder.Cnpj,
                    builder.ModeloDocumento, builder.Serie, builder.NumeroFiscal, builder.FormaEmissao,
                    builder.CodigoNumerico, builder.DigitoVerificador);

                return chave;
            }
        }


        protected bool Equals(Chave other)
        {
            return _codigoIbgeUf == other._codigoIbgeUf && _anoMes.Equals(other._anoMes) && string.Equals(_cnpj, other._cnpj) && Equals(_cnpjTyne, other._cnpjTyne) && _modeloDocumento == other._modeloDocumento && _serie == other._serie && Equals(_serieTyne, other._serieTyne) && _numeroFiscal == other._numeroFiscal && Equals(_numeroFiscalTyne, other._numeroFiscalTyne) && _formaEmissao == other._formaEmissao && _codigoNumerico == other._codigoNumerico && Equals(_codigoNumericoTyne, other._codigoNumericoTyne) && Equals(_digitoVerificador, other._digitoVerificador) && Equals(_digitoVerificadorTyne, other._digitoVerificadorTyne);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Chave) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _codigoIbgeUf.GetHashCode();
                hashCode = (hashCode*397) ^ _anoMes.GetHashCode();
                hashCode = (hashCode*397) ^ (_cnpj != null ? _cnpj.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_cnpjTyne != null ? _cnpjTyne.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) _modeloDocumento;
                hashCode = (hashCode*397) ^ _serie.GetHashCode();
                hashCode = (hashCode*397) ^ (_serieTyne != null ? _serieTyne.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _numeroFiscal.GetHashCode();
                hashCode = (hashCode*397) ^ (_numeroFiscalTyne != null ? _numeroFiscalTyne.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) _formaEmissao;
                hashCode = (hashCode*397) ^ _codigoNumerico;
                hashCode = (hashCode*397) ^ (_codigoNumericoTyne != null ? _codigoNumericoTyne.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_digitoVerificador != null ? _digitoVerificador.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_digitoVerificadorTyne != null ? _digitoVerificadorTyne.GetHashCode() : 0);
                return hashCode;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}