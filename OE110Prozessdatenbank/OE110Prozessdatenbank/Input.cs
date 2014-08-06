using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OE110Prozessdatenbank
{
    public static class Input
    {
        private static Regex m_decimalRegex = new Regex(@"^[0-9]{1,6}([,][0-9]{0,10})?$");
        private static Regex m_integerRegex = new Regex(@"^[0-9]+$");
        private static Regex m_characterRegex = new Regex(@"^[a-zA-Z0-9\-_+-,.:;/'&=öäü]?$");

        public static Regex DecimalRegex
        { get { return m_decimalRegex; } }

        public static Regex IntegerRegex
        { get { return m_integerRegex; } }

        public static Regex CharacterRegex
        { get { return m_characterRegex; } }

    }
}
