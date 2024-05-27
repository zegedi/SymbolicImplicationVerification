namespace SymImply.Implies
{
    internal class ImplyEvaluationLeaf : ImplyEvaluation
    {
        #region Fields

        /// <summary>
        /// The result of the evaluation.
        /// </summary>
        protected ImplyEvaluationResult result;

        #endregion

        #region Constructors

        public ImplyEvaluationLeaf(Imply imply, ImplyEvaluationResult result) : this(imply, null, result) { }

        public ImplyEvaluationLeaf(Imply imply, string? message, ImplyEvaluationResult result)
            : base(imply, message)
        {
            this.result = result;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the result of the evaluation.
        /// </summary>
        public ImplyEvaluationResult Result
        {
            get { return result; }
            set { result = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines the result of the evaluation.
        /// </summary>
        /// <returns>The result of the evaluation.</returns>
        public override ImplyEvaluationResult EvaluationResult()
        {
            return result;
        }

        #endregion
    }
}
