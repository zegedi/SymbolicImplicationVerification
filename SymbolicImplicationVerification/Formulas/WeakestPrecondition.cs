
using SymbolicImplicationVerification.Programs;

namespace SymbolicImplicationVerification.Formulas
{
    
    public class WeakestPrecondition : Formula //, IEvaluable<Formula>
    {
        #region Fields

        private Programs.Program program;
        private Formula statement;

        #endregion

        #region Constructors

        public WeakestPrecondition(Programs.Program program, Formula statement) : this(null, program, statement) { }

        public WeakestPrecondition(string? identifier, Programs.Program program, Formula statement) : base(identifier)
        {
            this.program   = program;
            this.statement = statement;
        }

        #endregion

        #region Public properties

        public Programs.Program Program
        {
            get { return program; }
            set { program = value; }
        }

        public Formula Statement
        {
            get { return statement; }
            set { statement = value; }
        }

        #endregion

        #region Public properties

        /*
        public Formula Evaluate()
        {
            Formula evaluatedFormula = FALSE.Instance();

            if (program is ABORT || statement is FALSE)
            {
                evaluatedFormula = FALSE.Instance();
            }
            else if (program is SKIP)
            {
                evaluatedFormula = statement;
            }
            else if (program is Assignment assignment)
            {
                evaluatedFormula = FALSE.Instance();

                foreach ((Variable variable, AssignedValue value) assign in assignment.Assingments)
                {
                    if ()
                }
            }

            return evaluatedFormula;
        }
        */

        #endregion
    }
}
