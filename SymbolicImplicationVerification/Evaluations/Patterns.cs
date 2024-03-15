global using IntegerTypePattern = SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>;
global using PatternMatches = System.Collections.Generic.Dictionary<
    SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>,
    SymbolicImplicationVerification.Terms.Term<SymbolicImplicationVerification.Types.IntegerType>>;

using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Patterns;
using SymbolicImplicationVerification.Types;
using System;
using System.Collections.ObjectModel;

namespace SymbolicImplicationVerification.Evaluations
{
    public static class Patterns
    {
        #region Static fields

        private static readonly IntegerConstant one  = new IntegerConstant(1);
        private static readonly IntegerConstant two  = new IntegerConstant(2);
        private static readonly IntegerConstant zero = new IntegerConstant(0);
        private static readonly IntegerConstant minusOne = new IntegerConstant(-1);

        private static readonly IntegerTypeAnythingPattern any1 = new IntegerTypeAnythingPattern(101);
        private static readonly IntegerTypeAnythingPattern any2 = new IntegerTypeAnythingPattern(102);
        private static readonly IntegerTypeAnythingPattern any3 = new IntegerTypeAnythingPattern(103);
        private static readonly IntegerTypeAnythingPattern any4 = new IntegerTypeAnythingPattern(104);

        private static readonly IntegerTypeConstantPattern const1 = new IntegerTypeConstantPattern(201);
        private static readonly IntegerTypeConstantPattern const2 = new IntegerTypeConstantPattern(202);

        private static readonly IntegerTypeVariablePattern var1 = new IntegerTypeVariablePattern(301);
        private static readonly IntegerTypeVariablePattern var2 = new IntegerTypeVariablePattern(302);

        #endregion

        #region Pattern fields

        public static readonly PatternMatches ExpandRules = new PatternMatches
        {
            //=======================================================//
            //   (A + B) * (C + D) = A * C + A * D + B * C + B * D   //
            //=======================================================//
            { (any1 + any2) * (any3 + any4), (((any1 * any3) + (any1 * any4)) + (any2 * any3)) + (any2 * any4) },
            { (any1 - any2) * (any3 + any4), (((any1 * any3) + (any1 * any4)) - (any2 * any3)) - (any2 * any4) },
            { (any1 + any2) * (any3 - any4), (((any1 * any3) - (any1 * any4)) + (any2 * any3)) - (any2 * any4) },
            { (any1 - any2) * (any3 - any4), (((any1 * any3) - (any1 * any4)) - (any2 * any3)) + (any2 * any4) },

            //=================================//
            //   A * (B + C) = A * B + A * C   //
            //=================================//
            {  any1 * (any2 + any3), (any1 * any2) + (any1 * any3) },
            {  any1 * (any2 - any3), (any1 * any2) - (any1 * any3) },
            { (any1 + any2) * any3 , (any1 * any3) + (any2 * any3) },
            { (any1 - any2) * any3 , (any1 * any3) - (any2 * any3) }
        };

        /// <summary>
        /// Itt már nincsen kibontatlan zárójel.
        /// </summary>
        public static readonly PatternMatches LeftAssociateRules = new PatternMatches
        {
            //==========================================//
            //   Asssociative everything to the left.   //
            //==========================================//
            {  any1 + (any2 + any3), (any1 + any2) + any3 },
            {  any1 - (any2 + any3), (any1 - any2) - any3 },
            {  any1 + (any2 - any3), (any1 + any2) - any3 },
            {  any1 - (any2 - any3), (any1 - any2) + any3 },
            {  any1 * (any2 * any3), (any1 * any2) * any3 },
        };


        /// <summary>
        /// Itt már nincsen kibontatlan zárójel. Kétszer kell meghívni a konstans szorzások kiértékelése miatt.
        /// </summary>
        public static readonly PatternMatches OrderingRules = new PatternMatches
        {
            //=========================================================//
            //   Bring the constants forward in the multiplications.   //
            //=========================================================//

            {         var1  * const1,         const1  * var1 },
            { (any1 * var1) * const1, (any1 * const1) * var1 },

            //===============================================================//
            //   Bring the constants back in the additions / subtractions.   //
            //===============================================================//

            { const1 + var1         ,                       var1  + const1 },
            { const1 - var1         ,           (minusOne * var1) + const1 },
            { const1 + (any1 * any2),               (any1 * any2) + const1 },
            { const1 - (any1 * any2),  ((minusOne * any1) * any2) + const1 },

            { (any1 + const1) + var1,  (any1 + var1) + const1 },
            { (any1 - const1) + var1,  (any1 + var1) - const1 },
            { (any1 + const1) - var1,  (any1 - var1) + const1 },
            { (any1 - const1) - var1,  (any1 - var1) - const1 },

            { (any1 + const1) + (any2 * any3),  (any1 + (any2 * any3)) + const1 },
            { (any1 - const1) + (any2 * any3),  (any1 + (any2 * any3)) - const1 },
            { (any1 + const1) - (any2 * any3),  (any1 - (any2 * any3)) + const1 },
            { (any1 - const1) - (any2 * any3),  (any1 - (any2 * any3)) - const1 },


            //====================================================//
            //   Associate constant additions and subtractions.   //
            //====================================================//
            {  (any1 + const1) + const2,    any1 + (const1 + const2) },
            {  (any1 + const1) - const2,    any1 + (const1 - const2) },
            {  (any1 - const1) + const2,    any1 - (const1 - const2) },
            {  (any1 - const1) - const2,    any1 - (const1 + const2) },

            {  (any1 + const1) + (const2 + any2),    any1 + (const1 + (const2 + any2)) },
            {  (any1 - const1) + (const2 + any2),    any1 - (const1 - (const2 + any2)) },
            {  (any1 + const1) - (const2 + any2),    any1 + (const1 - (const2 + any2)) },
            {  (any1 + const1) + (const2 - any2),    any1 + (const1 + (const2 - any2)) },
            {  (any1 + const1) - (const2 - any2),    any1 + (const1 - (const2 - any2)) },
            {  (any1 - const1) + (const2 - any2),    any1 - (const1 - (const2 - any2)) },
            {  (any1 - const1) - (const2 + any2),    any1 - (const1 + (const2 + any2)) },
            {  (any1 - const1) - (const2 - any2),    any1 - (const1 + (const2 + any2)) },
        };

