using SymbolicImplicationVerification.Converts;
using SymbolicImplicationVerification.Implies;
using SymbolicImplicationVerification.Persistence;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace SymbolicImplicationVerification.Model
{
    public class SimImplyModel : IDataAccess
    {
        #region Fields

        private LatexEditor stateSpaceEditor;

        private LinkedList<LatexEditor> formulaEditors;

        private LinkedList<LatexEditor> implyEditors;

        private LinkedList<LatexEditor>? currentEditorList;

        private LinkedListNode<LatexEditor>? currentEditorNode;

        private LatexEditor currentEditor;

        private CurrentEditorMode currentEditorMode = CurrentEditorMode.None;

        private int currentEditorIndex = -1;

        private LinkedList<ImplyEvaluation> evaluations;

        #endregion

        #region Constructors

        public SimImplyModel()
        {
            stateSpaceEditor = CreateStateSpaceEditor();

            formulaEditors = new LinkedList<LatexEditor>();
            implyEditors   = new LinkedList<LatexEditor>();
            evaluations    = new LinkedList<ImplyEvaluation>();

            currentEditorNode = null;
            currentEditorList = null;

            currentEditor = stateSpaceEditor;
        }

        #endregion

        #region Constant values

        private const char separator = ';';

        #endregion

        #region Public properties

        public int? MinimumValue
        {
            get { return currentEditor.MinimumValue; }
        }

        public int? CurrentEditorListCount
        {
            get { return currentEditorList?.Count; }
        }

        public int CurrentEditorIndex
        {
            get { return currentEditorIndex; }
        }

        public CurrentEditorMode EditorMode
        {
            get { return currentEditorMode; }
            set
            {
                switch (value)
                {
                    case CurrentEditorMode.StateSpaceEditor:
                        ChangeToStateSpaceEditor();
                        break;

                    case CurrentEditorMode.FormulaEditors:
                        ChangeToFormulaEditor();
                        break;

                    case CurrentEditorMode.ImplyEditors:
                        ChangeToImplyEditor();
                        break;
                }

                currentEditorMode = value;
            }
        }

        #endregion

        #region Public methods

        public void Add(LatexCommand command)
        {
            currentEditor.Add(command);
        }

        public void Add(string identifier)
        {
            currentEditor.Add(identifier);
        }

        public void Add(int value)
        {
            currentEditor.Add(value);
        }

        public bool Enabled(LatexCommand command)
        {
            return currentEditor.Enabled(command);
        }

        public bool IdentifierEnabled()
        {
            return currentEditor.IdentifierEnabled();
        }

        public bool NewIdentifierEnabled()
        {
            return currentEditor.NewIdentifierEnabled();
        }

        public bool ConstantEnabled()
        {
            return currentEditor.ConstantEnabled();
        }

        public string EditorString(out int pointerIndex)
        {
            return currentEditor.ToString(out pointerIndex);
        }

        public int EditorPointerIndex()
        {
            return currentEditor.PointerIndex();
        }

        public bool MoveLeft()
        {
            return currentEditor.MoveLeft();
        }

        public bool MoveRight()
        {
            return currentEditor.MoveRight();
        }

        public void Remove()
        {
            currentEditor.Remove();
        }

        public bool FormulaOrImplyEditorCreateNew()
        {
            bool success = currentEditorList is not null;

            if (success)
            {
                bool createFormula = currentEditorList == formulaEditors;

                currentEditorNode = currentEditorList!.AddLast(
                    createFormula ? CreateFormulaEditor() : CreateImplyEditor()
                );

                currentEditorIndex = currentEditorList.Count - 1;

                currentEditor = currentEditorNode.Value;
            }

            return success;
        }

        public bool FormulaOrImplyEditorNext()
        {
            bool success = currentEditorNode is not null && currentEditorNode.Next is not null;

            if (success)
            {
                currentEditorNode = currentEditorNode!.Next;

                currentEditor = currentEditorNode!.Value;

                ++currentEditorIndex;
            }

            return success;
        }

        public bool FormulaOrImplyEditorPrevious()
        {
            bool success = currentEditorNode is not null && currentEditorNode.Previous is not null;

            if (success)
            {
                currentEditorNode = currentEditorNode!.Previous;

                currentEditor = currentEditorNode!.Value;

                --currentEditorIndex;
            }

            return success;
        }

        public bool FormulaOrImplyEditorRemove()
        {
            bool success = currentEditorList is not null && currentEditorNode is not null;

            if (success)
            {
                LinkedListNode<LatexEditor> editorToRemove = currentEditorNode!;

                bool currentEditorListEmpty = false;

                if (currentEditorNode!.Previous is not null)
                {
                    currentEditorNode = currentEditorNode.Previous;
                    
                    --currentEditorIndex;
                }
                else if (currentEditorNode!.Next is not null)
                {
                    currentEditorNode = currentEditorNode.Next;

                    ++currentEditorIndex;
                }
                else
                {
                    currentEditorListEmpty = true;
                }

                currentEditorList!.Remove(editorToRemove);

                if (currentEditorListEmpty)
                {
                    if (currentEditorList == formulaEditors)
                    {
                        ChangeToFormulaEditor();
                    }
                    else
                    {
                        ChangeToImplyEditor();
                    }
                }
                else
                {
                    currentEditor = currentEditorNode.Value;
                }
            }

            return success;
        }

        public void ChangeToStateSpaceEditor()
        {
            // Set the current editor.
            currentEditor = stateSpaceEditor;

            // Reset the current editor list.
            currentEditorList = null;

            // Reset the current editor node;
            currentEditorNode = null;

            // Reset the current editor index;
            currentEditorIndex = -1;
        }

        public void ChangeToFormulaEditor()
        {
            currentEditorList = formulaEditors;

            // Set the current editor node.
            if (currentEditorList.First is null)
            {
                currentEditorNode = currentEditorList.AddFirst(CreateFormulaEditor());
            }
            else
            {
                currentEditorNode = currentEditorList.First;
            }

            // Set the current editor.
            currentEditor = currentEditorNode.Value;

            // Set the current editor index;
            currentEditorIndex = 0;
        }

        public void ChangeToImplyEditor()
        {
            currentEditorList = implyEditors;

            // Set the current editor node.
            if (currentEditorList.First is null)
            {
                currentEditorNode = currentEditorList.AddFirst(CreateImplyEditor());
            }
            else
            {
                currentEditorNode = currentEditorList.First;
            }

            // Set the current editor.
            currentEditor = currentEditorNode.Value;

            // Set the current editor index;
            currentEditorIndex = 0;
        }

        public void StateSpaceVariables(out LinkedList<string> allVariables, out LinkedList<string> arrayVariables)
        {
            const string variableDeclaration
                = @"\\declare\{(?<identifier>[a-zA-Z']+)\}";

            const string arrayVariableDeclaration
                = @"\\declare\{(?<identifier>[a-zA-Z']+)\}\{\\arraytype\{.+\}\{(?<length>[a-zA-Z'0-9]+)\}\}";

            string stateSpaceString = stateSpaceEditor.ToString();

            allVariables   = new LinkedList<string>();
            arrayVariables = new LinkedList<string>();

            foreach (Match variable in Regex.Matches(stateSpaceString, variableDeclaration))
            {
                string identifier = variable.Groups["identifier"].Value;

                allVariables.AddLast(identifier);
            }

            foreach (Match arrayVariable in Regex.Matches(stateSpaceString, arrayVariableDeclaration))
            {
                string identifier = arrayVariable.Groups["identifier"].Value;
                string length     = arrayVariable.Groups["length"].Value;

                arrayVariables.AddLast(identifier);

                if (!int.TryParse(length, out int res))
                {
                    allVariables.AddLast(length);
                }
            }
        }

        public LinkedList<string> FormulaIndetifiers()
        {
            const string formulaDeclaration
                = @"\\symboldeclare\{(?<identifier>[a-zA-Z']+)\}";

            LinkedList<string> result = new LinkedList<string>();

            LinkedListNode<LatexEditor>? editor = formulaEditors.First;

            while (editor is not null && editor != currentEditorNode)
            {
                Match match = Regex.Match(editor.Value.ToString(), formulaDeclaration);

                if (match.Success)
                {
                    result.AddLast(match.Groups["identifier"].Value);
                }

                editor = editor.Next;
            }

            return result;
        }

        public LinkedList<ImplyEvaluation> Evaluate(out LinkedList<string> errors)
        {
            errors = new LinkedList<string>();

            evaluations.Clear();

            Converter converter = new Converter();

            try
            {
                converter.DeclareStateSpace(stateSpaceEditor.ToString());
            }
            catch (ConvertException exc)
            {
                errors.AddLast($"Állapottér hiba: {exc.Message}");
            }

            int index = 1;

            foreach (LatexEditor formulaEditor in formulaEditors)
            {
                try
                {
                    converter.DeclareFormula(formulaEditor.ToString());
                }
                catch (ConvertException exc)
                {
                    errors.AddLast($"Formula hiba ({index} / {formulaEditors.Count}): {exc.Message}");
                }

                ++index;
            }

            index = 1;

            foreach (LatexEditor implyEditor in implyEditors)
            {
                try
                {
                    Imply imply = converter.ConvertToImply(implyEditor.ToString());

                    evaluations.AddLast(imply.Evaluated());
                }
                catch (ConvertException exc)
                {
                    errors.AddLast($"Következtetés hiba ({index} / {implyEditors.Count}): {exc.Message}");
                }

                ++index;
            }

            return evaluations;
        }

        #endregion

        #region Load and save methods

        /// <summary>
        /// Loads the given object.
        /// </summary>
        /// <param name="path">The path of the source file.</param>
        /// <returns>The loaded object.</returns>
        public async Task LoadAsync(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string input = await reader.ReadLineAsync() ?? string.Empty;

                stateSpaceEditor = new LatexEditor(input, separator);

                stateSpaceEditor.FormulaEdition = false;
                stateSpaceEditor.Unremoveable   = LatexCommand.SymbolDeclaration;

                currentEditorList = formulaEditors;

                bool currentlyFormulaEditors = true;

                for (int editorIndex = 0; editorIndex < 2; ++editorIndex)
                {
                    currentEditorList.Clear();

                    input = await reader.ReadLineAsync() ?? string.Empty;

                    bool parseSuccess = int.TryParse(input, out int numberOfLines);

                    for (int line = 0; parseSuccess && line < numberOfLines; ++line)
                    {
                        input = await reader.ReadLineAsync() ?? string.Empty;

                        LatexEditor editor = new LatexEditor(input, separator);

                        editor.FormulaEdition = true;
                        editor.Unremoveable   =
                            currentlyFormulaEditors ? LatexCommand.SymbolDeclaration : LatexCommand.Imply;

                        currentEditorList.AddLast(editor);
                    }

                    currentEditorList = implyEditors;
                    currentlyFormulaEditors = false;
                }
            }
        }

        /// <summary>
        /// Saves the given object.
        /// </summary>
        /// <param name="path">The path of the destination file.</param>
        /// <returns></returns>
        public async Task SaveAsync(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                await writer.WriteLineAsync(stateSpaceEditor.Join(separator));

                LinkedList<LatexEditor> currentEditorList = formulaEditors;

                for (int editorIndex = 0; editorIndex < 2; ++editorIndex)
                {
                    writer.WriteLine(currentEditorList.Count);

                    foreach (LatexEditor editor in currentEditorList)
                    {
                        await writer.WriteLineAsync(editor.Join(separator));
                    }

                    currentEditorList = implyEditors;
                }
            }
        }

        #endregion

        #region Private methods

        public LatexEditor CreateStateSpaceEditor()
        {
            // Create the new state space editor.
            LatexEditor stateSpaceEditor = new LatexEditor();

            // Indicate the statespace edition.
            stateSpaceEditor.FormulaEdition = false;

            // Set the unremoveable command.
            stateSpaceEditor.Unremoveable = LatexCommand.SymbolDeclaration;

            // Add the state space declaration.
            stateSpaceEditor.Add(LatexCommand.SymbolDeclaration);
            stateSpaceEditor.Add("A");
            stateSpaceEditor.MoveRight();

            return stateSpaceEditor;
        }

        public LatexEditor CreateFormulaEditor()
        {
            // Create the new formula editor.
            LatexEditor formulaEditor = new LatexEditor();

            // Set the formula edition.
            formulaEditor.FormulaEdition = true;

            // Set the unremoveable command.
            formulaEditor.Unremoveable = LatexCommand.SymbolDeclaration;

            // Add the formula declaration.
            formulaEditor.Add(LatexCommand.SymbolDeclaration);

            return formulaEditor;
        }

        public LatexEditor CreateImplyEditor()
        {
            // Create the new formula editor.
            LatexEditor implyEditor = new LatexEditor();

            // Set the formula edition.
            implyEditor.FormulaEdition = true;

            // Set the unremoveable command.
            implyEditor.Unremoveable = LatexCommand.Imply;

            // Add the formula declaration.
            implyEditor.Add(LatexCommand.Imply);

            return implyEditor;
        }

        #endregion
    }
}
