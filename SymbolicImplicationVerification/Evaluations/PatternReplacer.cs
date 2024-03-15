using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Patterns;
using SymbolicImplicationVerification.Types;
using System.Collections.ObjectModel;
using System.Reflection;

namespace SymbolicImplicationVerification.Evaluations
{
    public struct EntryPoint
    {
        #region Fields

        private readonly IntegerTypeTerm patternEntry;

        private IntegerTypeTerm? parentEntry;

        private PropertyInfo? parentProperty;

        private Dictionary<int, IntegerTypeTerm> matchedPatternTerms;

        #endregion

        #region Constructors

        public EntryPoint(IntegerTypeTerm patternEntry, Dictionary<int, IntegerTypeTerm> matchedPatternTerms)
        {
            this.patternEntry = patternEntry;
            this.matchedPatternTerms = matchedPatternTerms;
        }

        #endregion

        #region Public properties

        public bool HasParent
        {
            get { return parentEntry is not null && parentProperty is not null; }
        }

        public IntegerTypeTerm PatternEntry
        {
            get { return patternEntry; }
        }

        public IntegerTypeTerm? ParentEntry
        {
            get { return parentEntry; }
            set { parentEntry = value; }
        }

        public PropertyInfo? ParentProperty
        {
            get { return parentProperty; }
            set { parentProperty = value; }
        }

        public Dictionary<int, IntegerTypeTerm> MatchedPatternTerms
        {
            get { return matchedPatternTerms; }
        }

        #endregion
    }
    public static class PatternReplacer
    {
        private static bool MatchPatternTerms(
            IntegerTypeTerm entryPoint, IntegerTypeTerm matchedPattern, 
            Dictionary<int, IntegerTypeTerm> patternTerms)
        {
            LinkedList<IntegerTypeTerm> unprocessedTerms    = new LinkedList<IntegerTypeTerm>();
            LinkedList<IntegerTypeTerm> unprocessedPatterns = new LinkedList<IntegerTypeTerm>();

            unprocessedTerms.AddLast(entryPoint);
            unprocessedPatterns.AddLast(matchedPattern);

            while (unprocessedTerms.Count > 0 && unprocessedPatterns.Count > 0)
            {
                IntegerTypeTerm nextTerm    = unprocessedTerms.First();
                IntegerTypeTerm nextPattern = unprocessedPatterns.First();

                unprocessedTerms.RemoveFirst();
                unprocessedPatterns.RemoveFirst();

                if (nextPattern is Pattern<IntegerType> pattern)
                {
                    // If the pattern is already matched.
                    if (patternTerms.ContainsKey(pattern.Identifier))
                    {
                        // If the matched pattern and the next term doesn't equal.
                        if (!patternTerms[pattern.Identifier].Equals(nextTerm))
                        {
                            return false;
                        }

                        continue;
                    }

                    // Match the pattern with the given term.
                    patternTerms.Add(pattern.Identifier, nextTerm);
                }
                else if (nextTerm    is IntegerTypeBinaryOperationTerm operation &&
                         nextPattern is IntegerTypeBinaryOperationTerm operationPattern)
                {
                    // Process the left and right operand of the term.
                    unprocessedTerms.AddLast(operation.LeftOperand);
                    unprocessedTerms.AddLast(operation.RightOperand);
                    
                    // Process the left and right operand of the pattern.
                    unprocessedPatterns.AddLast(operationPattern.LeftOperand);
                    unprocessedPatterns.AddLast(operationPattern.RightOperand);
                }
                else if (nextTerm is IntegerTypeConstant && nextPattern is IntegerTypeConstant)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static IntegerTypeTerm BuildNewTerm(
            IntegerTypePattern newPattern,
            Dictionary<int, IntegerTypeTerm> matchedPatternTerms) => newPattern switch
            {
                Addition addition => new Addition(
                                     BuildNewTerm(addition.LeftOperand, matchedPatternTerms),
                                     BuildNewTerm(addition.RightOperand, matchedPatternTerms)),

                Subtraction subtraction => new Subtraction(
                                           BuildNewTerm(subtraction.LeftOperand, matchedPatternTerms),
                                           BuildNewTerm(subtraction.RightOperand, matchedPatternTerms)),

                Multiplication multiplication => new Multiplication(
                                           BuildNewTerm(multiplication.LeftOperand, matchedPatternTerms),
                                           BuildNewTerm(multiplication.RightOperand, matchedPatternTerms)),

                Pattern<IntegerType> pattern => matchedPatternTerms[pattern.Identifier],

                IntegerTypeConstant constant => IntegerTypeConstant.DeepCopy((dynamic) constant),

                _ => throw new Exception()
            };


        private static void ReplacePattern(
            ref IntegerTypeTerm source, EntryPoint entryPoint, 
            IntegerTypeTerm matchedPattern, IntegerTypeTerm replacePattern)
        {
            IntegerTypeTerm newPattern;

            try
            {
                newPattern = BuildNewTerm(replacePattern, entryPoint.MatchedPatternTerms);
            }
            catch (Exception) 
            { 
                return;
            }

            if (entryPoint.HasParent)
            {
                entryPoint.ParentProperty!.SetValue(entryPoint.ParentEntry, newPattern);
            }
            else
            {
                source = newPattern;
            }
        }

        private static EntryPoint? FindEntryPoint(IntegerTypeTerm source, IntegerTypeTerm pattern)
        {
            // return pattern is IMatch matchPattern ? FindEntryPoint(source, matchPattern) : null;

            if (pattern is IMatch matchPattern)
            {
                if (matchPattern.Matches(source))
                {
                    Dictionary<int, IntegerTypeTerm> matchedPatternTerms = new Dictionary<int, IntegerTypeTerm>();

                    // Match the matchedPattern's patterns to the terms of the entryPoint.
                    if (MatchPatternTerms(source, pattern, matchedPatternTerms))
                    {
                        return new EntryPoint(source, matchedPatternTerms);
                    }
                }

                if (source is IntegerTypeBinaryOperationTerm operation)
                {
                    for (int operandIndex = 0; operandIndex <= 1; ++operandIndex)
                    {
                        IntegerTypeTerm operand =
                            operandIndex == 0 ? operation.LeftOperand : operation.RightOperand;

                        EntryPoint? operandEntryInfo = FindEntryPoint(operand, pattern);

                        if (operandEntryInfo is not null)
                        {
                            EntryPoint operandEntry = operandEntryInfo.Value;

                            if (!operandEntry.HasParent)
                            {
                                operandEntry.ParentEntry    = operation;
                                operandEntry.ParentProperty = typeof(IntegerTypeBinaryOperationTerm)
                                    .GetProperty(operandIndex == 0 ? "LeftOperand" : "RightOperand");
                            }

                            return operandEntry;
                        }
                    }
                }
            }

            return null; 
        }

        public static IntegerTypeTerm PatternsApplied(IntegerTypeTerm source, PatternMatches patternMatches)
        {
            IntegerTypeTerm result = IntegerTypeTerm.DeepCopy(source);

            foreach (KeyValuePair<IntegerTypeTerm, IntegerTypeTerm> patternPair in patternMatches)
            {
                IntegerTypeTerm matchPattern   = patternPair.Key;
                IntegerTypeTerm replacePattern = patternPair.Value;

                EntryPoint? entryInfo;

                for (entryInfo = FindEntryPoint(result, matchPattern);
                     entryInfo is not null;
                     entryInfo = FindEntryPoint(result, matchPattern))
                {
                    ReplacePattern(ref result, entryInfo.Value, matchPattern, replacePattern);
                }
            }

            return result;
        }
    }
}
