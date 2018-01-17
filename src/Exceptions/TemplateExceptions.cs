using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTemplate
{
    public class TemplateException : Exception
    {
        public TemplateException(string message, Exception innerEx = null) : base(message, innerEx) { }
    }

    class TemplateParseException : TemplateException
    {
        public TemplateParseException(string message) : base(message)
        {
        }
        public TemplateParseException(string message, int startIndex, string template)
            : base($"{message} at index: {startIndex}, template:\n{template}")
        {
        }
    }
    class TemplateRenderException : TemplateException
    {
        public TemplateRenderException(string message, Exception innerEx = null) : base(message, innerEx) { }
    }
}
