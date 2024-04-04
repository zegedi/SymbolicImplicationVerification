
using SymbolicImplicationVerification.Formulas;

namespace SymbolicImplicationVerification.Implies
{
    public class ImplyEvaluation
    {
        #region Fields

        protected Imply imply;

        protected string? message;

        #endregion

        #region Constructors

        public ImplyEvaluation(Imply imply) : this(imply, null) { }

        public ImplyEvaluation(Imply imply, string? message)
        {
            this.imply   = imply;
            this.message = message;
        }

        #endregion

        #region Public properties

        public Imply Imply
        {
            get { return imply; }
            set { imply = value; }
        }

        public string? Message
        {
            get { return message; }
            set { message = value; }
        }

        #endregion
    }
}
