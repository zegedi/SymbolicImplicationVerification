﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicImplicationVerification.Converts
{
    public class ConvertException : Exception
    {
        public ConvertException() : base() { }

        public ConvertException(string message) : base(message) { }
    }
}