        /*
        private static readonly Dictionary<Term<IntegerType>, Term<IntegerType>> OrderingRules = new Dictionary<Term<IntegerType>, Term<IntegerType>>
        {
            //=========================//
            //   Commutative property  //
            //=========================//
            { const1 + var1  ,   var1 + const1 },
            {   var1 * const1, const1 * var1   },

            //===========================//
            //   Asssociative property   //
            //===========================//
            {  (var1 + const1) + const2,    var1 + (const1 + const2) },
            {  (var1 + const1) - const2,    var1 + (const1 - const2) },
            {  (var1 - const1) + const2,    var1 - (const1 - const2) },
            {  (var1 - const1) - const2,    var1 - (const1 + const2) },
            { const1 * (const2 * var1) , (const1 * const2) * var1    },

            //===========================================//
            //   Commutative and asssociative property   //
            //===========================================//
            { (const1 + var1) + const2,              var1 + (const1 + const2) },
            { (const1 - var1) + const2, (const1 + const2) - var1              },
            { (const1 + var1) - const2,              var1 + (const1 - const2) },
            { (const1 - var1) - const2, (const1 - const2) - var1              }
        };*/

        public static readonly PatternMatches ConvertSubtractions = new PatternMatches
        {
            // (x + y) - (5 + (3 - k)) =
            // (x + y) + (-1) * (5 + (3 - k)) =
            // (x + y) + [(-1) * (5 + (3 + (-1) * k))] =
            // (x + y) + [((-1) * 5) + ((-1) * {(3) + (-1) * k)})]
            // (x + y) + [(-1) * 5] + [(-1) * 3] + ((-1) (-1) * k)}]

            { any1 - any2, any1 + (minusOne * any2) },

            { minusOne * (any1 + any2), (minusOne * any1) + (minusOne * any2) },

            { minusOne * (minusOne * any1), any1 },

            { minusOne * minusOne, one }
        };

        public static readonly PatternMatches CollapseGroups = new PatternMatches
        {
            //================================================//
            //   Asssociative multiplications to the right.   //
            //================================================//
            { (any1 * any2) * any3, any1 * (any2 * any3) },

            //=====================//
            //   Simplify zeros.   //
            //=====================//
            { any1 - any1, zero },
            { any1 + zero, any1 },
            { zero + any1, any1 },
            { any1 - zero, any1 },

            //====================//
            //   Simplify ones.   //
            //====================//
            { one  * any1, any1 },
            { any1 * one , any1 },

            //==========================================//
            //   Start the accumulation. (first case)   //
            //==========================================//
            { (const1 * any1) + (const1 * any1), ((two * const1) + zero) * any1 },
            { (const1 * any1) + (const2 * any1), (const1 + const2) * any1 },
            { (const1 * any1) - (const2 * any1), (const1 - const2) * any1 },

            //===========================================//
            //   Start the accumulation. (second case)   //
            //===========================================//
            { (const1 * any1) + any1, (const1 + one) * any1 },
            { (const1 * any1) - any1, (const1 - one) * any1 },

            //==========================================//
            //   Start the accumulation. (third case)   //
            //==========================================//
            { any1 + any1, two * any1 },

            //================================//
            //   Continue the accumulation.   //
            //================================//
            { ((any1 + const1) * any2) + (const2 * any2), ((any1 + const1) + const2) * any2 },
            { ((any1 - const1) * any2) + (const2 * any2), ((any1 - const1) + const2) * any2 },
            { ((any1 + const1) * any2) - (const2 * any2), ((any1 + const1) - const2) * any2 },
            { ((any1 - const1) * any2) - (const2 * any2), ((any1 - const1) - const2) * any2 },

            { ((any1 + const1) * any2) + any2, ((any1 + const1) + one) * any2 },
            { ((any1 - const1) * any2) + any2, ((any1 - const1) + one) * any2 },
            { ((any1 + const1) * any2) - any2, ((any1 + const1) - one) * any2 },
            { ((any1 - const1) * any2) - any2, ((any1 - const1) - one) * any2 },
        };

        #endregion
    }
}
