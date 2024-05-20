using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Formulas.Operations;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Implies;
using SymbolicImplicationVerification.Programs;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymImplTest
{
    internal class ImplyTest
    {
        static IEnumerable<object[]> IsPrimeData
        {
            get
            {
                var x = new Variable<IntegerType>("x", PositiveInteger.Instance());
                var xv = new Variable<IntegerType>("x'", PositiveInteger.Instance());
                var k = new Variable<IntegerType>("k", PositiveInteger.Instance());
                var l = new Variable<Logical>("l", Logical.Instance());
                var t0 = new Variable<IntegerType>("t_0", Integer.Instance());

                var j1 = new Variable<IntegerType>("j", new TermBoundedInteger(2, new Subtraction(x, 1)));
                var j2 = new Variable<IntegerType>("j", new TermBoundedInteger(2, new Subtraction(k, 1)));
                var j3 = new Variable<IntegerType>("j", new TermBoundedInteger(2, k));

                var zero = new IntegerTypeConstant(0);
                var one = new IntegerTypeConstant(1);
                var two = new IntegerTypeConstant(2);

                var igaz = new LogicalConstant(true);

                var Q = new IntegerTypeEqual(x, xv) & new GreaterThan(x, one);
                Q.Identifier = "Q";

                var R = Q.DeepCopy() & new LogicalEqual(
                    l, new FormulaTerm(new UniversallyQuantifiedFormula<IntegerType>(j1, new NotDivisor(j1, x))));
                R.Identifier = "R";

                var Qv = Q.DeepCopy() & new IntegerTypeEqual(k, two) & new LogicalEqual(l, igaz);
                Qv.Identifier = "Q'";

                var P =
                    Q.DeepCopy() &
                    new LessThanOrEqualTo(two, k) &
                    new LessThanOrEqualTo(k, x) &
                    new LogicalEqual(l,
                    new FormulaTerm(new UniversallyQuantifiedFormula<IntegerType>(j2, new NotDivisor(j2, x))));
                P.Identifier = "P";

                var Qvv =
                    Q.DeepCopy() &
                    new IntegerTypeEqual(new Subtraction(x, 1), t0) &
                    new LessThanOrEqualTo(two, new Addition(k, 1)) &
                    new LessThanOrEqualTo(new Addition(k, 1), x) &
                    new LogicalEqual(l,
                    new FormulaTerm(new UniversallyQuantifiedFormula<IntegerType>(j3, new NotDivisor(j3, x))));
                Qvv.Identifier = "Q''";

                var assignment1 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (k, two)
                },
                new List<(Variable<Logical>, Term<Logical>)>()
                {
                    (l, igaz)
                });

                var assignment2 = new Assignment(new List<(Variable<Logical>, Term<Logical>)>()
                {
                    (l, new FormulaTerm(new LogicalTermFormula(l) & new NotDivisor(k, x)))
                });

                var assignment3 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (k, new Addition(k, 1))
                });


                return new[]
                {
                    new object[] { new Imply(Q.DeepCopy(), new WeakestPrecondition(assignment1.DeepCopy(), Qv.DeepCopy())) },
                    new object[] { new Imply(Qv.DeepCopy(), P.DeepCopy()) },
                    new object[] { new Imply(P.DeepCopy() & new NegationFormula(new IntegerTypeNotEqual(k, x)), R.DeepCopy()) },
                    new object[] { new Imply(P.DeepCopy(), new IntegerTypeNotEqual(k, x) | new NegationFormula(new IntegerTypeNotEqual(k, x))) },
                    new object[] { new Imply(P.DeepCopy() & new IntegerTypeNotEqual(k, x), new GreaterThan(new Subtraction(x, k), zero)) },
                    new object[] { new Imply(P.DeepCopy() & new IntegerTypeNotEqual(k, x) & new IntegerTypeEqual(new Subtraction(x, k), t0), new WeakestPrecondition(assignment2, Qvv.DeepCopy())) },
                    new object[] { new Imply(Qvv.DeepCopy(), new WeakestPrecondition(assignment3, P.DeepCopy())) }
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(IsPrimeData))]
        public void IsPrimeTest(Imply imply)
        {
            Assert.AreEqual(ImplyEvaluationResult.True, imply.Evaluated().EvaluationResult());
        }


        static IEnumerable<object[]> IncrementArrayValuesData
        {
            get
            {
                var i = new Variable<IntegerType>("i", NaturalNumber.Instance());
                var n = new Variable<IntegerType>("n", NaturalNumber.Instance());
                var t0 = new Variable<IntegerType>("t_0", Integer.Instance());

                var zero = new IntegerTypeConstant(0);
                var one = new IntegerTypeConstant(1);
                var two = new IntegerTypeConstant(2);

                var k1 = new Variable<IntegerType>("k", new TermBoundedInteger(one, n));
                var k2 = new Variable<IntegerType>("k", new TermBoundedInteger(one, new Subtraction(i, 1)));
                var k3 = new Variable<IntegerType>("k", new TermBoundedInteger(i, n));
                var k4 = new Variable<IntegerType>("k", new TermBoundedInteger(one, i));
                var k5 = new Variable<IntegerType>("k", new TermBoundedInteger(new Addition(i, 1), n));

                var x = new ArrayVariable<IntegerType>("x", n, Integer.Instance());
                var xv = new ArrayVariable<IntegerType>("x'", n, Integer.Instance());
                var xi = new ArrayVariable<IntegerType>("x", n, i, Integer.Instance());
                var xvi = new ArrayVariable<IntegerType>("x'", n, i, Integer.Instance());
                var xk = new ArrayVariable<IntegerType>("x", n, k1, Integer.Instance());
                var xvk = new ArrayVariable<IntegerType>("x'", n, k1, Integer.Instance());

                var Q = new IntegerTypeEqual(x, xv);
                Q.Identifier = "Q";

                var R = new UniversallyQuantifiedFormula<IntegerType>(k1, new IntegerTypeEqual(xk, new Addition(xvk, one)));
                R.Identifier = "R";

                var Qv = Q.DeepCopy() & new IntegerTypeEqual(i, one);
                Qv.Identifier = "Q'";

                var P = new LessThanOrEqualTo(one, i) &
                         new LessThanOrEqualTo(i, new Addition(n, 1)) &
                         new UniversallyQuantifiedFormula<IntegerType>(k2, new IntegerTypeEqual(xk, new Addition(xvk, one))) &
                         new UniversallyQuantifiedFormula<IntegerType>(k3, new IntegerTypeEqual(xk, xvk));
                P.Identifier = "P";

                var Qvv = new LessThanOrEqualTo(one, new Addition(i, 1)) &
                          new LessThanOrEqualTo(new Addition(i, 1), new Addition(n, 1)) &
                          new UniversallyQuantifiedFormula<IntegerType>(k4, new IntegerTypeEqual(xk, new Addition(xvk, one))) &
                          new UniversallyQuantifiedFormula<IntegerType>(k5, new IntegerTypeEqual(xk, xvk));
                Qvv.Identifier = "Q''";

                var assignment1 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (i, one)
                });

                var assignment2 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (xi, new Addition(xi, one))
                });

                var assignment3 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (i, new Addition(i, 1))
                });


                return new[]
                {
                    new object[] { new Imply(Q.DeepCopy(), new WeakestPrecondition(assignment1.DeepCopy(), Qv.DeepCopy())) },
                    new object[] { new Imply(Qv.DeepCopy(), P.DeepCopy()) },
                    new object[] { new Imply(P.DeepCopy() & new NegationFormula(new IntegerTypeNotEqual(i, new Addition(n, 1))), R.DeepCopy()) },
                    new object[] { new Imply(P.DeepCopy(), new IntegerTypeNotEqual(i, new Addition(n, 1)) | new NegationFormula(new IntegerTypeNotEqual(i, new Addition(n, 1)))) },
                    new object[] { new Imply(P.DeepCopy() & new IntegerTypeNotEqual(i, new Addition(n, 1)) & new IntegerTypeEqual(new Addition(n, 1) - i, t0), new WeakestPrecondition(assignment2, Qvv.DeepCopy() & new IntegerTypeEqual(new Addition(n, 1) - i, t0))) },
                    new object[] { new Imply(Qvv.DeepCopy() & new IntegerTypeEqual(new Addition(n, 1) - i, t0), new WeakestPrecondition(assignment3, P.DeepCopy() & new LessThan(new Addition(n, 1) - i, t0))) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(IncrementArrayValuesData))]
        public void IncrementArrayValuesTest(Imply imply)
        {
            Assert.AreEqual(ImplyEvaluationResult.True, imply.Evaluated().EvaluationResult());
        }


        static IEnumerable<object[]> SummationData
        {
            get
            {
                var i = new Variable<IntegerType>("i", NaturalNumber.Instance());
                var n = new Variable<IntegerType>("n", NaturalNumber.Instance());
                var t0 = new Variable<IntegerType>("t_0", Integer.Instance());
                var sum = new Variable<IntegerType>("sum", Integer.Instance());

                var zero = new IntegerTypeConstant(0);
                var one = new IntegerTypeConstant(1);
                var two = new IntegerTypeConstant(2);

                var k1 = new Variable<IntegerType>("k", new TermBoundedInteger(one, n));
                var k2 = new Variable<IntegerType>("k", new TermBoundedInteger(one, i));
                var k3 = new Variable<IntegerType>("k", new TermBoundedInteger(one, new Addition(i, 1)));

                var x = new ArrayVariable<IntegerType>("x", n, Integer.Instance());
                var xv = new ArrayVariable<IntegerType>("x'", n, Integer.Instance());
                var xip = new ArrayVariable<IntegerType>("x", n, new Addition(i, 1), Integer.Instance());
                var xk1 = new ArrayVariable<IntegerType>("x", n, k1, Integer.Instance());
                var xk2 = new ArrayVariable<IntegerType>("x", n, k2, Integer.Instance());
                var xk3 = new ArrayVariable<IntegerType>("x", n, k3, Integer.Instance());

                var sum1 = new Summation(k1, one, n, xk1, Integer.Instance());
                var sum2 = new Summation(k2, one, i, xk2, Integer.Instance());
                var sum3 = new Summation(k3, one, new Addition(i, 1), xk3, Integer.Instance());

                var Q = new IntegerTypeEqual(x, xv);
                Q.Identifier = "Q";

                var R = Q.DeepCopy() & new IntegerTypeEqual(sum, sum1);
                R.Identifier = "R";

                var Qv = Q.DeepCopy() & new IntegerTypeEqual(i, zero) & new IntegerTypeEqual(sum, zero);
                Qv.Identifier = "Q'";

                var P = Q.DeepCopy() &
                        new LessThanOrEqualTo(zero, i) &
                        new LessThanOrEqualTo(i, n) &
                        new IntegerTypeEqual(sum, sum2);
                P.Identifier = "P";

                var Qvv = Q.DeepCopy() &
                          new LessThanOrEqualTo(zero, new Addition(i, 1)) &
                          new LessThanOrEqualTo(new Addition(i, 1), n) &
                          new IntegerTypeEqual(sum, sum3);
                Qvv.Identifier = "Q''";

                var assignment1 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (i, zero), (sum, zero)
                });

                var assignment2 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (sum, new Addition(sum, xip))
                });

                var assignment3 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (i, new Addition(i, 1))
                });


                return new[]
                {
                    new object[] { new Imply(Q.DeepCopy(), new WeakestPrecondition(assignment1.DeepCopy(), Qv.DeepCopy())) },
                    new object[] { new Imply(Qv.DeepCopy(), P.DeepCopy()) },
                    new object[] { new Imply(P.DeepCopy() & new NegationFormula(new IntegerTypeNotEqual(i, n)), R.DeepCopy()) },
                    new object[] { new Imply(P.DeepCopy(), new IntegerTypeNotEqual(i, n) | new NegationFormula(new IntegerTypeNotEqual(i, n))) },
                    new object[] { new Imply(P.DeepCopy() & new IntegerTypeNotEqual(i, n), new GreaterThan(new Subtraction(n, i), zero)) },
                    new object[] { new Imply(P.DeepCopy() & new IntegerTypeNotEqual(i, n) & new IntegerTypeEqual(new Subtraction(n, i), t0), new WeakestPrecondition(assignment2.DeepCopy(), Qvv.DeepCopy() & new IntegerTypeEqual(new Subtraction(n, i), t0))) },
                    new object[] { new Imply(Qvv.DeepCopy() & new IntegerTypeEqual(new Subtraction(n, i), t0), new WeakestPrecondition(assignment3, P.DeepCopy() & new LessThan(new Subtraction(n, i), t0))) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(SummationData))]
        public void SummationTest(Imply imply)
        {
            Assert.AreEqual(ImplyEvaluationResult.True, imply.Evaluated().EvaluationResult());
        }
    }
}
