using SymImply.Converts.Tokens;
using SymImply.Converts.Tokens.Operands;
using SymImply.Converts.Tokens.Operators;
using SymImply.Formulas;
using SymImply.Formulas.Quantified;
using SymImply.Implies;
using SymImply.Programs;
using SymImply.Terms;
using SymImply.Terms.Constants;
using SymImply.Terms.FunctionValues;
using SymImply.Terms.Variables;
using SymImply.Types;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace SymImply.Converts
{
    public class Converter
    {
        #region Fields

        /// <summary>
        /// The list of converted formulas.
        /// </summary>
        private List<Formula> formulas;

        /// <summary>
        /// The list of converted integer type variables.
        /// </summary>
        private List<IntegerTypeVariable> integerTypeVariables;

        /// <summary>
        /// The list of converted logical variables.
        /// </summary>
        private List<Variable<Logical>> logicalVariables;

        #endregion

        #region Constructors

        public Converter()
        {
            formulas = new List<Formula>();

            integerTypeVariables = new List<IntegerTypeVariable>();

            logicalVariables = new List<Variable<Logical>>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Used to initialize all the variables.
        /// </summary>
        /// <param name="input">The input of the state space.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        public void DeclareStateSpace(string input)
        {
            string stateSpacePattern =
                $"\\A\\\\symboldeclare{LatexParameterRegex()}{LatexParameterRegex("declarations")}";

            Match stateSpacMatch = Regex.Match(input, stateSpacePattern);

            if (stateSpacMatch.Success)
            {
                InitializeVariables(stateSpacMatch.Groups["declarations"].Value);

                return;
            }

            throw new ConvertException($"Nem található állapottér deklaráció a következő bemenet: \"{input}\"");
        }

        /// <summary>
        /// Used to initialize the given named formula.
        /// </summary>
        /// <param name="input">The input of the named formula.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        public void DeclareFormula(string input)
        {
            string formulaDeclarationPattern =
                $"\\A\\\\symboldeclare{LatexParameterRegex("identifier")}{LatexParameterRegex("formula")}";

            Match formulaDeclarationMatch = Regex.Match(input, formulaDeclarationPattern);

            if (formulaDeclarationMatch.Success)
            {
                string identifier   = formulaDeclarationMatch.Groups["identifier"].Value;
                string formulaInput = formulaDeclarationMatch.Groups["formula"].Value;

                if (!(string.IsNullOrEmpty(formulaInput) && string.IsNullOrWhiteSpace(formulaInput)))
                {
                    Formula result = ConvertToFormula(formulaInput);

                    if (!(string.IsNullOrEmpty(identifier) && string.IsNullOrWhiteSpace(identifier)))
                    {
                        result.Identifier = identifier;
                    }

                    formulas.Add(result);
                }

                return;
            }

            throw new ConvertException($"Nem található formula deklaráció a következő bemenet: \"{input}\"");
        }

        /// <summary>
        /// Convert the given input string into an integer type term.
        /// </summary>
        /// <param name="input">The input of the term.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        /// <returns>The result of the convertion.</returns>
        public IntegerTypeTerm ConvertToIntegerTypeTerm(string input)
        {
            Token result = ConvertInputString(input);

            if (result is Operand operand && operand.TryGetOperand(out IntegerTypeTerm? term) && term is not null)
            {
                return term;
            }

            throw new ConvertException($"Nem konvertálható szám típusú kifejezéssé a token: \"{result}\"");
        }

        /// <summary>
        /// Convert the given input string into an logical term.
        /// </summary>
        /// <param name="input">The input of the term.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        /// <returns>The result of the convertion.</returns>
        public LogicalTerm ConvertToLogicalTerm(string input)
        {
            Token result = ConvertInputString(input);

            if (result is Operand operand && operand.TryGetOperand(out LogicalTerm? term) && term is not null)
            {
                return term;
            }

            throw new ConvertException($"Nem konvertálható logikai típusú kifejezéssé a következő token: \"{result}\"");
        }

        /// <summary>
        /// Convert the given input string into a formula.
        /// </summary>
        /// <param name="input">The input of the formula.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        /// <returns>The result of the convertion.</returns>
        public Formula ConvertToFormula(string input)
        {
            Token result = ConvertInputString(input);

            if (result is Operand operand && operand.TryGetOperand(out Formula? formula) && formula is not null)
            {
                return formula;
            }

            throw new ConvertException($"Nem konvertálható formulává a következő token: \"{result}\"");
        }

        /// <summary>
        /// Convert the given input string into a program.
        /// </summary>
        /// <param name="input">The input of the program.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        /// <returns>The result of the convertion.</returns>
        public Program ConvertToProgram(string input)
        {
            Token result = ConvertInputString(input);

            if (result is Operand operand && operand.TryGetOperand(out Program? program) && program is not null)
            {
                return program;
            }

            throw new ConvertException($"Nem konvertálható programmá a következő bemenet: \"{input}\"");
        }

        /// <summary>
        /// Convert the given input string into an imply.
        /// </summary>
        /// <param name="input">The input of the imply.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        /// <returns>The result of the convertion.</returns>
        public Imply ConvertToImply(string input)
        {
            string implyPattern =
                $"\\A\\\\imply{LatexParameterRegex("hypothesis")}{LatexParameterRegex("consequence")}";

            Match implyMatch = Regex.Match(input, implyPattern);

            if (implyMatch.Success)
            {
                Formula hypothesis  = ConvertToFormula(implyMatch.Groups["hypothesis"].Value);
                Formula consequence = ConvertToFormula(implyMatch.Groups["consequence"].Value);

                return new Imply(hypothesis, consequence);
            }

            throw new ConvertException($"Nem konvertálható implikációvá a következő bemenet: \"{input}\"");
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Converts the given input string into a <see cref="Token"/>.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        /// <returns>The result of the convertion.</returns>
        private Token ConvertInputString(string input)
        {
            LinkedList<Token> infixTokens = InfixTokens(input);

            LinkedList<Token> postfixTokens = Postfix(infixTokens);

            return PostfixEvaluated(postfixTokens);
        }

        /// <summary>
        /// Converts the given input string into a list of infix tokens.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        /// <returns>The list of infix tokens.</returns>
        private LinkedList<Token> InfixTokens(string input)
        {
            LinkedList<Token> infixTokens = new LinkedList<Token>();

            InfixTokens(input, infixTokens);

            return infixTokens;
        }

        /// <summary>
        /// Converts the given input string into a list of infix tokens.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="tokens">The list of infix tokens.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        private void InfixTokens(string input, LinkedList<Token> tokens)
        {
            const char delimiterChar = ' ';

            const StringSplitOptions bothSplitOptions
                = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

            string[] symbols = input.Split(delimiterChar, bothSplitOptions);

            int openSymbolsCount = 0;

            bool buildSymbols = false;

            StringBuilder commandBuilder = new StringBuilder();

            foreach (string symbol in input.Split(delimiterChar, bothSplitOptions))
            {
                const string parameterizedCommandBeginPattern  = @"\A\\[a-zA-Z]+{";
                const string parameterizedCommandEndingPattern = @"}\z";

                bool parameterizedCommandBegin = Regex.IsMatch(symbol, parameterizedCommandBeginPattern);
                bool parameterizedCommandEnd   = Regex.IsMatch(symbol, parameterizedCommandEndingPattern);

                int openBracketCount  = symbol.Count(character => character == '{');
                int closeBracketCount = symbol.Count(character => character == '}');

                bool notValidSymbol = parameterizedCommandBegin ^ parameterizedCommandEnd || 
                                      openBracketCount != closeBracketCount;

                if (notValidSymbol || buildSymbols)
                {
                    commandBuilder.AppendFormat(buildSymbols ? " {0}" : "{0}", symbol);

                    openSymbolsCount += openBracketCount - closeBracketCount;

                    buildSymbols = openSymbolsCount != 0;
                }

                if (!buildSymbols)
                {
                    Token token;

                    if (notValidSymbol)
                    {
                        token = ConvertToToken(commandBuilder.ToString());

                        commandBuilder.Clear();
                    }
                    else
                    {
                        token = ConvertToToken(symbol);
                    }

                    tokens.AddLast(token);
                }
            }
        }

        /// <summary>
        /// Converts the symbol into a <see cref="Token"/>.
        /// </summary>
        /// <param name="symbol">The input symbol.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        /// <returns>The converted token.</returns>
        private Token ConvertToToken(string symbol)
        {
            switch (symbol)
            {
                case "\\true":
                    return new TermOperand(new LogicalConstant(true));

                case "\\false":
                    return new TermOperand(new LogicalConstant(false));

                case "\\TRUE":
                    return new FormulaOperand(TRUE.Instance());

                case "\\FALSE":
                    return new FormulaOperand(FALSE.Instance());

                case "\\NOTEVAL":
                    return new FormulaOperand(NotEvaluable.Instance());

                case "\\ABORT":
                    return new ProgramOperand(ABORT.Instance());

                case "\\SKIP":
                    return new ProgramOperand(SKIP.Instance());

                case "+":
                    return AdditionOperator.Instance();

                case "-":
                    return SubtractionOperator.Instance();

                case "\\cdot" or "*":
                    return MultiplicationOperator.Instance();

                case "\\mid":
                    return DivisorOperator.Instance();

                case "\\nmid":
                    return NotDivisorOperator.Instance();

                case "<":
                    return LessThanOperator.Instance();

                case ">":
                    return GreaterThanOperator.Instance();

                case "\\leq":
                    return LessThanOrEqualToOperator.Instance();

                case "\\geq":
                    return GreaterThanOrEqualToOperator.Instance();

                case "=":
                    return EqualOperator.Instance();

                case "\\neq":
                    return NotEqualOperator.Instance();

                case "\\wedge":
                    return ConjunctionOperator.Instance();

                case "\\vee":
                    return DisjunctionOperand.Instance();

                case "\\rightarrow":
                    return ImplicationOperator.Instance();

                case "\\neg":
                    return NegationOperator.Instance();

                case "(":
                    return LeftParenthesis.Instance();

                case ")":
                    return RightParenthesis.Instance();
            }

            Token? result =
                TryConvertToNumber(symbol) ??
                TryConvertToFormula(symbol) ??
                TryConvertToVariable(symbol) ??
                TryConvertToSummation(symbol) ??
                TryConvertToFunction(symbol) ??
                TryConvertToWeakestPrecondition(symbol) ??
                TryConvertToQuantifiedFormula(symbol) ??
                (Token?) TryConvertToAssignment(symbol);

            if (result is not null)
            {
                return result;
            }

            throw new ConvertException($"Ismeretlen szimbólum: \"{symbol}\"");
        }

        /// <summary>
        /// Tries to convert the input symbol into a number.
        /// </summary>
        /// <param name="symbol">The input symbol.</param>
        /// <returns>The converted token.</returns>
        private TermOperand? TryConvertToNumber(string symbol)
        {
            if (int.TryParse(symbol, out int value))
            {
                return new TermOperand(new IntegerTypeConstant(value));
            }

            return null;
        }

        /// <summary>
        /// Tries to convert the input symbol into a variable.
        /// </summary>
        /// <param name="symbol">The input symbol.</param>
        /// <returns>The converted token.</returns>
        private TermOperand? TryConvertToVariable(string symbol)
        {
            Variable<IntegerType>? integerVariable = integerTypeVariables.Find(var => var.Identifier == symbol);

            if (integerVariable is not null)
            {
                return new TermOperand(integerVariable.DeepCopy());
            }

            Variable<Logical>? logicalVariable = logicalVariables.Find(var => var.Identifier == symbol);

            if (logicalVariable is not null)
            {
                return new TermOperand(logicalVariable.DeepCopy());
            }

            return TryConvertToArrayVariable(symbol);
        }

        /// <summary>
        /// Tries to convert the input symbol into a function value.
        /// </summary>
        /// <param name="symbol">The input symbol.</param>
        /// <returns>The converted token.</returns>
        private TermOperand? TryConvertToFunction(string symbol)
        {
            string betaPattern = $"\\A\\\\betafunc{LatexParameterRegex("argument")}";

            Match betaMatch = Regex.Match(symbol, betaPattern);
            
            if (betaMatch.Success)
            {
                IntegerTypeTerm argument = ConvertToIntegerTypeTerm(betaMatch.Groups["argument"].Value);

                return new TermOperand(new BetaFunction(argument));
            }

            string chiPattern = $"\\A\\\\chifunc{LatexParameterRegex("argument")}";

            Match chiMatch = Regex.Match(symbol, chiPattern);

            if (chiMatch.Success)
            {
                LogicalTerm argument = ConvertToLogicalTerm(chiMatch.Groups["argument"].Value);

                return new TermOperand(new ChiFunction(argument));
            }

            return null;
        }

        /// <summary>
        /// Tries to convert the input symbol into a summation.
        /// </summary>
        /// <param name="symbol">The input symbol.</param>
        /// <returns>The converted token.</returns>
        private TermOperand? TryConvertToSummation(string symbol)
        {
            string summationPattern = 
                $"\\A\\\\summation{LatexParameterRegex("indexVariable")}{LatexParameterRegex("lower")}{LatexParameterRegex("upper")}{LatexParameterRegex("argument")}";

            Match summationMatch = Regex.Match(symbol, summationPattern);

            if (summationMatch.Success)
            {
                string indexVariable  = summationMatch.Groups["indexVariable"].Value.Trim();

                integerTypeVariables.Add(new Variable<IntegerType>(indexVariable, Integer.Instance()));

                IntegerTypeTerm lower = ConvertToIntegerTypeTerm(summationMatch.Groups["lower"].Value);
                IntegerTypeTerm upper = ConvertToIntegerTypeTerm(summationMatch.Groups["upper"].Value);
                IntegerTypeTerm argument = ConvertToIntegerTypeTerm(summationMatch.Groups["argument"].Value);

                return new TermOperand(new Summation(indexVariable, lower, upper, argument));
            }

            return null;
        }

        /// <summary>
        /// Tries to convert the input symbol into an array variable.
        /// </summary>
        /// <param name="symbol">The input symbol.</param>
        /// <returns>The converted token.</returns>
        private TermOperand? TryConvertToArrayVariable(string symbol)
        {
            string arrayVariablePattern =
                $"\\A\\\\arrayvar{LatexParameterRegex("identifier")}{LatexParameterRegex("index")}";

            Match arrayVariableMatch = Regex.Match(symbol, arrayVariablePattern);

            if (!arrayVariableMatch.Success)
            {
                return null;
            }

            string identifier   = arrayVariableMatch.Groups["identifier"].Value.Trim();
            IntegerTypeTerm ind = ConvertToIntegerTypeTerm(arrayVariableMatch.Groups["index"].Value);

            Variable<IntegerType>? integerVariable = integerTypeVariables.Find(var => var.Identifier == identifier);

            if (integerVariable is not null && integerVariable is ArrayVariable<IntegerType> integerArray)
            {
                ArrayVariable<IntegerType> result = integerArray.DeepCopy();

                result.IndexTerm = ind;

                return new TermOperand(result);
            }

            Variable<Logical>? logicalVariable = logicalVariables.Find(var => var.Identifier == identifier);

            if (logicalVariable is not null && logicalVariable is ArrayVariable<Logical> logicalArray)
            {
                ArrayVariable<Logical> result = logicalArray.DeepCopy();

                result.IndexTerm = ind;

                return new TermOperand(result);
            }

            return null;
        }

        /// <summary>
        /// Tries to convert the input symbol into an assignment.
        /// </summary>
        /// <param name="symbol">The input symbol.</param>
        /// <returns>The converted token.</returns>
        private ProgramOperand? TryConvertToAssignment(string symbol)
        {
            string assignmentPattern =
                $"\\A\\\\assign{LatexParameterRegex("variables")}{LatexParameterRegex("values")}";

            Match assignmentMatch = Regex.Match(symbol, assignmentPattern);

            if (!assignmentMatch.Success)
            {
                return null;
            }

            List<(Variable<IntegerType>, IntegerTypeTerm)> integerVariables 
                = new List<(Variable<IntegerType>, IntegerTypeTerm)>();

            List<(Variable<Logical>, LogicalTerm)> logicalVariables 
                = new List<(Variable<Logical>, LogicalTerm)>();

            const char delimiterChar = ',';

            const StringSplitOptions bothSplitOptions
                = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

            string[] variables = assignmentMatch.Groups["variables"].Value.Split(delimiterChar, bothSplitOptions);
            string[] values = assignmentMatch.Groups["values"].Value.Split(delimiterChar, bothSplitOptions);

            if (variables.Length != values.Length)
            {
                return null;
            }

            for (int ind = 0; ind < variables.Length; ++ind)
            {
                TermOperand? variable = TryConvertToVariable(variables[ind]);
                
                if (variable?.IntegerTypeTerm is Variable<IntegerType> integerVariable)
                {
                    integerVariables.Add((integerVariable, ConvertToIntegerTypeTerm(values[ind])));
                }
                else if (variable?.LogicalTerm is Variable<Logical> logicalVariable)
                {
                    logicalVariables.Add((logicalVariable, ConvertToLogicalTerm(values[ind])));
                }
                else
                {
                    return null;
                }
            }

            return new ProgramOperand(new Assignment(integerVariables, logicalVariables));
        }

        /// <summary>
        /// Tries to convert the input symbol into a quantified formula.
        /// </summary>
        /// <param name="symbol">The input symbol.</param>
        /// <returns>The converted token.</returns>
        private FormulaOperand? TryConvertToQuantifiedFormula(string symbol)
        {
            string universallyPattern =
                $"\\A\\\\universally{LatexParameterRegex("variable")}{LatexParameterRegex("type")}{LatexParameterRegex("statement")}";

            string existentiallyPattern =
                $"\\A\\\\existentially{LatexParameterRegex("variable")}{LatexParameterRegex("type")}{LatexParameterRegex("statement")}";

            Match universallyMatch   = Regex.Match(symbol, universallyPattern);
            Match existentiallyMatch = Regex.Match(symbol, existentiallyPattern);

            if (universallyMatch.Success || existentiallyMatch.Success)
            {
                Match match = universallyMatch.Success ? universallyMatch : existentiallyMatch;

                InitializeVariable(
                    match.Groups["variable"].Value, match.Groups["type"].Value,
                    out Variable<IntegerType>? integerVariable, out Variable<Logical>? logicalVariable
                );

                if (integerVariable is not null)
                {
                    integerTypeVariables.Add(integerVariable);
                }
                else if (logicalVariable is not null)
                {
                    logicalVariables.Add(logicalVariable);
                }

                Formula statement = ConvertToFormula(match.Groups["statement"].Value);

                if (integerVariable is not null)
                {
                    return new FormulaOperand(
                        universallyMatch.Success ?
                        new UniversallyQuantifiedFormula<IntegerType>(integerVariable, statement) :
                        new ExistentiallyQuantifiedFormula<IntegerType>(integerVariable, statement)
                    );
                }
                else if (logicalVariable is not null)
                {
                    return new FormulaOperand(
                        universallyMatch.Success ?
                        new UniversallyQuantifiedFormula<Logical>(logicalVariable, statement) :
                        new ExistentiallyQuantifiedFormula<Logical>(logicalVariable, statement)
                    );
                }
            }

            return null;
        }

        /// <summary>
        /// Tries to convert the input symbol into a formula.
        /// </summary>
        /// <param name="symbol">The input symbol.</param>
        /// <returns>The converted token.</returns>
        private FormulaOperand? TryConvertToFormula(string symbol)
        {
            Formula? formula = formulas.Find(currentFormula => currentFormula.Identifier == symbol);

            return formula is not null ? new FormulaOperand(formula.DeepCopy()) : null;
        }

        /// <summary>
        /// Tries to convert the input symbol into a weakest precondition.
        /// </summary>
        /// <param name="symbol">The input symbol.</param>
        /// <returns>The converted token.</returns>
        private FormulaOperand? TryConvertToWeakestPrecondition(string symbol)
        {
            string weakestPreconditionPattern =
                $"\\A\\\\weakestprec{LatexParameterRegex("program")}{LatexParameterRegex("formulaInput")}";

            Match weakestPreconditionMatch = Regex.Match(symbol, weakestPreconditionPattern);

            if (weakestPreconditionMatch.Success)
            {
                Program program = ConvertToProgram(weakestPreconditionMatch.Groups["program"].Value);
                Formula formula = ConvertToFormula(weakestPreconditionMatch.Groups["formulaInput"].Value);

                return new FormulaOperand(new WeakestPrecondition(program, formula));
            }

            return null;
        }

        /// <summary>
        /// Converts the given infix token list into a postfix token list.
        /// </summary>
        /// <param name="symbol">The list of infix tokens.</param>
        /// <returns>The list of postfix tokens.</returns>
        private LinkedList<Token> Postfix(LinkedList<Token> infixTokens)
        {
            LinkedList<Token> postfix = new LinkedList<Token>();

            Stack<Token> stack = new Stack<Token>();

            foreach (Token token in infixTokens)
            {
                if (token is Operand)
                {
                    postfix.AddLast(token);
                }

                else if (token is LeftParenthesis)
                {
                    stack.Push(token);
                }

                else if (token is RightParenthesis)
                {
                    bool notFoundLeftParenthesis = true;

                    while (NotEmpty(stack) && notFoundLeftParenthesis)
                    {
                        Token lastToken = stack.Pop();

                        notFoundLeftParenthesis = lastToken is not LeftParenthesis;

                        if (notFoundLeftParenthesis)
                        {
                            postfix.AddLast(lastToken);
                        }
                    }
                }

                else if (token is Operator currentOperator)
                {
                    Predicate<Operator> precedencePredicate = stackTopOperator =>
                    {
                        return currentOperator.LeftAssociative() ?
                               stackTopOperator.Precedence() >= currentOperator.Precedence() :
                               stackTopOperator.Precedence() >  currentOperator.Precedence();
                    };

                    while (NotEmpty(stack) && stack.Peek() is Operator stackTop && precedencePredicate(stackTop))
                    {
                        postfix.AddLast(stack.Pop());
                    }

                    stack.Push(currentOperator);
                }
            }

            while (NotEmpty(stack))
            {
                postfix.AddLast(stack.Pop());
            }

            return postfix;
        }

        /// <summary>
        /// Evaluates the given list of postfix tokens.
        /// </summary>
        /// <param name="postfix">The list of postfix tokens.</param>
        /// <returns>The result of the evaluation.</returns>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        private Operand PostfixEvaluated(LinkedList<Token> postfix)
        {
            Stack<Operand> stack = new Stack<Operand>();

            try
            {
                foreach (Token token in postfix)
                {
                    if (token is UnaryOperator unary)
                    {
                        Operand operand = stack.Pop();

                        stack.Push(unary.Evaluated(operand));
                    }
                    else if (token is BinaryOperator binary)
                    {
                        Operand right = stack.Pop();
                        Operand left = stack.Pop();

                        stack.Push(binary.Evaluated(left, right));
                    }
                    else if (token is Operand operand)
                    {
                        stack.Push(operand);
                    }
                    else
                    {
                        throw new ConvertException("Sikertelen kiértékelés!");
                    }
                }

                return stack.Pop();
            }
            catch (InvalidOperationException)
            {
                throw new ConvertException("A konvertáláshoz szükséges adatok hiányosak!" );
            }
            catch (Exception exc)
            {
                throw new ConvertException(exc.Message);
            }
        }

        /// <summary>
        /// Initialize all the variables, based on the given statespace.
        /// </summary>
        /// <param name="stateSpace">The statespace input.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        private void InitializeVariables(string stateSpace)
        {
            const char delimiterChar = ',';

            const StringSplitOptions bothSplitOptions 
                = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

            foreach (string declare in stateSpace.Split(delimiterChar, bothSplitOptions))
            {
                InitializeVariable(declare);
            }
        }

        /// <summary>
        /// Initialize the given variable.
        /// </summary>
        /// <param name="variableDeclaration">The variable declaration string.</param>
        /// <exception cref="ConvertException">If any convert error occures.</exception>
        private void InitializeVariable(string variableDeclaration)
        {
            string declarePattern = $"\\A\\\\declare{LatexParameterRegex("identifier")}{LatexParameterRegex("type")}";

            Match declareMatch = Regex.Match(variableDeclaration, declarePattern);

            if (declareMatch.Success)
            {
                string arrayPattern = $"\\A\\\\arraytype{LatexParameterRegex("base")}{LatexParameterRegex("length")}";

                Match arrayMatch = Regex.Match(declareMatch.Groups["type"].Value, arrayPattern);

                string identifier = declareMatch.Groups["identifier"].Value;
                string length     = arrayMatch  .Groups["length"].Value;

                Type type = ConvertBaseType(
                    arrayMatch.Success ? arrayMatch.Groups["base"].Value : declareMatch.Groups["type"].Value);

                bool lengthIsVariable = Regex.IsMatch(length, "[a-zA-Z_']");

                if (lengthIsVariable)
                {
                    integerTypeVariables.Add(new Variable<IntegerType>(length, NaturalNumber.Instance()));
                }

                if (type is IntegerType integerType)
                {
                    Variable<IntegerType> result = arrayMatch.Success ?
                        new ArrayVariable<IntegerType>(
                            identifier,
                            lengthIsVariable ? integerTypeVariables.Last() : ConvertToIntegerTypeTerm(length), 
                            integerType.DeepCopy()) :
                        new Variable<IntegerType>(identifier, integerType.DeepCopy());

                    integerTypeVariables.Add(result);
                    return;
                }

                if (type is Logical logical)
                {
                    Variable<Logical> result = arrayMatch.Success ?
                        new ArrayVariable<Logical>(
                            identifier,
                            lengthIsVariable ? integerTypeVariables.Last() : ConvertToIntegerTypeTerm(length),
                            logical.DeepCopy()) :
                        new Variable<Logical>(identifier, logical.DeepCopy());

                    logicalVariables.Add(result);
                    return;
                }
            }

            throw new ConvertException($"Nem konvertálható változóvá a következő bemenet: \"{variableDeclaration}\"");
        }


        /// <summary>
        /// Initialize the given variable, based on the information.
        /// </summary>
        /// <param name="identifier">The identifier of the variable.</param>
        /// <param name="variableType">The type of the variable.</param>
        /// <param name="integerVariable">The created integer variable.</param>
        /// <param name="logicalVariable">The created logical variable.</param>
        private void InitializeVariable(string identifier, string variableType, 
            out Variable<IntegerType>? integerVariable, out Variable<Logical>? logicalVariable)
        {
            integerVariable = null;
            logicalVariable = null;

            Type type = ConvertBaseType(variableType);

            if (type is IntegerType integerType)
            {
                integerVariable = new Variable<IntegerType>(identifier, integerType.DeepCopy());
            }
            else if (type is Logical logical)
            {
                logicalVariable = new Variable<Logical>(identifier, logical.DeepCopy());
            }
        }

        /// <summary>
        /// Initialize the given variable, based on the information.
        /// </summary>
        /// <param name="type">The type string.</param>
        /// <returns>The converted type.</returns>
        /// /// <exception cref="ConvertException">If any convert error occures.</exception>
        private Type ConvertBaseType(string type)
        {
            switch (type.Trim())
            {
                case "\\B":
                    return Logical.Instance();

                case "\\Z":
                    return Integer.Instance();

                case "\\N":
                    return NaturalNumber.Instance();

                case "\\posN":
                    return PositiveInteger.Instance();

                case "\\zeroone":
                    return ZeroOrOne.Instance();
            }

            string intervalPattern = $"\\A\\\\interval{LatexParameterRegex("lower")}{LatexParameterRegex("upper")}";

            Match intervalMatch = Regex.Match(type, intervalPattern);

            if (intervalMatch.Success)
            {
                IntegerTypeTerm lowerBound = ConvertToIntegerTypeTerm(intervalMatch.Groups["lower"].Value);
                IntegerTypeTerm upperBound = ConvertToIntegerTypeTerm(intervalMatch.Groups["upper"].Value);

                if (lowerBound is IntegerTypeConstant lower && upperBound is IntegerTypeConstant upper)
                {
                    return new ConstantBoundedInteger(lower.Value, upper.Value);
                }
                else
                {
                    return new TermBoundedInteger(lowerBound, upperBound);
                }
            }

            throw new ConvertException($"Nem konvertálható típussá a következő bemenet: \"{type}\"");
        }

        /// <summary>
        /// Creates the latex parameter regex string.
        /// </summary>
        /// <returns>The latex parameter regex string.</returns>
        private string LatexParameterRegex()
        {
            return "{(?>{(?<c>)|[^{}]+|}(?<-c>))*(?(c)(?!))}";
        }

        /// <summary>
        /// Creates the latex parameter regex string with the given group name.
        /// </summary>
        /// <param name="groupName">The group name of the parameter.</param>
        /// <returns>The latex parameter regex string.</returns>
        private string LatexParameterRegex(string groupName)
        {
            return $"{{(?<{groupName}>(?>{{(?<c>)|[^{{}}]+|}}(?<-c>))*(?(c)(?!)))}}";
        }

        #endregion

        #region Inline methods

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private bool NotEmpty(ICollection collection)
        {
            return collection.Count > 0;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private bool IsEmpty(ICollection collection)
        {
            return collection.Count == 0;
        }

        #endregion
    }
}
