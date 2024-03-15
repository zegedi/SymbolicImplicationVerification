using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicImplicationVerification.Formulas
{
    public abstract class Formula
    {
        #region Fields

        protected string? identifier;

        #endregion

        #region Constructors

        public Formula(string? identifier)
        {
            this.identifier = identifier;
        }

        #endregion

        #region Public properties

        public string? Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        #endregion
    }
}
