using SymImply.Terms;
using SymImply.Terms.Operations.Binary;
using SymImply.Terms.Patterns;
using SymImply.Terms.Variables;
using SymImply.Formulas;
using SymImply.Formulas.Operations;
using SymImply.Formulas.Quantified;
using SymImply.Formulas.Relations;
using SymImply.Types;
using System.Runtime.ExceptionServices;

namespace SymImply.Evaluations
{
    public static class PatternReplacer<T> where T : Type
    {
        /// <summary>
        /// Match the pattern terms with the given entry terms.
        /// </summary>
        /// <param name="entryPoint">The entry point of the pattern.</param>
        /// <param name="matchedPattern">The matched pattern.</param>
        /// <param name="patternTerms">The dictionary of matched terms.</param>
        /// <returns><see langword="true"/> if the match is successful, otherwise <see langword="false"/>.</returns>
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
                else if (nextTerm    is BinaryOperationTerm<Term<T>, T> operation &&
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
                else if (nextTerm    is ArrayVariable<T> nextArray && 
                         nextPattern is ArrayVariable<T> nextArrayPattern)
                {
                    if (typeof(T) == typeof(IntegerType) &&
                        nextArray.IndexTerm is not null  && nextArrayPattern.IndexTerm is not null)
                    {
                        // Process the argument of the array variable.
                        unprocessedTerms.AddLast((Term<T>)(object) nextArray.IndexTerm!);

                        // Process the argument of the array variable pattern.
                        unprocessedPatterns.AddLast((Term<T>)(object) nextArrayPattern.IndexTerm!);
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Builds the new term given the matched pattern and it's terms.
        /// </summary>
        /// <param name="newPattern">The matched pattern.</param>
        /// <param name="matchedPatternTerms">The matched pattern term.</param>
        /// <returns>The newly built term.</returns>
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


        /// <summary>
        /// Replaces the given pattern inside the source term.
        /// </summary>
        /// <param name="source">The root of the term.</param>
        /// <param name="entryPoint">The entry point info for the pattern.</param>
        /// <param name="matchedPattern">The matched pattern.</param>
        /// <param name="replacePattern">The replace pattern.</param>
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

        /// <summary>
        /// Replaces the given pattern inside the source term.
        /// </summary>
        /// <param name="source">The root of the term.</param>
        /// <param name="entryPoint">The entry point info for the pattern.</param>
        /// <param name="replaceTerm">The replace term.</param>
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

        /// <summary>
        /// Replaces the given pattern based on the entry point.
        /// </summary>
        /// <param name="entryPoint">The entry point info for the pattern.</param>
        /// <param name="replaceTerm">The replace term.</param>
        private static void ReplacePattern(EntryPoint<T> entryPoint, Term<T> replaceTerm)
        {
            if (entryPoint.HasParent)
            {
                entryPoint.ParentProperty!.SetValue(entryPoint.ParentEntry, replaceTerm);
            }
        }

        /// <summary>
        /// Find the first entry point inside the source term.
        /// </summary>
        /// <param name="source">The source term.</param>
        /// <param name="pattern">The replace to search for.</param>
        /// <returns>The found entry point.</returns>
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

        /// <summary>
        /// Find the first entry point inside the source term.
        /// </summary>
        /// <param name="parent">The parent of the entry point.</param>
        /// <param name="property">The name of the parent property.</param>
        /// <param name="source">The source term.</param>
        /// <param name="pattern">The replace to search for.</param>
        /// <returns>The found entry point.</returns>
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

        /// <summary>
        /// Find all the entry points inside the source term.
        /// </summary>
        /// <param name="source">The source term.</param>
        /// <param name="pattern">The replace to search for.</param>
        /// <returns>The list of found entry points.</returns>
        private static LinkedList<EntryPoint<T>> FindEntryPoints(Term<T> source, Term<T> pattern)
        {
            LinkedList<EntryPoint<T>> entryPoints = new LinkedList<EntryPoint<T>>();

            FindEntryPoints(entryPoints, source, pattern);

            return entryPoints;
        }

        /// <summary>
        /// Find all the entry points inside the source term.
        /// </summary>
        /// <param name="entryPoints">The list of found entry points.</param>
        /// <param name="source">The source term.</param>
        /// <param name="pattern">The replace to search for.</param>
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
                FindEntryPoints(entryPoints, operation, "LeftOperand" , operation.LeftOperand , pattern);
                FindEntryPoints(entryPoints, operation, "RightOperand", operation.RightOperand, pattern);
            }
            else if (source is FormulaTerm formulaTerm)
            {
                FindEntryPoints(entryPoints, formulaTerm.Formula, pattern);
            }

            if (typeof(T) != typeof(IntegerType))
            {
                return;
            }

            if (source is ArrayVariable<T> arrayVariable)
            {
                if (!pattern.Matches(arrayVariable) && arrayVariable.IndexTerm is not null)
                {
                    FindEntryPoints(entryPoints, arrayVariable, "IndexTerm",
                               (Term<T>)(object)arrayVariable.IndexTerm, pattern);
                }
            }
            else if (source is Summation summation)
            {
                FindEntryPoints(entryPoints, summation, "Argument",
                               (Term<T>)(object) summation.Argument, pattern);
                
                FindEntryPoints(entryPoints , summation.IndexBounds, "LowerBound",
                               (Term<T>)(object) summation.IndexBounds.LowerBound, pattern);

                FindEntryPoints(entryPoints , summation.IndexBounds, "UpperBound",
                               (Term<T>)(object) summation.IndexBounds.UpperBound, pattern);
            }
        }

        /// <summary>
        /// Find all the entry points inside the source formula.
        /// </summary>
        /// <param name="formula">The source formula.</param>
        /// <param name="pattern">The replace to search for.</param>
        /// <returns>The list of found entry points.</returns>
        public static LinkedList<EntryPoint<T>> FindEntryPoints(Formula formula, Term<T> pattern)
        {
            LinkedList<EntryPoint<T>> entryPoints = new LinkedList<EntryPoint<T>>();

            FindEntryPoints(entryPoints, formula, pattern);

            return entryPoints;
        }


        /// <summary>
        /// Find all the entry points inside the source formula.
        /// </summary>
        /// <param name="entryPoints">The list of found entry points.</param>
        /// <param name="formula">The source formula.</param>
        /// <param name="pattern">The replace to search for.</param>
        public static void FindEntryPoints(
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
                if (typeof(T) == typeof(IntegerType) &&
                    quantifiedFormula.QuantifiedVariable.TermType is TermBoundedInteger bounded)
                {
                    FindEntryPoints(entryPoints, bounded, "LowerBound", 
                                   (Term<T>)(object) bounded.LowerBound, pattern);

                    FindEntryPoints(entryPoints, bounded, "UpperBound", 
                                   (Term<T>)(object) bounded.UpperBound, pattern);
                }

                FindEntryPoints(entryPoints, quantifiedFormula.Statement, pattern);
            }
            else if (formula is BinaryRelationFormula<T> relationFormula)
            {
                FindEntryPoints(entryPoints, relationFormula, "LeftComponent",
                                relationFormula.LeftComponent, pattern);

                FindEntryPoints(entryPoints, relationFormula, "RightComponent",
                                relationFormula.RightComponent, pattern);
            }
            else if (formula is BinaryRelationFormula<Logical> logicalRelation)
            {
                if (logicalRelation.LeftComponent is FormulaTerm left)
                {
                    FindEntryPoints(entryPoints, left.Formula, pattern);
                }

                if (logicalRelation.RightComponent is FormulaTerm right)
                {
                    FindEntryPoints(entryPoints, right.Formula, pattern);
                }
            }
            else if (formula is LogicalTermFormula logicalTerm && typeof(T) == typeof(Logical))
            {
                FindEntryPoints(entryPoints, logicalTerm, "Argument", 
                               (Term<T>)(object) logicalTerm.Argument, pattern);
            }
        }

