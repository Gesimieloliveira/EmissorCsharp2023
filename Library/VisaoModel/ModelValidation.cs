using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace FusionLibrary.VisaoModel
{
    public class ModelValidation : ModelBase, IDataErrorInfo
    {
        private bool _botaoSalvar = true;
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
        private readonly Dictionary<string, int> _erros = new Dictionary<string, int>();

        public bool BotaoSalvar
        {
            get { return _botaoSalvar; }
            set
            {
                if (value == _botaoSalvar) return;
                _botaoSalvar = value;
                PropriedadeAlterada();
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                throw new NotSupportedException(
                    @"IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");
            }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return OnValidate(propertyName); }
        }

        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }
        public int Errors { get; set; }

        protected void SetValue<TE>(Expression<Func<TE>> propertySelector, TE value)
        {
            var propertyName = GetPropertyName(propertySelector);

            SetValue(propertyName, value);
        }

        protected void SetValue<TE>(TE value, [CallerMemberName] string propertyName = null)
        {
            SetValue(propertyName, value);
        }

        protected void SetValue<TE>(string propertyName, TE value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException(@"Invalid property name", propertyName);
            }

            _values[propertyName] = value;
            PropriedadeAlterada(propertyName);
        }

        protected TE GetValue<TE>(Expression<Func<TE>> propertySelector)
        {
            var propertyName = GetPropertyName(propertySelector);

            return GetValue<TE>(propertyName);
        }

        protected TE GetValue<TE>([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException(@"Invalid property name", propertyName);
            }

            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                value = default(TE);
                _values.Add(propertyName, value);
            }

            return (TE) value;
        }

        protected virtual string OnValidate(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException(@"Invalid property name", propertyName);
            }

            var error = string.Empty;
            var value = GetValue(propertyName);
            var results = new List<ValidationResult>(1);
            var result = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null)
                {
                    MemberName = propertyName
                },
                results);

            if (!result)
            {
                var validationResult = results.First();

                _erros[propertyName] = results.Count;

                error = validationResult.ErrorMessage;
            }
            else
            {
                _erros[propertyName] = 0;
            }


            var temErro = _erros.Any(erro => erro.Value > 0);

            BotaoSalvar = !temErro;

            return error;
        }

        private string GetPropertyName(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }

        private object GetValue(string propertyName)
        {
            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                var propertyDescriptor = TypeDescriptor.GetProperties(GetType()).Find(propertyName, false);
                if (propertyDescriptor == null)
                {
                    throw new ArgumentException(@"Invalid property name", propertyName);
                }

                value = propertyDescriptor.GetValue(this);
                _values.Add(propertyName, value);
            }

            return value;
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] != null) return;
            var msg = @"Invalid property name: " + propertyName;

            if (ThrowOnInvalidPropertyName)
                throw new Exception(msg);
            Debug.Fail(msg);
        }

        public virtual void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) Errors -= 1;

            BotaoSalvar = Errors == 0;
        }
    }
}