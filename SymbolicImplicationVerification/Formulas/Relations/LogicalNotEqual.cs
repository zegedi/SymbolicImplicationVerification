﻿using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public class LogicalNotEqual : Equal<Logical>
    {
        #region Constructors

        public LogicalNotEqual(LogicalNotEqual notEqual) : base(
            notEqual.identifier,
            notEqual.leftComponent .DeepCopy(),
            notEqual.rightComponent.DeepCopy()) { }

        public LogicalNotEqual(LogicalTerm leftComponent, LogicalTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public LogicalNotEqual(
            string? identifier, LogicalTerm leftComponent, LogicalTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the specified object is notEqual to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is notEqual to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj switch
        {
            LogicalNotEqual other => IdenticalComponentsEquals(other),
                                _ => false
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
            (LogicalNotEqual, LogicalNotEqual otherEval)
                => IdenticalOrOppositeComponentsEquals(otherEval),

            (Formula thisEval, Formula otherEval) => thisEval.Equals(otherEval)
        };


        /// <summary>
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            LogicalTerm left =
                leftComponent is FormulaTerm leftFormula ?
                leftFormula.Evaluated() : leftComponent.DeepCopy();

            LogicalTerm right =
                rightComponent is FormulaTerm rightFormula ?
                rightFormula.Evaluated() : rightComponent.DeepCopy();

            return (left, right) switch
            {
                (FormulaTerm { Formula: NotEvaluable }, _) => NotEvaluable.Instance(),
                (_, FormulaTerm { Formula: NotEvaluable }) => NotEvaluable.Instance(),

                (LogicalConstant leftConstant, LogicalConstant rightConstant) =>
                leftConstant.Value != rightConstant.Value ? TRUE.Instance() : FALSE.Instance(),

                (_, _) => left.Equals(right) ? FALSE.Instance() : new LogicalEqual(left, right)
            };
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new LogicalEqual(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current notEqual formula.
        /// </summary>
        /// <returns>The created deep copy of the notEqual formula.</returns>
        public override LogicalNotEqual DeepCopy()
        {
            return new LogicalNotEqual(this);
        }

        #endregion
    }
}