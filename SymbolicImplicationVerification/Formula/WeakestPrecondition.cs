
namespace SymbolicImplicationVerification.Formula
{
    public class WeakestPrecondition : Formula
    {
        #region Fields

        private Program.Program program;
        private Formula statement;

        #endregion

        #region Constructors

        public WeakestPrecondition(Program.Program program, Formula statement) : this(null, program, statement) { }

        public WeakestPrecondition(string? identifier, Program.Program program, Formula statement) : base(identifier)
        {
            this.program   = program;
            this.statement = statement;
        }

        #endregion

        #region Public properties

        public Program.Program Program
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
    }
}
