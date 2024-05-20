using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;
using System.Text;

namespace SymbolicImplicationVerification.Implies
{
    public class Imply : IEvaluable<ImplyEvaluation>
    {
        #region Fields

        /// <summary>
        /// The hypothesis of the imply.
        /// </summary>
        protected Formula hypothesis;

        /// <summary>
        /// The consequence of the imply.
        /// </summary>
        protected Formula consequence;

        /// <summary>
        /// The list of used formulas.
        /// </summary>
        protected LinkedList<Formula> usedFormulas;

        #endregion

        #region Constructors

        public Imply(Imply imply) : this(imply.hypothesis.DeepCopy(), imply.consequence.DeepCopy()) { }

        public Imply(Formula hypothesis, Formula consequence)
            : this(hypothesis, consequence, new LinkedList<Formula>()) { }

        public Imply(Formula hypothesis, Formula consequence, LinkedList<Formula> usedFormulas)
        {
            this.hypothesis   = hypothesis;
            this.consequence  = consequence;
            this.usedFormulas = new LinkedList<Formula>();

            foreach (Formula used in usedFormulas)
            {
                this.usedFormulas.AddLast(used.DeepCopy());
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the hypothesis of the imply.
        /// </summary>
        public Formula Hypothesis
        {
            get { return hypothesis; }
            set { hypothesis = value; }
        }

        /// <summary>
        /// Gets or sets the consequence of the imply.
        /// </summary>
        public Formula Consequence
        {
            get { return consequence; }
            set { consequence = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is Imply other &&
                   hypothesis.Equals(other.hypothesis) &&
                   consequence.Equals(other.consequence);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            bool noParentheses = hypothesis.HasIdentifier ||
                                 hypothesis is FALSE or TRUE or NotEvaluable or WeakestPrecondition;

            result.AppendFormat(noParentheses ? "\\imply{{{0}}}" : "\\imply{{({0})}}", hypothesis);

            noParentheses = consequence.HasIdentifier ||
                            consequence is FALSE or TRUE or NotEvaluable or WeakestPrecondition;

            result.AppendFormat(noParentheses ? "{{{0}}}" : "{{({0})}}", consequence);

            return result.ToString();
        }


        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public ImplyEvaluation Evaluated() => (hypothesis, consequence) switch
        {
            (FALSE or NotEvaluable, _)
                => new ImplyEvaluationLeaf(this, 
                   @$"Mivel a \( {hypothesis} \) igazsághalmaza üres, ezért teljesül az implikáció.",
                   ImplyEvaluationResult.True),

            (_, TRUE)
                => new ImplyEvaluationLeaf(this,
                   @$"Mivel az \( {consequence} \) minden állapotban igaz, ezért \( {hypothesis} \) bekövetkezése mindig maga után vonja \( {consequence} \) bekövetkezését is.",
                   ImplyEvaluationResult.True),

            (_, _) => EvaluationAlgorithm()
        };

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public ImplyEvaluation EvaluationAlgorithm()
        {
            Formula hypothesisEval  = hypothesis .Evaluated();
            Formula consequenceEval = consequence.Evaluated();

            Imply nextImply;
            string message;

            bool hypothesisSimplified  = !hypothesis .Equals(hypothesisEval);
            bool consequenceSimplified = !consequence.Equals(consequenceEval);

            if (hypothesisSimplified)
            {
                if (hypothesis.HasIdentifier)
                {
                    Formula nextHypothesis = hypothesis.DeepCopy();
                    nextHypothesis.Identifier = null;

                    message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\).", hypothesis, nextHypothesis);

                    nextImply = new Imply(nextHypothesis, consequence);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }

                nextImply = new Imply(hypothesisEval, consequence);
                message   = "Egyszerűsítve a hipotézist.";

                return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            }

            if (consequenceSimplified)
            {
                if (consequence.HasIdentifier)
                {
                    Formula nextConsequence = consequence.DeepCopy();
                    nextConsequence.Identifier = null;

                    message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\).", consequence, nextConsequence);

                    nextImply = new Imply(hypothesis, nextConsequence);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }

                nextImply = new Imply(hypothesis, consequenceEval);
                message   = "Egyszerűsítve a következményt.";

                return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            }

            if (consequence is FALSE or NotEvaluable)
            {
                return new ImplyEvaluationLeaf(
                    new Imply(this),
                    @$"Mivel \( {consequence} \) igazsághalmaza üres, következésképpen \( {hypothesis} \) igazsághalmaza is üres kell, hogy legyen. Ugyanakkor az utóbbi feltevés kielégíthető, ezért az implikáció nem teljesül.",
                    ImplyEvaluationResult.False
                );
            }

            if (hypothesis is TRUE)
            {
                return new ImplyEvaluationLeaf(
                    new Imply(this),
                    @$"Mivel az \( {hypothesis} \) minden állapotban igaz, következésképpen a \( {consequence} \) állításnak is mindig teljesülnie kéne. Ugyanakkor az utóbbi következmény nem áll fenn minden esetben, ezért az implikáció nem teljesül.",
                    ImplyEvaluationResult.False
                );
            }


            if (hypothesis.Equivalent(consequence))
            {
                message = string.Format("Mivel \\( {0} \\) és \\( {1} \\) ekvivalensek, ezért teljesül az implikáció.", hypothesis, consequence);

                return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
            }

            if (hypothesis.Implies(consequence))
            {
                message = string.Format("Mivel \\( {0} \\) maga után vonja \\( {1} \\) bekövetkezését, ezért az állítás igaz.", hypothesis, consequence);

                return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
            }

            if (consequence is DisjunctionFormula disjunctionCons)
            {
                LinkedList<Formula> consequenceOperands = disjunctionCons.SimplifiedLinearOperands();

                if (consequenceOperands.Count != disjunctionCons.LinearOperands().Count)
                {
                    message = "Egyszerűsítve a következményt.";

                    nextImply = new Imply(hypothesis, disjunctionCons.Simplified());

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }
            }

            if (hypothesis.HasIdentifier)
            {
                Formula nextHypothesis = hypothesis.DeepCopy();
                nextHypothesis.Identifier = null;

                message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\).", hypothesis, nextHypothesis);

                nextImply = new Imply(nextHypothesis, consequence);

                return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            }

            if (hypothesis is ConjunctionFormula conjunctionHypot)
            {
                LinkedList<Formula> hypothesisOperands = conjunctionHypot.LinearOperands();

                foreach (Formula operand in hypothesisOperands)
                {
                    if (operand.Equivalent(consequence))
                    {
                        message = string.Format("Mivel \\( {0} \\) és \\( {1} \\) ekvivalensek, ezért teljesül az implikáció.", operand, consequence);

                        return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
                    }

                    if (operand.Implies(consequence))
                    {
                        message = string.Format("Mivel \\( {0} \\) maga után vonja \\( {1} \\) bekövetkezését, ezért az állítás igaz.", operand, consequence);

                        return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
                    }

                    if (operand is ConjunctionFormula conjunctionOperand && operand.HasIdentifier)
                    {
                        ConjunctionFormula noIdentifier = conjunctionOperand.DeepCopy();
                        noIdentifier.Identifier = null;

                        LinkedList<Formula> operands = noIdentifier.LinearOperands();

                        LinkedListNode<Formula>? current = operands.First;

                        while (current is not null)
                        {
                            if (current.Value.Equivalent(consequence) || current.Value.Implies(consequence))
                            {
                                Formula previous = operand.DeepCopy();

                                operand.Identifier = null;

                                message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\).", previous, operand);

                                nextImply = new Imply(conjunctionHypot.Binarize(hypothesisOperands), consequence);

                                return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                            }

                            if (current.Value is ConjunctionFormula conj && conj.HasIdentifier)
                            {
                                conj.Identifier = null;

                                foreach (Formula formula in conj.LinearOperands())
                                {
                                    operands.AddLast(formula);
                                }
                            }

                            current = current.Next;
                        }
                    }
                }
            }

