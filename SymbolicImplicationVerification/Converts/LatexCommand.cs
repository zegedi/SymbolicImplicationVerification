using System.ComponentModel;

namespace SymbolicImplicationVerification.Converts
{
    public enum LatexCommand
    {
        [Description("(")]
        LeftParentheses,

        [Description(")")]
        RightParentheses,

        [Description(",")]
        Separator,

        [Description("+")]
        Addition,

        [Description("-")]
        Subtraction,

        [Description("\\cdot")]
        Multiplication,

        [Description("=")]
        Equal,

        [Description("\\neq")]
        NotEqual,

        [Description("<")]
        LessThan,

        [Description(">")]
        GreaterThan,

        [Description("\\leq")]
        LessThanOrEqual,

        [Description("\\geq")]
        GreaterThanOrEqual,

        [Description("\\mid")]
        Divisor,

        [Description("\\nmid")]
        NotDivisor,

        [Description("\\wedge")]
        Conjunction,

        [Description("\\vee")]
        Disjunction,

        [Description("\\rightarrow")]
        Implication,

        [Description("\\neg")]
        Negation,

        [Description("\\true")]
        TrueConstant,

        [Description("\\false")]
        FalseConstant,

        [Description("\\TRUE")]
        TrueFormula,

        [Description("\\FALSE")]
        FalseFormula,

        [Description("\\NOTEVAL")]
        NotEvaluableFormula,

        [Description("\\ABORT")]
        Abort,

        [Description("\\SKIP")]
        Skip,

        [Description("\\B")]
        Boolean,

        [Description("\\Z")]
        Integer,

        [Description("\\N")]
        NaturalNumber,

        [Description("\\posN")]
        PositiveInteger,

        [Description("\\zeroone")]
        ZeroOrOne,

        [Description("\\assign{;}{;}")]
        Assignment,

        [Description("\\interval{;}{;}")]
        IntegerInterval,

        [Description("\\declare{;}{;}")]
        VariableDeclaration,

        [Description("\\arrayvar{;}{;}")]
        ArrayVariable,

        [Description("\\arraytype{;}{;}")]
        ArrayVariableDeclaration,

        [Description("\\symboldeclare{;}{;}")]
        SymbolDeclaration,

        [Description("\\chifunc{;}")]
        ChiFunction,

        [Description("\\betafunc{;}")]
        BetaFunction,

        [Description("\\imply{;}{;}")]
        Imply,

        [Description("\\universally{;}{;}{;}")]
        UniversallyQuantifiedFormula,

        [Description("\\existentially{;}{;}{;}")]
        ExistentiallyQuantifiedFormula,

        [Description("\\summation{;}{;}{;}{;}")]
        Summation,

        [Description("\\weakestprec{;}{;}")]
        WeakestPrecondition,
    }
}
