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

        protected Formula hypothesis;

        protected Formula consequence;

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
            StringBuilder formatString = new StringBuilder();

            bool noParentheses = hypothesis.HasIdentifier ||
                                 hypothesis is FALSE or TRUE or NotEvaluable or WeakestPrecondition;

            formatString.Append(noParentheses ? "{0} \\Longrightarrow " : "({0}) \\Longrightarrow ");

            noParentheses = consequence.HasIdentifier ||
                            consequence is FALSE or TRUE or NotEvaluable or WeakestPrecondition;

            formatString.Append(noParentheses ? "{1}" : "({1})");

            return string.Format(formatString.ToString(), hypothesis, consequence);
        }


        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public ImplyEvaluation Evaluated() => (hypothesis, consequence) switch
        {
            (FALSE or NotEvaluable, _)
                => new ImplyEvaluationLeaf(this, 
                   string.Format("Mivel \\( [ {0} ] = \\emptyset \\), ezért \\( {0} \\Longrightarrow {1} \\)", 
                       hypothesis, consequence),
                   ImplyEvaluationResult.True),

            (_, TRUE)
                => new ImplyEvaluationLeaf(this,
                   string.Format("Mivel \\( [ {0} ] \\subseteq [ {1} ] \\).", hypothesis, consequence),
                   ImplyEvaluationResult.True),

            (_, FALSE or NotEvaluable) 
                => new ImplyEvaluationLeaf(this,
                   string.Format("Mivel \\( [ {1} ] = \\emptyset \\), viszont \\( [ {0} ] \\neq \\emptyset \\).", hypothesis, consequence),
                   ImplyEvaluationResult.False),

            (TRUE, _)
                => new ImplyEvaluationLeaf(this,
                   string.Format("Mivel \\( [ {0} ] \\nsubseteq [ {1} ] \\).", hypothesis, consequence),
                   ImplyEvaluationResult.False),

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

            //if (hypothesisSimplified && !hypothesis.HasIdentifier)
            //{
            //    nextImply = new Imply(hypothesisEval, consequence);
            //    message   = "Egyszerűsítsd a baloldali részformulát.";

            //    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            //}

            //if (consequenceSimplified && !consequence.HasIdentifier)
            //{
            //    nextImply = new Imply(hypothesis, consequenceEval);
            //    message   = "Egyszerűsítsd a jobboldali részformulát.";

            //    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            //}

            if (hypothesisSimplified)
            {
                if (hypothesis.HasIdentifier)
                {
                    Formula nextHypothesis = hypothesis.DeepCopy();
                    nextHypothesis.Identifier = null;

                    message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\)", hypothesis, nextHypothesis);

                    nextImply = new Imply(nextHypothesis, consequence);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }

                nextImply = new Imply(hypothesisEval, consequence);
                message   = "Egyszerűsítsd a baloldali részformulát.";

                return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            }

            if (consequenceSimplified)
            {
                if (consequence.HasIdentifier)
                {
                    Formula nextConsequence = consequence.DeepCopy();
                    nextConsequence.Identifier = null;

                    message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\)", consequence, nextConsequence);

                    nextImply = new Imply(hypothesis, nextConsequence);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }

                nextImply = new Imply(hypothesis, consequenceEval);
                message   = "Egyszerűsítsd a jobboldali részformulát.";

                return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            }

            //if (hypothesisSimplified)
            //{
            //    if (hypothesisEval.HasIdentifier)
            //    {
            //        Formula nextHypothesis = hypothesisEval.DeepCopy();
            //        nextHypothesis.Identifier = null;

            //        message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\)", hypothesisEval, nextHypothesis);

            //        nextImply = new Imply(nextHypothesis, consequence);

            //        return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            //    }

            //    nextImply = new Imply(hypothesisEval, consequence);
            //    message   = "Egyszerűsítsd a baloldali részformulát.";

            //    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            //}

            //if (!hypothesis.Equals(hypothesisEval) || !consequence.Equals(consequenceEval))
            //{
            //    nextImply = new Imply(hypothesisEval, consequenceEval);
            //    message   = "Egyszerűsítsd a jobb és baloldali részformulákat.";

            //    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
            //}

            // Tovább nem egyszerűsíthető a program.

            if (hypothesis.Equivalent(consequence))
            {
                message = string.Format("Mivel \\( {0} \\) és \\( {1} \\) ekvivalensek.", hypothesis, consequence);

                return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
            }

            if (hypothesis.Implies(consequence))
            {
                message = string.Format("Mivel \\( {0} \\) maga után vonja \\( {1} \\) bekövetkezését.", hypothesis, consequence);

                return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
            }

            if (consequence is DisjunctionFormula disjunctionCons)
            {
                LinkedList<Formula> consequenceOperands = disjunctionCons.SimplifiedLinearOperands();

                if (consequenceOperands.Count != disjunctionCons.LinearOperands().Count)
                {
                    message = "Egyszerűsítsd a jobboldali részformulát.";

                    nextImply = new Imply(hypothesis, disjunctionCons.Simplified());

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }
            }

            if (hypothesis.HasIdentifier)
            {
                Formula nextHypothesis = hypothesis.DeepCopy();
                nextHypothesis.Identifier = null;

                message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\)", hypothesis, nextHypothesis);

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
                        message = string.Format("Mivel \\( {0} \\) és \\( {1} \\) ekvivalensek.", operand, consequence);

                        return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
                    }

                    if (operand.Implies(consequence))
                    {
                        message = string.Format("Mivel \\( {0} \\) maga után vonja \\( {1} \\) bekövetkezését.", operand, consequence);

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

                                message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\)", previous, operand);

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

                        //foreach (Formula rec in operands)
                        //{
                        //    if (rec.Equivalent(consequence) || rec.Implies(consequence))
                        //    {
                        //        Formula previous = operand.DeepCopy();

                        //        operand.Identifier = null;

                        //        message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\)", previous, operand);

                        //        nextImply = new Imply(conjunctionHypot.Binarize(hypothesisOperands), consequence);

                        //        return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                        //    }

                        //    if (rec is ConjunctionFormula conj && conj.HasIdentifier)
                        //    {
                        //        conj.Identifier = null;

                        //    }
                        //}
                    }
                }
            }

            if (consequence.HasIdentifier)
            {
                Formula nextConsequence = consequence.DeepCopy();
                nextConsequence.Identifier = null;

                message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\)", consequence, nextConsequence);

                nextImply = new Imply(hypothesis, nextConsequence);

                return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
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
                        message = string.Format("Mivel \\( {0} \\) és \\( {1} \\) ekvivalensek.", operand, consequence);

                        return new ImplyEvaluationLeaf(new Imply(this), message, ImplyEvaluationResult.True);
                    }

                    if (operand.Implies(consequence))
                    {
                        message = string.Format("Mivel \\( {0} \\) maga után vonja \\( {1} \\) bekövetkezését.", operand, consequence);

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

                //foreach (Formula operand in hypothesisOperands)
                //{
                //    if (operand is Equal<Logical> logicalEqual && !usedFormulas.Contains(logicalEqual))
                //    {
                //        Formula? substituted = logicalEqual.SubstituteVariable(consequence);

                //        if (substituted is not null)
                //        {
                //            message = string.Format("Helyettesítsünk: \\( {0} \\).", logicalEqual);

                //            usedFormulas.AddLast(logicalEqual);

                //            nextImply = new Imply(hypothesis, substituted, usedFormulas);

                //            return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                //        }
                //    }

                //    if (operand is Equal<IntegerType> integerTypeEqual && !usedFormulas.Contains(integerTypeEqual))
                //    {
                //        Formula? substituted = integerTypeEqual.SubstituteVariable(consequence);

                //        if (substituted is not null)
                //        {
                //            message = string.Format("Helyettesítsünk: \\( {0} \\).", integerTypeEqual);

                //            usedFormulas.AddLast(integerTypeEqual);

                //            nextImply = new Imply(hypothesis, substituted, usedFormulas);

                //            return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                //        }
                //    }
                //}

                if (hypothesisOperands.Count != conjunctionHypothesis.SimplifiedLinearOperands().Count)
                {
                    message = "Egyszerűsítsd a bal oldali részformulát.";

                    nextImply = new Imply(conjunctionHypothesis.Simplified(), consequence);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }

                foreach (Formula operand in hypothesisOperands)
                {
                    if (operand.HasIdentifier)
                    {
                        Formula previous = operand.DeepCopy();

                        operand.Identifier = null;

                        message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\)", previous, operand);

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
                    message = "Egyszerűsítsd a bal oldali részformulát.";

                    nextImply = new Imply(disjunctionHypothesis.Simplified(), consequence);

                    return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                }

                foreach (Formula operand in hypothesisOperands)
                {
                    if (operand.HasIdentifier)
                    {
                        Formula previous = operand.DeepCopy();

                        operand.Identifier = null;

                        message = string.Format("Helyettesítünk: \\( {0} = ({1}) \\)", previous, operand);

                        nextImply = new Imply(disjunctionHypothesis.Binarize(hypothesisOperands), consequence);

                        return new ImplyEvaluationNode(new Imply(this), message, nextImply.Evaluated());
                    }
                }
            }

            return new ImplyEvaluationLeaf(new Imply(this), "Nem sikerült.", ImplyEvaluationResult.Unverifiable);
        }

        public void Test()
        {
            ImplyEvaluation eval = Evaluated();

            string path = @"F:\ELTE\Programtervező informatika - BSc\6-félév\Szakdolgozat\SymbolicImplicationVerification\evalTest.txt";

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine();
                WriteOut(sw, eval);
            }
        }

        private void WriteOut(StreamWriter sw, ImplyEvaluation eval)
        {
            sw.WriteLine(string.Format("\\[ {0} \\]", eval.Imply.ToString()));
            sw.WriteLine(string.Format("Message: {0}", eval.Message));

            if (eval is ImplyEvaluationNode node)
            {
                foreach (ImplyEvaluation child in node.Evaluations)
                {
                    WriteOut(sw, child);
                }
            }

            if (eval is ImplyEvaluationLeaf leaf)
            {
                sw.WriteLine(string.Format("EvaluationResult: {0}", leaf.Result.ToString()));
            }
        }

        #endregion
    }
}
