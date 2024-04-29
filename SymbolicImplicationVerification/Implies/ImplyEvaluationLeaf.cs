namespace SymbolicImplicationVerification.Implies
{
    internal class ImplyEvaluationLeaf : ImplyEvaluation
    {
        #region Fields

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

        public ImplyEvaluationResult Result
        {
            get { return result; }
            set { result = value; }
        }

        #endregion

        #region Public methods

        public override ImplyEvaluationResult EvaluationResult()
        {
            return result;
        }

        #endregion
    }
}
