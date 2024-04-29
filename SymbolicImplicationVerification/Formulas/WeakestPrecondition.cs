
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Programs;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Formulas.Operations;

namespace SymbolicImplicationVerification.Formulas
{
    
    public class WeakestPrecondition : Formula
    {
        #region Fields

        private Program program;
        private Formula statement;

        #endregion

        #region Constructors

        public WeakestPrecondition(WeakestPrecondition weakestPrecondition) : this(
            weakestPrecondition.identifier, 
            weakestPrecondition.program.DeepCopy(), 
            weakestPrecondition.statement.DeepCopy()) { }

        public WeakestPrecondition(Program program, Formula statement) : this(null, program, statement) { }

        public WeakestPrecondition(string? identifier, Program program, Formula statement) : base(identifier)
        {
            this.program   = program;
            this.statement = statement;
        }

        #endregion

        #region Public properties

        public Program Program
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

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            return string.Format("\\weakestprec{{{0}}}{{{1}}}", program, statement);
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated() => (program, statement) switch
        {
            (ABORT, _                    ) => FALSE.Instance(),
            (_    , FALSE or NotEvaluable) => FALSE.Instance(),
            (SKIP , _                    ) => statement.DeepCopy(),
            (Assignment assignment, _    ) => assignment.SubstituteAssignments(statement.DeepCopy()),
            (_    , _                    ) => FALSE.Instance()
        };

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new NegationFormula(new WeakestPrecondition(this));
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
        public override bool Equivalent(Formula other)
        {
            bool equivalent = Equals(other);

            if (!equivalent)
            {
                Formula thisEvaluated  = this .Evaluated();
                Formula otherEvaluated = other.Evaluated();

                equivalent = thisEvaluated.Equivalent(otherEvaluated);
            }

            return equivalent;
        }

        /// <summary>
        /// Create a deep copy of the current program.
        /// </summary>
        /// <returns>The created deep copy of the program.</returns>
        public override WeakestPrecondition DeepCopy()
        {
            return new WeakestPrecondition(this);
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
            return obj is WeakestPrecondition other &&
                   statement.Equals(other.statement) &&
                   program  .Equals(other.program);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
