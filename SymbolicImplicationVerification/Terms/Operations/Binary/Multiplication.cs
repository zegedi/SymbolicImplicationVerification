using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations.Linear;
using SymbolicImplicationVerification.Types;
using System.Text;

namespace SymbolicImplicationVerification.Terms.Operations.Binary
{
    public class Multiplication : IntegerTypeBinaryOperationTerm
    {
        #region Constructors

        /// <summary>
        /// Constructor for the multiplication of two constant operands.
        /// </summary>
        /// <param name="leftConstant">The constant left operand of the multiplication.</param>
        /// <param name="rightConstant">The constant right operand of the multiplication.</param>
        public Multiplication(IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant)
            : this(leftConstant, rightConstant,
                   leftConstant.TermType.MultiplicationWithType(leftConstant.Value, rightConstant.Value)) { }

        /// <summary>
        /// Constructor for the multiplication, where the left operand is a constant.
        /// </summary>
        /// <param name="leftConstant">The constant left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        public Multiplication(IntegerTypeConstant leftConstant, IntegerTypeTerm rightOperand)
            : this(leftConstant, rightOperand, 
                   rightOperand.TermType.MultiplicationWithType(leftConstant.Value)) { }

        /// <summary>
        /// Constructor for the multiplication, where the right operand is a constant.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightConstant">The constant right operand of the multiplication.</param>
        public Multiplication(IntegerTypeTerm leftOperand, IntegerTypeConstant rightConstant)
            : this(leftOperand, rightConstant, 
                   leftOperand.TermType.MultiplicationWithType(rightConstant.Value)) { }

        public Multiplication(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand)
            : this(leftOperand, rightOperand, 
                   leftOperand.TermType.MultiplicationWithType(rightOperand.TermType)) { }

        public Multiplication(IntegerTypeTerm leftOperand, IntegerTypeTerm rightOperand, IntegerType termType)
            : base(leftOperand, rightOperand, termType) { }


        #endregion

        #region Public static operators

        /// <summary>
        /// Creates a new addition between a multiplication left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(Multiplication leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        ///// <summary>
        ///// Creates a new addition between a constant left and a multiplication right operand.
        ///// </summary>
        ///// <param name="leftOperand">The left operand of the addition.</param>
        ///// <param name="rightOperand">The right operand of the addition.</param>
        ///// <returns>The created addition instance.</returns>
        //public static Addition operator +(IntegerTypeConstant leftOperand, Multiplication rightOperand)
        //{
        //    return new Addition(leftOperand, rightOperand);
        //}

        /// <summary>
        /// Creates a new addition between a multiplication left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(Multiplication leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        ///// <summary>
        ///// Creates a new addition between an IntegerTypeTerm left and a multiplication right operand.
        ///// </summary>
        ///// <param name="leftOperand">The left operand of the addition.</param>
        ///// <param name="rightOperand">The right operand of the addition.</param>
        ///// <returns>The created addition instance.</returns>
        //public static Addition operator +(IntegerTpyeTerm leftOperand, Multiplication rightOperand)
        //{
        //    return new Addition(leftOperand, rightOperand);
        //}

        /// <summary>
        /// Creates a new subtraction between a multiplication left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(Multiplication leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        ///// <summary>
        ///// Creates a new subtraction between a constant left and a multiplication right operand.
        ///// </summary>
        ///// <param name="leftOperand">The left operand of the subtraction.</param>
        ///// <param name="rightOperand">The right operand of the subtraction.</param>
        ///// <returns>The created subtraction instance.</returns>
        //public static Subtraction operator -(IntegerTypeConstant leftOperand, Multiplication rightOperand)
        //{
        //    return new Subtraction(leftOperand, rightOperand);
        //}

        /// <summary>
        /// Creates a new subtraction between a multiplication left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(Multiplication leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        ///// <summary>
        ///// Creates a new subtraction between an IntegerTypeTerm left and a multiplication right operand.
        ///// </summary>
        ///// <param name="leftOperand">The left operand of the subtraction.</param>
        ///// <param name="rightOperand">The right operand of the subtraction.</param>
        ///// <returns>The created subtraction instance.</returns>
        //public static Subtraction operator -(IntegerTpyeTerm leftOperand, Multiplication rightOperand)
        //{
        //    return new Subtraction(leftOperand, rightOperand);
        //}

