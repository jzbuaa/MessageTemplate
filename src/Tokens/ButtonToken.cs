using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MessageTemplate
{
    class ButtonToken : IToken
    {
        private IToken _parent;
        private List<IToken> _children;

        private string _linkMethod;
        private IToken _token;
        public ButtonToken(IToken parent, string template, ref int index, string o, string c)
        {
            _parent = parent;
            _linkMethod = CommonFunctions.SubStringBefore(template, Constants.Separater, ref index);
            CommonFunctions.MoveForeward(template, Constants.Separater, ref index);
            _token = new Token(this, template, ref index, o, c);
            CommonFunctions.MoveForeward(template, c, ref index);
        }

        
        public string Populate(object entity, object linkProvider, Dictionary<string, object> extraProperties)
        {
            if(_token == null)
            {
                throw new TemplateRenderException("null token while populating button");
            }
            
            if (_linkMethod == null)
            {
                throw new TemplateRenderException("null link method while populating button");
            }

            string buttonText = _token.Populate(entity, linkProvider, extraProperties);
            string link = CommonFunctions.GetMethod(_linkMethod, linkProvider, extraProperties);
            
            return $"<a href='{link}'>{buttonText}</a>";
        }
    }
}
