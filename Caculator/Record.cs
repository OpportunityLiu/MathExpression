using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caculator
{
    class Record : ObservableObject
    {
        private string input;

        public string Input
        {
            get
            {
                return input;
            }
            set
            {
                Set(ref input, value);
            }
        }

        private string output;

        public string Output
        {
            get
            {
                return output;
            }
            set
            {
                Set(ref output, value);
            }
        }
    }
}
