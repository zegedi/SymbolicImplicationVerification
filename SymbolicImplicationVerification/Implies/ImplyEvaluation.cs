
using SymbolicImplicationVerification.Formulas;

namespace SymbolicImplicationVerification.Implies
{
    public abstract class ImplyEvaluation
    {
        #region Fields

        /// <summary>
        /// The imply to evaluate.
        /// </summary>
        protected Imply imply;

        /// <summary>
        /// The message of the evaluation.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the imply to evaluate.
        /// </summary>
        public Imply Imply
        {
            get { return imply; }
            set { imply = value; }
        }

        /// <summary>
        /// Gets or sets message of the evaluation.
        /// </summary>
        public string? Message
        {
            get { return message; }
            set { message = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Determines the result of the evaluation.
        /// </summary>
        /// <returns>The result of the evaluation.</returns>
        public abstract ImplyEvaluationResult EvaluationResult();

        #endregion
    }
}
