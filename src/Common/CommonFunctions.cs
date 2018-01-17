using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MessageTemplate
{
    
    public static class CommonFunctions
    {
        static Regex alphaNumberic = new Regex("^[a-zA-Z0-9]*$");
        public static bool IsAlphaNumberic(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            return alphaNumberic.IsMatch(s);
        }
        public static string SubStringBefore(string template, string lookfor, ref int index)
        {
            if (index < 0 || index >= template.Length)
            {
                throw new TemplateParseException($"invalid start index: {index}");
            }
            int index_s = index;
            int index_lookfor = template.Substring(index).IndexOf(lookfor);
            if (index_lookfor == -1)
            {
                throw new TemplateParseException($"not found {lookfor} after index {index}, template:\n{template}");
            }
            else
            {
                index += index_lookfor;
                return template.Substring(index_s, index_lookfor);
            }
        }
        public static void MoveForeward(string template, string expected, ref int index)
        {
            if (index < 0 || index >= template.Length)
            {
                throw new TemplateParseException($"invalid start index: {index}");
            }
            if (template.Substring(index).StartsWith(expected))
            {
                index += expected.Length;
            }
            else
            {
                throw new TemplateParseException($"not found expected '{expected}'", index, template);
            }
        }
        public static bool TryMoveForeward(string template, string expected, ref int index)
        {
            if (index < 0 || index >= template.Length)
            {
                throw new TemplateParseException($"invalid start index: {index}");
            }
            if (template.Substring(index).StartsWith(expected))
            {
                index += expected.Length;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetProperty(string propName, object entity, Dictionary<string, object> extraProperties)
        {
            if (string.IsNullOrEmpty(propName))
            {
                throw new TemplateRenderException("null property name found");
            }
            string result = string.Empty;
            if (extraProperties.ContainsKey(propName))
            {
                result = extraProperties[propName].ToString();
            }
            else
            {
                var propertyInfo = entity.GetType().GetProperty(propName);
                if (propertyInfo == null)
                {
                    throw new TemplateRenderException($"not found property '{propName}' in '{entity.GetType()}'");
                }
                var p = propertyInfo.GetValue(entity);
                if (p == null)
                {
                    result = string.Empty;
                }
                else
                {
                    result = p.ToString();
                }
                extraProperties.Add(propName, result);
            }
            return result;
        }
        public static string GetMethod(string methodName, object entity, Dictionary<string, object> extraProperties)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new TemplateRenderException("null method name found");
            }
            const string linkKeyPrefix = "#link.";
            string result = string.Empty;
            if (extraProperties.ContainsKey(linkKeyPrefix + methodName))
            {
                result = extraProperties[linkKeyPrefix + methodName].ToString();
            }
            else
            {
                var method = entity.GetType().GetMethod(methodName);
                if (method == null)
                {
                    throw new TemplateRenderException($"not found link method '{methodName}' in link provider:{entity.GetType()}");
                }
                try
                {
                    result = method.Invoke(entity, null).ToString(); // no parameter support yet.
                    extraProperties.Add(linkKeyPrefix + methodName, result);
                }
                catch (Exception e)
                {
                    throw new TemplateRenderException($"exception while invoking '{methodName}' of '{entity.GetType()}'", e);
                }
            }
            return result;
        }
    }

}
