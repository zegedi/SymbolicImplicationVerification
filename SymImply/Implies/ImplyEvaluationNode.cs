using SymImply.Formulas;

namespace SymImply.Implies
{
    public class ImplyEvaluationNode : ImplyEvaluation
    {
        #region Fields

        /// <summary>
        /// The list of evaluations.
        /// </summary>
        protected List<ImplyEvaluation> evaluations;

        #endregion

        #region Constructors

        public ImplyEvaluationNode(Imply imply, ImplyEvaluation evaluation) : this(imply, null, evaluation) { }

        public ImplyEvaluationNode(Imply imply, string? message, ImplyEvaluation evaluation)
            : base(imply, message)
        {
            evaluations = new List<ImplyEvaluation>
            {
                evaluation
            };
        }

        public ImplyEvaluationNode(Imply imply, List<ImplyEvaluation> evaluations)
            : this(imply, null, evaluations) { }

        public ImplyEvaluationNode(Imply imply, string? message, List<ImplyEvaluation> evaluations)
            : base(imply, message)
        {
            this.evaluations = evaluations;
        }

        public ImplyEvaluationNode(
            Imply imply, Formula hypothesis, ICollection<Formula> consequences)
            : this(imply, null, hypothesis, consequences) { }

        public ImplyEvaluationNode(
            Imply imply, string? message, Formula hypothesis, ICollection<Formula> consequences)
            : base(imply, message)
        {
            evaluations = new List<ImplyEvaluation>(consequences.Count);

            foreach (Formula consequence in consequences)
            {
                Imply nextImply = new Imply(hypothesis.DeepCopy(), consequence);
                
                evaluations.Add(nextImply.Evaluated());
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the list of evaluations.
        /// </summary>
        public List<ImplyEvaluation> Evaluations
        {
            get { return evaluations; }
            set { evaluations = value; }
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Determines the result of the evaluation.
        /// </summary>
        /// <returns>The result of the evaluation.</returns>
        public override ImplyEvaluationResult EvaluationResult()
        {
            ImplyEvaluationResult result = ImplyEvaluationResult.Unverifiable;

            Func<ImplyEvaluation, bool> IsTrue =
                imply => imply.EvaluationResult() == ImplyEvaluationResult.True;

            Func<ImplyEvaluation, bool> IsFalse =
                imply => imply.EvaluationResult() == ImplyEvaluationResult.False;

            if (evaluations.All(IsTrue))
            {
                result = ImplyEvaluationResult.True;
            }
            else if (evaluations.Any(IsFalse))
            {
                result = ImplyEvaluationResult.False;
            }

            return result;
        }

        #endregion
    }
}
