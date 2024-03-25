using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public class NotDivisor : BinaryRelationFormula<IntegerType>
    {
        #region Constructors

        public NotDivisor(NotDivisor notDivisor) : base(
            notDivisor.identifier,
            notDivisor.leftComponent.DeepCopy(),
            notDivisor.rightComponent.DeepCopy()) { }

        public NotDivisor(IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public NotDivisor(string? identifier, IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}!|{1}", leftComponent.ToString(), rightComponent.ToString());
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
            NotDivisor other => IdenticalComponentsEquals(other),
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
            (NotDivisor      , NotDivisor otherEval) => IdenticalComponentsEquals(otherEval),
            (Formula thisEval, Formula    otherEval) => thisEval.Equals(otherEval)
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
        /// <exception cref="InvalidOperationException">If the left component is zero.</exception>
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
                (IntegerTypeConstant { Value: 0       }, _) => NotEvaluable.Instance(),
                (IntegerTypeConstant { Value: 1 or -1 }, _) => FALSE.Instance(),
                (_ , IntegerTypeConstant { Value: 0 })      => FALSE.Instance(),

                (IntegerTypeConstant leftConstant, IntegerTypeConstant rightConstant) =>
                rightConstant.Value % leftConstant.Value != 0 ? TRUE.Instance() : FALSE.Instance(),

                (_, _) => left.Equals(right) ? FALSE.Instance() : new NotDivisor(left, right)
            };
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new Divisor(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current not divisor formula.
        /// </summary>
        /// <returns>The created deep copy of the not divisor formula.</returns>
        public override NotDivisor DeepCopy()
        {
            return new NotDivisor(this);
        }

        #endregion
    }
}
