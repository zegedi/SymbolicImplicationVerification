using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public class Divisor: BinaryRelationFormula<IntegerType>
    {
        #region Constructors

        public Divisor(Divisor divisor) : base(
            divisor.identifier, 
            divisor.leftComponent .DeepCopy(), 
            divisor.rightComponent.DeepCopy()) { }

        public Divisor(IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public Divisor(string? identifier, IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            return string.Format("{0} \\mid {1}", leftComponent, rightComponent);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj switch
        {
            Divisor other => IdenticalComponentsEquals(other),
                        _ => false
        };

        /// <summary>
        /// Determines whether the specified program is equivalent to the current program.
        /// </summary>
        /// <param name="other">The program to compare with the current program.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the formulas are the equivalent.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Equivalent(Formula other) => (Evaluated(), other.Evaluated()) switch
        {
            (Divisor         , Divisor otherEval) => IdenticalComponentsEquals(otherEval),
            (Formula thisEval, Formula otherEval) => thisEval.Equals(otherEval)
        };

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated() => (leftComponent.Evaluated(), rightComponent.Evaluated()) switch
        {
            (IntegerTypeConstant { Value: 0       }, _) => NotEvaluable.Instance(),
            (IntegerTypeConstant { Value: 1 or -1 }, _) => TRUE.Instance(),
            (_, IntegerTypeConstant { Value: 0 }      ) => TRUE.Instance(),

            (IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant) =>
                rightConstant.Value % leftConstant.Value == 0 ? TRUE.Instance() : FALSE.Instance(),

            (IntegerTypeTerm left, IntegerTypeTerm right) => 
                left.Equals(right) ? TRUE.Instance() : ReturnOrDeepCopy(new Divisor(left, right))
        };

        /// <summary>
        /// Calculate the conjuction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the conjunction.</param>
        /// <returns>The result of the conjunction.</returns>
        public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        {
            Func<BinaryRelationFormula<IntegerType>, BinaryRelationFormula<IntegerType>, Formula> AnyRearrangementEqualsConjuctionWith
                = (divisor, other) => NotEvaluable.Instance();

            Func<Formula, Formula, Formula?> IdenticalComponentsEquivalentConjunctionWith
                = (divisor, other) => other switch
                {
                    GreaterThan => FALSE.Instance(),
                    GreaterThanOrEqualTo geq => new IntegerTypeEqual(geq.LeftComponent .DeepCopy(),
                                                                     geq.RightComponent.DeepCopy()),
                    _ => null,
                };

            Func<Formula, Formula, Formula?> OppositeComponentsEquivalentConjunctionWith
                = (divisor, other) => other switch
                {
                    LessThan => FALSE.Instance(),
                    LessThanOrEqualTo leq => new IntegerTypeEqual(leq.LeftComponent .DeepCopy(),
                                                                  leq.RightComponent.DeepCopy()),
                    _ => null,
                };

            return ConjunctionWith(this, other, AnyRearrangementEqualsConjuctionWith,
                IdenticalComponentsEquivalentConjunctionWith, OppositeComponentsEquivalentConjunctionWith);
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new NotDivisor(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current divisor program.
        /// </summary>
        /// <returns>The created deep copy of the divisor program.</returns>
        public override Divisor DeepCopy()
        {
            return new Divisor(this);
        }

        #endregion
    }
}
