using SymbolicImplicationVerification.Types;
using SymbolicImplicationVerification.Terms.Constants;
using System.ComponentModel.Design;
using SymbolicImplicationVerification.Terms.Operations.Linear;
using SymbolicImplicationVerification.Terms.Patterns;
using SymbolicImplicationVerification.Evaluations;
using System.Numerics;
using System.Text;

namespace SymbolicImplicationVerification.Terms.Operations.Binary
{
    public class Addition : IntegerTypeBinaryOperationTerm
    {
        #region Constructors

        /// <summary>
        /// Constructor for the leftOperand of two rightOperand operands.
        /// </summary>
        /// <param name="leftConstant">The rightOperand left operand of the leftOperand.</param>
        /// <param name="rightConstant">The rightOperand right operand of the leftOperand.</param>
        public Addition(IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant)
            : this(leftConstant, rightConstant,
                   leftConstant.TermType.AdditionWithType(leftConstant.Value, rightConstant.Value)) { }

        /// <summary>
        /// Constructor for the leftOperand, where the left operand is a rightOperand.
        /// </summary>
        /// <param name="leftConstant">The rightOperand left operand of the leftOperand.</param>
        /// <param name="rightOperand">The right operand of the leftOperand.</param>
        public Addition(IntegerTypeConstant leftConstant, IntegerTypeTerm rightOperand)
            : this(leftConstant, rightOperand, rightOperand.TermType.AdditionWithType(leftConstant.Value)) { }

        /// <summary>
        /// Constructor for the leftOperand, where the right operand is a rightOperand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the leftOperand.</param>
        /// <param name="rightConstant">The rightOperand right operand of the leftOperand.</param>
        public Addition(IntegerTypeTerm leftOperand, IntegerTypeConstant rightConstant)
            : this(leftOperand, rightConstant, leftOperand.TermType.AdditionWithType(rightConstant.Value)) { }

        public Addition(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand)
            : this(leftOperand, rightOperand, leftOperand.TermType.AdditionWithType(rightOperand.TermType)) { }

        public Addition(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand, IntegerType termType)
            : base(leftOperand, rightOperand, termType) { }

        #endregion

        #region Public static operators

        /// <summary>
        /// Creates a new addition between an addition left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(Addition leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new addition between an addition left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(Addition leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between an addition left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(Addition leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between an addition left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(Addition leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new multiplication between an addition left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(Addition leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new multiplication between an addition left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(Addition leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("{0}+", leftOperand.ToString());

