using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTemplate
{
    internal class DualToken : IToken
    {
        private IToken _parent;
        private Token _left;
        private Token _right;
        private string _token;
        public DualToken(IToken parent, string template, ref int index, string o, string c)
        {
            _parent = parent;
            _token = CommonFunctions.SubStringBefore(template, "?", ref index);
            CommonFunctions.MoveForeward(template, "?" + o, ref index);
            _left = new Token(this, template, ref index, o, c);
            CommonFunctions.MoveForeward(template, c + ":" + o, ref index);
            _right = new Token(this, template, ref index, o, c);
            CommonFunctions.MoveForeward(template, c + c, ref index); // end _right and end this dual token.
        }
        public string Populate(object entity, object linkProvider, Dictionary<string, object> extraProperties)
        {
            string judge = CommonFunctions.GetProperty(_token, entity, extraProperties);
            
            if(!string.IsNullOrEmpty(judge))
            {
                return _left.Populate(entity, linkProvider, extraProperties);
            }
            else
            {
                return _right.Populate(entity, linkProvider, extraProperties);
            }
        }
    }
}
