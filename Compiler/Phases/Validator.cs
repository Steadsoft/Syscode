using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscode
{
    public class Validator
    {
        private readonly Reporter reporter;
        public Validator(Reporter reporter)
        {
            this.reporter = reporter;
        }

        public static void Validate(Compilation root)
        {

        }

    }
}
