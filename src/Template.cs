using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTemplate
{
    public static class Template
    {
        const string OC = "[]";
        private static ConcurrentDictionary<string, IToken> ParsedTemplates = new ConcurrentDictionary<string, IToken>();
        
        public static IToken Parse(string template, string oc =OC)
        {
            if(ParsedTemplates.ContainsKey(template))
            {
                IToken t = null;
                if(ParsedTemplates.TryGetValue(template, out t))
                {
                    return t;
                }
            }
            if(oc.Length%2!=0)
            {
                throw new ArgumentException($"invalid oc for template parsing: {oc}");
            }
            string o = oc.Substring(0, oc.Length / 2);
            string c = oc.Substring(oc.Length / 2);
            int index = 0;
            Token root = new Token(null, template, ref index, o, c);
            if(index!=template.Length)
            {
                throw new TemplateParseException("");
            }
            return root;
        }
        public static string Render(string template, object entity, object linkProvider, Dictionary<string, object> extraProperties = null, string oc=OC)
        {
            var root = Parse(template, oc);
            if(extraProperties == null)
            {
                extraProperties = new Dictionary<string, object>();
            }

            return root.Populate(entity, linkProvider, extraProperties);
        }

    }
}
