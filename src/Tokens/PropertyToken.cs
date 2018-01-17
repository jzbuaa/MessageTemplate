using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTemplate
{
    internal class PropertyToken : IToken
    {
        private IToken _parent;
        private List<IToken> _children;
        private string _token;
        public PropertyToken(IToken parent, string template, ref int index, string o,string c)
        {
            _parent = parent;
            _token = CommonFunctions.SubStringBefore(template, c, ref index);
            CommonFunctions.MoveForeward(template, c, ref index);
        }

        public string Populate(object entity, object linkProvider, Dictionary<string, object> extraProperties)
        {
            return CommonFunctions.GetProperty(_token, entity, extraProperties);
        }
    }
}