        /// <summary>
        /// Find all the entry points inside the source term.
        /// </summary>
        /// <param name="entryPoints">The list of found entry points.</param>
        /// <param name="parent">The parent of the entry point.</param>
        /// <param name="property">The name of the parent property.</param>
        /// <param name="source">The source term.</param>
        /// <param name="pattern">The replace to search for.</param>
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

        /// <summary>
        /// Apply all the patterns onto the given source term.
        /// </summary>
        /// <param name="source">The source term.</param>
        /// <param name="patternMatches">The patterns to apply.</param>
        /// <returns>The result of the operation.</returns>
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

        /// <summary>
        /// Replace all the variable entry points with the given replace term.
        /// </summary>
        /// <param name="entryPoints">The list of variable entry points.</param>
        /// <param name="replaceTerm">The term to replace the variable with.</param>
        public static void VariableReplaced(LinkedList<EntryPoint<T>> entryPoints, Term<T> replaceTerm)
        {
            foreach (EntryPoint<T> entryPoint in entryPoints)
            {
                if (replaceTerm             is ArrayVariable<T> replaceVariable &&
                    entryPoint.PatternEntry is ArrayVariable<T> entryVariable   &&
                    entryVariable  .IndexTerm is not null &&
                    replaceVariable.IndexTerm is null)
                {
                    ArrayVariable<T> replace = replaceVariable.DeepCopy();
                    replace.IndexTerm        = entryVariable.IndexTerm.DeepCopy();

                    ReplacePattern(entryPoint, replace);
                }
                else
                {
                    ReplacePattern(entryPoint, replaceTerm);
                }
            }
        }

        /// <summary>
        /// Replace all occurrences of the given variable inside the source formula.
        /// </summary>
        /// <param name="source">The source formula.</param>
        /// <param name="variable">The variable to replace.</param>
        /// <param name="replaceTerm">The term to replace the variable with.</param>
        /// <returns>The result of the replacement.</returns>
        public static Formula VariableReplaced(Formula source, Variable<T> variable, Term<T> replaceTerm)
        {
            Formula result = source.DeepCopy();

            LinkedList<EntryPoint<T>> entryPoints = FindEntryPoints(result, variable);

            VariableReplaced(entryPoints, replaceTerm);

            return result;
        }

