using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using FusionLibrary.Execoes;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global

namespace FusionLibrary.VisaoModel
{
    public abstract class ViewModel : ModelBase, IDataErrorInfo
    {
        private readonly Dictionary<string, object> _valores = new Dictionary<string, object>();
        private readonly Dictionary<string, string> _erros = new Dictionary<string, string>();

        public bool PossuiErros
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool NaoPossuiErros
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string this[string propertyName] => OnValidate(propertyName);

        public string Error => throw new NotSupportedException(
            @"IDataErrorInfo.Error is not supported," +
            @"use IDataErrorInfo.this[propertyName] instead.");

        public void SetValue<TE>(TE value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException($"Invalid propertyName {propertyName}");
            }

            if (!_valores.ContainsKey(propertyName))
            {
                _valores[propertyName] = value;
                PropriedadeAlterada(propertyName);
                return;
            }

            var lastValue = _valores[propertyName];

            if (lastValue?.Equals(value) == true)
            {
                return;
            }

            _valores[propertyName] = value;
            PropriedadeAlterada(propertyName);
        }

        public string GetValue([CallerMemberName] string properyName = null)
        {
            return GetValue<string>(properyName);
        }

        public TE GetValue<TE>([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException($"Invalid propertyName {propertyName}");
            }

            if (HasValue(propertyName))
            {
                return (TE) _valores[propertyName];
            }

            return default(TE);
        }

        public bool HasValue([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException($"Invalid propertyName {propertyName}");
            }

            return _valores.ContainsKey(propertyName);
        }

        protected void LimparMapaValores()
        {
            if (!_valores.Any())
            {
                return;
            }

            var copiaMapa = new Dictionary<string, object>(_valores);

            foreach (var map in copiaMapa)
            {
                _valores.Remove(map.Key);
                PropriedadeAlterada(map.Key);
            }
        }

        private string OnValidate(string propertyName)
        {
            string errorMessage = null;
            var value = GetValue<object>(propertyName);
            var results = new List<ValidationResult>(1);

            try
            {
                var context = new ValidationContext(this, null, null) {MemberName = propertyName};
                var isValido = Validator.TryValidateProperty(value, context, results);

                if (isValido == false)
                {
                    var validationResult = results.First();

                    errorMessage = validationResult.ErrorMessage;
                    _erros[propertyName] = errorMessage;
                }
                else if (_erros.ContainsKey(propertyName))
                {
                    _erros.Remove(propertyName);
                }
            }
            catch (Exception)
            {
                // ignore
            }
            finally
            {
                UpdateErrors();
            }

            return string.IsNullOrWhiteSpace(errorMessage) 
                ? null 
                : errorMessage;
        }

        private void UpdateErrors()
        {
            PossuiErros = _erros.Count > 0;
            NaoPossuiErros = PossuiErros == false;
        }

        //Compatibilidade com models antigos
        public void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            UpdateErrors();
        }

        protected void ThrowExceptionSeExistirErros()
        {
            RevalidateAllRules();

            if (_erros.Count > 0)
            {
                throw new ViewModelErrorsException(_erros);
            }
        }

        protected void RevalidateAllRules()
        {
            var properties = GetType().GetRuntimeProperties();

            foreach (var property in properties)
            {
                OnValidate(property.Name);
            }
        }

        protected void DeixaTipoStringVazia()
        {
            foreach (var propertyInfo in GetType().GetProperties())
            {
                if (propertyInfo.PropertyType != typeof(string)) continue;

                if (propertyInfo.Name == "Item" || propertyInfo.Name == nameof(Error))
                {
                    continue;
                }

                if (!(propertyInfo.GetValue(this, null) is string))
                {
                    propertyInfo.SetValue(this, string.Empty);
                }
            }
        }

        public void ShowDialog<T>() where T : Window
        {
            var view = Activator.CreateInstance<T>();

            if (view == null)
            {
                throw new InvalidOperationException("Não foi possível criar uma view para o tipo informado");
            }

            ShowDialog(view);
        }

        public void ShowDialog(Window view)
        {
            view.DataContext = this;
            view.ShowDialog();
        }
    }
}