            if (rightOperand is Addition or Subtraction)
            {
                stringBuilder.AppendFormat("({0})", rightOperand.ToString());
            }
            else
            {
                stringBuilder.Append(rightOperand.ToString());
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Determines wheter the given <see cref="object"/> matches the pattern.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to match against the pattern.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the <see cref="object"/> matches the pattern.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Matches(object? obj)
        {
            return obj is not null &&
                   obj is Addition addition &&
                   leftOperand  is IMatch leftPattern  &&
                   rightOperand is IMatch rightPattern &&
                   leftPattern .Matches(addition.leftOperand) &&
                   rightPattern.Matches(addition.rightOperand);
        }

        public override IntegerTypeTerm Evaluated(IntegerTypeTerm left, IntegerTypeTerm right)
        {
            const int additiveNeutralElement = 0;
            const int additiveInverseOfOne = -1;

            return (left, right) switch
            {
                (IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant) 
                    => new IntegerConstant(leftConstant.Value + rightConstant.Value),

                (IntegerTypeConstant constant, _) => constant.Value switch
                {
                    < additiveNeutralElement => new Subtraction(right, new IntegerConstant(-1 * constant.Value)),
                    > additiveNeutralElement => new Addition(right, constant),
                                           _ => right
                },

                (_, Multiplication multip) => multip.LeftOperand switch
                {
                    IntegerTypeConstant { Value: additiveInverseOfOne } =>
                        new Subtraction(left, multip.RightOperand),

                    IntegerTypeConstant { Value: < additiveInverseOfOne } constant =>
                         new Subtraction(left, (IntegerConstant)(-1 * constant.Value) * multip.RightOperand),

                    _ => new Addition(left, multip)
                },

                (_, IntegerTypeConstant constant) => constant.Value switch
                {
                    < additiveNeutralElement => new Subtraction(left, new IntegerConstant(-1 * constant.Value)),
                    > additiveNeutralElement => new Addition(left, constant),
                                           _ => left
                },

                (_, _) => new Addition(left, right)
            };
        }

        public override IntegerTypeTerm Simplified()
        {
            IntegerTypeTerm result = Evaluated();

            if (result is IntegerTypeBinaryOperationTerm operation)
            {
                IntegerTypeLinearOperationTerm linearized = operation.Linearized();

                result = linearized.Process();

                result = PatternReplacer.PatternsApplied(result, Evaluations.Patterns.CollapseGroups);

                if (result is IntegerTypeBinaryOperationTerm operationTerm)
                {
                    result = operationTerm.Evaluated();

                    result = PatternReplacer.PatternsApplied(result, Evaluations.Patterns.LeftAssociateRules);
                }
            }

            return result;
        }

        /*
        /// <summary>
        /// Creates a simplified version of the leftOperand.
        /// </summary>
        /// <returns>The simplified version of the leftOperand.</returns>
        public override IntegerTpyeTerm Evaluated()
        {
            return Evaluated(
                (left, right) => new IntegerConstant(left.Value + right.Value),
                (left, right) =>
                {
                    const int additionNeutralElement = 0;

                    if (left is IntegerTypeConstant leftConstant)
                    {
                        if (leftConstant.Value == additionNeutralElement)
                        {
                            return right;
                        }

                        return new Addition(leftConstant, right);
                    }

                    if (right is IntegerTypeConstant rightConstant)
                    {
                        if (rightConstant.Value == additionNeutralElement)
                        {
                            return left;
                        }

                        return new Addition(left, rightConstant);
                    }

                    return new Addition(left, right);
                }
            );
        }
        */

        public override IntegerTypeLinearOperationTerm Linearized()
        {
            // Expand all parenthesis.
            IntegerTypeTerm expanded 
                = PatternReplacer.PatternsApplied(this, Evaluations.Patterns.ExpandRules);

            // Convert all subtractions into multiplications and additions.
            IntegerTypeTerm subtractionsConverted
                = PatternReplacer.PatternsApplied(expanded, Evaluations.Patterns.ConvertSubtractions);

            // Create the "queue" of unprocessed terms and list of operands.
            LinkedList<IntegerTypeTerm> unprocessed = new LinkedList<IntegerTypeTerm>();
            LinkedList<IntegerTypeTerm> operandList = new LinkedList<IntegerTypeTerm>();

            unprocessed.AddLast(subtractionsConverted);

            while (unprocessed.Count > 0)
            {
                IntegerTypeTerm nextInProcess = unprocessed.First();
                unprocessed.RemoveFirst();

                if (nextInProcess is Multiplication multiplication)
                {
                    operandList.AddLast(multiplication.Linearized());
                }
                else if (nextInProcess is IntegerTypeBinaryOperationTerm addition)
                {
                    unprocessed.AddLast(addition.LeftOperand);
                    unprocessed.AddLast(addition.RightOperand);
                }
                else
                {
                    operandList.AddLast(nextInProcess);
                }
            }

            return new LinearAddition(operandList, Type.DeepCopy(subtractionsConverted.TermType));
        }

        public void ProcessNext(IntegerTypeTerm nextInProcess, 
            LinkedList<IntegerTypeTerm> unprocessed, LinkedList<IntegerTypeTerm> operandList)
        {
            if (nextInProcess is Multiplication multiplication)
            {
                var linearMuliplication = multiplication.Linearized();

                foreach (var operands in linearMuliplication.OperandList)
                {
                    operandList.AddLast(operands);
                }
            }
            else if (nextInProcess is IntegerTypeBinaryOperationTerm addition)
            {
                unprocessed.AddLast(addition.LeftOperand);
                unprocessed.AddLast(addition.RightOperand);
            }
            else
            {
                operandList.AddLast(nextInProcess);
            }
        }


        /*
        public Addition Expand()
        {
            // Expand the left operation if it's an expression.
            IntegerTpyeTerm leftExpanded = leftConstant is IExpression<IntegerTypeBinaryOperationTerm> left ? left.Expand() : leftConstant;

            // Expand the right operation if it's an expression.
            IntegerTpyeTerm rightExpanded = rightConstant is IExpression<IntegerTypeBinaryOperationTerm> right ? right.Expand() : rightConstant;

            IntegerType additionTermType = leftConstant.TermType.AdditionWithType((dynamic)rightConstant.TermType);

            return new Addition(leftExpanded, rightExpanded, additionTermType);
        }
        */

        /*
        public LinearAddition LinearizeAddition()
        {
            LinkedList<Term<IntegerType>> linearOperands = new LinkedList<Term<IntegerType>>();

            LinkedList<(bool invertRightOperand, Term<IntegerType> term)> unprocessedOperands = new LinkedList<(bool, Term<IntegerType>)>();
            unprocessedOperands.AddLast((false, leftConstant));
            unprocessedOperands.AddLast((false, rightConstant));

            while (unprocessedOperands.Count > 0)
            {
                (bool invertRightOperand, Term<IntegerType> term) nextOperand = unprocessedOperands.First!.Value;
                unprocessedOperands.RemoveFirst();

                if (nextOperand.term is Addition asd || nextOperand.term is Subtraction)
                {
                    unprocessedOperands.AddLast((false, nextOperand.term.L));
                    unprocessedOperands.AddLast((false, rightConstant));
                }

                if (nextOperand.term is IntegerTypeBinaryOperationTerm)
                {
                    // linearizáljuk
                }
                else
                {
                    if (nextOperand.term is IntegerTypeConstant<IntegerType>)
                    {

                    }
                }
            }
        }
        */

        #endregion
    }
}