        /// <summary>
        /// Replace all occurrences of the given variable inside the source term.
        /// </summary>
        /// <param name="source">The source term.</param>
        /// <param name="variable">The variable to replace.</param>
        /// <param name="replaceTerm">The term to replace the variable with.</param>
        /// <returns>The result of the replacement.</returns>
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

        /// <summary>
        /// Match the pattern terms with the given entry terms.
        /// </summary>
        /// <param name="patternStatement">The pattern formula.</param>
        /// <param name="statement">The source formula.</param>
        /// <param name="patternTerms">The dictionary of matched terms.</param>
        /// <returns><see langword="true"/> if the match is successful, otherwise <see langword="false"/>.</returns>
        private static bool MatchPatternTerms(
            Formula patternStatement, Formula statement, Dictionary<int, Term<T>> patternTerms)
        {
            if (patternStatement.GetType() != statement.GetType())
            {
                return false;
            }

            bool result;

            if (patternStatement is BinaryOperationFormula operationPattern &&
                statement        is BinaryOperationFormula operationStatement)
            {
                result = MatchPatternTerms(
                    operationPattern.LeftOperand , operationStatement.LeftOperand , patternTerms);

                result &= MatchPatternTerms(
                    operationPattern.RightOperand, operationStatement.RightOperand, patternTerms);

                return result;
            }
            
            if (patternStatement is UnaryOperationFormula unaryOperationPattern &&
                statement        is UnaryOperationFormula unaryOperationStatement)
            {
                return MatchPatternTerms(
                    unaryOperationPattern.Operand, unaryOperationStatement.Operand, patternTerms);
            }
            
            if (patternStatement is QuantifiedFormula<T> quantifiedPattern &&
                statement        is QuantifiedFormula<T> quantifiedStatement)
            {
                return MatchPatternTerms(
                    quantifiedPattern.Statement, quantifiedStatement.Statement, patternTerms);
            }
            
            if (patternStatement is BinaryRelationFormula<T> relationPattern &&
                statement        is BinaryRelationFormula<T> relationStatement)
            {
                result =
                    relationPattern.LeftComponent.Matches(relationStatement.LeftComponent) &&
                    MatchPatternTerms(relationStatement.LeftComponent, relationPattern.LeftComponent, patternTerms);

                result &=
                    relationPattern.RightComponent.Matches(relationStatement.RightComponent) &&
                    MatchPatternTerms(relationStatement.RightComponent, relationPattern.RightComponent, patternTerms);

                return result;
            }

            return false;
        }

        /// <summary>
        /// Replace the quantified variable inside a quantified formula.
        /// </summary>
        /// <param name="quantified">The source quantified formula.</param>
        /// <param name="statement">The replace statement of the new formula.</param>
        public static Term<T>? QuantifiedVariableReplaced(QuantifiedFormula<T> quantified, Formula statement)
        {
            const int anythingPatternIdentifier = 1;

            AnythingPattern<T> anythingPattern 
                = new AnythingPattern<T>(anythingPatternIdentifier, (T) quantified.QuantifiedVariable.TermType.DeepCopy());

            // Change the quantified variable to the given anything pattern.
            Formula patternStatement = VariableReplaced(
                quantified.Statement, quantified.QuantifiedVariable.DeepCopy(), anythingPattern);

            // Matched pattern terms.
            Dictionary<int, Term<T>> patternTerms = new Dictionary<int, Term<T>>();

            bool successfull
                = MatchPatternTerms(patternStatement, statement.DeepCopy(), patternTerms) &&
                  patternTerms.ContainsKey(anythingPatternIdentifier);

            return successfull ? patternTerms[anythingPatternIdentifier].DeepCopy() : null;
        }

        /// <summary>
        /// Match the variables inside the source term.
        /// </summary>
        /// <param name="source">The source statement.</param>
        /// <param name="variable">The variable to find.</param>
        /// <param name="other">The replace term.</param>
        public static Term<T>? MatchVariable(Term<T> source, Variable<T> variable, Term<T> other)
        {
            const int anythingPatternIdentifier = 1;

            AnythingPattern<T> anythingPattern = new AnythingPattern<T>(
                anythingPatternIdentifier, (T) variable.TermType.DeepCopy());

            Term<T> sourceVariableReplaced = VariableReplaced(
                source.DeepCopy(), variable.DeepCopy(), anythingPattern);

            // Matched pattern terms.
            Dictionary<int, Term<T>> patternTerms = new Dictionary<int, Term<T>>();

            bool successFullMatch =
                sourceVariableReplaced.Matches(other) &&
                MatchPatternTerms(other, sourceVariableReplaced, patternTerms) &&
                patternTerms.ContainsKey(anythingPatternIdentifier);

            return successFullMatch ? patternTerms[anythingPatternIdentifier].DeepCopy() : null;
        }
    }
}
