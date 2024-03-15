global using TypeTerm = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.Type>;
global using IntegerTypeTerm = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>;

using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Variables;

namespace SymbolicImplicationVerification.Terms
{
    public abstract class Term<T> where T : Type
    {
        #region Fields

        protected T termType;

        #endregion

        #region Constructors

        public Term(T termType)
        {
            this.termType = termType;
        }

        #endregion

        #region Factory methods

        /// <summary>
        /// Creates a deep copy of the given <see cref="Term{T}"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="Term{T}"/> term to be copied.</param>
        /// <returns>The copied <see cref="Term{T}"/> instance.</returns>
        public static Term<T> DeepCopy(Term<T> termToCopy)
        {
            return DeepCopy((dynamic) termToCopy);
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="IntegerConstant"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="IntegerConstant"/> term to be copied.</param>
        /// <returns>The copied <see cref="IntegerConstant"/> instance.</returns>
        public static IntegerConstant DeepCopy(IntegerConstant termToCopy)
        {
            return new IntegerConstant(termToCopy);
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="NaturalNumberConstant"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="NaturalNumberConstant"/> term to be copied.</param>
        /// <returns>The copied <see cref="NaturalNumberConstant"/> instance.</returns>
        public static NaturalNumberConstant DeepCopy(NaturalNumberConstant termToCopy)
        {
            return new NaturalNumberConstant(termToCopy);
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="PositiveIntegerConstant"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="PositiveIntegerConstant"/> term to be copied.</param>
        /// <returns>The copied <see cref="PositiveIntegerConstant"/> instance.</returns>
        public static PositiveIntegerConstant DeepCopy(PositiveIntegerConstant termToCopy)
        {
            return new PositiveIntegerConstant(termToCopy);
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="ZeroOrOneConstant"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="ZeroOrOneConstant"/> term to be copied.</param>
        /// <returns>The copied <see cref="ZeroOrOneConstant"/> instance.</returns>
        public static ZeroOrOneConstant DeepCopy(ZeroOrOneConstant termToCopy)
        {
            return new ZeroOrOneConstant(termToCopy);
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="LogicalConstant"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="LogicalConstant"/> term to be copied.</param>
        /// <returns>The copied <see cref="LogicalConstant"/> instance.</returns>
        public static LogicalConstant DeepCopy(LogicalConstant termToCopy)
        {
            return new LogicalConstant(termToCopy);
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="BetaFunction"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="BetaFunction"/> term to be copied.</param>
        /// <returns>The copied <see cref="BetaFunction"/> instance.</returns>
        public static BetaFunction DeepCopy(BetaFunction termToCopy)
        {
            return new BetaFunction(Term<T>.DeepCopy((dynamic) termToCopy.Argument));
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="ChiFunction"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="ChiFunction"/> term to be copied.</param>
        /// <returns>The copied <see cref="ChiFunction"/> instance.</returns>
        public static ChiFunction DeepCopy(ChiFunction termToCopy)
        {
            return new ChiFunction(Term<T>.DeepCopy((dynamic) termToCopy.Argument));
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="Addition"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="Addition"/> term to be copied.</param>
        /// <returns>The copied <see cref="Addition"/> instance.</returns>
        public static Addition DeepCopy(Addition termToCopy)
        {
            return new Addition(
                Term<T>.DeepCopy((dynamic) termToCopy.LeftOperand),
                Term<T>.DeepCopy((dynamic) termToCopy.RightOperand),
                Type.DeepCopy((dynamic) termToCopy.TermType)
            );
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="Subtraction"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="Subtraction"/> term to be copied.</param>
        /// <returns>The copied <see cref="Subtraction"/> instance.</returns>
        public static Subtraction DeepCopy(Subtraction termToCopy)
        {
            return new Subtraction(
                Term<T>.DeepCopy((dynamic) termToCopy.LeftOperand),
                Term<T>.DeepCopy((dynamic) termToCopy.RightOperand),
                Type.DeepCopy((dynamic) termToCopy.TermType)
            );
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="Multiplication"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="Multiplication"/> term to be copied.</param>
        /// <returns>The copied <see cref="Multiplication"/> instance.</returns>
        public static Multiplication DeepCopy(Multiplication termToCopy)
        {
            return new Multiplication(
                Term<T>.DeepCopy((dynamic) termToCopy.LeftOperand),
                Term<T>.DeepCopy((dynamic) termToCopy.RightOperand),
                Type.DeepCopy((dynamic) termToCopy.TermType)
            );
        }

        //==============================================================//
        //   TODO: DeepCopy for LinearAddition, LinearMultiplication.   //
        //==============================================================//

        /// <summary>
        /// Creates a deep copy of the given <see cref="IntegerTypeVariable"/> instance.
        /// </summary>
        /// <param name="termToCopy">The <see cref="IntegerTypeVariable"/> term to be copied.</param>
        /// <returns>The copied <see cref="IntegerTypeVariable"/> instance.</returns>
        public static IntegerTypeVariable DeepCopy(IntegerTypeVariable termToCopy)
        {
            return new IntegerTypeVariable(termToCopy);
        }

        #endregion

        #region Public properties

        public T TermType
        { 
            get { return termType; }
            private set
            {
                termType = value;
            }
        }

        #endregion

        #region Public abstract methods

        public abstract string Hash(HashLevel level);

        #endregion

        #region Implicit conversions 


        public static implicit operator TypeTerm(Term<T> term)
        {
            return term;
        }

        #endregion
    }
}
