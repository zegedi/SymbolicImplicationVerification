
// Aliases for the variable and assigned value types.
global using Variable = SymbolicImplicationVerification.Term.Variable<SymbolicImplicationVerification.Type.Type>;
global using AssignedValue = SymbolicImplicationVerification.Term.Term<SymbolicImplicationVerification.Type.Type>;

using SymbolicImplicationVerification.Type;
using SymbolicImplicationVerification.Term;

namespace SymbolicImplicationVerification.Program
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
