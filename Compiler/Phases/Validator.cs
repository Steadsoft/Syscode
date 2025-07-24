using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscode
{
    public class Validator
    {
        private Reporter reporter;
        public Validator(Reporter reporter)
        {
            this.reporter = reporter;
        }

        public void Validate(Compilation root)
        {

        }

    }
}
