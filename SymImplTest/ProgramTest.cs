using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Programs;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymImplTest
{
    [TestClass]
    public class ProgramTest
    {
        static IEnumerable<object[]> DeepCopyData
        {
            get
            {
                var x = new Variable<IntegerType>("x", Integer.Instance());
                var y = new Variable<IntegerType>("y", Integer.Instance());

                var l = new Variable<Logical>("l", Logical.Instance());

                return new[]
                {
                    new object[] { ABORT.Instance() },
                    new object[] { SKIP.Instance() },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, y) }) },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (y, x) }) },
                    new object[] { new Assignment(new List<(Variable<Logical>, Term<Logical>)> { (l, new LogicalConstant(false)) }) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(DeepCopyData))]
        public void ConjunctionFormulaEquivalentTest(Program program)
        {
            Assert.AreEqual(program, program.DeepCopy());
        }

        static IEnumerable<object[]> SubstituteAssignmentsData
        {
            get
            {
                var x = new Variable<IntegerType>("x", Integer.Instance());
                var y = new Variable<IntegerType>("y", Integer.Instance());

                var l = new Variable<Logical>("l", Logical.Instance());
                var k = new Variable<Logical>("k", Logical.Instance());

                var False = new LogicalConstant(false);

                return new[]
                {
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, y) }), FALSE.Instance(), FALSE.Instance() },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, y) }), TRUE.Instance(), TRUE.Instance() },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, y) }), new IntegerTypeEqual(x,x), new IntegerTypeEqual(y,y) },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, y) }), new IntegerTypeEqual(y,y), new IntegerTypeEqual(y,y) },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (x, y) }), new LogicalEqual(l,k), new LogicalEqual(l,k) },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (y, x) }), FALSE.Instance(), FALSE.Instance() },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (y, x) }), TRUE.Instance(), TRUE.Instance() },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (y, x) }), new IntegerTypeEqual(y,y), new IntegerTypeEqual(x,x) },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (y, x) }), new IntegerTypeEqual(x,x), new IntegerTypeEqual(x,x) },
                    new object[] { new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)> { (y, x) }), new LogicalEqual(l,k), new LogicalEqual(l,k) },
                    new object[] { new Assignment(new List<(Variable<Logical>, Term<Logical>)> { (l, False) }), FALSE.Instance(), FALSE.Instance() },
                    new object[] { new Assignment(new List<(Variable<Logical>, Term<Logical>)> { (l, False) }), TRUE.Instance(), TRUE.Instance() },
                    new object[] { new Assignment(new List<(Variable<Logical>, Term<Logical>)> { (l, False) }), new IntegerTypeEqual(x, x), new IntegerTypeEqual(x, x) },
                    new object[] { new Assignment(new List<(Variable<Logical>, Term<Logical>)> { (l, False) }), new IntegerTypeEqual(y, y), new IntegerTypeEqual(y, y) },
                    new object[] { new Assignment(new List<(Variable<Logical>, Term<Logical>)> { (l, False) }), new IntegerTypeEqual(x, y), new IntegerTypeEqual(x, y) },
                    new object[] { new Assignment(new List<(Variable<Logical>, Term<Logical>)> { (l, False) }), new LogicalEqual(l,k), new LogicalEqual(False, k) },
                    new object[] { new Assignment(new List<(Variable<Logical>, Term<Logical>)> { (l, False) }), new LogicalEqual(l,l), new LogicalEqual(False, False) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(SubstituteAssignmentsData))]
        public void SubstituteAssignmentsTest(Assignment assignment, Formula formula, Formula expectedResult)
        {
            Assert.AreEqual(expectedResult, assignment.SubstituteAssignments(formula));
        }
    }
}
