global using Type = SymbolicImplicationVerification.Types.Type;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Types
{
    public abstract class Type
    {
        #region Constructors

        public Type() { }

        #endregion

        #region Factory methods

        /// <summary>
        /// Creates a deep copy of the given <see cref="Type"/> instance.
        /// </summary>
        /// <param name="typeToCopy">The <see cref="Type"/> type to be copied.</param>
        /// <returns>The copied <see cref="Type"/> instance.</returns>
        public static Type DeepCopy(Type typeToCopy)
        {
            return Type.DeepCopy((dynamic) typeToCopy);
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="Type"/> instance.
        /// </summary>
        /// <param name="typeToCopy">The <see cref="Type"/> type to be copied.</param>
        /// <returns>The copied <see cref="Type"/> instance.</returns>
        public static IntegerType DeepCopy(IntegerType typeToCopy)
        {
            return IntegerType.DeepCopy((dynamic) typeToCopy);
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="Logical"/> instance.
        /// </summary>
        /// <param name="typeToCopy">The <see cref="Logical"/> type to be copied.</param>
        /// <returns>The copied <see cref="Logical"/> instance.</returns>
        public static Logical DeepCopy(Logical typeToCopy)
        {
            return Logical.Instance();
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="Integer"/> instance.
        /// </summary>
        /// <param name="typeToCopy">The <see cref="Integer"/> type to be copied.</param>
        /// <returns>The copied <see cref="Integer"/> instance.</returns>
        public static Integer DeepCopy(Integer typeToCopy)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="NaturalNumber"/> instance.
        /// </summary>
        /// <param name="typeToCopy">The <see cref="NaturalNumber"/> type to be copied.</param>
        /// <returns>The copied <see cref="NaturalNumber"/> instance.</returns>
        public static NaturalNumber DeepCopy(NaturalNumber typeToCopy)
        {
            return NaturalNumber.Instance();
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="PositiveInteger"/> instance.
        /// </summary>
        /// <param name="typeToCopy">The <see cref="PositiveInteger"/> type to be copied.</param>
        /// <returns>The copied <see cref="PositiveInteger"/> instance.</returns>
        public static PositiveInteger DeepCopy(PositiveInteger typeToCopy)
        {
            return PositiveInteger.Instance();
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="ZeroOrOne"/> instance.
        /// </summary>
        /// <param name="typeToCopy">The <see cref="ZeroOrOne"/> type to be copied.</param>
        /// <returns>The copied <see cref="ZeroOrOne"/> instance.</returns>
        public static ZeroOrOne DeepCopy(ZeroOrOne typeToCopy)
        {
            return ZeroOrOne.Instance();
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="ConstantBoundedInteger"/> instance.
        /// </summary>
        /// <param name="typeToCopy">The <see cref="ConstantBoundedInteger"/> type to be copied.</param>
        /// <returns>The copied <see cref="ConstantBoundedInteger"/> instance.</returns>
        public static ConstantBoundedInteger DeepCopy(ConstantBoundedInteger typeToCopy)
        {
            return new ConstantBoundedInteger(typeToCopy);
        }

        /// <summary>
        /// Creates a deep copy of the given <see cref="TermBoundedInteger"/> instance.
        /// </summary>
        /// <param name="typeToCopy">The <see cref="TermBoundedInteger"/> type to be copied.</param>
        /// <returns>The copied <see cref="TermBoundedInteger"/> instance.</returns>
        public static TermBoundedInteger DeepCopy(TermBoundedInteger typeToCopy)
        {
            return new TermBoundedInteger(
                IntegerTypeTerm.DeepCopy((dynamic) typeToCopy.LowerBound) ,
                IntegerTypeTerm.DeepCopy((dynamic) typeToCopy.UpperBound) );
        }

        #endregion
    }
}
