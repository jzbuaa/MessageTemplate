using MessageTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTemplate
{
    class Token : IToken
    {
        public List<IToken> _children;
        private IToken _parent;
        public Token(IToken parent, string template, ref int index, string o, string c)
        {
            _parent = parent;
            _children = new List<IToken>();
            int length = template.Length;

            while (index < length)
            {
                int next_o = template.IndexOf(o, index);
                int next_c = template.IndexOf(c, index);
                if (next_c > 0 && (next_o < 0 || next_c < next_o))// find close first.
                {
                    _children.Add(new TextToken(this, template.Substring(index, next_c - index)));
                    index = next_c;

                    return;
                }
                else if (next_o > 0 && (next_c < 0 || next_o < next_c))// find opennig first.
                {
                    if (next_o > index)
                    {
                        _children.Add(new TextToken(this, template.Substring(index, next_o - index)));
                        index = next_o;
                    }
                    CommonFunctions.MoveForeward(template, o, ref index);
                    if (CommonFunctions.TryMoveForeward(template, Constants.PropertyKeyword, ref index))
                    {
                        _children.Add(new PropertyToken(this, template, ref index, o, c));
                        continue;
                    }
                    else if (CommonFunctions.TryMoveForeward(template, Constants.ButtonKeyword, ref index))
                    {
                        _children.Add(new ButtonToken(this, template, ref index, o, c));
                        continue;
                    }
                    else if (CommonFunctions.TryMoveForeward(template, Constants.DualKeyword, ref index))
                    {
                        _children.Add(new DualToken(this, template, ref index, o, c));
                        continue;
                    }
                    else
                    {
                        throw new TemplateParseException($"error parsing template", index, template);
                    }
                }
                else // not found openning or close.
                {
                    _children.Add(new TextToken(this, template.Substring(index)));
                    index = template.Length;
                    return;
                }
            }
        }

        public string Populate(object entity, object linkProvider, Dictionary<string, object> extraProperties)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var token in _children)
            {
                sb.Append(token.Populate(entity, linkProvider, extraProperties));
            }
            return sb.ToString();
        }
    }
}
