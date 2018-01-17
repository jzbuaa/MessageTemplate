using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Entity
    {
        public string name { get { return "Jing"; } }
        public string linkName1 { get { return "L1"; } }
        public string linkName2 { get { return "L2"; } }
        public string link1() { return "link1"; }
        public string link2() { return "link2"; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string template = @"this is a [key:name], with a [button:link1,[key:linkName1]] and a [dual:name?[My name is [key:name]]:[I have no name but I have a [button:link2,this is [key:linkName2]]]]";

            string ss = template.Substring(0, 0);

            var entity = new Entity();
            var root = MessageTemplate.Template.Render(template, entity, entity);


            Dictionary<string, string> dd = new Dictionary<string, string>();
            TestDict(dd);

            Console.WriteLine(root);
            Console.Read();
        }
        static void TestDict(Dictionary<string, string> dict)
        {
            dict.Add("name", "Jing");
        }
    }
}
