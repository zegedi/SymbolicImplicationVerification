using System;

namespace SymbolicImplicationVerification.Term
{
    public class Variable<T> : Term<T> where T : Type.Type
    {
        #region Fields

        protected string identifier;

        #endregion

        #region Constructors

        public Variable(string identifier, T termType) : base(termType)
        {
            this.identifier = identifier;
        }

        #endregion

        #region Public properties

        public string Identifier
        {
            get { return identifier; }
            private set { identifier = value; }
        }

        #endregion
    }
}
