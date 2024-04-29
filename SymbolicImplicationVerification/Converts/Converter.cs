using SymbolicImplicationVerification.Converts.Tokens;
using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Converts.Tokens.Operators;
using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Formulas.Quantified;
using SymbolicImplicationVerification.Implies;
using SymbolicImplicationVerification.Programs;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.FunctionValues;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SymbolicImplicationVerification.Converts
{
    internal class Converter
    {
        #region Fields

        private List<Formula> formulas;

        private List<Variable<IntegerType>> integerTypeVariables;

        private List<Variable<Logical>> logicalVariables;

        #endregion

        #region Constant values

        //private const string latexParameterRegex = "\\{(?>\\{(?<c>)|[^{}]+|\\}(?<-c>))*(?(c)(?!))\\}";

        #endregion

        #region Constructors

        public Converter()
        {
            formulas = new List<Formula>();

            integerTypeVariables = new List<Variable<IntegerType>>();

            logicalVariables = new List<Variable<Logical>>();
        }

        #endregion

        #region Public methods

        public void DeclareStateSpace(string input)
        {
            string stateSpacePattern =
                $"\\symboldeclare{LatexParameterRegex()}{LatexParameterRegex("declarations")}";

            Match stateSpacMatch = Regex.Match(input, stateSpacePattern);

            if (stateSpacMatch.Success)
            {
                InitializeVariables(stateSpacMatch.Groups["declarations"].Value);

                return;
            }

            throw new ConvertException($"Nem található állapottér deklaráció a következő bemenet: \"{input}\"");
        }

        public void DeclareFormula(string input)
        {
            string formulaDeclarationPattern =
                $"\\symboldeclare{LatexParameterRegex("identifier")}{LatexParameterRegex("formula")}";

            Match formulaDeclarationMatch = Regex.Match(input, formulaDeclarationPattern);

            if (formulaDeclarationMatch.Success)
            {
                string identifier   = formulaDeclarationMatch.Groups["identifier"].Value;
                string formulaInput = formulaDeclarationMatch.Groups["formula"].Value;

                Formula result = ConvertToFormula(formulaInput);

                if (!(string.IsNullOrEmpty(identifier) || string.IsNullOrWhiteSpace(identifier)))
                {
                    result.Identifier = identifier;
                }

                formulas.Add(result);

                return;
            }

            throw new ConvertException($"Nem található formula deklaráció a következő bemenet: \"{input}\"");
        }

        public IntegerTypeTerm ConvertToIntegerTypeTerm(string input)
        {
            Token result = ConvertInputString(input);

            if (result is TermOperand termOperand && termOperand.IntegerTypeTerm is not null)
            {
                return termOperand.IntegerTypeTerm.DeepCopy();
            }

            throw new ConvertException($"Nem konvertálható szám típusú kifejezéssé a token: \"{result}\"");
        }

        public LogicalTerm ConvertToLogicalTerm(string input)
        {
            Token result = ConvertInputString(input);

            if (result is TermOperand termOperand && termOperand.LogicalTerm is not null)
            {
                return termOperand.LogicalTerm.DeepCopy();
            }

            throw new ConvertException($"Nem konvertálható logikai típusú kifejezéssé a következő token: \"{result}\"");
        }

        public Formula ConvertToFormula(string input)
        {
            Token result = ConvertInputString(input);

            if (result is FormulaOperand formulaOperand)
            {
                return formulaOperand.Formula.DeepCopy();
            }

            throw new ConvertException($"Nem konvertálható formulává a következő token: \"{result}\"");
        }

        public Program ConvertToProgram(string input)
        {
            Token result = ConvertInputString(input);

            if (result is ProgramOperand programOperand)
            {
                return programOperand.Program.DeepCopy();
            }

            throw new ConvertException($"Nem konvertálható programmá a következő bemenet: \"{input}\"");
        }

        public Imply ConvertToImply(string input)
        {
            string implyPattern =
                $"\\imply{LatexParameterRegex("hypothesis")}{LatexParameterRegex("consequence")}";

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

        private Token ConvertInputString(string input)
        {
            LinkedList<Token> infixTokens = InfixTokens(input);

            LinkedList<Token> postfixTokens = Postfix(infixTokens);

            return PostfixEvaluated(postfixTokens);
        }

        private LinkedList<Token> InfixTokens(string input)
        {
            LinkedList<Token> infixTokens = new LinkedList<Token>();

            InfixTokens(input, infixTokens);

            return infixTokens;
        }

        private void InfixTokens(string input, LinkedList<Token> tokens)
        {
            const char delimiterChar = ' ';

            const StringSplitOptions bothSplitOptions
                = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

            foreach (string symbol in input.Split(delimiterChar, bothSplitOptions))
            {
                foreach (string prefix in Prefixes(symbol))
                {
                    Token token = ConvertToToken(prefix);

                    tokens.AddLast(token);
                }
            }
        }

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

        private TermOperand? TryConvertToNumber(string symbol)
        {
            if (int.TryParse(symbol, out int value))
            {
                return new TermOperand(new IntegerConstant(value));
            }

            return null;
        }

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

        //private TermOperand? TryConvertToNumberOrVariable(string symbol)
        //{
        //    if (int.TryParse(symbol, out int value))
        //    {
        //        return new TermOperand(new IntegerConstant(value));
        //    }

        //    Variable<IntegerType>? integerVariable = integerTypeVariables.Find(var => var.Identifier == symbol);

        //    if (integerVariable is not null)
        //    {
        //        return new TermOperand(integerVariable.DeepCopy());
        //    }

        //    Variable<Logical>? logicalVariable = logicalVariables.Find(var => var.Identifier == symbol);

        //    if (logicalVariable is not null)
        //    {
        //        return new TermOperand(logicalVariable.DeepCopy());
        //    }

        //    return null;
        //}

        private TermOperand? TryConvertToFunction(string symbol)
        {
            string betaPattern = $"\\betafunc{LatexParameterRegex("argument")}";

            Match betaMatch = Regex.Match(symbol, betaPattern);
            
            if (betaMatch.Success)
            {
                IntegerTypeTerm argument = ConvertToIntegerTypeTerm(betaMatch.Groups["argument"].Value);

                return new TermOperand(new BetaFunction(argument));
            }

            string chiPattern = $"\\chifunc{LatexParameterRegex("argument")}";

            Match chiMatch = Regex.Match(symbol, chiPattern);

            if (chiMatch.Success)
            {
                LogicalTerm argument = ConvertToLogicalTerm(chiMatch.Groups["argument"].Value);

                return new TermOperand(new ChiFunction(argument));
            }

            return null;
        }

        private TermOperand? TryConvertToSummation(string symbol)
        {
            string summationPattern = 
                $"\\summation{LatexParameterRegex("currentFormula")}{LatexParameterRegex("lower")}" +
                $"{LatexParameterRegex("upper")}{LatexParameterRegex("argument")}";

            Match summationMatch = Regex.Match(symbol, summationPattern);

            if (summationMatch.Success)
            {
                string indexVariable  = summationMatch.Groups["currentFormula"].Value.Trim();
                IntegerTypeTerm lower = ConvertToIntegerTypeTerm(summationMatch.Groups["lower"].Value);
                IntegerTypeTerm upper = ConvertToIntegerTypeTerm(summationMatch.Groups["upper"].Value);
                IntegerTypeTerm argument = ConvertToIntegerTypeTerm(summationMatch.Groups["argument"].Value);

                return new TermOperand(new Summation(indexVariable, lower, upper, argument));
            }

            return null;
        }

        private TermOperand? TryConvertToArrayVariable(string symbol)
        {
            string arrayVariablePattern =
                $"\\arrayvar{LatexParameterRegex("identifier")}{LatexParameterRegex("index")}";

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

        private ProgramOperand? TryConvertToAssignment(string symbol)
        {
            string assignmentPattern =
                $"\\assign{LatexParameterRegex("variables")}{LatexParameterRegex("values")}";

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

        private FormulaOperand? TryConvertToQuantifiedFormula(string symbol)
        {
            string universallyPattern =
                $"\\universally{LatexParameterRegex("variable")}{LatexParameterRegex("type")}{LatexParameterRegex("statement")}";

            string existentiallyPattern =
                $"\\existentially{LatexParameterRegex("variable")}{LatexParameterRegex("type")}{LatexParameterRegex("statement")}";

            Match universallyMatch   = Regex.Match(symbol, universallyPattern);
            Match existentiallyMatch = Regex.Match(symbol, existentiallyPattern);

            if (universallyMatch.Success || existentiallyMatch.Success)
            {
                Match match = universallyMatch.Success ? universallyMatch : existentiallyMatch;

                InitializeVariable(
                    match.Groups["variable"].Value, match.Groups["type"].Value,
                    out Variable<IntegerType>? integerVariable, out Variable<Logical>? logicalVariable
                );

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

        private FormulaOperand? TryConvertToFormula(string symbol)
        {
            Formula? formula = formulas.Find(currentFormula => currentFormula.Identifier == symbol);

            return formula is not null ? new FormulaOperand(formula.DeepCopy()) : null;
        }

        private FormulaOperand? TryConvertToWeakestPrecondition(string symbol)
        {
            string weakestPreconditionPattern =
                $"\\weakestprec{LatexParameterRegex("program")}{LatexParameterRegex("formulaInput")}";

            Match weakestPreconditionMatch = Regex.Match(symbol, weakestPreconditionPattern);

            if (weakestPreconditionMatch.Success)
            {
                Program program = ConvertToProgram(weakestPreconditionMatch.Groups["program"].Value);
                Formula formula = ConvertToFormula(weakestPreconditionMatch.Groups["formulaInput"].Value);

                return new FormulaOperand(new WeakestPrecondition(program, formula));
            }

            return null;
        }

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

        private Operand PostfixEvaluated(LinkedList<Token> postfix)
        {
            Stack<Operand> stack = new Stack<Operand>();

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
                    Operand left  = stack.Pop();

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

        //private string Prefix(string input, out string remainder)
        //{
        //    const string inputStartsWithConstantPattern =
        //        @"\A(?<constant>(-[0-9]*|[0-9]+))(?<variable>[a-zA-Z]*)";

        //    const string inputStartsWithCommandPattern =
        //        @"\A(\\\P{Nd}+(\{(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))\})+)";

        //    Match startCommandMatch  = Regex.Match(input, inputStartsWithCommandPattern);
        //    Match startConstantMatch = Regex.Match(input, inputStartsWithConstantPattern);

        //    string prefix;

        //    if (startCommandMatch.Success)
        //    {
        //        prefix    = startCommandMatch.Value;
        //        remainder = input.Substring(startCommandMatch.Length);
        //    }
        //    else
        //    {
        //        const int firstCharIndex = 0;

        //        int charLocation = input.IndexOfAny(new char[] { '\\', '+', '-', '<', '>', '=', '(', ')' });

        //        bool foundChar = charLocation >= firstCharIndex;

        //        prefix    = foundChar ? input.Substring(firstCharIndex, charLocation + 1) : input;
        //        remainder = foundChar ? input.Substring(charLocation + 1) : string.Empty;
        //    }

        //    return prefix;
        //}

        private LinkedList<string> Prefixes(string input)
        {
            const string inputStartsWithConstantPattern =
                @"\A(?<constant>(-[0-9]*|[0-9]+))(?<variable>[a-zA-Z(\\]*)";

            const string inputStartsWithCommandPattern =
                @"\A(\\\P{Nd}+(\{(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))\})+)";

            LinkedList<string> prefixes = new LinkedList<string>();

            while (!string.IsNullOrEmpty(input))
            {
                Match startCommandMatch  = Regex.Match(input, inputStartsWithCommandPattern);
                Match startConstantMatch = Regex.Match(input, inputStartsWithConstantPattern);

                if (startCommandMatch.Success)
                {
                    prefixes.AddLast(startCommandMatch.Value);

                    input = input.Substring(startCommandMatch.Length);
                }
                else if (startConstantMatch.Success)
                {
                    string constant = startConstantMatch.Groups["constant"].Value;
                    string variable = startConstantMatch.Groups["variable"].Value;

                    if (constant[0] != '-')
                    {
                        prefixes.AddLast(constant);

                        string remainder = input.Substring(constant.Length);

                        input = string.IsNullOrEmpty(variable) ? remainder : $"*{remainder}";
                    }
                    else
                    {
                        string[] symbols = new string[] { "\\cdot", "-", "+", "*", "(" };

                        bool isNegativeConstant = IsEmpty(prefixes) || symbols.Contains(prefixes.Last());

                        if (isNegativeConstant)
                        {
                            prefixes.AddLast(constant == "-" ? "-1" : constant);

                            string remainder = input.Substring(constant.Length);

                            input = string.IsNullOrEmpty(variable) ? remainder : $"*{remainder}";
                        }
                        else
                        {
                            prefixes.AddLast("-");

                            input = input.Substring(1);
                        }
                    }
                }
                else
                {
                    const int firstCharIndex = 0;

                    int charLocation = input.IndexOfAny(new char[] { '\\', '+', '-', '*', '<', '>', '=', '(', ')' });

                    bool foundChar = charLocation >= firstCharIndex;

                    prefixes.AddLast(foundChar ? input.Substring(firstCharIndex, charLocation + 1) : input);

                    input = foundChar ? input.Substring(charLocation + 1) : string.Empty;
                }
            }

            return prefixes;
        }

        //private string Prefix(string input, out string remainder)
        //{
        //    char[] chars = new char[] { '\\', '+', '-', '<', '>', '=', '(', ')' };

        //    const int firstCharIndex  = 0;
        //    const int secondCharIndex = 1;

        //    remainder = string.Empty;

        //    if (!string.IsNullOrEmpty(input) && input.Length > secondCharIndex)
        //    {
        //        int charLocation = input.IndexOfAny(chars, secondCharIndex);

        //        if (charLocation > firstCharIndex)
        //        {
        //            remainder = input.Substring(charLocation);

        //            return input.Substring(firstCharIndex, charLocation);
        //        }
        //    }

        //    return input;
        //}


        //private int OperatorPrecedence(string operatorString)
        //{
        //    switch (operatorString.Trim())
        //    {
        //        case "\\neg":
        //            return 8;

        //        case "\\cdot":
        //            return 7;

        //        case "+" or "-":
        //            return 6;

        //        case ">" or "<" or "\\geq" or "\\leq":
        //            return 5;

        //        case "=" or "\\neq":
        //            return 4;

        //        case "\\wedge":
        //            return 3;

        //        case "\\vee":
        //            return 2;

        //        case "\\rightarrow":
        //            return 1;

        //        default:
        //            return -1;
        //    }
        //}

        //private int OperatorArity(string operatorString)
        //{
        //    if (NotOperator(operatorString))
        //    {
        //        return -1;
        //    }

        //    switch (operatorString.Trim())
        //    {
        //        case "\\neg":
        //            return 1;

        //        default:
        //            return 2;
        //    }
        //}

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

        private void InitializeVariable(string variableDeclaration)
        {
            string declarePattern = $"\\declare{LatexParameterRegex("identifier")}{LatexParameterRegex("type")}";

            Match declareMatch = Regex.Match(variableDeclaration, declarePattern);

            if (declareMatch.Success)
            {
                string arrayPattern = $"\\arraytype{LatexParameterRegex("base")}{LatexParameterRegex("length")}";

                Match arrayMatch = Regex.Match(declareMatch.Groups["type"].Value, arrayPattern);

                string identifier = declareMatch.Groups["identifier"].Value;

                Type type = ConvertBaseType(
                    arrayMatch.Success ? arrayMatch.Groups["base"].Value : declareMatch.Groups["type"].Value);

                if (type is IntegerType integerType)
                {
                    Variable<IntegerType> result = arrayMatch.Success ?
                        new ArrayVariable<IntegerType>(identifier,
                            ConvertToIntegerTypeTerm(arrayMatch.Groups["length"].Value), integerType.DeepCopy()) :
                        new Variable<IntegerType>(identifier, integerType.DeepCopy());

                    integerTypeVariables.Add(result);
                    return;
                }

                if (type is Logical logical)
                {
                    Variable<Logical> result = arrayMatch.Success ?
                        new ArrayVariable<Logical>(identifier,
                            ConvertToIntegerTypeTerm(arrayMatch.Groups["length"].Value), logical.DeepCopy()) :
                        new Variable<Logical>(identifier, logical.DeepCopy());

                    logicalVariables.Add(result);
                    return;
                }
            }

            throw new ConvertException($"Nem konvertálható változóvá a következő bemenet: \"{variableDeclaration}\"");
        }

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

            string intervalPattern = $"\\interval{LatexParameterRegex("lower")}{LatexParameterRegex("upper")}";

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

        //private Type ConverType(string type)
        //{
        //    type = type.Trim();

        //    switch (type)
        //    {
        //        case "\\B":
        //            return Logical.Instance();

        //        case "\\Z":
        //            return Integer.Instance();

        //        case "\\N":
        //            return NaturalNumber.Instance();

        //        case "\\posN":
        //            return PositiveInteger.Instance();

        //        case "\\zeroone":
        //            return ZeroOrOne.Instance();
        //    }

        //    string arrayTypePattern    = $"\\arraytype{LatexParameterRegex("type")}{LatexParameterRegex("length")}";
        //    string intervalTypePattern = $"\\interval{LatexParameterRegex("lower")}{LatexParameterRegex("upper")}";

        //    Match arrayMatch    = Regex.Match(type, arrayTypePattern);
        //    Match intervalMatch = Regex.Match(type, intervalTypePattern);

        //    if (!(arrayMatch.Success || intervalMatch.Success))
        //    {
        //        throw new Exception();
        //    }

        //    if (arrayMatch.Success)
        //    {
        //        Type arrayType = ConverType(arrayMatch.Groups["type"].Value.Trim());

        //        IntegerTypeTerm length = ConvertToIntegerTypeTerm(arrayMatch.Groups["length"].Value);



        //    }
        //}

        //private void InitializeVariable(List<string> parameters)
        //{
        //    string identifier = parameters.First();

        //    Type type = ParseType(parameters.Last());

        //    switch (type)
        //    {
        //        case IntegerType integerType:
        //            integerTypeVariables.AddCommandParts(new Variable<IntegerType>(identifier, integerType));
        //            break;

        //        case Logical logical:
        //            logicalVariables.AddCommandParts(new Variable<Logical>(identifier, logical));
        //            break;
        //    }
        //}

        //private Type ParseType(string type)
        //{
        //    switch (type)
        //    {
        //        case "\\Z":
        //            return Integer.Instance();

        //        case "\\N":
        //            return NaturalNumber.Instance();

        //        case "\\posN":
        //            return PositiveInteger.Instance();

        //        case "\\zeroone":
        //            return ZeroOrOne.Instance();

        //        case "\\B":
        //            return Logical.Instance();
        //    }
        //}


        //private string GetCurrentCommandArgument(string command, int start = 0)
        //{
        //    int startIndex = command.IndexOf(argumentStart, start);
        //    int endIndex   = command.LastIndexOf(argumentEnd);

        //    return command.Substring(startIndex, endIndex - startIndex);
        //}

        //private List<string> GetArguments(string command, int numberOfArguments, int start = 0)
        //{
        //    List<string> result = new List<string>(numberOfArguments);

        //    List<int> startIndexes = new List<int>(numberOfArguments);
        //    List<int> endIndexes   = new List<int>(numberOfArguments);

        //    int openBrackets   = 0;
        //    int foundArguments = 0;

        //    for (int ind = start; foundArguments != numberOfArguments && ind < command.Length; ++ind)
        //    {
        //        char currentChar = command[ind];

        //        if (currentChar == argumentStart)
        //        {
        //            if (openBrackets == 0)
        //            {
        //                startIndexes.AddCommandParts(ind);
        //            }

        //            ++openBrackets;
        //        }
        //        else if (currentChar == argumentEnd)
        //        {
        //            if (openBrackets == 1)
        //            {
        //                endIndexes.AddCommandParts(ind);

        //                ++foundArguments;
        //            }

        //            --openBrackets;
        //        }
        //    }

        //    if (foundArguments != numberOfArguments)
        //    {
        //        throw new Exception();
        //    }

        //    for (int i = 0; i < numberOfArguments; ++i)
        //    {
        //        int begin  = startIndexes[i];
        //        int length = endIndexes[i] - begin;

        //        result.AddCommandParts(command.Substring(begin, length));
        //    }

        //    return result;
        //}

        private string LatexParameterRegex()
        {
            return "\\{{(?>\\{{(?<c>)|[^{{}}]+|\\}}(?<-c>))*(?(c)(?!))\\}}";
        }

        private string LatexParameterRegex(string groupName)
        {
            return $"\\{{(?<{groupName}>(?>\\{{(?<c>)|[^{{}}]+|\\}}(?<-c>))*(?(c)(?!)))\\}}";
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