        /// <summary>
        /// Creates a new multiplication between a multiplication left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(Multiplication leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        ///// <summary>
        ///// Creates a new multiplication between a constant left and a multiplication right operand.
        ///// </summary>
        ///// <param name="leftOperand">The left operand of the multiplication.</param>
        ///// <param name="rightOperand">The right operand of the multiplication.</param>
        ///// <returns>The created multiplication instance.</returns>
        //public static Multiplication operator *(IntegerTypeConstant leftOperand, Multiplication rightOperand)
        //{
        //    return new Multiplication(leftOperand, rightOperand);
        //}

        /// <summary>
        /// Creates a new multiplication between a multiplication left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(Multiplication leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        ///// <summary>
        ///// Creates a new multiplication between an IntegerTypeTerm left and a multiplication right operand.
        ///// </summary>
        ///// <param name="leftOperand">The left operand of the multiplication.</param>
        ///// <param name="rightOperand">The right operand of the multiplication.</param>
        ///// <returns>The created multiplication instance.</returns>
        //public static Multiplication operator *(IntegerTpyeTerm leftOperand, Multiplication rightOperand)
        //{
        //    return new Multiplication(leftOperand, rightOperand);
        //}

        #endregion

        /*
        #region Public static operators

        public static Addition operator +(Multiplication multiplication, IntegerTpyeTerm term)
        {
            IntegerType resultType = multiplication.termType.AdditionWithType((dynamic)term.TermType);

            return new Addition(multiplication, term, resultType);
        }

        public static Subtraction operator -(Multiplication multiplication, IntegerTpyeTerm term)
        {
            IntegerType resultType = multiplication.termType.SubtractionWithType((dynamic)term.TermType);

            return new Subtraction(multiplication, term, resultType);
        }

        public static Multiplication operator *(Multiplication multiplication, IntegerTpyeTerm term)
        {
            IntegerType resultType = multiplication.termType.MultiplicationWithType((dynamic)term.TermType);

            return new Multiplication(multiplication, term, resultType);
        }

        #endregion
        */

        #region Public methods

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (leftOperand is IntegerTypeConstant { Value: -1 })
            {
                stringBuilder.Append('-');
            }
            else
            {
                string formatString = leftOperand is Addition or Subtraction ? "({0})*" : "{0}*";

                stringBuilder.AppendFormat(formatString, leftOperand.ToString());
            }

            if (rightOperand is IntegerTypeBinaryOperationTerm)
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
                   obj is Multiplication multiplication &&
                   leftOperand  is IMatch leftPattern   &&
                   rightOperand is IMatch rightPattern  &&
                   leftPattern .Matches(multiplication.leftOperand) &&
                   rightPattern.Matches(multiplication.rightOperand);
        }

        public override IntegerTypeTerm Evaluated(IntegerTypeTerm left, IntegerTypeTerm right)
        {
            const int additiveNeutralElement = 0;
            const int multiplicativeNeutralElement = 1;

            return (left, right) switch
            {
                (IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant)
                    => new IntegerConstant(leftConstant.Value * rightConstant.Value),

                (IntegerTypeConstant constant, _) => constant.Value switch
                {
                    multiplicativeNeutralElement => right,
                          additiveNeutralElement => new IntegerConstant(additiveNeutralElement),
                                               _ => new Multiplication(constant, right)
                },

                (_, IntegerTypeConstant constant) => constant.Value switch
                {
                    multiplicativeNeutralElement => left,
                          additiveNeutralElement => new IntegerConstant(additiveNeutralElement),
                                               _ => new Multiplication(constant, left)
                },

                (_, _) => new Multiplication(left, right)
            };
        }

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

                if (nextInProcess is Addition addition)
                {
                    operandList.AddLast(addition.Linearized());
                }
                else if (nextInProcess is IntegerTypeBinaryOperationTerm multiplication)
                {
                    unprocessed.AddLast(multiplication.LeftOperand);
                    unprocessed.AddLast(multiplication.RightOperand);
                }
                else
                {
                    operandList.AddLast(nextInProcess);
                }
            }

            return new LinearMultiplication(operandList, Type.DeepCopy(subtractionsConverted.TermType));
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
        /// Creates a simplified version of the multiplication.
        /// </summary>
        /// <returns>The simplified version of the multiplication.</returns>
        public override IntegerTpyeTerm Simplified()
        {
            return Evaluated(
                (left, right) => new IntegerConstant(left.Value * right.Value),
                (left, right) =>
                {
                    const int neutralValue = 1;

                    if (left is IntegerTypeConstant leftConstant &&
                        leftConstant.Value == neutralValue)
                    {
                        return right;
                    }

                    if (right is IntegerTypeConstant rightConstant &&
                        rightConstant.Value == neutralValue)
                    {
                        return left;
                    }

                    return new Multiplication(left, right);
                }
            );
        }
        */

        #endregion
    }
}
