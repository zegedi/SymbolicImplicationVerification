using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public class LessThan : BinaryRelationFormula<IntegerType>
    {
        #region Constructors

        public LessThan(LessThan lessThan) : base(
            lessThan.identifier,
            lessThan.leftComponent.DeepCopy(),
            lessThan.rightComponent.DeepCopy()) { }

        public LessThan(IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public LessThan(string? identifier, IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}<{1}", leftComponent.ToString(), rightComponent.ToString());
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
            LessThan other => IdenticalComponentsEquals(other),
                         _ => false
        };

        /// <summary>
        /// Determines whether the specified formula is equivalent to the current formula.
        /// </summary>
        /// <param name="other">The formula to compare with the current formula.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the formulas are the equivalent.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Equivalent(Formula other) => (Evaluated(), other.Evaluated()) switch
        {
            (LessThan thisEval, LessThan otherEval)
                => IdenticalSideRearrangementEquals(thisEval, otherEval),

            (LessThan thisEval, GreaterThan otherEval)
                => OppositeSideRearrangementEquals(thisEval, otherEval),

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
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            IntegerTypeTerm left =
                leftComponent is IntegerTypeBinaryOperationTerm leftOperation ?
                leftOperation.Simplified() : leftComponent.DeepCopy();

            IntegerTypeTerm right =
                rightComponent is IntegerTypeBinaryOperationTerm rightOperation ?
                rightOperation.Simplified() : rightComponent.DeepCopy(); 

            return (left, right) switch
            {
                (IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant) =>
                leftConstant.Value < rightConstant.Value? TRUE.Instance() : FALSE.Instance(),

                (_, _) => left.Equals(right) ? FALSE.Instance() : new LessThan(left, right)
            };
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new GreaterThanOrEqualTo(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current less than formula.
        /// </summary>
        /// <returns>The created deep copy of the less than formula.</returns>
        public override LessThan DeepCopy()
        {
            return new LessThan(this);
        }

        #endregion
    }
}
