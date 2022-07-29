using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Ardalis.GuardClauses;

namespace DynamicCRUD.Services
{
    public static class StringHelperService
    {        public static string GetCamelCase(string value)
        {
            return $"{value.Substring(0, 1).ToLower()}{value.Substring(1)}";
        }
        public static string RemoveUnsupportedCharacters(string value)
        {
            value = value.Replace(" ", "").Replace("/", "").Replace("»", "").Replace("¿", "").Replace("(", "").Replace(")", "").Replace("_","").Replace("-","");
            bool isIntString = value.Substring(0, 1).All(char.IsDigit);
                if (isIntString) value = $"_{value}";
            return value;
        }
        public static string AddSpacesToSentence(string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            text = RemoveUnsupportedCharacters(text);
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if (text[i - 1] != ' ' && !char.IsUpper(text[i - 1]) ||
                        preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1]))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

    }
}