using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Patterns;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Formulas.Operations;
using SymbolicImplicationVerification.Formulas.Quantified;
using SymbolicImplicationVerification.Formulas.Relations;

namespace SymbolicImplicationVerification.Evaluations
{
    public static class PatternReplacer<T> where T : Type
    {
        private static bool MatchPatternTerms(
            Term<T> entryPoint, Term<T> matchedPattern, Dictionary<int, Term<T>> patternTerms)
        {
            LinkedList<Term<T>> unprocessedTerms = new LinkedList<Term<T>>();
            LinkedList<Term<T>> unprocessedPatterns = new LinkedList<Term<T>>();

            unprocessedTerms.AddLast(entryPoint);
            unprocessedPatterns.AddLast(matchedPattern);

            while (unprocessedTerms.Count > 0 && unprocessedPatterns.Count > 0)
            {
                Term<T> nextTerm = unprocessedTerms.First();
                Term<T> nextPattern = unprocessedPatterns.First();

                unprocessedTerms.RemoveFirst();
                unprocessedPatterns.RemoveFirst();

                if (nextPattern is Pattern<T> pattern)
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
                else if (nextTerm is BinaryOperationTerm<Term<T>, T> operation &&
                         nextPattern is BinaryOperationTerm<Term<T>, T> operationPattern)
                {
                    // Process the left and right operand of the term.
                    unprocessedTerms.AddLast(operation.LeftOperand);
                    unprocessedTerms.AddLast(operation.RightOperand);

                    // Process the left and right operand of the pattern.
                    unprocessedPatterns.AddLast(operationPattern.LeftOperand);
                    unprocessedPatterns.AddLast(operationPattern.RightOperand);
                }
                else if (nextTerm.Equals(nextPattern))
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

        private static Term<T> BuildNewTerm(
            Term<T> newPattern, Dictionary<int, Term<T>> matchedPatternTerms) => newPattern switch
            {
                BinaryOperationTerm<Term<T>, T> operation =>
                    operation.CreateInstance(
                        BuildNewTerm(operation.LeftOperand, matchedPatternTerms),
                        BuildNewTerm(operation.RightOperand, matchedPatternTerms)),

                Pattern<T> pattern => matchedPatternTerms[pattern.Identifier],

                Term<T> other => other.DeepCopy(),
            };


        private static void ReplacePattern(
            ref Term<T> source, EntryPoint<T> entryPoint, Term<T> matchedPattern, Term<T> replacePattern)
        {
            Term<T> replaceTerm;

            try
            {
                replaceTerm = BuildNewTerm(replacePattern, entryPoint.MatchedPatternTerms);
            }
            catch (Exception)
            {
                return;
            }

            ReplacePattern(ref source, entryPoint, replaceTerm);
        }

        private static void ReplacePattern(
            ref Term<T> source, EntryPoint<T> entryPoint, Term<T> replaceTerm)
        {
            if (entryPoint.HasParent)
            {
                entryPoint.ParentProperty!.SetValue(entryPoint.ParentEntry, replaceTerm);
            }
            else
            {
                source = replaceTerm;
            }
        }

        private static void ReplacePattern(EntryPoint<T> entryPoint, Term<T> replaceTerm)
        {
            if (entryPoint.HasParent)
            {
                entryPoint.ParentProperty!.SetValue(entryPoint.ParentEntry, replaceTerm);
            }
        }

        private static EntryPoint<T>? FindEntryPoint(Term<T> source, Term<T> pattern)
        {
            if (pattern.Matches(source))
            {
                Dictionary<int, Term<T>> matchedPatternTerms = new Dictionary<int, Term<T>>();

                // Match the matchedPattern's patterns to the terms of the entryPoint.
                if (MatchPatternTerms(source, pattern, matchedPatternTerms))
                {
                    return new EntryPoint<T>(source, matchedPatternTerms);
                }
            }

            if (source is BinaryOperationTerm<Term<T>, T> operation)
            {
                EntryPoint<T>? entryPoint
                    = FindEntryPoint(operation, "LeftOperand", operation.LeftOperand, pattern);

                bool entryPointFound = entryPoint is not null;

                return entryPointFound ? entryPoint :
                       FindEntryPoint(operation, "RightOperand", operation.RightOperand, pattern);
            }

            return null;
        }

        private static EntryPoint<T>? FindEntryPoint<U>(
            U parent, string property, Term<T> source, Term<T> pattern) where U : notnull
        {
            EntryPoint<T>? entryPointInfo = FindEntryPoint(source, pattern);

            if (entryPointInfo is not null)
            {
                EntryPoint<T> entryPointValue = entryPointInfo.Value;

                if (!entryPointValue.HasParent)
                {
                    entryPointValue.ParentEntry    = parent;
                    entryPointValue.ParentProperty = parent.GetType().GetProperty(property);
                }

                entryPointInfo = entryPointValue;
            }

            return entryPointInfo;
        }

        private static LinkedList<EntryPoint<T>> FindEntryPoints(Term<T> source, Term<T> pattern)
        {
            LinkedList<EntryPoint<T>> entryPoints = new LinkedList<EntryPoint<T>>();

            FindEntryPoints(entryPoints, source, pattern);

            return entryPoints;
        }

        private static void FindEntryPoints(
            LinkedList<EntryPoint<T>> entryPoints, Term<T> source, Term<T> pattern)
        {
            if (pattern.Matches(source))
            {
                Dictionary<int, Term<T>> matchedPatternTerms = new Dictionary<int, Term<T>>();

                // Match the matchedPattern's patterns to the terms of the entryPoint.
                if (MatchPatternTerms(source, pattern, matchedPatternTerms))
                {
                    entryPoints.AddLast(new EntryPoint<T>(source, matchedPatternTerms));
                }
            }

            if (source is BinaryOperationTerm<Term<T>, T> operation)
            {
                FindEntryPoints(entryPoints, operation, "LeftOperand", operation.LeftOperand, pattern);
                FindEntryPoints(entryPoints, operation, "RightOperand", operation.RightOperand, pattern);
            }
        }

        public static LinkedList<EntryPoint<T>> FindEntryPoints(Formula formula, Term<T> pattern)
        {
            LinkedList<EntryPoint<T>> entryPoints = new LinkedList<EntryPoint<T>>();

            FindEntryPoints(entryPoints, formula, pattern);

            return entryPoints;
        }

        private static void FindEntryPoints(
            LinkedList<EntryPoint<T>> entryPoints, Formula formula, Term<T> pattern)
        {
            if (formula is UnaryOperationFormula unaryOperationFormula)
            {
                FindEntryPoints(entryPoints, unaryOperationFormula.Operand, pattern);
            }
            else if (formula is BinaryOperationFormula binaryOperationFormula)
            {
                FindEntryPoints(entryPoints, binaryOperationFormula.LeftOperand, pattern);
                FindEntryPoints(entryPoints, binaryOperationFormula.RightOperand, pattern);
            }
            else if (formula is QuantifiedFormula<T> quantifiedFormula)
            {
                FindEntryPoints(entryPoints, quantifiedFormula.Statement, pattern);
            }
            else if (formula is BinaryRelationFormula<T> realtionFormula)
            {
                FindEntryPoints(entryPoints, realtionFormula, "LeftComponent",
                                realtionFormula.LeftComponent, pattern);

                FindEntryPoints(entryPoints, realtionFormula, "RightComponent",
                                realtionFormula.RightComponent, pattern);
            }
        }

        private static void FindEntryPoints<U>(
            LinkedList<EntryPoint<T>> entryPoints, U parent, string property, Term<T> source, Term<T> pattern)
            where U : notnull
        {
            FindEntryPoints(entryPoints, source, pattern);

            if (entryPoints.Count == 0)
            {
                return;
            }

            EntryPoint<T> lastEntryPoint = entryPoints.Last();

            if (!lastEntryPoint.HasParent)
            {
                lastEntryPoint.ParentEntry    = parent;
                lastEntryPoint.ParentProperty = parent.GetType().GetProperty(property);

                entryPoints.RemoveLast();
                entryPoints.AddLast(lastEntryPoint);
            }
        }


        public static Term<T> PatternsApplied(Term<T> source, Dictionary<Term<T>, Term<T>> patternMatches)
        {
            Term<T> result = source.DeepCopy();

            foreach (KeyValuePair<Term<T>, Term<T>> patternPair in patternMatches)
            {
                Term<T> matchPattern = patternPair.Key;
                Term<T> replacePattern = patternPair.Value;

                EntryPoint<T>? entryInfo;

                for (entryInfo = FindEntryPoint(result, matchPattern);
                     entryInfo is not null;
                     entryInfo = FindEntryPoint(result, matchPattern))
                {
                    ReplacePattern(ref result, entryInfo.Value, matchPattern, replacePattern);
                }
            }

            return result;
        }

        public static void VariableReplaced(LinkedList<EntryPoint<T>> entryPoints, Term<T> replaceTerm)
        {
            foreach (EntryPoint<T> entryPoint in entryPoints)
            {
                ReplacePattern(entryPoint, replaceTerm);
            }
        }

        public static Formula VariableReplaced(Formula source, Variable<T> variable, Term<T> replaceTerm)
        {
            Formula result = source.DeepCopy();

            LinkedList<EntryPoint<T>> entryPoints = FindEntryPoints(result, variable);

            VariableReplaced(entryPoints, replaceTerm);

            return result;
        }

        public static Term<T> VariableReplaced(Term<T> source, Variable<T> variable, Term<T> replaceTerm)
        {
            Term<T> result = source.DeepCopy();

            LinkedList<EntryPoint<T>> entryPoints = FindEntryPoints(result, variable);

            foreach (EntryPoint<T> entryPoint in entryPoints)
            {
                ReplacePattern(ref result, entryPoint, replaceTerm);
            }

            return result;
        }
    }
}
