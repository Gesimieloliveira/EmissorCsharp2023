using System;
using System.Collections.Generic;
using System.Linq;

namespace FusionLibrary.Execoes
{
    public class ViewModelErrorsException : InvalidOperationException
    {
        private readonly Dictionary<string, string> _errors;

        public ViewModelErrorsException(Dictionary<string, string> errors)
        {
            _errors = errors;
        }

        public override string Message => string.Join(Environment.NewLine, _errors.Select(i => i.Value));
    }
}