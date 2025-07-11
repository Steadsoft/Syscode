using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscode
{

    public class Report
    {
        private AstNode node;
        private int code;
        private string[] args;

        internal Report (AstNode Node, int Code, params string[] Args)
        {
            node = Node;
            code = Code;
            args = Args;
        }

        public AstNode Node { get => node; set => node = value; }
        public int Code { get => code; set => code = value; }
        public string[] Args { get => args; set => args = value; }
    }
}
