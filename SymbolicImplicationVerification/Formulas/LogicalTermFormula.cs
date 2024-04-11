﻿using SymbolicImplicationVerification.Formulas.Operations;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicImplicationVerification.Formulas
{
    public class LogicalTermFormula : Formula
    {
        #region Fields

        protected LogicalTerm argumentum;

        #endregion

        #region Constructors

        public LogicalTermFormula(LogicalTermFormula logicalTerm)
            : this(logicalTerm.identifier, logicalTerm.argumentum.DeepCopy()) { }

        public LogicalTermFormula(LogicalTerm argumentum) : this(null, argumentum) { }

        public LogicalTermFormula(string? identifier, LogicalTerm argumentum) : base(identifier)
        {
            this.argumentum = argumentum;
        }

        #endregion

        #region Public properties

        public LogicalTerm Argumentum
        {
            get { return argumentum; }
            set { argumentum = value; }
        }

        #endregion

        #region Public static operators

        public static ConjunctionFormula operator &(LogicalTermFormula leftOperand, Formula rightOperand)
        {
            return new ConjunctionFormula(leftOperand, rightOperand);
        }

        public static DisjunctionFormula operator |(LogicalTermFormula leftOperand, Formula rightOperand)
        {
            return new DisjunctionFormula(leftOperand, rightOperand);
        }

        public static NegationFormula operator ~(LogicalTermFormula operand)
        {
            return new NegationFormula(operand);
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            if (argumentum is FormulaTerm formulaTerm)
            {
                return formulaTerm.Formula.Negated();
            }

            if (argumentum is LogicalConstant logicalConstant)
            {
                return logicalConstant.Value? FALSE.Instance() : TRUE.Instance();
            }

            return new NegationFormula(DeepCopy());
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            if (argumentum is FormulaTerm formulaTerm)
            {
                return formulaTerm.Formula.Evaluated();
            }

            LogicalTerm argumentumEval = argumentum.Evaluated();

            if (argumentumEval is LogicalConstant logicalConstant)
            {
                return logicalConstant.Value? FALSE.Instance() : TRUE.Instance();
            }

            return new LogicalTermFormula(argumentumEval);
        }

        /// <summary>
        /// Determines whether the specified formulaTerm is equivalent to the current formulaTerm.
        /// </summary>
        /// <param name="other">The formulaTerm to compare with the current formulaTerm.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the formulas are the equivalent.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Equivalent(Formula other)
        {
            Formula thisEval  = Evaluated();
            Formula otherEval = other.Evaluated();

            bool useEquality = thisEval is LogicalTermFormula || otherEval is LogicalTermFormula;

            return useEquality ? thisEval.Equals(otherEval) : thisEval.Equivalent(otherEval);
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
            return obj is LogicalTermFormula other && argumentum.Equals(other.argumentum);
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
        /// Creates a deep copy of the current formulaTerm.
        /// </summary>
        /// <returns>The created deep copy of the formulaTerm.</returns>
        public override Formula DeepCopy()
        {
            return new LogicalTermFormula(this);
        }

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            if (argumentum is FormulaTerm formulaTerm)
            {
                return formulaTerm.Formula.ToLatex();
            }

            string? result = argumentum.ToString();

            return result is not null ? result : string.Empty;
        }

        public override Formula ConjunctionWith(Formula other)
        {
            if (other is NotEvaluable)
            {
                return NotEvaluable.Instance();
            }

            if (argumentum is FormulaTerm formulaTerm)
            {
                return formulaTerm.Formula.ConjunctionWith(other);
            }

            return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        public override Formula DisjunctionWith(Formula other)
        {
            if (other is NotEvaluable)
            {
                return NotEvaluable.Instance();
            }

            if (argumentum is FormulaTerm formulaTerm)
            {
                return formulaTerm.Formula.DisjunctionWith(other);
            }

            return new DisjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        #endregion
    }
}
