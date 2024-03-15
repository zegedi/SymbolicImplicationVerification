using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymImplTest
{
    [TestClass]
    public class SymplificationTest
    {
        [DataTestMethod]
        [DataRow(   3,   4,   7)]
        [DataRow(  12,   3,   5)]
        [DataRow(   0,  -8,  65)]
        [DataRow( 234,  94,  42)]
        [DataRow(-122, 852,  19)]
        [DataRow( 288,  23, 196)]
        public void IntegerAdditionEvaluatedTest(int constA, int constB, int constC)
        {
            IntegerConstant A = constA;
            IntegerConstant B = constB;
            IntegerConstant C = constC;

            var expression1 = (A + B) + C;
            var expression2 = (A + C) + B;
            var expression3 = (B + A) + C;
            var expression4 = (B + C) + A;
            var expression5 = (C + A) + B;
            var expression6 = (C + B) + A;

            var expression7  = A + (B + C);
            var expression8  = A + (C + B);
            var expression9  = B + (A + C);
            var expression10 = B + (C + A);
            var expression11 = C + (A + B);
            var expression12 = C + (B + A);

            var expectedResult = new IntegerConstant(constA + constB + constC);

            Assert.AreEqual(expectedResult, expression1.Evaluated());
            Assert.AreEqual(expectedResult, expression2.Evaluated());
            Assert.AreEqual(expectedResult, expression3.Evaluated());
            Assert.AreEqual(expectedResult, expression4.Evaluated());
            Assert.AreEqual(expectedResult, expression5.Evaluated());
            Assert.AreEqual(expectedResult, expression6.Evaluated());
            Assert.AreEqual(expectedResult, expression7.Evaluated());
            Assert.AreEqual(expectedResult, expression8.Evaluated());
            Assert.AreEqual(expectedResult, expression9.Evaluated());
            Assert.AreEqual(expectedResult, expression10.Evaluated());
            Assert.AreEqual(expectedResult, expression11.Evaluated());
            Assert.AreEqual(expectedResult, expression12.Evaluated());
        }

        [DataTestMethod]
        [DataRow(   3,   4,   7)]
        [DataRow(  12,   3,   5)]
        [DataRow(   0,  -8,  65)]
        [DataRow( 234,  94,  42)]
        [DataRow(-122, 852,  19)]
        [DataRow( 288,  23, 196)]
        public void IntegerSubtractionEvaluatedTest(int constA, int constB, int constC)
        {
            IntegerConstant A = constA;
            IntegerConstant B = constB;
            IntegerConstant C = constC;

            var expression1 = (A - B) - C;
            var expression2 =  A - (B + C);

            var expectedResult = new IntegerConstant(constA - constB - constC);

            Assert.AreEqual(expectedResult, expression1.Evaluated());
            Assert.AreEqual(expectedResult, expression2.Evaluated());
        }

        [DataTestMethod]
        [DataRow(   3,   4,   7)]
        [DataRow(  12,   3,   5)]
        [DataRow(   0,  -8,  65)]
        [DataRow( 234,  94,  42)]
        [DataRow(-122, 852,  19)]
        [DataRow( 288,  23, 196)]
        public void IntegerMultiplicationEvaluatedTest(int constA, int constB, int constC)
        {
            IntegerConstant A = constA;
            IntegerConstant B = constB;
            IntegerConstant C = constC;

            var expression1  = (A * B) * C;
            var expression2  = (A * C) * B;
            var expression3  = (B * A) * C;
            var expression4  = (B * C) * A;
            var expression5  = (C * A) * B;
            var expression6  = (C * B) * A;

            var expression7  = A * (B * C);
            var expression8  = A * (C * B);
            var expression9  = B * (A * C);
            var expression10 = B * (C * A);
            var expression11 = C * (A * B);
            var expression12 = C * (B * A);

            var expectedResult = new IntegerConstant(constA * constB * constC);

            Assert.AreEqual(expectedResult, expression1.Evaluated());
            Assert.AreEqual(expectedResult, expression2.Evaluated());
            Assert.AreEqual(expectedResult, expression3.Evaluated());
            Assert.AreEqual(expectedResult, expression4.Evaluated());
            Assert.AreEqual(expectedResult, expression5.Evaluated());
            Assert.AreEqual(expectedResult, expression6.Evaluated());
            Assert.AreEqual(expectedResult, expression7.Evaluated());
            Assert.AreEqual(expectedResult, expression8.Evaluated());
            Assert.AreEqual(expectedResult, expression9.Evaluated());
            Assert.AreEqual(expectedResult, expression10.Evaluated());
            Assert.AreEqual(expectedResult, expression11.Evaluated());
            Assert.AreEqual(expectedResult, expression12.Evaluated());
        }

        [DataTestMethod]
        [DataRow( 3,   4)]
        [DataRow(-9,  31)]
        [DataRow(62, -83)]
        [DataRow(53,  81)]
        [DataRow( 0,  22)]
        [DataRow(-1,   0)]
        public void MultiplicationOrderingTest(int constA, int constB)
        {
            var x = new IntegerTypeVariable("x", Integer.Instance());
            var y = new IntegerTypeVariable("y", Integer.Instance());

            IntegerConstant A = constA;
            IntegerConstant B = constB;

            var expression1 = A * B * x * y;
            var expression2 = A * B * y * x;
            var expression3 = A * x * B * y;
            var expression4 = A * y * B * x;
            var expression5 = A * x * y * B;
            var expression6 = A * y * x * B;

            var expression7  = B * A * x * y;
            var expression8  = B * A * y * x;
            var expression9  = B * x * A * y;
            var expression10 = B * y * A * x;
            var expression11 = B * x * y * A;
            var expression12 = B * y * x * A;

            var expression13 = x * A * B * y;
            var expression14 = x * A * y * B;
            var expression15 = x * B * A * y;
            var expression16 = x * B * y * A;
            var expression17 = x * y * A * B;
            var expression18 = x * y * B * A;

            var expression19 = y * A * B * x;
            var expression20 = y * A * x * B;
            var expression21 = y * B * A * x;
            var expression22 = y * B * x * A;
            var expression23 = y * x * A * B;
            var expression24 = y * x * B * A;

            var expectedExpression = (IntegerConstant)(constA * constB) * x * y;
            var expectedEvaluation = expectedExpression.Evaluated();

            Assert.AreEqual(expectedEvaluation, expression1.Simplified());
            Assert.AreEqual(expectedEvaluation, expression2.Simplified());
            Assert.AreEqual(expectedEvaluation, expression3.Simplified());
            Assert.AreEqual(expectedEvaluation, expression4.Simplified());
            Assert.AreEqual(expectedEvaluation, expression5.Simplified());
            Assert.AreEqual(expectedEvaluation, expression6.Simplified());
            Assert.AreEqual(expectedEvaluation, expression7.Simplified());
            Assert.AreEqual(expectedEvaluation, expression8.Simplified());
            Assert.AreEqual(expectedEvaluation, expression9.Simplified());
            Assert.AreEqual(expectedEvaluation, expression10.Simplified());
            Assert.AreEqual(expectedEvaluation, expression11.Simplified());
            Assert.AreEqual(expectedEvaluation, expression12.Simplified());
            Assert.AreEqual(expectedEvaluation, expression13.Simplified());
            Assert.AreEqual(expectedEvaluation, expression14.Simplified());
            Assert.AreEqual(expectedEvaluation, expression15.Simplified());
            Assert.AreEqual(expectedEvaluation, expression16.Simplified());
            Assert.AreEqual(expectedEvaluation, expression17.Simplified());
            Assert.AreEqual(expectedEvaluation, expression18.Simplified());
            Assert.AreEqual(expectedEvaluation, expression19.Simplified());
            Assert.AreEqual(expectedEvaluation, expression20.Simplified());
            Assert.AreEqual(expectedEvaluation, expression21.Simplified());
            Assert.AreEqual(expectedEvaluation, expression22.Simplified());
            Assert.AreEqual(expectedEvaluation, expression23.Simplified());
            Assert.AreEqual(expectedEvaluation, expression24.Simplified());

        }


        [DataTestMethod]
        [DataRow(   3,   4,   7)]
        [DataRow(  12,   3,   5)]
        [DataRow(   0,  -8,  65)]
        [DataRow( 234,  94,  42)]
        [DataRow(-122, 852,  19)]
        [DataRow( 288,  23, 196)]
        public void OneVariableSimplificationTest1(int number1, int number2, int number3)
        {
            var x = new IntegerTypeVariable("x", Integer.Instance());

            var expression1 = ((x + number1) + number2) + number3;
            var expression2 = ((x + number1) + number3) + number2;
            var expression3 = ((x + number2) + number1) + number3;
            var expression4 = ((x + number2) + number3) + number1;
            var expression5 = ((x + number3) + number1) + number2;
            var expression6 = ((x + number3) + number2) + number1;

            var expectedResult = x + (number1 + number2 + number3);

            Assert.AreEqual(expectedResult, expression1.Simplified());
            Assert.AreEqual(expectedResult, expression2.Simplified());
            Assert.AreEqual(expectedResult, expression3.Simplified());
            Assert.AreEqual(expectedResult, expression4.Simplified());
            Assert.AreEqual(expectedResult, expression5.Simplified());
            Assert.AreEqual(expectedResult, expression6.Simplified());
        }

        [DataTestMethod]
        [DataRow( 1, 1, 1)]
        [DataRow( 6, 0, 3)]
        [DataRow(-3, 4, 0)]
        [DataRow(-3, 3, 3)]
        public void SimpliftyTest2(int A, int B, int C)
        {
            var x = new IntegerTypeVariable("x", Integer.Instance());

            IntegerConstant constA = A;
            IntegerConstant constB = B;
            IntegerConstant constC = C;

            var expression1 = (constA * x) + (constB * x) + constC;
            var expression2 = (constB * x) + (constA * x) + constC;
            var expression3 = (constA * x) + constC + (constB * x);
            var expression4 = (constB * x) + constC + (constA * x);
            var expression5 = constC + (constA * x) + (constB * x);
            var expression6 = constC + (constB * x) + (constA * x);

            var resultExpression = (IntegerConstant)(A + B) * x + constC;
            var expectedResult = resultExpression.Evaluated();

            Assert.AreEqual(expectedResult, expression1.Simplified());
            Assert.AreEqual(expectedResult, expression2.Simplified());
            Assert.AreEqual(expectedResult, expression3.Simplified());
            Assert.AreEqual(expectedResult, expression4.Simplified());
            Assert.AreEqual(expectedResult, expression5.Simplified());
            Assert.AreEqual(expectedResult, expression6.Simplified());
        }

        [TestMethod]
        public void SimplifyTest()
        {
            var x = new IntegerTypeVariable("x", Integer.Instance());
            var y = new IntegerTypeVariable("y", Integer.Instance());

            var expression = x - (x + y);
            var exprected = (IntegerConstant)(-1) * y;

            var result = expression.Simplified();

            Assert.AreEqual(exprected, result);
        }

        [DataTestMethod]
        [DataRow( 3,   4,   7,  23)]
        [DataRow(23,   2,  65,   0)]
        [DataRow(12,   3,   5,  61)]
        [DataRow( 0,  -8,  65,  28)]
        [DataRow(67, -12, -17, -85)]
        public void ExpansionTest(int constA, int constB, int constC, int constD)
        {
            var x = new IntegerTypeVariable("x", Integer.Instance());
            var y = new IntegerTypeVariable("y", Integer.Instance());

            IntegerConstant A = constA;
            IntegerConstant B = constB;
            IntegerConstant C = constC;
            IntegerConstant D = constD;

            //===============================================//
            //  (Ax + B) * (Cy + D) = ACxy + ADx + BCy + BD  //
            //===============================================//

            var expression1 = (A * x + B) * (C * y + D);
            var expression2 = (A * x + B) * (D + C * y);
            var expression3 = (B + A * x) * (C * y + D);
            var expression4 = (B + A * x) * (D + C * y);
            
            var expression5 = (C * y + D) * (A * x + B);
            var expression6 = (C * y + D) * (B + A * x);
            var expression7 = (D + C * y) * (A * x + B);
            var expression8 = (D + C * y) * (B + A * x);

            var expectedExpression = (IntegerConstant)(constA * constC) * x * y +
                                     (IntegerConstant)(constA * constD) * x +
                                     (IntegerConstant)(constB * constC) * y +
                                     (IntegerConstant)(constB * constD);

            var expectedEvaluation = expectedExpression.Evaluated();

            Assert.AreEqual(expectedEvaluation, expression1.Simplified());
            Assert.AreEqual(expectedEvaluation, expression2.Simplified());
            Assert.AreEqual(expectedEvaluation, expression3.Simplified());
            Assert.AreEqual(expectedEvaluation, expression4.Simplified());
            Assert.AreEqual(expectedEvaluation, expression5.Simplified());
            Assert.AreEqual(expectedEvaluation, expression6.Simplified());
            Assert.AreEqual(expectedEvaluation, expression7.Simplified());
            Assert.AreEqual(expectedEvaluation, expression8.Simplified());
        }

        [DataTestMethod]
        [DataRow( 3,  4,   7,  23,   2,   56,  78, 11)]
        [DataRow(12,  3,   5,  61, -23,   52,  54,  2)]
        [DataRow( 0, -8,  65,  28,  -3,   15,  -1,  5)]
        [DataRow(67, 12, -17, 291, -85, -125, -95, 28)]
        public void SimpliftyTest3(int A, int B, int C, int D, int E, int F, int G, int H)
        {
            var x = new IntegerTypeVariable("x", Integer.Instance());
            var y = new IntegerTypeVariable("y", Integer.Instance());

            IntegerConstant constA = A;
            IntegerConstant constB = B;
            IntegerConstant constC = C;
            IntegerConstant constD = D;
            IntegerConstant constE = E;
            IntegerConstant constF = F;
            IntegerConstant constG = G;
            IntegerConstant constH = H;

            //==========================================================//
            // ((xA + (xBy - Cy)) - ((Dyx + E(x - y + F)) - Gx)) + H =  //
            //   xA + (xBy - Cy)  -  (Dyx + E(x - y + F)  - Gx)  + H =  //
            //   Ax +  Bxy - Cy   -   Dxy - Ex + Ey - EF  + Gx   + H =  //
            //                                                          //
            // (B - D)xy + (A - E + G)x + (E - C)y + H - E * F          //
            //==========================================================//

            var term1 = x * constA;
            var term2 = x * (constB * y);
            var term3 = constC * y;
            var term4 = constD * y * x;
            var term5 = constE * (x - y + constF);
            var term6 = constG * x;
            var term7 = constH;

            var expression = ((term1 + (term2 - term3)) - ((term4 + term5) - term6)) + term7;

            var expectedResult = (IntegerConstant)(B - D) * x * y +
                                 (IntegerConstant)(A - E + G) * x +
                                 (IntegerConstant)(E - C) * y + 
                                 H - E * F;

            Assert.AreEqual(expectedResult.Evaluated(), expression.Simplified());
        }
    }
}