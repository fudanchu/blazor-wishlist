using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;

namespace Wishlist.Client.Shared
{
    public partial class CommonInput<T> : InputBase<T>
    {
        [Parameter] public string Id { get; set; } = null;
        [Parameter] public string Label { get; set; }
        [Parameter] public string ExtraCSS { get; set; }
        [Parameter] public string Icon { get; set; }
        [Parameter] public string Placeholder { get; set; } = "";
        [Parameter] public string InputType { get; set; } = "text";
        [Parameter] public bool IsAutoFocused { get; set; } = false;
        [Parameter] public bool IsLabelInline { get; set; } = false;

        [Parameter] public Expression<Func<T>> ValidationFor { get; set; }

        protected string GetInputType()
        {
            return (InputType == "decimal" ? "number" : InputType);
        }
        protected string GetStep()
        {
            return (InputType == "decimal" ? "any" : null);
        }
        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            if (typeof(T) == typeof(string))
            {
                result = (T)(object)value;
                validationErrorMessage = null;

                return true;
            }
            else if (typeof(T) == typeof(int))
            {
                int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
                result = (T)(object)parsedValue;
                validationErrorMessage = null;

                return true;
            }
            else if (typeof(T) == typeof(Decimal))
            {
                Decimal.TryParse(value, out var parsedValue);
                result = (T)(object)parsedValue;
                validationErrorMessage = null;

                return true;
            }
            else if (typeof(T).IsEnum)
            {
                try
                {
                    result = (T)Enum.Parse(typeof(T), value);
                    validationErrorMessage = null;

                    return true;
                }
                catch (ArgumentException)
                {
                    result = default;
                    validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";

                    return false;
                }
            }

            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(T)}'.");
        }
    }
}
