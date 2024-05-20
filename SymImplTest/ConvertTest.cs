using SymbolicImplicationVerification.Programs;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Converts;
using SymbolicImplicationVerification.Implies;
using SymbolicImplicationVerification.Formulas.Relations;

namespace SymImplTest
{
    [TestClass]
    public class ConvertTest
    {
        static IEnumerable<object[]> ConvertToIntegerTypeTermData
        {
            get
            {
                var x = new Variable<IntegerType>("x", Integer.Instance());
                var n = new Variable<IntegerType>("n", NaturalNumber.Instance());

                var stateSpace = @"\symboldeclare{A}{\declare{x}{\Z}, \declare{n}{\N}}";

                return new[]
                {
                    new object[] { stateSpace, "x", x },
                    new object[] { stateSpace, "n", n },
                    new object[] { stateSpace, "x + 1", new Addition(x, 1) },
                    new object[] { stateSpace, "n + 1", new Addition(n, 1) },
                    new object[] { stateSpace, "x + n", new Addition(x, n) },
                    new object[] { stateSpace, "n + x", new Addition(n, x) },
                    new object[] { stateSpace, "n - x", new Subtraction(n, x) },
                    new object[] { stateSpace, "x - n", new Subtraction(x, n) },
                    new object[] { stateSpace, @"n \cdot 2", new Multiplication(n, 2) },
                    new object[] { stateSpace, @"2 \cdot x", new Multiplication(2, x) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConvertToIntegerTypeTermData))]
        public void ConvertToIntegerTypeTermTest(string stateSpace, string termInput, Term<IntegerType> expected)
        {
            Converter converter = new Converter();

            converter.DeclareStateSpace(stateSpace);

            Assert.AreEqual(expected, converter.ConvertToIntegerTypeTerm(termInput));
        }

        static IEnumerable<object[]> ConvertToLogicalTermData
        {
            get
            {
                var l = new Variable<Logical>("l", Logical.Instance());

                var stateSpace = @"\symboldeclare{A}{\declare{l}{\B}}";

                return new[]
                {
                    new object[] { stateSpace, "l", l },
                    new object[] { stateSpace, @"\true" , new LogicalConstant(true) },
                    new object[] { stateSpace, @"\false", new LogicalConstant(false) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConvertToLogicalTermData))]
        public void ConvertToLogicalTermTest(string stateSpace, string termInput, Term<Logical> expected)
        {
            Converter converter = new Converter();

            converter.DeclareStateSpace(stateSpace);

            Assert.AreEqual(expected, converter.ConvertToLogicalTerm(termInput));
        }

        static IEnumerable<object[]> ConvertToProgramData
        {
            get
            {
                var x = new Variable<IntegerType>("x", Integer.Instance());
                var y = new Variable<IntegerType>("y", Integer.Instance());
                var l = new Variable<Logical>("l", Logical.Instance());

                var stateSpace = @"\symboldeclare{A}{\declare{x}{\Z}, \declare{y}{\Z}, \declare{l}{\B}}";

                return new[]
                {
                    new object[] { stateSpace, @"\SKIP", SKIP.Instance() },
                    new object[] { stateSpace, @"\ABORT", ABORT.Instance() },
                    new object[] { stateSpace, @"\assign{x}{0}", new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, new IntegerTypeConstant(0)) }) },
                    new object[] { stateSpace, @"\assign{x}{y}", new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, y) }) },
                    new object[] { stateSpace, @"\assign{y}{x}", new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (y, x) }) },
                    new object[] { stateSpace, @"\assign{x, y}{0, 1}", new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, new IntegerTypeConstant(0)), (y, new IntegerTypeConstant(1)) }) },
                    new object[] { stateSpace, @"\assign{x, y}{0, x}", new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, new IntegerTypeConstant(0)), (y, x) }) },
                    new object[] { stateSpace, @"\assign{l}{\true}", new Assignment(new List<(Variable<Logical>, Term<Logical>)> { (l, new LogicalConstant(true)) }) },
                    new object[] { stateSpace, @"\assign{l}{\false}", new Assignment(new List<(Variable<Logical>, Term<Logical>)> { (l, new LogicalConstant(false)) }) },
                    new object[] { stateSpace, @"\assign{x, l}{y, \true}", new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, y) }, new List<(Variable<Logical>, Term<Logical>)> { (l, new LogicalConstant(true)) }) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConvertToProgramData))]
        public void ConvertToProgramTest(string stateSpace, string termInput, Program expected)
        {
            Converter converter = new Converter();

            converter.DeclareStateSpace(stateSpace);

            Assert.AreEqual(expected, converter.ConvertToProgram(termInput));
        }


        static IEnumerable<object[]> ConvertToFormulaData
        {
            get
            {
                var x = new Variable<IntegerType>("x", Integer.Instance());
                var y = new Variable<IntegerType>("y", Integer.Instance());
                var l = new Variable<Logical>("x", Logical.Instance());

                var stateSpace = @"\symboldeclare{A}{\declare{x}{\Z}, \declare{y}{\Z}}";

                return new[]
                {
                    new object[] { stateSpace, "x = y", new IntegerTypeEqual(x, y) },
                    new object[] { stateSpace, "x < y", new LessThan(x, y) },
                    new object[] { stateSpace, "x > y", new GreaterThan(x, y) },
                    new object[] { stateSpace, @"x \neq y", new IntegerTypeNotEqual(x, y) },
                    new object[] { stateSpace, @"x \leq y", new LessThanOrEqualTo(x, y) },
                    new object[] { stateSpace, @"x \geq y", new GreaterThanOrEqualTo(x, y) },
                    new object[] { stateSpace, @"x \mid y", new Divisor(x, y) },
                    new object[] { stateSpace, @"x \nmid y", new NotDivisor(x, y) },
                    new object[] { stateSpace, @"x = y \wedge x \leq y", new IntegerTypeEqual(x, y) & new LessThanOrEqualTo(x, y) },
                    new object[] { stateSpace, @"x = y \vee x \leq y", new IntegerTypeEqual(x, y) | new LessThanOrEqualTo(x, y) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConvertToFormulaData))]
        public void ConvertToFormulaTest(string stateSpace, string termInput, Formula expected)
        {
            Converter converter = new Converter();

            converter.DeclareStateSpace(stateSpace);

            Assert.AreEqual(expected, converter.ConvertToFormula(termInput));
        }

        static IEnumerable<object[]> ConvertToImplyData
        {
            get
            {
                var x = new Variable<IntegerType>("x", Integer.Instance());
                var y = new Variable<IntegerType>("y", Integer.Instance());
                var l = new Variable<Logical>("l", Logical.Instance());

                var stateSpace = @"\symboldeclare{A}{\declare{x}{\Z}, \declare{y}{\Z}, \declare{l}{\B}}";

                var two   = new IntegerTypeConstant(2);
                var zero  = new IntegerTypeConstant(0);
                var False = new LogicalConstant(false);

                return new[]
                {
                    new object[] { stateSpace, @"\imply{x = 2}{x > 0}", new Imply(new IntegerTypeEqual(x, two), new GreaterThan(x, zero)) },
                    new object[] { stateSpace, @"\imply{x = 2}{x < 0}", new Imply(new IntegerTypeEqual(x, two), new LessThan(x, zero)) },
                    new object[] { stateSpace, @"\imply{x = y}{x < 0}", new Imply(new IntegerTypeEqual(x, y), new LessThan(x, zero)) },
                    new object[] { stateSpace, @"\imply{l = \false}{x < 0}", new Imply(new LogicalEqual(l, False), new LessThan(x, zero)) }
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConvertToImplyData))]
        public void ConvertToImplyTest(string stateSpace, string termInput, Imply expected)
        {
            Converter converter = new Converter();

            converter.DeclareStateSpace(stateSpace);

            Assert.AreEqual(expected, converter.ConvertToImply(termInput));
        }
    }
}
