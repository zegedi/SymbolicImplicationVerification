using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Implies
{
    public class Imply : IEvaluable<ImplyEvaluation>
    {
        #region Fields

        protected Formula hypothesis;

        protected Formula consequence;

        #endregion

        #region Constructors

        public Imply(Imply imply) : this(imply.hypothesis.DeepCopy(), imply.consequence.DeepCopy()) { }

        public Imply(Formula hypothesis, Formula consequence)
        {
            this.hypothesis  = hypothesis;
            this.consequence = consequence;
        }

        #endregion

        #region Public properties

        public Formula Hypothesis
        {
            get { return hypothesis; }
            set { hypothesis = value; }
        }

        public Formula Consequence
        {
            get { return consequence; }
            set { consequence = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("({0}) ==> ({1})", hypothesis, consequence);
        }


        /// <summary>
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public ImplyEvaluation Evaluated() => (hypothesis, consequence) switch
        {
            (FALSE or NotEvaluable, _)
                => new ImplyEvaluationLeaf(this, 
                   string.Format("Mivel [{0}] = {}, ezért {0} => {1}", hypothesis.ToString(), consequence.ToString()),
                   ImplyEvaluationResult.True),

            (_, TRUE)
                => new ImplyEvaluationLeaf(this,
                   string.Format("Mivel [{0}] \\subseteq [{1}].", hypothesis.ToString(), consequence.ToString()),
                   ImplyEvaluationResult.True),

            (_, FALSE or NotEvaluable) 
                => new ImplyEvaluationLeaf(this,
                   string.Format("Mivel [{1}] = {}, viszont [{0}] != {}.", hypothesis.ToString(), consequence.ToString()),
                   ImplyEvaluationResult.False),

            (TRUE, _)
                => new ImplyEvaluationLeaf(this,
                   string.Format("Mivel [{0}] \\not\\subseteq [{1}].", hypothesis.ToString(), consequence.ToString()),
                   ImplyEvaluationResult.False),

            (_, _) => EvaluationAlgorithm()
        };

        /// <summary>
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public ImplyEvaluation EvaluationAlgorithm()
        {
            Formula hypothesisEval  = hypothesis .Evaluated();
            Formula consequenceEval = consequence.Evaluated();

            Imply nextImply;
            string message;

            if (!hypothesis.Equals(hypothesisEval) || !consequence.Equals(consequenceEval))
            {
                nextImply = new Imply(hypothesisEval, consequenceEval);
                message   = "Egyszerűsítsd a jobb és baloldali részformulákat.";

                return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            }

            // Tovább nem egyszerűsíthető a formula.

            if (hypothesis.Equivalent(consequence))
            {
                message = string.Format("Mivel {0} és {1} ekvivalensek.", hypothesis, consequence);

                return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
            }

            if (hypothesis.Implies(consequence))
            {
                message = string.Format("Mivel {0} maga után vonja {1} bekövetkezését.", hypothesis, consequence);

                return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
            }

            if (consequence is ConjunctionFormula conjunctionConsequence)
            {
                LinkedList<Formula> consequenceOperands = conjunctionConsequence.LinearOperands();

                message = "Mivel a jobb oldal konjunkció, ezért minden állítást igazolni kell.";

                return new ImplyEvaluationNode(new Imply(this), message, hypothesis, consequenceOperands);
            }

            if (consequence is DisjunctionFormula disjunctionConsequence)
            {
                LinkedList<Formula> consequenceOperands = disjunctionConsequence.SimplifiedLinearOperands();

                if (consequenceOperands.Count != disjunctionConsequence.LinearOperands().Count)
                {
                    message = "Egyszerűsítsd a jobboldali részformulát.";

                    nextImply = new Imply(hypothesis, disjunctionConsequence.Simplified());

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }

                message = "Mivel a jobb oldal diszjunkció, ezért legalább az egyik állítást igazolni kell.";

                return new ImplyEvaluationNode(new Imply(this), message, hypothesis, consequenceOperands);
            }

            if (hypothesis is ConjunctionFormula conjunctionHypothesis)
            {
                LinkedList<Formula> hypothesisOperands = conjunctionHypothesis.LinearOperands();

                foreach (Formula operand in hypothesisOperands)
                {
                    if (operand.Equivalent(consequence))
                    {
                        message = string.Format("Mivel {0} és {1} ekvivalensek.", operand, consequence);

                        return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
                    }

                    if (operand.Implies(consequence))
                    {
                        message = string.Format("Mivel {0} maga után vonja {1} bekövetkezését.", operand, consequence);

                        return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
                    }
                }

                if (hypothesisOperands.Count != conjunctionHypothesis.SimplifiedLinearOperands().Count)
                {
                    message = "Egyszerűsítsd a bal oldali részformulát.";

                    nextImply = new Imply(conjunctionHypothesis.Simplified(), consequence);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }
            }

            if (hypothesis is DisjunctionFormula disjunctionHypothesis)
            {
                LinkedList<Formula> hypothesisOperands = disjunctionHypothesis.LinearOperands();

                if (hypothesisOperands.Count != disjunctionHypothesis.SimplifiedLinearOperands().Count)
                {
                    message = "Egyszerűsítsd a bal oldali részformulát.";

                    nextImply = new Imply(disjunctionHypothesis.Simplified(), consequence);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }
            }

            return new ImplyEvaluationLeaf(new Imply(this), "Nem sikerült.", ImplyEvaluationResult.Unverifiable);
        }

        public void Test()
        {
            ImplyEvaluation eval    = Evaluated();

            using (StreamWriter sw = new StreamWriter("evalTest.txt"))
            {
                WriteOut(sw, eval);
            }
        }

        private void WriteOut(StreamWriter sw, ImplyEvaluation eval)
        {
            sw.WriteLine(string.Format("Formula: {0}"), eval.Imply.ToString());
            sw.WriteLine(string.Format("Message: {0}"), eval.Message);

            if (eval is ImplyEvaluationNode node)
            {
                foreach (ImplyEvaluation child in node.Evaluations)
                {
                    WriteOut(sw, child);
                }
            }

            if (eval is ImplyEvaluationLeaf leaf)
            {
                sw.WriteLine(string.Format("Result: {0}"), leaf.Result.ToString());
            }
        }

        #endregion
    }
}
