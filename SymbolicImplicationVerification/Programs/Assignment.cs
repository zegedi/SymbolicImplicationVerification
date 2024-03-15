
// Aliases for the variable and assigned value types.
global using Variable = SymbolicImplicationVerification.Terms.Variable<SymbolicImplicationVerification.Types.Type>;
global using AssignedValue = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.Type>;

using SymbolicImplicationVerification.Types;
using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Programs
{
    public class Assignment : Program
    {
        #region Fields

        private List<(Variable, AssignedValue)> assingments;

        #endregion

        #region Constructors

        public Assignment(List<(Variable, AssignedValue)> assingments)
        {
            this.assingments = assingments;
        }

        #endregion

        #region Public properties

        public List<(Variable, AssignedValue)> Assingments
        {
            get { return assingments; }
            set { assingments = value; }
        }

        #endregion
    }
}
