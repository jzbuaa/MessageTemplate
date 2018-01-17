using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTemplate
{
    public interface IToken
    {
        string Populate(object entity, object linkProvider, Dictionary<string, object> extraProperties);
    }
}
