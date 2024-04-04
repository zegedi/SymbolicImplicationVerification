using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Formulas.Operations;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Programs;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymImplTest
{
    [TestClass]
    public class FormulaTests
    {
        static IEnumerable<object[]> FormulaEvaluatedData
        {
            get
            {
                var x = new IntegerTypeVariable("x", Integer.Instance());
                var y = new IntegerTypeVariable("y", Integer.Instance());

                var zero  = new IntegerConstant(0);
                var one   = new IntegerConstant(1);
                var two   = new IntegerConstant(2);
                var three = new IntegerConstant(3);
                var four  = new IntegerConstant(4);
                var six   = new IntegerConstant(6);

                var expr1 = x + y;
                var expr2 = y + x;

                var expr3 = two * x + y;
                var expr4 = x + y + x;
                var expr5 = x + four * y + x - three * y;
                var expr6 = three * x + y - x;

                return new[]
                {
                    new object[] { FALSE.Instance(), FALSE.Instance() },
                    new object[] { TRUE .Instance(), TRUE .Instance() },
                    new object[] { NotEvaluable.Instance(), NotEvaluable.Instance() },

                    new object[] { new IntegerTypeEqual(one, one) , TRUE .Instance() },
                    new object[] { new IntegerTypeEqual(one, zero), FALSE.Instance() },
                    new object[] { new IntegerTypeEqual(x, x)     , TRUE .Instance() },

                    new object[] { new IntegerTypeEqual(expr1, expr1), TRUE.Instance() },
                    new object[] { new IntegerTypeEqual(expr1, expr2), TRUE.Instance() },
                    new object[] { new IntegerTypeEqual(expr3, expr4), TRUE.Instance() },
                    new object[] { new IntegerTypeEqual(expr5, expr6), TRUE.Instance() },

                    new object[] { new IntegerTypeEqual(expr1, expr3), new IntegerTypeEqual(expr1, expr3) },
                    new object[] { new IntegerTypeEqual(expr1, expr4), new IntegerTypeEqual(expr1, expr3) },
                    new object[] { new IntegerTypeEqual(expr1, expr5), new IntegerTypeEqual(expr1, expr3) },
                    new object[] { new IntegerTypeEqual(expr1, expr6), new IntegerTypeEqual(expr1, expr3) },
                    new object[] { new IntegerTypeEqual(expr2, expr3), new IntegerTypeEqual(expr1, expr3) },
                    new object[] { new IntegerTypeEqual(expr2, expr4), new IntegerTypeEqual(expr1, expr3) },
                    new object[] { new IntegerTypeEqual(expr2, expr5), new IntegerTypeEqual(expr1, expr3) },
                    new object[] { new IntegerTypeEqual(expr2, expr6), new IntegerTypeEqual(expr1, expr3) },

                    new object[] { new IntegerTypeNotEqual(one, one) , FALSE.Instance() },
                    new object[] { new IntegerTypeNotEqual(one, zero), TRUE .Instance() },
                    new object[] { new IntegerTypeNotEqual(x, x)     , FALSE.Instance() },

                    new object[] { new IntegerTypeNotEqual(expr1, expr1), FALSE.Instance() },
                    new object[] { new IntegerTypeNotEqual(expr1, expr2), FALSE.Instance() },
                    new object[] { new IntegerTypeNotEqual(expr3, expr4), FALSE.Instance() },
                    new object[] { new IntegerTypeNotEqual(expr5, expr6), FALSE.Instance() },

                    new object[] { new IntegerTypeNotEqual(expr1, expr3), new IntegerTypeNotEqual(expr1, expr3) },
                    new object[] { new IntegerTypeNotEqual(expr1, expr4), new IntegerTypeNotEqual(expr1, expr3) },
                    new object[] { new IntegerTypeNotEqual(expr1, expr5), new IntegerTypeNotEqual(expr1, expr3) },
                    new object[] { new IntegerTypeNotEqual(expr1, expr6), new IntegerTypeNotEqual(expr1, expr3) },
                    new object[] { new IntegerTypeNotEqual(expr2, expr3), new IntegerTypeNotEqual(expr1, expr3) },
                    new object[] { new IntegerTypeNotEqual(expr2, expr4), new IntegerTypeNotEqual(expr1, expr3) },
                    new object[] { new IntegerTypeNotEqual(expr2, expr5), new IntegerTypeNotEqual(expr1, expr3) },
                    new object[] { new IntegerTypeNotEqual(expr2, expr6), new IntegerTypeNotEqual(expr1, expr3) },

                    new object[] { new LessThan(zero, one), TRUE .Instance() },
                    new object[] { new LessThan(one, four), TRUE .Instance() },
                    new object[] { new LessThan(one, zero), FALSE.Instance() },
                    new object[] { new LessThan(one,  one), FALSE.Instance() },
                    new object[] { new LessThan(four, one), FALSE.Instance() },

                    new object[] { new LessThan(x, x)        , FALSE.Instance() },
                    new object[] { new LessThan(expr1, expr1), FALSE.Instance() },
                    new object[] { new LessThan(expr1, expr2), FALSE.Instance() },
                    new object[] { new LessThan(expr3, expr4), FALSE.Instance() },
                    new object[] { new LessThan(expr5, expr6), FALSE.Instance() },

                    new object[] { new LessThan(expr1, expr3), new LessThan(expr1, expr3) },
                    new object[] { new LessThan(expr1, expr4), new LessThan(expr1, expr3) },
                    new object[] { new LessThan(expr1, expr5), new LessThan(expr1, expr3) },
                    new object[] { new LessThan(expr1, expr6), new LessThan(expr1, expr3) },
                    new object[] { new LessThan(expr2, expr3), new LessThan(expr1, expr3) },
                    new object[] { new LessThan(expr2, expr4), new LessThan(expr1, expr3) },
                    new object[] { new LessThan(expr2, expr5), new LessThan(expr1, expr3) },
                    new object[] { new LessThan(expr2, expr6), new LessThan(expr1, expr3) },

                    new object[] { new GreaterThan(zero, one), FALSE.Instance() },
                    new object[] { new GreaterThan(one, four), FALSE.Instance() },
                    new object[] { new GreaterThan(one, zero), TRUE .Instance() },
                    new object[] { new GreaterThan(one,  one), FALSE.Instance() },
                    new object[] { new GreaterThan(four, one), TRUE .Instance() },

                    new object[] { new GreaterThan(x, x)        , FALSE.Instance() },
                    new object[] { new GreaterThan(expr1, expr1), FALSE.Instance() },
                    new object[] { new GreaterThan(expr1, expr2), FALSE.Instance() },
                    new object[] { new GreaterThan(expr3, expr4), FALSE.Instance() },
                    new object[] { new GreaterThan(expr5, expr6), FALSE.Instance() },

                    new object[] { new GreaterThan(expr1, expr3), new GreaterThan(expr1, expr3) },
                    new object[] { new GreaterThan(expr1, expr4), new GreaterThan(expr1, expr3) },
                    new object[] { new GreaterThan(expr1, expr5), new GreaterThan(expr1, expr3) },
                    new object[] { new GreaterThan(expr1, expr6), new GreaterThan(expr1, expr3) },
                    new object[] { new GreaterThan(expr2, expr3), new GreaterThan(expr1, expr3) },
                    new object[] { new GreaterThan(expr2, expr4), new GreaterThan(expr1, expr3) },
                    new object[] { new GreaterThan(expr2, expr5), new GreaterThan(expr1, expr3) },
                    new object[] { new GreaterThan(expr2, expr6), new GreaterThan(expr1, expr3) },

                    new object[] { new LessThanOrEqualTo(zero, one), TRUE .Instance() },
                    new object[] { new LessThanOrEqualTo(one, four), TRUE .Instance() },
                    new object[] { new LessThanOrEqualTo(one, zero), FALSE.Instance() },
                    new object[] { new LessThanOrEqualTo(one,  one), TRUE .Instance() },
                    new object[] { new LessThanOrEqualTo(four, one), FALSE.Instance() },

                    new object[] { new LessThanOrEqualTo(x, x)        , TRUE.Instance() },
                    new object[] { new LessThanOrEqualTo(expr1, expr1), TRUE.Instance() },
                    new object[] { new LessThanOrEqualTo(expr1, expr2), TRUE.Instance() },
                    new object[] { new LessThanOrEqualTo(expr3, expr4), TRUE.Instance() },
                    new object[] { new LessThanOrEqualTo(expr5, expr6), TRUE.Instance() },

                    new object[] { new LessThanOrEqualTo(expr1, expr3), new LessThanOrEqualTo(expr1, expr3) },
                    new object[] { new LessThanOrEqualTo(expr1, expr4), new LessThanOrEqualTo(expr1, expr3) },
                    new object[] { new LessThanOrEqualTo(expr1, expr5), new LessThanOrEqualTo(expr1, expr3) },
                    new object[] { new LessThanOrEqualTo(expr1, expr6), new LessThanOrEqualTo(expr1, expr3) },
                    new object[] { new LessThanOrEqualTo(expr2, expr3), new LessThanOrEqualTo(expr1, expr3) },
                    new object[] { new LessThanOrEqualTo(expr2, expr4), new LessThanOrEqualTo(expr1, expr3) },
                    new object[] { new LessThanOrEqualTo(expr2, expr5), new LessThanOrEqualTo(expr1, expr3) },
                    new object[] { new LessThanOrEqualTo(expr2, expr6), new LessThanOrEqualTo(expr1, expr3) },

                    new object[] { new GreaterThanOrEqualTo(zero, one), FALSE.Instance() },
                    new object[] { new GreaterThanOrEqualTo(one, four), FALSE.Instance() },
                    new object[] { new GreaterThanOrEqualTo(one, zero), TRUE .Instance() },
                    new object[] { new GreaterThanOrEqualTo(one,  one), TRUE .Instance() },
                    new object[] { new GreaterThanOrEqualTo(four, one), TRUE .Instance() },

                    new object[] { new GreaterThanOrEqualTo(x, x)        , TRUE.Instance() },
                    new object[] { new GreaterThanOrEqualTo(expr1, expr1), TRUE.Instance() },
                    new object[] { new GreaterThanOrEqualTo(expr1, expr2), TRUE.Instance() },
                    new object[] { new GreaterThanOrEqualTo(expr3, expr4), TRUE.Instance() },
                    new object[] { new GreaterThanOrEqualTo(expr5, expr6), TRUE.Instance() },

                    new object[] { new GreaterThanOrEqualTo(expr1, expr3), new GreaterThanOrEqualTo(expr1, expr3) },
                    new object[] { new GreaterThanOrEqualTo(expr1, expr4), new GreaterThanOrEqualTo(expr1, expr3) },
                    new object[] { new GreaterThanOrEqualTo(expr1, expr5), new GreaterThanOrEqualTo(expr1, expr3) },
                    new object[] { new GreaterThanOrEqualTo(expr1, expr6), new GreaterThanOrEqualTo(expr1, expr3) },
                    new object[] { new GreaterThanOrEqualTo(expr2, expr3), new GreaterThanOrEqualTo(expr1, expr3) },
                    new object[] { new GreaterThanOrEqualTo(expr2, expr4), new GreaterThanOrEqualTo(expr1, expr3) },
                    new object[] { new GreaterThanOrEqualTo(expr2, expr5), new GreaterThanOrEqualTo(expr1, expr3) },
                    new object[] { new GreaterThanOrEqualTo(expr2, expr6), new GreaterThanOrEqualTo(expr1, expr3) },

                    new object[] { new Divisor(zero,  one),  NotEvaluable.Instance() },
                    new object[] { new Divisor(zero,    x),  NotEvaluable.Instance() },
                    new object[] { new Divisor(one,  four),  TRUE .Instance() },
                    new object[] { new Divisor(one,     x),  TRUE .Instance() },
                    new object[] { new Divisor(two,   one),  FALSE.Instance() },
                    new object[] { new Divisor(two,   two),  TRUE .Instance() },
                    new object[] { new Divisor(two, three),  FALSE.Instance() },
                    new object[] { new Divisor(two,  four),  TRUE .Instance() },
                    new object[] { new Divisor(two,   six),  TRUE .Instance() },
                    new object[] { new Divisor(three, zero), TRUE .Instance() },
                    new object[] { new Divisor(three, six),  TRUE .Instance() },
                    new object[] { new Divisor(three, one),  FALSE.Instance() },
                    new object[] { new Divisor(four,  one),  FALSE.Instance() },

                    new object[] { new Divisor(    x, x)    , TRUE.Instance() },
                    new object[] { new Divisor(expr1, expr1), TRUE.Instance() },
                    new object[] { new Divisor(expr1, expr2), TRUE.Instance() },
                    new object[] { new Divisor(expr3, expr4), TRUE.Instance() },
                    new object[] { new Divisor(expr5, expr6), TRUE.Instance() },

                    new object[] { new Divisor(expr1, expr3), new Divisor(expr1, expr3) },
                    new object[] { new Divisor(expr1, expr4), new Divisor(expr1, expr3) },
                    new object[] { new Divisor(expr1, expr5), new Divisor(expr1, expr3) },
                    new object[] { new Divisor(expr1, expr6), new Divisor(expr1, expr3) },
                    new object[] { new Divisor(expr2, expr3), new Divisor(expr1, expr3) },
                    new object[] { new Divisor(expr2, expr4), new Divisor(expr1, expr3) },
                    new object[] { new Divisor(expr2, expr5), new Divisor(expr1, expr3) },
                    new object[] { new Divisor(expr2, expr6), new Divisor(expr1, expr3) },

                    new object[] { new NotDivisor(zero,  one),  NotEvaluable.Instance() },
                    new object[] { new NotDivisor(zero,    x),  NotEvaluable.Instance() },
                    new object[] { new NotDivisor(one,  four),  FALSE.Instance() },
                    new object[] { new NotDivisor(one,     x),  FALSE.Instance() },
                    new object[] { new NotDivisor(two,   one),  TRUE .Instance() },
                    new object[] { new NotDivisor(two,   two),  FALSE.Instance() },
                    new object[] { new NotDivisor(two, three),  TRUE .Instance() },
                    new object[] { new NotDivisor(two,  four),  FALSE.Instance() },
                    new object[] { new NotDivisor(two,   six),  FALSE.Instance() },
                    new object[] { new NotDivisor(three, zero), FALSE.Instance() },
                    new object[] { new NotDivisor(three, six),  FALSE.Instance() },
                    new object[] { new NotDivisor(three, one),  TRUE .Instance() },
                    new object[] { new NotDivisor(four,  one),  TRUE .Instance() },

                    new object[] { new NotDivisor(    x, x)    , FALSE.Instance() },
                    new object[] { new NotDivisor(expr1, expr1), FALSE.Instance() },
                    new object[] { new NotDivisor(expr1, expr2), FALSE.Instance() },
                    new object[] { new NotDivisor(expr3, expr4), FALSE.Instance() },
                    new object[] { new NotDivisor(expr5, expr6), FALSE.Instance() },

                    new object[] { new NotDivisor(expr1, expr4), new NotDivisor(expr1, expr3) },
                    new object[] { new NotDivisor(expr1, expr5), new NotDivisor(expr1, expr3) },
                    new object[] { new NotDivisor(expr1, expr6), new NotDivisor(expr1, expr3) },
                    new object[] { new NotDivisor(expr2, expr3), new NotDivisor(expr1, expr3) },
                    new object[] { new NotDivisor(expr2, expr4), new NotDivisor(expr1, expr3) },
                    new object[] { new NotDivisor(expr2, expr5), new NotDivisor(expr1, expr3) },
                    new object[] { new NotDivisor(expr2, expr6), new NotDivisor(expr1, expr3) },

                    new object[] { new NegationFormula(FALSE.Instance()), TRUE .Instance() },
                    new object[] { new NegationFormula(TRUE .Instance()), FALSE.Instance() },
                    new object[] { new NegationFormula(NotEvaluable.Instance()), NotEvaluable.Instance() },

                    new object[] { new NegationFormula(new IntegerTypeEqual(expr2, expr5)), new IntegerTypeNotEqual(expr1, expr3) },
                    new object[] { new NegationFormula(new IntegerTypeNotEqual(expr2, expr5)), new IntegerTypeEqual(expr1, expr3) },
                    new object[] { new NegationFormula(new LessThan(expr2, expr5)) , new GreaterThanOrEqualTo(expr1, expr3) },
                    new object[] { new NegationFormula(new LessThanOrEqualTo(expr2, expr5)) , new GreaterThan(expr1, expr3) },
                    new object[] { new NegationFormula(new GreaterThan(expr2, expr5)) , new LessThanOrEqualTo(expr1, expr3) },
                    new object[] { new NegationFormula(new GreaterThanOrEqualTo(expr2, expr5)) , new LessThan(expr1, expr3) },
                    new object[] { new NegationFormula(new Divisor(expr2, expr5)) , new NotDivisor(expr1, expr3) },
                    new object[] { new NegationFormula(new NotDivisor(expr2, expr5)) , new Divisor(expr1, expr3) },

                    new object[] { new ConjunctionFormula(FALSE.Instance(), FALSE.Instance()), FALSE.Instance() },
                    new object[] { new ConjunctionFormula(TRUE .Instance(), FALSE.Instance()), FALSE.Instance() },
                    new object[] { new ConjunctionFormula(FALSE.Instance(), TRUE .Instance()), FALSE.Instance() },
                    new object[] { new ConjunctionFormula(TRUE .Instance(), TRUE .Instance()), TRUE .Instance() },

                    new object[] { new DisjunctionFormula(FALSE.Instance(), FALSE.Instance()), FALSE.Instance() },
                    new object[] { new DisjunctionFormula(TRUE .Instance(), FALSE.Instance()), TRUE .Instance() },
                    new object[] { new DisjunctionFormula(FALSE.Instance(), TRUE .Instance()), TRUE .Instance() },
                    new object[] { new DisjunctionFormula(TRUE .Instance(), TRUE .Instance()), TRUE .Instance() },

                    new object[] { new ImplicationFormula(FALSE.Instance(), FALSE.Instance()), TRUE .Instance() },
                    new object[] { new ImplicationFormula(TRUE .Instance(), FALSE.Instance()), FALSE.Instance() },
                    new object[] { new ImplicationFormula(FALSE.Instance(), TRUE .Instance()), TRUE .Instance() },
                    new object[] { new ImplicationFormula(TRUE .Instance(), TRUE .Instance()), TRUE .Instance() },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(FormulaEvaluatedData))]
        public void FormulaEvaluatedTest(Formula formulaToEvaluate, Formula expectedResult)
        {
            Assert.AreEqual(expectedResult, formulaToEvaluate.Evaluated());
        }

        static IEnumerable<object[]> WeakestPreconditionEvaluatedData
        {
            get
            {
                IntegerTypeVariable x = new IntegerTypeVariable("x", Integer.Instance());
                IntegerTypeVariable y = new IntegerTypeVariable("y", Integer.Instance());
                IntegerTypeVariable n = new IntegerTypeVariable("n", NaturalNumber.Instance());

                Term<IntegerType> zero = new IntegerConstant(0);
                Term<IntegerType> one  = new IntegerConstant(1);

                var statement1 = new IntegerTypeEqual(x, zero);
                var statement2 = new IntegerTypeEqual(y, one);
                var statement3 = new ConjunctionFormula(statement1, statement2);
                var statement4 = new ConjunctionFormula(new IntegerTypeEqual(y, zero), 
                                                        new IntegerTypeEqual(x, one));

                var zeroEqualsZero = new IntegerTypeEqual(zero, zero);
                var oneEqualsOne   = new IntegerTypeEqual(one, one);
                var zeroEqualsZeroAndOneEqualsOne = new ConjunctionFormula(zeroEqualsZero, oneEqualsOne);

                var assignment1 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (x, zero),
                });

                var assignment2 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (y, one)
                });

                var assignment3 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (x, zero),
                    (y, one)
                });

                var assignment4 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (x, y),
                    (y, x)
                });

                var assignment5 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (n, n + 1)
                });

                var assignment6 = new Assignment(new List<(Variable<IntegerType>, Term<IntegerType>)>()
                {
                    (n, n - 5)
                });

                return new[]
                {
                    new object[] { ABORT.Instance(), TRUE .Instance(), FALSE.Instance() },
                    new object[] { ABORT.Instance(), FALSE.Instance(), FALSE.Instance() },
                    new object[] { ABORT.Instance(), NotEvaluable.Instance(), FALSE.Instance() },
                    new object[] { ABORT.Instance(), statement1, FALSE.Instance() },
                    new object[] { ABORT.Instance(), statement2, FALSE.Instance() },
                    new object[] { ABORT.Instance(), statement3, FALSE.Instance() },

                    new object[] { SKIP .Instance(), FALSE.Instance(), FALSE.Instance() },
                    new object[] { assignment1     , FALSE.Instance(), FALSE.Instance() },
                    new object[] { assignment2     , FALSE.Instance(), FALSE.Instance() },
                    new object[] { assignment3     , FALSE.Instance(), FALSE.Instance() },

                    new object[] { SKIP .Instance(), NotEvaluable.Instance(), FALSE.Instance() },
                    new object[] { assignment1     , NotEvaluable.Instance(), FALSE.Instance() },
                    new object[] { assignment2     , NotEvaluable.Instance(), FALSE.Instance() },
                    new object[] { assignment3     , NotEvaluable.Instance(), FALSE.Instance() },

                    new object[] { SKIP.Instance(), TRUE .Instance(), TRUE .Instance() },
                    new object[] { SKIP.Instance(), FALSE.Instance(), FALSE.Instance() },
                    new object[] { SKIP.Instance(), statement1, statement1 },
                    new object[] { SKIP.Instance(), statement2, statement2 },
                    new object[] { SKIP.Instance(), statement3, statement3 },

                    new object[] { assignment1, statement1, zeroEqualsZero },
                    new object[] { assignment2, statement2, oneEqualsOne   },
                    new object[] { assignment3, statement3, zeroEqualsZeroAndOneEqualsOne },
                    new object[] { assignment4, statement3, statement4 },
                    new object[] { assignment3, statement2, oneEqualsOne },
                    new object[] { assignment3, statement1, zeroEqualsZero },

                    new object[] { assignment5, TRUE.Instance(), TRUE.Instance() },
                    new object[] { assignment6, TRUE.Instance(), new GreaterThanOrEqualTo(n - 5, zero) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(WeakestPreconditionEvaluatedData))]
        public void WeakestPreconditionEvaluatedTest(Program program, Formula statment, Formula expectedResult)
        {
            var wp = new WeakestPrecondition(program, statment);

            Assert.AreEqual(expectedResult, wp.Evaluated());
        }


        static IEnumerable<object[]> FormulasEquivalentData
        {
            get
            {
                var x = new IntegerTypeVariable("x", Integer.Instance());
                var y = new IntegerTypeVariable("y", Integer.Instance());

                var _x = new Multiplication(-1, x);
                var _y = new Multiplication(-1, y);

                var zero  = new IntegerConstant(0);
                var one   = new IntegerConstant(1);
                var two   = new IntegerConstant(2);
                var three = new IntegerConstant(3);
                var four  = new IntegerConstant(4);
                var six   = new IntegerConstant(6);

                var True  = new LogicalConstant(true);
                var False = new LogicalConstant(false);

                return new[]
                {
                    new object[] { FALSE.Instance(), FALSE.Instance() },
                    new object[] { TRUE .Instance(), TRUE .Instance() },
                    new object[] { NotEvaluable.Instance(), NotEvaluable.Instance() },

                    //////===========================================================================//
                    ////// IntegerType constant equalities:   1=1 <-> 0=0 <-> 0!=1 <-> 1!=0 <-> TRUE //
                    ////// IntegerType constant inequalities: 0<1 <-> 1>0 <-> 0<=1 <-> 1>=0 <-> TRUE //
                    //////===========================================================================//

                    //new object[] { new IntegerTypeEqual(one , one) , TRUE.Instance() },
                    //new object[] { new IntegerTypeEqual(zero, zero), TRUE.Instance() },
                    //new object[] { new IntegerTypeNotEqual(one , zero), TRUE.Instance() },
                    //new object[] { new IntegerTypeNotEqual(zero, one) , TRUE.Instance() },

                    //new object[] { new LessThan(zero, one)   , TRUE.Instance() },
                    //new object[] { new GreaterThan(one, zero), TRUE.Instance() },
                    //new object[] { new LessThanOrEqualTo(zero, one)   , TRUE.Instance() },
                    //new object[] { new LessThanOrEqualTo(zero, zero)  , TRUE.Instance() },
                    //new object[] { new GreaterThanOrEqualTo(one, zero), TRUE.Instance() },
                    //new object[] { new GreaterThanOrEqualTo(one, one ), TRUE.Instance() },

                    //new object[] { new IntegerTypeEqual(one, one), new IntegerTypeEqual(zero, zero)   },
                    //new object[] { new IntegerTypeEqual(one, one), new IntegerTypeNotEqual(zero, one) },
                    //new object[] { new IntegerTypeEqual(one, one), new LessThan(zero, one)            },
                    //new object[] { new IntegerTypeEqual(one, one), new LessThanOrEqualTo(zero, one)   },

                    ////======================================================================//
                    //// IntegerType constant divisors: 1|0 <-> 2|4 <-> 1|3 <-> 3!|4 <-> TRUE //
                    ////======================================================================//

                    //new object[] { new Divisor(one, zero) , TRUE.Instance() },
                    //new object[] { new Divisor(one, three), TRUE.Instance() },
                    //new object[] { new Divisor(two, four) , TRUE.Instance() },
                    //new object[] { new Divisor(two, six)  , TRUE.Instance() },
                    //new object[] { new Divisor(three, six), TRUE.Instance() },

                    //new object[] { new NotDivisor(one, zero) , FALSE.Instance() },
                    //new object[] { new NotDivisor(one, three), FALSE.Instance() },
                    //new object[] { new NotDivisor(two, four) , FALSE.Instance() },
                    //new object[] { new NotDivisor(two, six)  , FALSE.Instance() },
                    //new object[] { new NotDivisor(three, six), FALSE.Instance() },

                    //new object[] { new NotDivisor(two, three) , TRUE.Instance() },
                    //new object[] { new NotDivisor(three, one) , TRUE.Instance() },
                    //new object[] { new NotDivisor(three, four), TRUE.Instance() },

                    //new object[] { new Divisor(zero, one), NotEvaluable.Instance() },
                    //new object[] { new Divisor(zero, six), NotEvaluable.Instance() },
                    //new object[] { new Divisor(zero, x)  , NotEvaluable.Instance() },
                    //new object[] { new NotDivisor(zero, one), NotEvaluable.Instance() },
                    //new object[] { new NotDivisor(zero, six), NotEvaluable.Instance() },
                    //new object[] { new NotDivisor(zero, x)  , NotEvaluable.Instance() },


                    //==================================================================//
                    // IntegerType variable equalities: x=y <-> y=x <-> -x=-y <-> -y=-x //
                    //==================================================================//

                    new object[] { new IntegerTypeEqual(x, y), new IntegerTypeEqual(x, y)   },
                    new object[] { new IntegerTypeEqual(x, y), new IntegerTypeEqual(y, x)   },
                    new object[] { new IntegerTypeEqual(x, y), new IntegerTypeEqual(_x, _y) },
                    new object[] { new IntegerTypeEqual(x, y), new IntegerTypeEqual(_y, _x) },

                    //==================================================================//
                    // IntegerType variable inequalities: x>1 <-> y=x <-> -x=-y <-> -y=-x //
                    //==================================================================//

                    new object[] { new GreaterThan(x, one), new LessThan(one, x) },
                    new object[] { new GreaterThan(x, one), new LessThanOrEqualTo(two, x) },
                    new object[] { new GreaterThan(x, one), new GreaterThanOrEqualTo(x, two) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(FormulasEquivalentData))]
        public void FormulasEquivalentTest(Formula firstFormula, Formula secondFormula)
        {
            Assert.IsTrue(firstFormula.Equivalent(firstFormula));
            Assert.IsTrue(firstFormula.Equivalent(secondFormula));
            Assert.IsTrue(secondFormula.Equivalent(secondFormula));
            Assert.IsTrue(secondFormula.Equivalent(firstFormula));
        }

        static IEnumerable<object[]> FormulasNotEquivalentData
        {
            get
            {
                var x = new IntegerTypeVariable("x", Integer.Instance());
                var y = new IntegerTypeVariable("y", Integer.Instance());

                var _x = new Multiplication(-1, x);
                var _y = new Multiplication(-1, y);

                var one  = new IntegerConstant(1);
                var zero = new IntegerConstant(0);

                var True  = new LogicalConstant(true);
                var False = new LogicalConstant(false);

                return new[]
                {
                    new object[] { TRUE .Instance(), FALSE.Instance() },
                    new object[] { FALSE.Instance(), NotEvaluable.Instance() },
                    new object[] { TRUE .Instance(), NotEvaluable.Instance() },

                    new object[] { new IntegerTypeEqual(one, one), FALSE.Instance() },
                    new object[] { new IntegerTypeEqual(one, one), new GreaterThan(zero, one) },
                    new object[] { new IntegerTypeEqual(one, one), new GreaterThanOrEqualTo(zero, one) },

                    new object[] { new IntegerTypeEqual(x, y), TRUE .Instance() },
                    new object[] { new IntegerTypeEqual(x, y), FALSE.Instance() },
                    new object[] { new IntegerTypeEqual(x, y), NotEvaluable.Instance() },

                    new object[] { new IntegerTypeEqual(x, y), new IntegerTypeEqual(x, _y) },
                    new object[] { new IntegerTypeEqual(x, y), new IntegerTypeEqual(_x, y) },
                    new object[] { new IntegerTypeEqual(x, y), new IntegerTypeNotEqual(x, y)  },
                    new object[] { new IntegerTypeEqual(x, y), new LessThan(x, y)             },
                    new object[] { new IntegerTypeEqual(x, y), new GreaterThan(x, y)          },
                    new object[] { new IntegerTypeEqual(x, y), new LessThanOrEqualTo(x, y)    },
                    new object[] { new IntegerTypeEqual(x, y), new GreaterThanOrEqualTo(x, y) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(FormulasNotEquivalentData))]
        public void FormulasNotEquivalentTest(Formula firstFormula, Formula secondFormula)
        {
            Assert.IsFalse(firstFormula.Equivalent(secondFormula));
            Assert.IsFalse(secondFormula.Equivalent(firstFormula));
        }

        static IEnumerable<object[]> ConjunctionFormulaEquivalentData
        {
            get
            {
                var x = new IntegerTypeVariable("x", Integer.Instance());
                var y = new IntegerTypeVariable("y", Integer.Instance());

                return new[]
                {
                    new object[] { new IntegerTypeEqual(x, y), new LessThan(x, y), FALSE.Instance() },
                    new object[] { new IntegerTypeEqual(y, x), new LessThan(x, y), FALSE.Instance() },
                    new object[] { new IntegerTypeEqual(x, y), new LessThanOrEqualTo(x, y), new IntegerTypeEqual(x, y) },
                    new object[] { new IntegerTypeEqual(y, x), new LessThanOrEqualTo(x, y), new IntegerTypeEqual(y, x) },

                    new object[] { new IntegerTypeNotEqual(x, y), new LessThan(x, y), new LessThan(x, y) },
                    new object[] { new IntegerTypeNotEqual(y, x), new LessThan(x, y), new LessThan(x, y) },
                    new object[] { new IntegerTypeNotEqual(x, y), new LessThanOrEqualTo(x, y), new LessThan(x, y) },
                    new object[] { new IntegerTypeNotEqual(y, x), new LessThanOrEqualTo(x, y), new LessThan(x, y) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConjunctionFormulaEquivalentData))]
        public void ConjunctionFormulaEquivalentTest(
            BinaryRelationFormula<IntegerType> firstFormula,
            BinaryRelationFormula<IntegerType> secondFormula, Formula expectedResult)
        {
            Assert.IsTrue(expectedResult.Equivalent(firstFormula.ConjunctionWith(secondFormula)));
            Assert.IsTrue(expectedResult.Equivalent(secondFormula.ConjunctionWith(firstFormula)));
        }

        //static IEnumerable<object[]> MindenSzamNoveleseEggyel
        //{
        //    get
        //    {
        //        var x = new IntegerTypeVariable("x", Integer.Instance());
        //        var y = new IntegerTypeVariable("y", Integer.Instance());

        //        return new[]
        //        {
        //            new object[] { new IntegerTypeEqual(x, y), new LessThan(x, y), FALSE.Instance() },
        //            new object[] { new IntegerTypeEqual(y, x), new LessThan(x, y), FALSE.Instance() },
        //            new object[] { new IntegerTypeEqual(x, y), new LessThanOrEqualTo(x, y), new IntegerTypeEqual(x, y) },
        //            new object[] { new IntegerTypeEqual(y, x), new LessThanOrEqualTo(x, y), new IntegerTypeEqual(y, x) },

        //            new object[] { new IntegerTypeNotEqual(x, y), new LessThan(x, y), new LessThan(x, y) },
        //            new object[] { new IntegerTypeNotEqual(y, x), new LessThan(x, y), new LessThan(x, y) },
        //            new object[] { new IntegerTypeNotEqual(x, y), new LessThanOrEqualTo(x, y), new LessThan(x, y) },
        //            new object[] { new IntegerTypeNotEqual(y, x), new LessThanOrEqualTo(x, y), new LessThan(x, y) },
        //        };
        //    }
        //}
    }
}