            if (consequence.HasIdentifier)
            {
                Formula nextConsequence = consequence.DeepCopy();
                nextConsequence.Identifier = null;

                message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\).", consequence, nextConsequence);

                nextImply = new Imply(hypothesis, nextConsequence);

                return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            }

            if (consequence is ConjunctionFormula conjunctionConsequence)
            {
                LinkedList<Formula> consequenceOperands = conjunctionConsequence.LinearOperands();

                message = "Mivel a jobb oldal konjunkció, ezért a következmény minden részformulájának következnie kell a feltevésből.";

                return new ImplyEvaluationNode(new Imply(this), message, hypothesis, consequenceOperands);
            }

            if (consequence is DisjunctionFormula disjunctionConsequence)
            {
                LinkedList<Formula> consequenceOperands = disjunctionConsequence.SimplifiedLinearOperands();

                if (consequenceOperands.Count != disjunctionConsequence.LinearOperands().Count)
                {
                    message = "Egyszerűsítve a következményt.";

                    nextImply = new Imply(hypothesis, disjunctionConsequence.Simplified());

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }
            }

            // Típusinformáció alapján igazolunk.

            if (consequence is BinaryRelationFormula<IntegerType> relation)
            {
                IntegerTypeTerm left = relation.LeftComponent;
                IntegerTypeTerm right = relation.RightComponent;

                Formula leftComponentConstraint = left.TermType.TypeConstraintOn(left);
                Formula rightComponentConstraint = right.TermType.TypeConstraintOn(right);

                Formula constraint = (leftComponentConstraint & rightComponentConstraint).Evaluated();

                if (constraint.Equivalent(consequence) || constraint.Implies(consequence))
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    if (left is not IntegerTypeConstant)
                    {
                        stringBuilder.AppendFormat("\\( {0} \\in {1} \\)", left, left.TermType);
                    }

                    if (right is not IntegerTypeConstant)
                    {
                        stringBuilder.AppendFormat(
                            stringBuilder.Length > 0 ? " és \\( {0} \\in {1} \\)" : "\\( {0} \\in {1} \\)",
                            right, right.TermType);
                    }

                    message = string.Format("Mivel {0}, ezért \\( {1} \\) miatt teljesül az állítás.",
                        stringBuilder.ToString(), constraint);

                    return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
                }
            }

            if (hypothesis is ConjunctionFormula conjunctionHypothesis)
            {
                LinkedList<Formula> hypothesisOperands = conjunctionHypothesis.LinearOperands();

                foreach (Formula operand in hypothesisOperands)
                {
                    if (operand.Equivalent(consequence))
                    {
                        message = string.Format("Mivel \\( {0} \\) és \\( {1} \\) ekvivalensek, ezért teljesül az implikáció.", operand, consequence);

                        return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
                    }

                    if (operand.Implies(consequence))
                    {
                        message = string.Format("Mivel \\( {0} \\) maga után vonja \\( {1} \\) bekövetkezését, ezért az állítás igaz.", operand, consequence);

                        return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
                    }
                }

                (int score, Formula? equal, Formula? result) bestEqual = (-1, null, null);

                foreach (Formula operand in hypothesisOperands)
                {
                    if (operand.HasIdentifier)
                    {
                        continue;
                    }

                    if (operand is Equal<Logical> logicalEqual && !usedFormulas.Contains(logicalEqual))
                    {
                        Formula? substituted = logicalEqual.SubstituteVariable(consequence);

                        int score = (logicalEqual.LeftComponent is LogicalConstant  ? 2 :
                                    logicalEqual.LeftComponent  is Variable<Logical> ? 1 : 3)
                                    +
                                    (logicalEqual.RightComponent is LogicalConstant  ? 2 :
                                    logicalEqual.RightComponent  is Variable<Logical> ? 1 : 3);

                        bool change = substituted is not null &&
                                     (bestEqual.equal is null || bestEqual.score < score);

                        if (change)
                        {
                            bestEqual = (score, logicalEqual, substituted);
                        }
                    }

                    if (operand is IntegerTypeEqual integerTypeEqual && !usedFormulas.Contains(integerTypeEqual))
                    {
                        Formula? substituted = integerTypeEqual.SubstituteVariable(consequence);

                        int score = (integerTypeEqual.LeftComponent is IntegerTypeConstant ? 2 :
                                    integerTypeEqual.LeftComponent is Variable<IntegerType> ? 1 : 3)
                                    +
                                    (integerTypeEqual.RightComponent is IntegerTypeConstant ? 2 :
                                    integerTypeEqual.RightComponent is Variable<IntegerType> ? 1 : 3);

                        bool change = substituted is not null &&
                                     (bestEqual.equal is null || bestEqual.score < score);

                        if (change)
                        {
                            bestEqual = (score, integerTypeEqual, substituted);
                        }
                    }
                }

                if (bestEqual.equal is not null && bestEqual.result is not null)
                {
                    message = string.Format("Helyettesítsünk: \\( {0} \\).", bestEqual.equal);

                    usedFormulas.AddLast(bestEqual.equal);

                    nextImply = new Imply(hypothesis, bestEqual.result, usedFormulas);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }

                if (hypothesisOperands.Count != conjunctionHypothesis.SimplifiedLinearOperands().Count)
                {
                    message = "Egyszerűsítve a hipotézist.";

                    nextImply = new Imply(conjunctionHypothesis.Simplified(), consequence);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }

                foreach (Formula operand in hypothesisOperands)
                {
                    if (operand.HasIdentifier)
                    {
                        Formula previous = operand.DeepCopy();

                        operand.Identifier = null;

                        message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\).", previous, operand);

                        nextImply = new Imply(conjunctionHypothesis.Binarize(hypothesisOperands), consequence);

                        return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                    }
                }
            }

            if (hypothesis is DisjunctionFormula disjunctionHypothesis)
            {
                LinkedList<Formula> hypothesisOperands = disjunctionHypothesis.LinearOperands();

                if (hypothesisOperands.Count != disjunctionHypothesis.SimplifiedLinearOperands().Count)
                {
                    message = "Egyszerűsítve a hipotézist.";

                    nextImply = new Imply(disjunctionHypothesis.Simplified(), consequence);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }

                foreach (Formula operand in hypothesisOperands)
                {
                    if (operand.HasIdentifier)
                    {
                        Formula previous = operand.DeepCopy();

                        operand.Identifier = null;

                        message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\).", previous, operand);

                        nextImply = new Imply(disjunctionHypothesis.Binarize(hypothesisOperands), consequence);

                        return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                    }
                }
            }

            if (hypothesis is Equal<Logical> logicalEqual2 && !usedFormulas.Contains(logicalEqual2))
            {
                Formula? substituted = logicalEqual2.SubstituteVariable(consequence);

                if (substituted is not null)
                {
                    usedFormulas.AddLast(logicalEqual2);

                    nextImply = new Imply(logicalEqual2, substituted, usedFormulas);

                    message = string.Format("Helyettesítünk: \\( {0} \\).", logicalEqual2);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }
            }

            if (hypothesis is IntegerTypeEqual integerTypeEqual2 && !usedFormulas.Contains(integerTypeEqual2))
            {
                Formula? substituted = integerTypeEqual2.SubstituteVariable(consequence);
                
                if (substituted is not null)
                {
                    usedFormulas.AddLast(integerTypeEqual2);

                    nextImply = new Imply(integerTypeEqual2, substituted, usedFormulas);

                    message = string.Format("Helyettesítünk: \\( {0} \\).", integerTypeEqual2);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }
            }

            return new ImplyEvaluationLeaf(new Imply(this), "Sajnos nem tudjuk igazolni vagy cáfolni a kívánt állítást.", ImplyEvaluationResult.Unverifiable);
        }

        #endregion
    }
}
