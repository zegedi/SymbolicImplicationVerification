﻿using SymImply.Formulas.Operations;
using SymImply.Formulas.Relations;
using SymImply.Terms.Constants;
using SymImply.Terms.Operations.Binary;
using SymImply.Types;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SymImply.Formulas
{
    public abstract class Formula : IEvaluable<Formula>, IDeepCopy<Formula>
    {
        #region Fields

        /// <summary>
        /// The identifier of the formula.
        /// </summary>
        protected string? identifier;

        #endregion

        #region Constructors

        public Formula(string? identifier)
        {
            this.identifier = identifier;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines if the formula has an indentifier or not.
        /// </summary>
        public bool HasIdentifier
        {
            get { return identifier is not null; }
        }

        /// <summary>
        /// Gets or sets the identifier of the formula.
        /// </summary>
        public string? Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        #endregion

        #region Public static operators

        public static ConjunctionFormula operator &(Formula leftOperand, Formula rightOperand)
        {
            return new ConjunctionFormula(leftOperand, rightOperand);
        }

        public static DisjunctionFormula operator |(Formula leftOperand, Formula rightOperand)
        {
            return new DisjunctionFormula(leftOperand, rightOperand);
        }

        public static NegationFormula operator ~(Formula operand)
        {
            return new NegationFormula(operand);
        }

        public static bool operator ==(Formula leftOperand, Formula rightOperand)
        {
            return leftOperand.Equals(rightOperand);
        }

        public static bool operator !=(Formula leftOperand, Formula rightOperand)
        {
            return !leftOperand.Equals(rightOperand);
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public abstract Formula Negated();

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public abstract Formula Evaluated();

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
        public abstract bool Equivalent(Formula other);

        /// <summary>
        /// Determines whether the current program implies the specified program.
        /// </summary>
        /// <param name="other">The consequence of the implication.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the current program implies the consequence program.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public bool Implies(Formula consequence)
        {
            bool implies = Equivalent(consequence);
            
            if (!implies)
            {
                Formula intersection = ConjunctionWith(consequence);

                implies = Equivalent(intersection);
            }

            return implies;
        }

        /// <summary>
        /// Creates a deep copy of the current program.
        /// </summary>
        /// <returns>The created deep copy of the program.</returns>
        public abstract Formula DeepCopy();

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public abstract string ToLatex();

        #endregion

        #region Public virtual methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return identifier is not null ? identifier : ToLatex();
        }

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
            return base.Equals(obj);
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
        /// Calculate the conjuction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the conjunction.</param>
        /// <returns>The result of the conjunction.</returns>
        public virtual Formula ConjunctionWith(Formula other)
        {
            return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        /// <summary>
        /// Calculate the disjunction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the disjunction.</param>
        /// <returns>The result of the disjunction.</returns>
        public virtual Formula DisjunctionWith(Formula other)
        {
            return new DisjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Completely evaluates the given program, without modifying the original.
        /// </summary>
        /// <returns>The completely evaluated instance of the program.</returns>
        public Formula CompletelyEvaluated()
        {
            Formula result    = DeepCopy();
            Formula evaluated = Evaluated();

            while (result != evaluated)
            {
                result    = evaluated;
                evaluated = evaluated.Evaluated();
            }

            return result;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Determines whether the specified program complements the current program.
        /// </summary>
        /// <param name="other">The other program.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the specified program is equivalent to the negated current program.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        protected bool Complements(Formula other)
        {
            Formula evaluation        = CompletelyEvaluated();
            Formula negatedEvaluation = other.Negated().CompletelyEvaluated();

            return evaluation.Equivalent(negatedEvaluation);
        }

        /// <summary>
        /// Returns the other formula if it doesn't equal to the current formula,
        /// otherwise the copy of the current formula.
        /// </summary>
        /// <param name="other">The other formula to compare with.</param>
        /// <returns>The result of the compare.</returns>
        protected Formula ReturnOrDeepCopy(Formula other)
        {
            return other == this ? DeepCopy() : other;
        }

        #endregion
    }
}
