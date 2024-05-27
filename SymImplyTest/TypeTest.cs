global using Type = SymImply.Types.Type;
using SymImply.Terms.Variables;
using SymImply.Types;

namespace SymImplyTest
{
    [TestClass]
    public class TypeTest
    {
        static IEnumerable<object[]> TypesData
        {
            get
            {
                Variable<IntegerType> n = new Variable<IntegerType>("n", NaturalNumber.Instance());

                return new[]
                {
                    new object[] { Logical.Instance() },
                    new object[] { Integer.Instance() },
                    new object[] { NaturalNumber.Instance() },
                    new object[] { PositiveInteger.Instance() },
                    new object[] { ZeroOrOne.Instance() },
                    new object[] { new ConstantBoundedInteger(10, 20) },
                    new object[] { new ConstantBoundedInteger(-23, 234) },
                    new object[] { new ConstantBoundedInteger(123, 65) },
                    new object[] { new ConstantBoundedInteger(-2, 12) },
                    new object[] { new TermBoundedInteger(0, n) },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(TypesData))]
        public void DeepCopyTest(Type type)
        {
            Assert.AreEqual(type, type.DeepCopy());
        }

        static IEnumerable<object[]> AdditionWithData
        {
            get
            {
                Variable<IntegerType> n = new Variable<IntegerType>("n", NaturalNumber.Instance());

                return new[]
                {
                    new object[] { Integer.Instance(), Integer.Instance(), Integer.Instance() },
                    new object[] { Integer.Instance(), NaturalNumber.Instance(), Integer.Instance() },
                    new object[] { Integer.Instance(), ZeroOrOne.Instance(), Integer.Instance() },
                    new object[] { PositiveInteger.Instance(), PositiveInteger.Instance(), PositiveInteger.Instance() },
                    new object[] { PositiveInteger.Instance(), NaturalNumber.Instance(), PositiveInteger.Instance() },
                    new object[] { PositiveInteger.Instance(), ZeroOrOne.Instance(), PositiveInteger.Instance() },
                    new object[] { NaturalNumber.Instance(), NaturalNumber.Instance(), NaturalNumber.Instance() },
                    new object[] { NaturalNumber.Instance(), ZeroOrOne.Instance(), NaturalNumber.Instance() },
                    new object[] { ZeroOrOne.Instance(), ZeroOrOne.Instance(), NaturalNumber.Instance() },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(AdditionWithData))]
        public void AdditionWithTest(IntegerType first, IntegerType second, IntegerType expectedResult)
        {
            Assert.AreEqual(expectedResult, first.AdditionWithType(second));
            Assert.AreEqual(expectedResult, second.AdditionWithType(first));
        }

        static IEnumerable<object[]> SubtractionWithData
        {
            get
            {
                Variable<IntegerType> n = new Variable<IntegerType>("n", NaturalNumber.Instance());

                return new[]
                {
                    new object[] { Integer.Instance(), Integer.Instance(), Integer.Instance() },
                    new object[] { Integer.Instance(), NaturalNumber.Instance(), Integer.Instance() },
                    new object[] { Integer.Instance(), ZeroOrOne.Instance(), Integer.Instance() },
                    new object[] { PositiveInteger.Instance(), PositiveInteger.Instance(), Integer.Instance() },
                    new object[] { PositiveInteger.Instance(), NaturalNumber.Instance(), Integer.Instance() },
                    new object[] { PositiveInteger.Instance(), ZeroOrOne.Instance(), NaturalNumber.Instance() },
                    new object[] { NaturalNumber.Instance(), NaturalNumber.Instance(), Integer.Instance() },
                    new object[] { NaturalNumber.Instance(), ZeroOrOne.Instance(), Integer.Instance() },
                    new object[] { ZeroOrOne.Instance(), ZeroOrOne.Instance(), Integer.Instance() },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(SubtractionWithData))]
        public void SubtractionWithTest(IntegerType first, IntegerType second, IntegerType expectedResult)
        {
            Assert.AreEqual(expectedResult, first.SubtractionWithType(second));
        }

        static IEnumerable<object[]> MultiplicationWithData
        {
            get
            {
                Variable<IntegerType> n = new Variable<IntegerType>("n", NaturalNumber.Instance());

                return new[]
                {
                    new object[] { Integer.Instance(), Integer.Instance(), Integer.Instance() },
                    new object[] { Integer.Instance(), NaturalNumber.Instance(), Integer.Instance() },
                    new object[] { Integer.Instance(), ZeroOrOne.Instance(), Integer.Instance() },
                    new object[] { PositiveInteger.Instance(), PositiveInteger.Instance(), PositiveInteger.Instance() },
                    new object[] { PositiveInteger.Instance(), NaturalNumber.Instance(), NaturalNumber.Instance() },
                    new object[] { PositiveInteger.Instance(), ZeroOrOne.Instance(), NaturalNumber.Instance() },
                    new object[] { NaturalNumber.Instance(), NaturalNumber.Instance(), NaturalNumber.Instance() },
                    new object[] { NaturalNumber.Instance(), ZeroOrOne.Instance(), NaturalNumber.Instance() },
                    new object[] { ZeroOrOne.Instance(), ZeroOrOne.Instance(), ZeroOrOne.Instance() },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(MultiplicationWithData))]
        public void MultiplicationWithTest(IntegerType first, IntegerType second, IntegerType expectedResult)
        {
            Assert.AreEqual(expectedResult, first.MultiplicationWithType(second));
        }

        static IEnumerable<object[]> ValidValuesData
        {
            get
            {
                return new[]
                {
                    new object[] { Integer.Instance(), -23 },
                    new object[] { Integer.Instance(), 182 },
                    new object[] { Integer.Instance(), 234 },
                    new object[] { Integer.Instance(), 123243 },
                    new object[] { PositiveInteger.Instance(), 324 },
                    new object[] { PositiveInteger.Instance(), 1 },
                    new object[] { NaturalNumber.Instance(), 0 },
                    new object[] { NaturalNumber.Instance(), 123 },
                    new object[] { NaturalNumber.Instance(), 34 },
                    new object[] { ZeroOrOne.Instance(), 0 },
                    new object[] { ZeroOrOne.Instance(), 1 }
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ValidValuesData))]
        public void ValidValuesTest(IntegerType integerType, int value)
        {
            Assert.IsTrue( integerType.IsValueValid(value));
            Assert.IsFalse(integerType.IsValueOutOfRange(value));
        }

        static IEnumerable<object[]> InvalidValuesData
        {
            get
            {
                return new[]
                {
                    new object[] { PositiveInteger.Instance(), 0 },
                    new object[] { PositiveInteger.Instance(), -43 },
                    new object[] { PositiveInteger.Instance(), -234 },
                    new object[] { NaturalNumber.Instance(), -1 },
                    new object[] { NaturalNumber.Instance(), -123 },
                    new object[] { NaturalNumber.Instance(), -34 },
                    new object[] { ZeroOrOne.Instance(), 2 },
                    new object[] { ZeroOrOne.Instance(), -1 },
                    new object[] { ZeroOrOne.Instance(), 1234 },
                    new object[] { ZeroOrOne.Instance(), -11234 }
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(InvalidValuesData))]
        public void ValueOutOfRangeTest(IntegerType integerType, int value)
        {
            Assert.IsTrue(integerType.IsValueOutOfRange(value));
            Assert.IsFalse(integerType.IsValueValid(value));
        }
    }
}
