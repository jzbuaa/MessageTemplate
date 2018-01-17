using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTemplate
{
    class TextToken : IToken
    {
        private IToken _parent;
        private string _text;
        public TextToken(IToken parent, string text)
        {
            _parent = parent;
            _text = text;
        }
        public string Populate(object entity, object linkProvider, Dictionary<string, object> extraProperties)
        {
            return _text;
        }
    }
}
