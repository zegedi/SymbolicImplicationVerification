using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace SymbolicImplicationVerification.Converts
{
    public class LatexEditor
    {
        #region Fields

        private LinkedList<string> latexTokens;

        private LinkedListNode<string>? pointer;

        private LatexCommand? currentCommand;

        private int currentArgumentIndex;

        #endregion

        #region Constructors

        public LatexEditor() : this(new LinkedList<string>()) { }

        public LatexEditor(string input, char separator)
        {
            latexTokens = new LinkedList<string>();

            foreach (string token in input.Split(separator, bothSplitOptions))
            {
                latexTokens.AddLast(token);
            }

            pointer = latexTokens.Last;
        }

        public LatexEditor(LinkedList<string> latexTokens)
        {
            this.latexTokens = new LinkedList<string>();

            foreach (string token in latexTokens)
            {
                this.latexTokens.AddLast(token);
            }

            pointer = latexTokens.Last;
        }

        #endregion

        #region Constant values

        private const int firstArgument  = 0;
        private const int secondArgument = 1;
        private const int thirdArgument  = 2;
        private const int fourthArgument = 3;

        private const StringSplitOptions bothSplitOptions
            = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

        #endregion

        #region Public properties

        public int? MinimumValue
        {
            get
            {
                int? result = null;

                if (currentCommand == LatexCommand.ArrayVariableDeclaration && currentArgumentIndex == secondArgument)
                {
                    result = 0;
                }

                return result;
            }
        }

        public LatexCommand Unremoveable
        {
            get; set;
        }

        public bool FormulaEdition
        {
            get; set;
        }

        public LatexCommand? CurrentCommand
        {
            get { return currentCommand; }
        }

        public int CurrentArgumentIndex
        {
            get { return currentArgumentIndex; }
        }

        #endregion

        #region Public static properties

        /// <summary>
        /// Gets the Latex code of the current command.
        /// </summary>
        /// <param name="command">The specified command.</param>
        /// <returns>The LaTeX code of the command.</returns>
        public static string LatexCode(LatexCommand command)
        {
            DescriptionAttribute[]? attributes = command
               .GetType()
               .GetField(command.ToString())
              ?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            string result;

            if (attributes is not null && attributes.Length > 0)
            {
                result = attributes[0].Description;
            }
            else
            {
                result = string.Empty;
            }

            return result;
        }

        #endregion

        #region Public methods

        public bool Enabled(LatexCommand command)
        {
            return AddEnabled(command) && !AddDisabled(command);
        }

        public bool IdentifierEnabled()
        {
            return AddIdentifierEnabled() && !AddIdentifierDisabled();
        }

        public bool NewIdentifierEnabled()
        {
            return AddNewIdentifierEnabled();
        }

        public bool ConstantEnabled()
        {
            return AddConstantEnabled() && !AddConstantDisabled();
        }

        public void Add(LatexCommand command)
        {
            string[] commandParts = CommandParts(command);

            bool addSeparatorBefore =
                command == LatexCommand.VariableDeclaration && pointer?.Value == "}";

            bool addSeparatorAfter =
                command == LatexCommand.VariableDeclaration && 
                          (pointer?.Value == ToLatex(LatexCommand.Separator) || StartOfParameterisedCommand(pointer?.Next));

            AddCommandParts(commandParts, addSeparatorBefore, addSeparatorAfter);

            if (commandParts.Length >= 2)
            {
                currentCommand = command;

                currentArgumentIndex = 0;
            }
        }

        public void Add(int value)
        {
            Add(value.ToString());
        }

        public void Add(string identifier)
        {
            if (pointer is null)
            {
                pointer = latexTokens.AddFirst(identifier);
            }
            else
            {
                pointer = latexTokens.AddAfter(pointer, identifier);
            }
        }

        public string Join(char separator)
        {
            return string.Join(separator, latexTokens);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return ToString(out int pointerIndex);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="pointerIndex">The current index of the pointer.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(out int pointerIndex)
        {
            const char separatorChar       = ' ';
            const int  separatorCharLength = 1;

            StringBuilder stringBuilder = new StringBuilder();

            LinkedListNode<string>? current = latexTokens.First;

            pointerIndex = 0;

            bool addSeparatorBefore = false;
            bool pointerNotFound    = pointer is not null;

            while (current is not null)
            {
                string currentValue = current.Value;

                if (addSeparatorBefore && currentValue != "}{" && currentValue != "}")
                {
                    stringBuilder.Append(separatorChar);

                    if (pointerNotFound)
                    {
                        pointerIndex += separatorCharLength;
                    }
                }

                addSeparatorBefore = !PartOfParameterisedCommand(current) || currentValue == "}";

                stringBuilder.Append(currentValue);

                if (pointerNotFound)
                {
                    pointerIndex += currentValue.Length;

                    pointerNotFound = current != pointer;
                }

                current = current!.Next;
            }

            return stringBuilder.ToString();
        }

        public int PointerIndex()
        {
            const int separatorCharLength = 1;

            LinkedListNode<string>? current = latexTokens.First;

            bool addSeparatorBefore = false;
            bool pointerNotFound    = pointer is not null;

            int pointerIndex = 0;

            while (current is not null && pointerNotFound)
            {
                string currentValue = current.Value;

                if (addSeparatorBefore && currentValue != "}{" && currentValue != "}")
                {
                    pointerIndex += separatorCharLength;
                }

                addSeparatorBefore = !PartOfParameterisedCommand(current) || currentValue == "}";
                
                pointerIndex    += currentValue.Length;
                pointerNotFound  = current != pointer;
                
                current = current!.Next;
            }

            return pointerIndex;
        }

        public bool MoveLeft()
        {
            bool moveLeft = pointer is not null;

            if (moveLeft)
            {
                string currentToken = pointer!.Value;

                if (currentToken == "}{")
                {
                    --currentArgumentIndex;
                }

                pointer = pointer!.Previous;

                if (pointer?.Value == ToLatex(LatexCommand.Separator))
                {
                    pointer = pointer.Previous;
                }

                if (PartOfParameterisedCommand(currentToken))
                {
                    UpdateCurrentCommandAndArgumentCounter();
                }
            }

            return moveLeft;
        }

        public bool MoveRight()
        {
            bool moveRight   = pointer is not null && pointer.Next      is not null;
            bool moveToFirst = pointer is     null && latexTokens.First is not null;

            if (moveRight)
            {
                pointer = pointer!.Next;
            }
            else if (moveToFirst)
            {
                pointer = latexTokens.First;
            }

            bool moved = moveRight || moveToFirst;

            if (moved)
            {
                if (pointer!.Value == ToLatex(LatexCommand.Separator) && pointer!.Next is not null)
                {
                    pointer = pointer.Next;
                }

                string currentToken = pointer!.Value;

                if (currentToken == "}{")
                {
                    ++currentArgumentIndex;
                }
                else if (StartOfParameterisedCommand(currentToken))
                {
                    currentArgumentIndex = 0;

                    currentCommand = FindCommand(currentToken);
                }
                else if (currentToken == "}")
                {
                    UpdateCurrentCommandAndArgumentCounter();
                }
            }

            return moved;
        }

        public void Remove()
        {
            if (pointer is null)
            {
                return;
            }

            if (PartOfParameterisedCommand(pointer))
            {
                if (AllParametersEmpty(pointer, out LinkedListNode<string>? commandStart) && 
                    commandStart is not null && !CommandPrefixMatch(commandStart, Unremoveable))
                {
                    pointer = commandStart;

                    MoveLeft();

                    bool commandNotRemoved = true;

                    while (commandNotRemoved && commandStart is not null)
                    {
                        LinkedListNode<string> commandNode = commandStart;

                        commandStart = commandStart.Next;

                        commandNotRemoved = commandNode.Value != "}";

                        latexTokens.Remove(commandNode);
                    }
                }

                else if (StartOfParameterisedCommand(pointer))
                {
                    MoveLeft();
                }

                else
                {
                    while (PartOfParameterisedCommand(pointer))
                    {
                        MoveLeft();
                    }
                }
            }
            else
            {
                LinkedListNode<string> current = pointer;

                MoveLeft();

                latexTokens.Remove(current);
            }

            if (pointer?.Next?.Value == ToLatex(LatexCommand.Separator))
            {
                latexTokens.Remove(pointer!.Next!);
            }
        }

        public string? GetCurrentCommandArgument(int argumentIndex)
        {
            if (currentCommand is null || argumentIndex < 0 || 
                CommandArgumentCount(currentCommand.Value) <= argumentIndex)
            {
                return null;
            }

            LinkedListNode<string>? current = FindCommandStart(pointer);

            int currentArgumentIndex = -1;

            while (current is not null && currentArgumentIndex != argumentIndex)
            {
                if (PartOfParameterisedCommand(current))
                {
                    ++currentArgumentIndex;
                }

                current = current.Next;
            }

            return current?.Value;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the Latex code of the current command.
        /// </summary>
        /// <param name="command">The specified command.</param>
        /// <returns>The LaTeX code of the command.</returns>
        private string ToLatex(LatexCommand command)
        {
            DescriptionAttribute[]? attributes = command
               .GetType()
               .GetField(command.ToString())
              ?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            string result;

            if (attributes is not null && attributes.Length > 0)
            {
                result = attributes[0].Description;
            }
            else
            {
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Adds the command parts to the latex tokens.
        /// </summary>
        /// <param name="parts">The parts of the command.</param>
        /// <param name="addSeparatorBefore">Indicates whether to add a separator before the command.</param>
        private void AddCommandParts(string[] parts, bool addSeparatorBefore, bool addSeparatorAfter)
        {
            LinkedListNode<string>? addingPointer = pointer;

            foreach (string part in parts)
            {
                if (addingPointer is null)
                {
                    addingPointer = latexTokens.AddFirst(part);
                }
                else
                {
                    addingPointer = latexTokens.AddAfter(addingPointer, part);
                }
            }

            if (parts.Length > 0)
            {
                string separator = ToLatex(LatexCommand.Separator);

                if (addSeparatorAfter && addingPointer is not null)
                {
                    latexTokens.AddAfter(addingPointer, separator);
                }

                if (addSeparatorBefore)
                {
                    pointer = pointer is null ?
                              latexTokens.AddFirst(separator).Next : 
                              latexTokens.AddAfter(pointer, separator).Next;
                }
                else
                {
                    pointer = pointer is null ? latexTokens.First : pointer.Next;
                }
            }
        }

        private LinkedListNode<string>? FindCommandStart(LinkedListNode<string>? current)
        {
            bool notFoundStart = true;

            while (notFoundStart && current is not null)
            {
                notFoundStart = !StartOfParameterisedCommand(current);

                if (notFoundStart)
                {
                    current = current.Previous;
                }
            }

            return current;
        }

        private bool AllParametersEmpty(LinkedListNode<string>? current, out LinkedListNode<string>? commandStart)
        {
            commandStart = FindCommandStart(current);

            current = commandStart;

            while (current?.Next?.Value == "}{")
            {
                current = current.Next;
            }

            return current?.Next?.Value == "}";
        }

        private bool PartOfParameterisedCommand(LinkedListNode<string>? current)
        {
            return PartOfParameterisedCommand(current?.Value);
        }

        private bool PartOfParameterisedCommand(string? currentValue)
        {
            const string partOfParameterisedCommandPattern = @"\}|\}\{|\\[a-zA-Z]+\{";

            return currentValue is not null && Regex.IsMatch(
                   currentValue, partOfParameterisedCommandPattern);
        }

        private bool StartOfParameterisedCommand(LinkedListNode<string>? current)
        {
            return StartOfParameterisedCommand(current?.Value);
        }

        private bool StartOfParameterisedCommand(string? currentValue)
        {
            const string startOfParameterisedCommandPattern = @"\\[a-zA-Z]+\{";

            return currentValue is not null && Regex.IsMatch(
                   currentValue, startOfParameterisedCommandPattern);
        }

        private bool IsCommand(LinkedListNode<string>? current)
        {
            return IsCommand(current?.Value);
        }

        private bool IsCommand(string? currentValue)
        {
            const string isCommandPattern = @"\\[a-zA-Z]+";

            return currentValue is not null && Regex.IsMatch(currentValue, isCommandPattern);
        }

        private bool IsIdentifier(LinkedListNode<string>? current)
        {
            return IsIdentifier(current?.Value);
        }

        private bool IsIdentifier(string? currentValue)
        {
            const string isIdentifierPattern = @"\A[a-zA-Z]+";

            return currentValue is not null && Regex.IsMatch(currentValue, isIdentifierPattern);
        }

        private bool IsConstant(LinkedListNode<string>? current)
        {
            return IsConstant(current?.Value);
        }

        private bool IsConstant(string? currentValue)
        {
            const string constantPattern = @"-[0-9]*|[0-9]+|\\false|\\true";

            return currentValue is not null && Regex.IsMatch(currentValue, constantPattern);
        }

        private bool IsNumberConstant(LinkedListNode<string>? current)
        {
            return IsNumberConstant(current?.Value);
        }

        private bool IsNumberConstant(string? currentValue)
        {
            return currentValue is not null && int.TryParse(currentValue, out int result);
        }

        private bool IsParameterEnd(LinkedListNode<string>? current)
        {
            return IsParameterEnd(current?.Value);
        }

        private bool IsParameterEnd(string? currentValue)
        {
            return currentValue == "}";
        }

        private bool IsType(LinkedListNode<string>? current)
        {
            return IsType(current?.Value);
        }

        private bool IsType(string? currentValue)
        {
            if (currentValue is null)
            {
                return false;
            }

            LatexCommand[] types = new LatexCommand[]
            {
                LatexCommand.Boolean,
                LatexCommand.Integer,
                LatexCommand.NaturalNumber,
                LatexCommand.PositiveInteger,
                LatexCommand.ZeroOrOne,
                LatexCommand.IntegerInterval
            };

            return types.Any(type => CommandPrefixMatch(currentValue, type));
        }

        private bool IsProgram(LinkedListNode<string>? current)
        {
            return IsProgram(current?.Value);
        }

        private bool IsProgram(string? currentValue)
        {
            if (currentValue is null)
            {
                return false;
            }

            LatexCommand[] programs = new LatexCommand[]
            {
                LatexCommand.Skip,
                LatexCommand.Abort,
                LatexCommand.Assignment
            };

            return programs.Any(program => CommandPrefixMatch(currentValue, program));
        }

        private bool CommandPrefixMatch(LinkedListNode<string>? current, LatexCommand command)
        {
            return CommandPrefixMatch(current?.Value, command);
        }

        private bool CommandPrefixMatch(string? current, LatexCommand command)
        {
            if (current is null)
            {
                return false;
            }

            string commandPrefix = CommandParts(command)[0];

            return current == commandPrefix;
        }

        private string[] CommandParts(LatexCommand command)
        {
            const char separatorChar = ';';

            return ToLatex(command).Split(separatorChar, bothSplitOptions);
        }

        private int CommandArgumentCount(LatexCommand command)
        {
            return CommandParts(command).Length - 1;
        }

        //private bool EndOfParameterisedCommand(LinkedListNode<string>? current)
        //{
        //    if (current is null)
        //    {
        //        return false;
        //    }

        //    return current.Value == "}";
        //}

        private void UpdateCurrentCommandAndArgumentCounter()
        {
            LinkedListNode<string>? current      = pointer;
            LinkedListNode<string>? commandStart = null;

            int argumentCounter = 0;
            int commandCounter  = 0;

            while (commandStart is null && current is not null)
            {
                string currentToken = current.Value;

                if (currentToken == "}")
                {
                    ++commandCounter;
                }
                else if (currentToken == "}{" && commandCounter == 0) 
                {
                    ++argumentCounter;
                }
                else if (StartOfParameterisedCommand(current))
                {
                    if (commandCounter > 0)
                    {
                        --commandCounter;
                    }
                    else
                    {
                        commandStart = current;
                    }
                }

                current = current.Previous;
            }

            if (commandStart is not null)
            {
                currentCommand = FindCommand(commandStart.Value);

                currentArgumentIndex = argumentCounter;
            }
            else
            {
                currentCommand = null;

                currentArgumentIndex = -1;
            }
        }

        private LatexCommand? FindCommand(string commandStart)
        {
            LatexCommand[] commands = Enum.GetValues<LatexCommand>();

            LatexCommand? result = null;

            commandStart = commandStart.Replace("\\", @"\\");

            for (int ind = commands.Length - 1; result is null && ind >= 0; --ind)
            {
                if (Regex.IsMatch(ToLatex(commands[ind]), commandStart))
                {
                    result = commands[ind];
                }
            }

            return result;
        }

        private bool AddEnabled(LatexCommand command)
        {
            switch (command)
            {
                case LatexCommand.Boolean         or
                     LatexCommand.Integer         or
                     LatexCommand.NaturalNumber   or
                     LatexCommand.PositiveInteger or
                     LatexCommand.ZeroOrOne       or
                     LatexCommand.IntegerInterval:

                    return (currentCommand == LatexCommand.VariableDeclaration && currentArgumentIndex == secondArgument) ||
                           (currentCommand == LatexCommand.ArrayVariableDeclaration && currentArgumentIndex == firstArgument) ||
                           (currentCommand == LatexCommand.UniversallyQuantifiedFormula && currentArgumentIndex == secondArgument) ||
                           (currentCommand == LatexCommand.ExistentiallyQuantifiedFormula && currentArgumentIndex == secondArgument);

                case LatexCommand.VariableDeclaration:

                    return currentCommand == LatexCommand.SymbolDeclaration && currentArgumentIndex == secondArgument;

                case LatexCommand.ArrayVariableDeclaration:

                    return currentCommand == LatexCommand.VariableDeclaration && currentArgumentIndex == secondArgument;

                case LatexCommand.Skip  or
                     LatexCommand.Abort or
                     LatexCommand.Assignment:

                    return currentCommand == LatexCommand.WeakestPrecondition && currentArgumentIndex == firstArgument;

                case LatexCommand.Separator:

                    return currentCommand == LatexCommand.Assignment;

                default:
                    return currentCommand is not null;
            }
        }

        private bool AddDisabled(LatexCommand command)
        {
            switch (command)
            {
                case LatexCommand.Boolean         or
                     LatexCommand.Integer         or
                     LatexCommand.NaturalNumber   or
                     LatexCommand.PositiveInteger or
                     LatexCommand.ZeroOrOne       or
                     LatexCommand.IntegerInterval:

                    return IsType(pointer) || IsType(pointer?.Next) || IsParameterEnd(pointer) ||
                           CommandPrefixMatch(pointer?.Next, LatexCommand.ArrayVariableDeclaration);

                case LatexCommand.ArrayVariableDeclaration:

                    return IsParameterEnd(pointer) || IsCommand(pointer) || IsCommand(pointer?.Next);

                case LatexCommand.Skip  or
                     LatexCommand.Abort or
                     LatexCommand.Assignment:

                    return IsProgram(pointer) || IsProgram(pointer?.Next) || IsParameterEnd(pointer);

                default:

                    if (currentCommand is LatexCommand.IntegerInterval or LatexCommand.Summation)
                    {
                        return !(command is LatexCommand.LeftParentheses  or 
                                            LatexCommand.RightParentheses or
                                            LatexCommand.ChiFunction      or
                                            LatexCommand.Addition         or
                                            LatexCommand.Subtraction      or
                                            LatexCommand.Multiplication)  || 
                               AddNewIdentifierEnabled() && currentArgumentIndex != fourthArgument;
                    }
                    else if (currentCommand is LatexCommand.Assignment && currentArgumentIndex == firstArgument)
                    {
                        return command != LatexCommand.Separator && command != LatexCommand.ArrayVariable;
                    }

                    return (currentCommand == LatexCommand.SymbolDeclaration && currentArgumentIndex == firstArgument) ||
                           (currentCommand == LatexCommand.ExistentiallyQuantifiedFormula && (currentArgumentIndex == firstArgument || currentArgumentIndex == secondArgument)) ||
                           (currentCommand == LatexCommand.UniversallyQuantifiedFormula && (currentArgumentIndex == firstArgument || currentArgumentIndex == secondArgument)) ||
                           (currentCommand == LatexCommand.WeakestPrecondition && currentArgumentIndex == firstArgument);

            }
        }

        private bool AddNewIdentifierEnabled()
        {
            return (currentCommand == LatexCommand.Summation && (currentArgumentIndex == firstArgument || currentArgumentIndex == fourthArgument)) ||
                   (currentCommand == LatexCommand.SymbolDeclaration && currentArgumentIndex == firstArgument) ||
                   (currentCommand == LatexCommand.ExistentiallyQuantifiedFormula && (currentArgumentIndex == firstArgument || currentArgumentIndex == thirdArgument)) ||
                   (currentCommand == LatexCommand.UniversallyQuantifiedFormula && (currentArgumentIndex == firstArgument || currentArgumentIndex == thirdArgument));
        }

        private bool AddIdentifierEnabled()
        {
            return currentCommand is not null;
        }

        private bool AddIdentifierDisabled()
        {
            bool result = false;

            switch (currentCommand)
            {
                case LatexCommand.IntegerInterval:

                    result = !FormulaEdition;
                    break;

                case LatexCommand.ArrayVariableDeclaration or
                     LatexCommand.WeakestPrecondition      or
                     LatexCommand.Summation:

                    result = currentArgumentIndex == firstArgument;
                    break;

                case LatexCommand.VariableDeclaration:

                    result = currentArgumentIndex == secondArgument;
                    break;

                case LatexCommand.UniversallyQuantifiedFormula or
                     LatexCommand.ExistentiallyQuantifiedFormula:

                    result = currentArgumentIndex == firstArgument ||
                             currentArgumentIndex == secondArgument;
                    break;

                case LatexCommand.SymbolDeclaration:

                    result = (currentArgumentIndex == secondArgument && !FormulaEdition) ||
                             (currentArgumentIndex == firstArgument  &&  FormulaEdition);
                    break;
            }

            result |= IsIdentifier(pointer) || IsIdentifier(pointer?.Next) ||
                      IsConstant(pointer)   || IsConstant(pointer?.Next);

            return result;
        }

        private bool AddConstantEnabled()
        {
            return currentCommand is not null;
        }

        private bool AddConstantDisabled()
        {
            bool result = false;

            switch (currentCommand)
            {
                case LatexCommand.Assignment or
                     LatexCommand.Summation  or
                     LatexCommand.ArrayVariable or
                     LatexCommand.WeakestPrecondition or
                     LatexCommand.ArrayVariableDeclaration:

                    result = currentArgumentIndex == firstArgument;
                    break;

                case LatexCommand.UniversallyQuantifiedFormula or
                     LatexCommand.ExistentiallyQuantifiedFormula:

                    result = currentArgumentIndex == firstArgument ||
                             currentArgumentIndex == secondArgument;
                    break;

                case LatexCommand.SymbolDeclaration:

                    result = currentArgumentIndex == firstArgument  ||
                             currentArgumentIndex == secondArgument && !FormulaEdition;
                    break;

                case LatexCommand.VariableDeclaration:

                    result = true;
                    break;
            }

            result |= IsIdentifier(pointer) || IsIdentifier(pointer?.Next) ||
                      IsConstant(pointer)   || IsConstant(pointer?.Next);

            return result;
        }

        #endregion
    }
}
