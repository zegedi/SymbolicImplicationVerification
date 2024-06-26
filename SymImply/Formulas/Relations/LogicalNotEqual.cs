﻿using SymImply.Terms.Constants;
using SymImply.Types;
using SymImply.Terms;
using SymImply.Terms.Variables;
using System.Text;

namespace SymImply.Formulas.Relations
{
    public class LogicalNotEqual : NotEqual<Logical>
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
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            StringBuilder formatString = new StringBuilder();

            bool noLeftParenthesis  = leftComponent  is Variable<Logical> or LogicalConstant;
            bool noRightParenthesis = rightComponent is Variable<Logical> or LogicalConstant;

            formatString.Append(noLeftParenthesis  ? "{0} \\neq " : "({0}) \\neq ");
            formatString.Append(noRightParenthesis ? "{1}" : "({1})");

            return string.Format(formatString.ToString(), leftComponent, rightComponent);
        }

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
            (LogicalNotEqual, LogicalNotEqual otherEval)
                => IdenticalOrOppositeComponentsEquals(otherEval),

            (Formula thisEval, Formula otherEval) => thisEval.Equals(otherEval)
        };


        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated() => (leftComponent, rightComponent) switch
        {
            (FormulaTerm { Formula: NotEvaluable }, _) => NotEvaluable.Instance(),
            (_, FormulaTerm { Formula: NotEvaluable }) => NotEvaluable.Instance(),

            (LogicalConstant leftConstant, LogicalConstant rightConstant) =>
                leftConstant.Value != rightConstant.Value ? TRUE.Instance() : FALSE.Instance(),

            (_, _) => leftComponent.Equals(rightComponent) ? FALSE.Instance() :
                      ReturnOrDeepCopy(new LogicalEqual(leftComponent.Evaluated(), rightComponent.Evaluated()))
        };

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new LogicalEqual(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current notEqual program.
        /// </summary>
        /// <returns>The created deep copy of the notEqual program.</returns>
        public override LogicalNotEqual DeepCopy()
        {
            return new LogicalNotEqual(this);
        }

        #endregion
    }
}
