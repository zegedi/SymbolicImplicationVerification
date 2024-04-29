using SymbolicImplicationVerification.Converts;
using SymbolicImplicationVerification.Model;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;

namespace SimImplyGUI
{
    public partial class SimImplyForm : Form
    {
        #region Fields

        //private LatexEditor stateSpaceEditor;

        //private LinkedList<LatexEditor> formulaEditors;

        //private LinkedList<LatexEditor> implyEditors;

        //private LinkedListNode<LatexEditor>? currentEditorNode;

        //private LatexEditor currentEditor;

        private SimImplyModel model;

        private Action? modificationAction;

        #endregion

        public SimImplyForm()
        {
            InitializeComponent();

            //stateSpaceEditor = new LatexEditor();

            //formulaEditors = new LinkedList<LatexEditor>();
            //implyEditors   = new LinkedList<LatexEditor>();

            model = new SimImplyModel();

            StateSpaceView();
            SetupMenus();

            //StateSpaceView();
            //FormulaView();
        }

        public void StateSpaceView()
        {
            const int firstLayoutRowCount = 3;
            const int firstLayoutColumnCount = 4;
            const int secondLayoutRowCount = 4;
            const int secondLayoutColumnCount = 3;

            model.EditorMode = CurrentEditorMode.StateSpaceEditor;

            // Remove all previous controls.
            tableLayoutPanel.Controls.Clear();

            // Set the number of rows and columns.
            tableLayoutPanel.RowCount    = firstLayoutRowCount;
            tableLayoutPanel.ColumnCount = firstLayoutColumnCount;

            TableLayoutPanel secondLayout = new TableLayoutPanel();

            secondLayout.RowCount    = secondLayoutRowCount;
            secondLayout.ColumnCount = secondLayoutColumnCount;
            secondLayout.Dock        = DockStyle.Fill;

            TextBox latexFormulaTextBox = new TextBox();
            TextBox identifierTextBox = new TextBox();

            latexFormulaTextBox.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Az eddig megszerkesztett állapottér.");
            identifierTextBox.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Egy változó azonosítójának a megadása.");
            latexFormulaTextBox.MouseLeave += HideStatusLabels;
            identifierTextBox.MouseLeave += HideStatusLabels;

            NumericUpDown numericUpDown = new NumericUpDown();

            Button addVariableButton = new Button();
            Button moveLeftButton = new Button();
            Button moveRightButton = new Button();
            Button backSpaceButton = new Button();
            Button addSymbolButton = new Button();
            Button integerButton = new Button();
            Button zeroOrOneButton = new Button();
            Button booleanButton = new Button();
            Button naturalNumberButton = new Button();
            Button positiveIntegerButton = new Button();
            Button integerIntervalButton = new Button();
            Button arrayVariableButton = new Button();

            Text = "Implikációs állítások - Állapottér szerkesztése";

            identifierTextBox.PlaceholderText = "Változónév megadása...";

            addVariableButton.Text = "Új változó deklarálása";
            moveLeftButton.Text = "Balra lépés";
            moveRightButton.Text = "Jobbra lépés";
            backSpaceButton.Text = "Törlés visszafelé";
            addSymbolButton.Text = "Hozzáadás";
            integerButton.Text = "\u2124";
            zeroOrOneButton.Text = "\u007B0, 1\u007D";
            booleanButton.Text = "\U0001D543";
            naturalNumberButton.Text = "\u2115";
            positiveIntegerButton.Text = "\u2115\u207A";
            integerIntervalButton.Text = "[m..n]";
            arrayVariableButton.Text = "\U0001D54B\u207F";

            addVariableButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Új változónak a deklarálása.", "\\declare{[azonosító]}{[típus]}");
            moveLeftButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Visszalépés az elõzõ szimbólumra.");
            moveRightButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Továbblépés a következõ szimbólumra.");
            backSpaceButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "A jelenlegi pozíciótól balra lévõ szimbólum törlése.");
            addSymbolButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "A megadott azonosító vagy szám hozzáadása.");
            zeroOrOneButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "A {0, 1} értékek halmaza.", LatexEditor.LatexCode(LatexCommand.ZeroOrOne));
            integerButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Az egész számok halmaza.", LatexEditor.LatexCode(LatexCommand.Integer));
            naturalNumberButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "A természetes számok halmaza.", LatexEditor.LatexCode(LatexCommand.NaturalNumber));
            positiveIntegerButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "A pozitív egész számok halmaza.", LatexEditor.LatexCode(LatexCommand.PositiveInteger));
            integerIntervalButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Egész számoknak egy intervalluma.", "\\interval{[alsó határ]}{[felsõ határ]}");
            arrayVariableButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Tömb típus.", "\\arraytype{[alaptípus]}{[tömb hossza]}");

            addVariableButton.MouseLeave += HideStatusLabels;
            moveLeftButton.MouseLeave += HideStatusLabels;
            moveRightButton.MouseLeave += HideStatusLabels;
            backSpaceButton.MouseLeave += HideStatusLabels;
            addSymbolButton.MouseLeave += HideStatusLabels;
            booleanButton.MouseLeave += HideStatusLabels;
            zeroOrOneButton.MouseLeave += HideStatusLabels;
            integerButton.MouseLeave += HideStatusLabels;
            naturalNumberButton.MouseLeave += HideStatusLabels;
            positiveIntegerButton.MouseLeave += HideStatusLabels;
            integerIntervalButton.MouseLeave += HideStatusLabels;
            arrayVariableButton.MouseLeave += HideStatusLabels;

            latexFormulaTextBox.ReadOnly  = true;
            latexFormulaTextBox.Multiline = true;
            identifierTextBox.Multiline = true;

            Action controlsEnabledRefresh = delegate ()
            {
                addVariableButton.Enabled = model.Enabled(LatexCommand.VariableDeclaration);
                integerButton.Enabled = model.Enabled(LatexCommand.Integer);
                zeroOrOneButton.Enabled = model.Enabled(LatexCommand.ZeroOrOne);
                booleanButton.Enabled = model.Enabled(LatexCommand.Boolean);
                naturalNumberButton.Enabled = model.Enabled(LatexCommand.NaturalNumber);
                positiveIntegerButton.Enabled = model.Enabled(LatexCommand.PositiveInteger);
                integerIntervalButton.Enabled = model.Enabled(LatexCommand.IntegerInterval);
                arrayVariableButton.Enabled = model.Enabled(LatexCommand.ArrayVariableDeclaration);
                identifierTextBox.Enabled = model.IdentifierEnabled();
                numericUpDown.Enabled = model.ConstantEnabled();
                addSymbolButton.Enabled = numericUpDown.Enabled || identifierTextBox.Enabled;

                numericUpDown.Minimum = model.MinimumValue is int value ?
                                        Convert.ToDecimal(value) : decimal.MinValue;
            };

            Action nonModificationAction = delegate ()
            {
                // Set the cursor position inside the input box.
                latexFormulaTextBox.SelectionStart  = model.EditorPointerIndex();
                latexFormulaTextBox.SelectionLength = 0;

                // Focus on the input box.
                latexFormulaTextBox.Focus();

                // Refresh the controls.
                controlsEnabledRefresh();
            };

            Action modificationAction = delegate ()
            {
                // Get the string reprecentation of the edited formula.
                string latexFormula = model.EditorString(out int pointerIndex);

                // Set the text of the input box.
                latexFormulaTextBox.Text = latexFormula;

                // Set the cursor position inside the input box.
                latexFormulaTextBox.SelectionStart  = pointerIndex;
                latexFormulaTextBox.SelectionLength = 0;

                // Focus on the input box.
                latexFormulaTextBox.Focus();

                // Clear the text form the identifier textbox.
                identifierTextBox.Clear();

                // Refresh the controls.
                controlsEnabledRefresh();
            };

            this.modificationAction = null;

            Action addSymbolAction = delegate ()
            {
                if (identifierTextBox.Text.Length > 0)
                {
                    model.Add(identifierTextBox.Text);
                }
                else
                {
                    model.Add(Convert.ToInt32(numericUpDown.Value));
                }

                model.MoveRight();

                // Do the modification action.
                modificationAction();

                // Refresh the controls.
                controlsEnabledRefresh();
            };

            //// Set the unremovable command
            //stateSpaceEditor.Unremoveable = LatexCommand.SymbolDeclaration;

            //stateSpaceEditor.FormulaEdition = false;

            //// Add the state space declaration.
            //stateSpaceEditor.Add(LatexCommand.SymbolDeclaration);
            //stateSpaceEditor.Add("A");
            //stateSpaceEditor.MoveRight();

            // Show the result.
            modificationAction();

            latexFormulaTextBox.MouseDown += (sender, e) => RunAction_Click(sender, e, nonModificationAction);

            moveLeftButton.Click += (sender, e) => MoveLeft_Click(sender, e, nonModificationAction);
            moveRightButton.Click += (sender, e) => MoveRight_Click(sender, e, nonModificationAction);
            addVariableButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.VariableDeclaration, modificationAction);
            backSpaceButton.Click += (sender, e) => Backspace_Click(sender, e, modificationAction);
            addSymbolButton.Click += (sender, e) => RunAction_Click(sender, e, addSymbolAction);
            integerButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Integer, modificationAction);
            zeroOrOneButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.ZeroOrOne, modificationAction);
            booleanButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Boolean, modificationAction);
            naturalNumberButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.NaturalNumber, modificationAction);
            positiveIntegerButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.PositiveInteger, modificationAction);
            integerIntervalButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.IntegerInterval, modificationAction);
            arrayVariableButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.ArrayVariableDeclaration, modificationAction);

            tableLayoutPanel.Controls.Add(latexFormulaTextBox, 0, 0);
            tableLayoutPanel.Controls.Add(addVariableButton, 0, 1);
            tableLayoutPanel.Controls.Add(moveLeftButton, 1, 1);
            tableLayoutPanel.Controls.Add(moveRightButton, 2, 1);
            tableLayoutPanel.Controls.Add(backSpaceButton, 3, 1);
            tableLayoutPanel.Controls.Add(secondLayout, 0, 2);

            secondLayout.Controls.Add(identifierTextBox, 0, 0);
            secondLayout.Controls.Add(numericUpDown, 1, 0);
            secondLayout.Controls.Add(addSymbolButton, 2, 0);
            secondLayout.Controls.Add(integerButton, 0, 1);
            secondLayout.Controls.Add(naturalNumberButton, 1, 1);
            secondLayout.Controls.Add(positiveIntegerButton, 2, 1);
            secondLayout.Controls.Add(zeroOrOneButton, 0, 2);
            secondLayout.Controls.Add(booleanButton, 1, 2);
            secondLayout.Controls.Add(integerIntervalButton, 2, 2);
            secondLayout.Controls.Add(arrayVariableButton, 0, 3);

            foreach (Control control in tableLayoutPanel.Controls)
            {
                control.Dock = DockStyle.Fill;
            }

            foreach (Control control in secondLayout.Controls)
            {
                control.Dock = DockStyle.Fill;

                if (control is Button button)
                {
                    button.Font = new Font("Segoe UI", 18);
                }
            }

            tableLayoutPanel.SetColumnSpan(latexFormulaTextBox, firstLayoutColumnCount);
            tableLayoutPanel.SetColumnSpan(secondLayout, firstLayoutColumnCount);
            secondLayout.SetColumnSpan(arrayVariableButton, secondLayoutColumnCount);

            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnStyles.Clear();

            const float oneSixth = (float)1.0 / 6;
            const float oneThrid = (float)1.0 / 3;
            const float twoThrid = (float)2.0 / 3;
            const float oneFourth = (float)1.0 / 4;

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, oneSixth));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, oneSixth));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, twoThrid));

            for (int j = 0; j < firstLayoutColumnCount; ++j)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, oneFourth));
            }

            for (int i = 0; i < secondLayoutRowCount; ++i)
            {
                secondLayout.RowStyles.Add(new RowStyle(SizeType.Percent, oneFourth));
            }

            for (int j = 0; j < secondLayoutColumnCount; ++j)
            {
                secondLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, oneThrid));
            }
        }

        private void FormulaView()
        {
            const int firstLayoutRowCount = 4;
            const int firstLayoutColumnCount = 4;
            const int secondLayoutRowCount = 4;
            const int secondLayoutColumnCount = 7;
            const int thirdLayoutRowCount = 11;
            const int thirdLayoutColumnCount = 1;

            // Change to formula editior mode.
            model.EditorMode = CurrentEditorMode.FormulaEditors;

            // Remove all previous controls.
            tableLayoutPanel.Controls.Clear();

            // Set the number of rows and columns.
            tableLayoutPanel.RowCount    = firstLayoutRowCount;
            tableLayoutPanel.ColumnCount = firstLayoutColumnCount;

            // Create the second layout.
            TableLayoutPanel secondLayout = new TableLayoutPanel();

            secondLayout.RowCount    = secondLayoutRowCount;
            secondLayout.ColumnCount = secondLayoutColumnCount;

            // The third layout.
            TableLayoutPanel thirdLayout = new TableLayoutPanel();

            thirdLayout.RowCount    = thirdLayoutRowCount;
            thirdLayout.ColumnCount = thirdLayoutColumnCount;

            // Create the first layout elements.
            TextBox latexFormulaTextBox = new TextBox();

            Button newFormulaButton = new Button();
            Button prevFormulaButton = new Button();
            Button nextFormulaButton = new Button();
            Button deleteFormulaButton = new Button();
            Button changeLayoutButton = new Button();
            Button moveRightButton = new Button();
            Button moveLeftButton = new Button();
            Button backSpaceButton = new Button();

            // Create the basic layout elements.

            Button leftParentheses_ChiFunction_Button = new Button();
            Button rightParentheses_BetaFunction_Button = new Button();
            Button trueConstant_Existentially_Button = new Button();
            Button falseConstant_Universally_Button = new Button();
            Button equal_TrueFormula_Button = new Button();
            Button notEqual_FalseFormula_Button = new Button();
            Button lessThan_Boolean_Button = new Button();
            Button lessThanOrEqual_Integer_Button = new Button();
            Button greaterThan_ZeroOrOne_Button = new Button();
            Button greaterThanOrEqual_NaturalNumber_Button = new Button();
            Button divisor_PositiveInteger_Button = new Button();
            Button notDivisor_IntegerInterval_Button = new Button();
            Button conjunction_WeakestPrecond_Button = new Button();
            Button disjunction_AssignmentProgram_Button = new Button();
            Button negation_SkipProgram_Button = new Button();
            Button implication_AbortProgram_Button = new Button();
            Button summation_Separator_Button = new Button();

            Button additionButton = new Button();
            Button subtractionButton = new Button();
            Button multiplicationButton = new Button();

            // Create the thirs layout elements

            TextBox identifierTextBox = new TextBox();
            ComboBox variableListBox = new ComboBox();
            ComboBox formulaListBox = new ComboBox();
            ComboBox arrayVariableListBox = new ComboBox();

            NumericUpDown numericUpDown = new NumericUpDown();
            Button addSymbolButton = new Button();

            Label identifierLabel = new Label();
            Label constantLabel = new Label();
            Label variableLabel = new Label();
            Label arrayVarLabel = new Label();
            Label formulaLabel = new Label();

            // Set the texts on the form.

            latexFormulaTextBox.Multiline = true;
            latexFormulaTextBox.ReadOnly  = true;

            newFormulaButton.Text = "Új formula hozzáadása";
            prevFormulaButton.Text = "Elõzõ formula szerkesztése";
            nextFormulaButton.Text = "Következõ formula szerkesztése";
            deleteFormulaButton.Text = "Formula törlése";
            moveRightButton.Text = "Balra lépés";
            moveLeftButton.Text = "Jobbra lépés";
            backSpaceButton.Text = "Visszafelé törlés";

            identifierLabel.Text = "Új változónév megadása";
            constantLabel.Text = "Szám megadása";
            variableLabel.Text = "Meglévõ változó megadása";
            arrayVarLabel.Text = "Indexelt tömbváltozó megadása";
            formulaLabel.Text = "Meglévõ formula megadása";
            addSymbolButton.Text = "Hozzáadás";

            additionButton.Text = "\u002B";
            subtractionButton.Text = "\u2212";
            multiplicationButton.Text ="\u00D7";

            // Add the elements of the layouts.

            tableLayoutPanel.Controls.Add(latexFormulaTextBox, 0, 0);
            tableLayoutPanel.Controls.Add(newFormulaButton, 0, 1);
            tableLayoutPanel.Controls.Add(prevFormulaButton, 1, 1);
            tableLayoutPanel.Controls.Add(nextFormulaButton, 2, 1);
            tableLayoutPanel.Controls.Add(deleteFormulaButton, 3, 1);
            tableLayoutPanel.Controls.Add(changeLayoutButton, 0, 2);
            tableLayoutPanel.Controls.Add(moveRightButton, 1, 2);
            tableLayoutPanel.Controls.Add(moveLeftButton, 2, 2);
            tableLayoutPanel.Controls.Add(backSpaceButton, 3, 2);
            tableLayoutPanel.Controls.Add(secondLayout, 0, 3);

            secondLayout.Controls.Add(leftParentheses_ChiFunction_Button, 0, 0);
            secondLayout.Controls.Add(rightParentheses_BetaFunction_Button, 1, 0);
            secondLayout.Controls.Add(trueConstant_Existentially_Button, 2, 0);
            secondLayout.Controls.Add(falseConstant_Universally_Button, 3, 0);
            secondLayout.Controls.Add(equal_TrueFormula_Button, 0, 1);
            secondLayout.Controls.Add(notEqual_FalseFormula_Button, 1, 1);
            secondLayout.Controls.Add(conjunction_WeakestPrecond_Button, 2, 1);
            secondLayout.Controls.Add(disjunction_AssignmentProgram_Button, 3, 1);
            secondLayout.Controls.Add(lessThan_Boolean_Button, 0, 2);
            secondLayout.Controls.Add(greaterThan_ZeroOrOne_Button, 1, 2);
            secondLayout.Controls.Add(negation_SkipProgram_Button, 2, 2);
            secondLayout.Controls.Add(implication_AbortProgram_Button, 3, 2);
            secondLayout.Controls.Add(lessThanOrEqual_Integer_Button, 0, 3);
            secondLayout.Controls.Add(greaterThanOrEqual_NaturalNumber_Button, 1, 3);
            secondLayout.Controls.Add(divisor_PositiveInteger_Button, 2, 3);
            secondLayout.Controls.Add(notDivisor_IntegerInterval_Button, 3, 3);

            secondLayout.Controls.Add(thirdLayout, 4, 0);
            secondLayout.Controls.Add(summation_Separator_Button, 6, 0);
            secondLayout.Controls.Add(additionButton, 6, 1);
            secondLayout.Controls.Add(subtractionButton, 6, 2);
            secondLayout.Controls.Add(multiplicationButton, 6, 3);

            thirdLayout.Controls.Add(identifierLabel);
            thirdLayout.Controls.Add(identifierTextBox);
            thirdLayout.Controls.Add(constantLabel);
            thirdLayout.Controls.Add(numericUpDown);
            thirdLayout.Controls.Add(variableLabel);
            thirdLayout.Controls.Add(variableListBox);
            thirdLayout.Controls.Add(arrayVarLabel);
            thirdLayout.Controls.Add(arrayVariableListBox);
            thirdLayout.Controls.Add(formulaLabel);
            thirdLayout.Controls.Add(formulaListBox);
            thirdLayout.Controls.Add(addSymbolButton);

            // Set the styles
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnStyles.Clear();

            foreach (Control control in tableLayoutPanel.Controls)
            {
                control.Dock = DockStyle.Fill;
            }

            foreach (Control control in secondLayout.Controls)
            {
                control.Dock = DockStyle.Fill;

                if (control is Button button)
                {
                    button.Font = new Font("Segoe UI", 16);
                }
            }

            foreach (Control control in thirdLayout.Controls)
            {
                control.Dock = DockStyle.Fill;
            }

            const float oneSeventh = (float)1.0 / 7;
            const float fourSeventh = (float)4.0 / 7;
            const float oneFourth = (float)1.0 / 4;
            const float oneEleventh = (float)1.0 / 11;

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, oneSeventh));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, oneSeventh));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, oneSeventh));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, fourSeventh));

            tableLayoutPanel.SetColumnSpan(latexFormulaTextBox, 4);
            tableLayoutPanel.SetColumnSpan(secondLayout, 4);

            secondLayout.SetRowSpan(thirdLayout, 4);
            secondLayout.SetColumnSpan(thirdLayout, 2);

            //secondLayout.SetColumnSpan(formulaListBox, 2);
            //secondLayout.SetColumnSpan(addSymbolButton, 2);

            for (int j = 0; j < firstLayoutColumnCount; ++j)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, oneFourth));
            }

            for (int i = 0; i < secondLayoutRowCount; ++i)
            {
                secondLayout.RowStyles.Add(new RowStyle(SizeType.Percent, oneFourth));
            }

            for (int j = 0; j < secondLayoutColumnCount; ++j)
            {
                secondLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, oneSeventh));
            }

            for (int i = 0; i < thirdLayoutRowCount; ++i)
            {
                secondLayout.RowStyles.Add(new RowStyle(SizeType.Percent, oneEleventh));
            }

            for (int j = 0; j < thirdLayoutColumnCount; ++j)
            {
                secondLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            EventHandler leftParentheses_ChiFunction_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Nyitó zárójel szimbólum.", LatexEditor.LatexCode(LatexCommand.LeftParentheses));
            EventHandler rightParentheses_BetaFunction_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Záró zárójel szimbólum.", LatexEditor.LatexCode(LatexCommand.RightParentheses));
            EventHandler trueConstant_Existentially_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Igaz logikai állandó.", LatexEditor.LatexCode(LatexCommand.TrueConstant));
            EventHandler falseConstant_Universally_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Hamis logikai állandó.", LatexEditor.LatexCode(LatexCommand.FalseConstant));
            EventHandler equal_TrueFormula_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Egyenlõ relációs jel.", LatexEditor.LatexCode(LatexCommand.Equal));
            EventHandler notEqual_FalseFormula_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Nem egyenlõ relációs jel.", LatexEditor.LatexCode(LatexCommand.NotEqual));
            EventHandler lessThan_Boolean_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Kisebb relációs jel.", LatexEditor.LatexCode(LatexCommand.LessThan));
            EventHandler lessThanOrEqual_Integer_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Kisebb vagy egyenlõ relációs jel.", LatexEditor.LatexCode(LatexCommand.LessThanOrEqual));
            EventHandler greaterThan_ZeroOrOne_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Nagyobb relációs jel.", LatexEditor.LatexCode(LatexCommand.GreaterThan));
            EventHandler greaterThanOrEqual_NaturalNumber_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Nagyobb vagy egyenlõ relációs jel.", LatexEditor.LatexCode(LatexCommand.GreaterThanOrEqual));
            EventHandler divisor_PositiveInteger_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Osztója relációs jel.", LatexEditor.LatexCode(LatexCommand.Divisor));
            EventHandler notDivisor_IntegerInterval_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Nem osztója relációs jel.", LatexEditor.LatexCode(LatexCommand.NotDivisor));
            EventHandler conjunction_WeakestPrecond_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Konjunkció (logikai és) operátor.", LatexEditor.LatexCode(LatexCommand.Conjunction));
            EventHandler disjunction_AssignmentProgram_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Diszjunkció (logikai vagy) operátor.", LatexEditor.LatexCode(LatexCommand.Disjunction));
            EventHandler negation_SkipProgram_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Negáció (logikai tagadás) operátor.", LatexEditor.LatexCode(LatexCommand.Negation));
            EventHandler implication_AbortProgram_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Implikáció (logikai következtetés) operátor.", LatexEditor.LatexCode(LatexCommand.Implication));
            EventHandler summation_Separator_Button_MouseEnter_1 = (sender, e) => ChangeStatusLabel(sender, e, "Összegzés.", "\\summation{[index változó]}{[alsó határ]}{[felsõ határ]}{[kifejezés]}");

            EventHandler leftParentheses_ChiFunction_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Chí függvény.", "\\chifunction{[logikai értékû argumentum]}");
            EventHandler rightParentheses_BetaFunction_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Béta függvény.", "\\betafunction{[szám értékû argumentum]}");
            EventHandler trueConstant_Existentially_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Egzisztenciálisan kvantált formula.", "\\existentially{[kötött változó]}{[változó típusa]}{[formula]}");
            EventHandler falseConstant_Universally_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Univerzálisan kvantált formula.", "\\universally{[kötött változó]}{[változó típusa]}{[formula]}");
            EventHandler equal_TrueFormula_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Azonosan igaz formula.", LatexEditor.LatexCode(LatexCommand.TrueFormula));
            EventHandler notEqual_FalseFormula_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Azonosan hamis formula.", LatexEditor.LatexCode(LatexCommand.FalseFormula));
            EventHandler lessThan_Boolean_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Logikai értékek halmaza.", LatexEditor.LatexCode(LatexCommand.Boolean));
            EventHandler lessThanOrEqual_Integer_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Az egész számok halmaza.", LatexEditor.LatexCode(LatexCommand.Integer));
            EventHandler greaterThan_ZeroOrOne_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "A {0, 1} értékek halmaza.", LatexEditor.LatexCode(LatexCommand.ZeroOrOne));
            EventHandler greaterThanOrEqual_NaturalNumber_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "A természetes számok halmaza.", LatexEditor.LatexCode(LatexCommand.NaturalNumber));
            EventHandler divisor_PositiveInteger_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "A pozitív egész számok halmaza.", LatexEditor.LatexCode(LatexCommand.PositiveInteger));
            EventHandler notDivisor_IntegerInterval_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Egész számoknak egy intervalluma.", "\\interval{[alsó határ]}{[felsõ határ]}");
            EventHandler conjunction_WeakestPrecond_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Program leggyengébb elõfeltétele.", "\\weakestprec{[program]}{[formula]}");
            EventHandler disjunction_AssignmentProgram_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Értékadás program.", "\\assign{[változók listája]}{[hozzárendelt értékek listája]}");
            EventHandler negation_SkipProgram_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "SKIP program.", LatexEditor.LatexCode(LatexCommand.Skip));
            EventHandler implication_AbortProgram_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "ABORT program.", LatexEditor.LatexCode(LatexCommand.Abort));
            EventHandler summation_Separator_Button_MouseEnter_2 = (sender, e) => ChangeStatusLabel(sender, e, "Elvásztójel (vesszõ) szimultán értékadásokhoz.", LatexEditor.LatexCode(LatexCommand.Separator));


            leftParentheses_ChiFunction_Button.MouseLeave += HideStatusLabels;
            rightParentheses_BetaFunction_Button.MouseLeave += HideStatusLabels;
            trueConstant_Existentially_Button.MouseLeave += HideStatusLabels;
            falseConstant_Universally_Button.MouseLeave += HideStatusLabels;
            equal_TrueFormula_Button.MouseLeave += HideStatusLabels;
            notEqual_FalseFormula_Button.MouseLeave += HideStatusLabels;
            lessThan_Boolean_Button.MouseLeave += HideStatusLabels;
            lessThanOrEqual_Integer_Button.MouseLeave += HideStatusLabels;
            greaterThan_ZeroOrOne_Button.MouseLeave += HideStatusLabels;
            greaterThanOrEqual_NaturalNumber_Button.MouseLeave += HideStatusLabels;
            divisor_PositiveInteger_Button.MouseLeave += HideStatusLabels;
            notDivisor_IntegerInterval_Button.MouseLeave += HideStatusLabels;
            conjunction_WeakestPrecond_Button.MouseLeave += HideStatusLabels;
            disjunction_AssignmentProgram_Button.MouseLeave += HideStatusLabels;
            negation_SkipProgram_Button.MouseLeave += HideStatusLabels;
            implication_AbortProgram_Button.MouseLeave += HideStatusLabels;

            Action symbolAdditionsEnable = delegate ()
            {
                bool newIndentifierEnabled = model.NewIdentifierEnabled();
                bool IndentifierEnabled = model.IdentifierEnabled();

                identifierTextBox.Enabled = newIndentifierEnabled;
                variableListBox.Enabled = IndentifierEnabled;
                formulaListBox.Enabled = IndentifierEnabled;
                arrayVariableListBox.Enabled = IndentifierEnabled;
                numericUpDown.Enabled = model.ConstantEnabled();
            };

            Action symbolAdditionFocusChange = delegate ()
            {
                Control[] controls = new Control[]
                {
                    identifierTextBox,
                    variableListBox,
                    formulaListBox,
                    arrayVariableListBox,
                    numericUpDown
                };

                Control? focused = Array.Find(controls, control => control.Focused);

                if (focused is null)
                {
                    return;
                }

                bool disableOthers =
                    focused is NumericUpDown numeric && numeric.Value != 0 ||
                    focused is TextBox textBox  && textBox.Text  != string.Empty ||
                    focused is ComboBox comboBox && comboBox.Text  != string.Empty;

                if (disableOthers)
                {
                    foreach (Control control in controls)
                    {
                        control.Enabled = control == focused;
                    }
                }
                else
                {
                    symbolAdditionsEnable();
                }
            };

            identifierTextBox.TextChanged += (sender, e) => RunAction_Click(sender, e, symbolAdditionFocusChange);
            variableListBox.SelectedValueChanged += (sender, e) => RunAction_Click(sender, e, symbolAdditionFocusChange);
            formulaListBox.SelectedValueChanged += (sender, e) => RunAction_Click(sender, e, symbolAdditionFocusChange);
            arrayVariableListBox.SelectedValueChanged += (sender, e) => RunAction_Click(sender, e, symbolAdditionFocusChange);
            numericUpDown.ValueChanged += (sender, e) => RunAction_Click(sender, e, symbolAdditionFocusChange);

            identifierTextBox.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Formula azonosító, index változó, vagy kvantor által kötött változó megadása.");
            variableListBox.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Az állapottérben bevezetett egyik változó megadása.");
            formulaListBox.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "A korábban bevezetett formulák közül az egyiknek a megadása.");
            arrayVariableListBox.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Az állapottérben bevezetett egyik tömbváltozó megindexelése.", "\\arrayvar{[változó]}{[index]}");
            addSymbolButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "A kiválasztott változó vagy szám konstansnak a hozzáadása."); ;

            identifierTextBox.MouseLeave += HideStatusLabels;
            variableListBox.MouseLeave += HideStatusLabels;
            formulaListBox.MouseLeave += HideStatusLabels;
            arrayVariableListBox.MouseLeave += HideStatusLabels;
            addSymbolButton.MouseLeave += HideStatusLabels;

            variableListBox.DropDownStyle = ComboBoxStyle.DropDownList;
            formulaListBox.DropDownStyle = ComboBoxStyle.DropDownList;
            arrayVariableListBox.DropDownStyle = ComboBoxStyle.DropDownList;

            //summation_Separator_Button.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Összegzés.", "\\summation{[index változó]}{[alsó határ]}{[felsõ határ]}{[kifejezés]}");
            additionButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Összeadás operátor.", LatexEditor.LatexCode(LatexCommand.Addition));
            subtractionButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Kivonás operátor.", LatexEditor.LatexCode(LatexCommand.Subtraction));
            multiplicationButton.MouseEnter += (sender, e) => ChangeStatusLabel(sender, e, "Szorzás operátor.", LatexEditor.LatexCode(LatexCommand.Multiplication));

            summation_Separator_Button.MouseLeave += HideStatusLabels;
            additionButton.MouseLeave += HideStatusLabels;
            subtractionButton.MouseLeave += HideStatusLabels;
            multiplicationButton.MouseLeave += HideStatusLabels;

            const string changeToBasic = "Alapvetõ beviteli eszközök megjelenítése";
            const string changeToOther = "További beviteli eszközök megjelenítése";

            Action controlsEnabledRefresh = delegate ()
            {
                bool basicEnabled = changeLayoutButton.Text == changeToOther;

                leftParentheses_ChiFunction_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.LeftParentheses : LatexCommand.ChiFunction);
                rightParentheses_BetaFunction_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.RightParentheses : LatexCommand.BetaFunction);
                trueConstant_Existentially_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.TrueConstant : LatexCommand.ExistentiallyQuantifiedFormula);
                falseConstant_Universally_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.FalseConstant : LatexCommand.UniversallyQuantifiedFormula);
                equal_TrueFormula_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.Equal : LatexCommand.TrueFormula);
                notEqual_FalseFormula_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.NotEqual : LatexCommand.FalseFormula);
                lessThan_Boolean_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.LessThan : LatexCommand.Boolean);
                lessThanOrEqual_Integer_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.LessThanOrEqual : LatexCommand.Integer);
                greaterThan_ZeroOrOne_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.GreaterThan : LatexCommand.ZeroOrOne);
                greaterThanOrEqual_NaturalNumber_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.GreaterThanOrEqual : LatexCommand.NaturalNumber);
                divisor_PositiveInteger_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.Divisor : LatexCommand.PositiveInteger);
                notDivisor_IntegerInterval_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.NotDivisor : LatexCommand.IntegerInterval);
                conjunction_WeakestPrecond_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.Conjunction : LatexCommand.WeakestPrecondition);
                disjunction_AssignmentProgram_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.Disjunction : LatexCommand.Assignment);
                negation_SkipProgram_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.Negation : LatexCommand.Skip);
                implication_AbortProgram_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.Implication : LatexCommand.Abort);
                summation_Separator_Button.Enabled = model.Enabled(basicEnabled ? LatexCommand.Summation : LatexCommand.Separator);

                additionButton.Enabled = model.Enabled(LatexCommand.Addition);
                subtractionButton.Enabled = model.Enabled(LatexCommand.Subtraction);
                multiplicationButton.Enabled = model.Enabled(LatexCommand.Multiplication);

                bool newIndentifierEnabled = model.NewIdentifierEnabled();
                bool IndentifierEnabled = model.IdentifierEnabled();

                identifierTextBox.Enabled = newIndentifierEnabled;
                variableListBox.Enabled = IndentifierEnabled;
                formulaListBox.Enabled = IndentifierEnabled;
                arrayVariableListBox.Enabled = IndentifierEnabled;
                numericUpDown.Enabled = model.ConstantEnabled();
            };

            Action nonModificationAction = delegate ()
            {
                // Set the cursor position inside the input box.
                latexFormulaTextBox.SelectionStart  = model.EditorPointerIndex();
                latexFormulaTextBox.SelectionLength = 0;

                // Focus on the input box.
                latexFormulaTextBox.Focus();

                // Refresh the controls.
                controlsEnabledRefresh();
            };

            Action modificationAction = delegate ()
            {
                // Get the string reprecentation of the edited formula.
                string latexFormula = model.EditorString(out int pointerIndex);

                // Set the text of the input box.
                latexFormulaTextBox.Text = latexFormula;

                // Set the cursor position inside the input box.
                latexFormulaTextBox.SelectionStart  = pointerIndex;
                latexFormulaTextBox.SelectionLength = 0;

                // Focus on the input box.
                latexFormulaTextBox.Focus();

                // Clear the text form the identifier textbox.
                identifierTextBox.Clear();

                // Refresh the controls.
                controlsEnabledRefresh();
            };

            this.modificationAction = modificationAction;

            Action editorChanged = delegate ()
            {
                // Run the modification action.
                modificationAction();

                int  currentEditorIndex = model.CurrentEditorIndex;
                int? currentEditorCount = model.CurrentEditorListCount;

                if (currentEditorCount is null)
                {
                    return;
                }

                LinkedList<string> formulaIndentifiers = model.FormulaIndetifiers();

                formulaListBox.Items.Clear();

                foreach (string formula in formulaIndentifiers)
                {
                    formulaListBox.Items.Add(formula);
                }

                nextFormulaButton.Enabled = currentEditorIndex + 1 != currentEditorCount;
                prevFormulaButton.Enabled = currentEditorIndex != 0;

                // Update the editor counter.
                counterLabel.Text = $"{model.CurrentEditorIndex + 1} / {model.CurrentEditorListCount}";
            };

            Action addSymbolAction = delegate ()
            {
                if (identifierTextBox.Text.Length > 0)
                {
                    model.Add(identifierTextBox.Text);
                }
                else if (variableListBox.SelectedItem is string variableName)
                {
                    model.Add(variableName);

                    variableListBox.SelectedItem = null;
                }
                else if (formulaListBox.SelectedItem is string formulaName)
                {
                    model.Add(formulaName);

                    formulaListBox.SelectedItem = null;
                }
                else if (arrayVariableListBox.SelectedItem is string arrayVariableName)
                {
                    model.Add(LatexCommand.ArrayVariable);
                    model.Add(arrayVariableName);
                    model.MoveRight();

                    arrayVariableListBox.SelectedItem = null;
                }
                else
                {
                    model.Add(Convert.ToInt32(numericUpDown.Value));

                    numericUpDown.Value = 0;
                }

                // Do the modification action.
                modificationAction();

                // Refresh the controls.
                controlsEnabledRefresh();
            };

            model.StateSpaceVariables(
                out LinkedList<string> allVariables, out LinkedList<string> arrayVariables);

            foreach (string variable in allVariables)
            {
                variableListBox.Items.Add(variable);
            }

            foreach (string arrayVariable in arrayVariables)
            {
                arrayVariableListBox.Items.Add(arrayVariable);
            }

            EventHandler leftParentheses_ChiFunction_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.LeftParentheses, modificationAction);
            EventHandler leftParentheses_ChiFunction_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.ChiFunction, modificationAction);
            EventHandler rightParentheses_BetaFunction_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.RightParentheses, modificationAction);
            EventHandler rightParentheses_BetaFunction_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.BetaFunction, modificationAction);
            EventHandler trueConstant_Existentially_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.TrueConstant, modificationAction);
            EventHandler trueConstant_Existentially_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.ExistentiallyQuantifiedFormula, modificationAction);
            EventHandler falseConstant_Universally_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.FalseConstant, modificationAction);
            EventHandler falseConstant_Universally_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.UniversallyQuantifiedFormula, modificationAction);
            EventHandler equal_TrueFormula_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Equal, modificationAction);
            EventHandler equal_TrueFormula_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.TrueFormula, modificationAction);
            EventHandler notEqual_FalseFormula_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.NotEqual, modificationAction);
            EventHandler notEqual_FalseFormula_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.FalseFormula, modificationAction);
            EventHandler lessThan_Boolean_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.LessThan, modificationAction);
            EventHandler lessThan_Boolean_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Boolean, modificationAction);
            EventHandler lessThanOrEqual_Integer_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.LessThanOrEqual, modificationAction);
            EventHandler lessThanOrEqual_Integer_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Integer, modificationAction);
            EventHandler greaterThan_ZeroOrOne_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.GreaterThan, modificationAction);
            EventHandler greaterThan_ZeroOrOne_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.ZeroOrOne, modificationAction);
            EventHandler greaterThanOrEqual_NaturalNumber_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.GreaterThanOrEqual, modificationAction);
            EventHandler greaterThanOrEqual_NaturalNumber_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.NaturalNumber, modificationAction);
            EventHandler divisor_PositiveInteger_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Divisor, modificationAction);
            EventHandler divisor_PositiveInteger_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.PositiveInteger, modificationAction);
            EventHandler notDivisor_IntegerInterval_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.NotDivisor, modificationAction);
            EventHandler notDivisor_IntegerInterval_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.IntegerInterval, modificationAction);
            EventHandler conjunction_WeakestPrecond_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Conjunction, modificationAction);
            EventHandler conjunction_WeakestPrecond_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.WeakestPrecondition, modificationAction);
            EventHandler disjunction_AssignmentProgram_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Disjunction, modificationAction);
            EventHandler disjunction_AssignmentProgram_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Assignment, modificationAction);
            EventHandler negation_SkipProgram_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Negation, modificationAction);
            EventHandler negation_SkipProgram_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Skip, modificationAction);
            EventHandler implication_AbortProgram_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Implication, modificationAction);
            EventHandler implication_AbortProgram_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Abort, modificationAction);
            EventHandler summation_Separator_Button_1 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Summation, modificationAction);
            EventHandler summation_Separator_Button_2 = (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Separator, modificationAction);

            Action changeBetweenControls = delegate ()
            {
                bool showOthers = changeLayoutButton.Text == changeToOther;

                changeLayoutButton.Text = showOthers ? changeToBasic : changeToOther;

                leftParentheses_ChiFunction_Button.Text = showOthers ? "\U0001D712" : "(";
                rightParentheses_BetaFunction_Button.Text = showOthers ? "\U0001D6FD" : ")";
                trueConstant_Existentially_Button.Text = showOthers ? "\u2203" : "igaz";
                falseConstant_Universally_Button.Text = showOthers ? "\u2200" : "hamis";
                equal_TrueFormula_Button.Text = showOthers ? "IGAZ" : "=";
                notEqual_FalseFormula_Button.Text = showOthers ? "HAMIS" : "\u2260";
                lessThan_Boolean_Button.Text = showOthers ? "\U0001D543" : "<";
                lessThanOrEqual_Integer_Button.Text = showOthers ? "\u2124" : "\u2264";
                greaterThan_ZeroOrOne_Button.Text = showOthers ? "{0, 1}" : ">";
                greaterThanOrEqual_NaturalNumber_Button.Text = showOthers ? "\u2115" : "\u2265";
                divisor_PositiveInteger_Button.Text = showOthers ? "\u2115\u207A" : "\u2223";
                notDivisor_IntegerInterval_Button.Text = showOthers ? "[m..n]" : "\u2224";
                conjunction_WeakestPrecond_Button.Text = showOthers ? "lf(S; R)" : "\u2227";
                disjunction_AssignmentProgram_Button.Text = showOthers ? "\u2254" : "\u2228";
                negation_SkipProgram_Button.Text = showOthers ? "SKIP" : "\u00AC";
                implication_AbortProgram_Button.Text = showOthers ? "ABORT" : "\u2192";
                summation_Separator_Button.Text = showOthers ? "Elválasztás" : "\u2211";

                leftParentheses_ChiFunction_Button.Click -= showOthers ? leftParentheses_ChiFunction_Button_1 : leftParentheses_ChiFunction_Button_2;
                leftParentheses_ChiFunction_Button.Click += showOthers ? leftParentheses_ChiFunction_Button_2 : leftParentheses_ChiFunction_Button_1;
                rightParentheses_BetaFunction_Button.Click -= showOthers ? rightParentheses_BetaFunction_Button_1 : rightParentheses_BetaFunction_Button_2;
                rightParentheses_BetaFunction_Button.Click += showOthers ? rightParentheses_BetaFunction_Button_2 : rightParentheses_BetaFunction_Button_1;
                trueConstant_Existentially_Button.Click -= showOthers ? trueConstant_Existentially_Button_1 : trueConstant_Existentially_Button_2;
                trueConstant_Existentially_Button.Click += showOthers ? trueConstant_Existentially_Button_2 : trueConstant_Existentially_Button_1;
                falseConstant_Universally_Button.Click -= showOthers ? falseConstant_Universally_Button_1 : falseConstant_Universally_Button_2;
                falseConstant_Universally_Button.Click += showOthers ? falseConstant_Universally_Button_2 : falseConstant_Universally_Button_1;
                equal_TrueFormula_Button.Click -= showOthers ? equal_TrueFormula_Button_1 : equal_TrueFormula_Button_2;
                equal_TrueFormula_Button.Click += showOthers ? equal_TrueFormula_Button_2 : equal_TrueFormula_Button_1;
                notEqual_FalseFormula_Button.Click -= showOthers ? notEqual_FalseFormula_Button_1 : notEqual_FalseFormula_Button_2;
                notEqual_FalseFormula_Button.Click += showOthers ? notEqual_FalseFormula_Button_2 : notEqual_FalseFormula_Button_1;
                lessThan_Boolean_Button.Click -= showOthers ? lessThan_Boolean_Button_1 : lessThan_Boolean_Button_2;
                lessThan_Boolean_Button.Click += showOthers ? lessThan_Boolean_Button_2 : lessThan_Boolean_Button_1;
                lessThanOrEqual_Integer_Button.Click -= showOthers ? lessThanOrEqual_Integer_Button_1 : lessThanOrEqual_Integer_Button_2;
                lessThanOrEqual_Integer_Button.Click += showOthers ? lessThanOrEqual_Integer_Button_2 : lessThanOrEqual_Integer_Button_1;
                greaterThan_ZeroOrOne_Button.Click -= showOthers ? greaterThan_ZeroOrOne_Button_1 : greaterThan_ZeroOrOne_Button_2;
                greaterThan_ZeroOrOne_Button.Click += showOthers ? greaterThan_ZeroOrOne_Button_2 : greaterThan_ZeroOrOne_Button_1;
                greaterThanOrEqual_NaturalNumber_Button.Click -= showOthers ? greaterThanOrEqual_NaturalNumber_Button_1 : greaterThanOrEqual_NaturalNumber_Button_2;
                greaterThanOrEqual_NaturalNumber_Button.Click += showOthers ? greaterThanOrEqual_NaturalNumber_Button_2 : greaterThanOrEqual_NaturalNumber_Button_1;
                divisor_PositiveInteger_Button.Click -= showOthers ? divisor_PositiveInteger_Button_1 : divisor_PositiveInteger_Button_2;
                divisor_PositiveInteger_Button.Click += showOthers ? divisor_PositiveInteger_Button_2 : divisor_PositiveInteger_Button_1;
                notDivisor_IntegerInterval_Button.Click -= showOthers ? notDivisor_IntegerInterval_Button_1 : notDivisor_IntegerInterval_Button_2;
                notDivisor_IntegerInterval_Button.Click += showOthers ? notDivisor_IntegerInterval_Button_2 : notDivisor_IntegerInterval_Button_1;
                conjunction_WeakestPrecond_Button.Click -= showOthers ? conjunction_WeakestPrecond_Button_1 : conjunction_WeakestPrecond_Button_2;
                conjunction_WeakestPrecond_Button.Click += showOthers ? conjunction_WeakestPrecond_Button_2 : conjunction_WeakestPrecond_Button_1;
                disjunction_AssignmentProgram_Button.Click -= showOthers ? disjunction_AssignmentProgram_Button_1 : disjunction_AssignmentProgram_Button_2;
                disjunction_AssignmentProgram_Button.Click += showOthers ? disjunction_AssignmentProgram_Button_2 : disjunction_AssignmentProgram_Button_1;
                negation_SkipProgram_Button.Click -= showOthers ? negation_SkipProgram_Button_1 : negation_SkipProgram_Button_2;
                negation_SkipProgram_Button.Click += showOthers ? negation_SkipProgram_Button_2 : negation_SkipProgram_Button_1;
                implication_AbortProgram_Button.Click -= showOthers ? implication_AbortProgram_Button_1 : implication_AbortProgram_Button_2;
                implication_AbortProgram_Button.Click += showOthers ? implication_AbortProgram_Button_2 : implication_AbortProgram_Button_1;
                summation_Separator_Button.Click -= showOthers ? summation_Separator_Button_1 : summation_Separator_Button_2;
                summation_Separator_Button.Click += showOthers ? summation_Separator_Button_2 : summation_Separator_Button_1;

                leftParentheses_ChiFunction_Button.MouseEnter -= showOthers ? leftParentheses_ChiFunction_Button_MouseEnter_1 : leftParentheses_ChiFunction_Button_MouseEnter_2;
                leftParentheses_ChiFunction_Button.MouseEnter += showOthers ? leftParentheses_ChiFunction_Button_MouseEnter_2 : leftParentheses_ChiFunction_Button_MouseEnter_1;
                rightParentheses_BetaFunction_Button.MouseEnter -= showOthers ? rightParentheses_BetaFunction_Button_MouseEnter_1 : rightParentheses_BetaFunction_Button_MouseEnter_2;
                rightParentheses_BetaFunction_Button.MouseEnter += showOthers ? rightParentheses_BetaFunction_Button_MouseEnter_2 : rightParentheses_BetaFunction_Button_MouseEnter_1;
                trueConstant_Existentially_Button.MouseEnter -= showOthers ? trueConstant_Existentially_Button_MouseEnter_1 : trueConstant_Existentially_Button_MouseEnter_2;
                trueConstant_Existentially_Button.MouseEnter += showOthers ? trueConstant_Existentially_Button_MouseEnter_2 : trueConstant_Existentially_Button_MouseEnter_1;
                falseConstant_Universally_Button.MouseEnter -= showOthers ? falseConstant_Universally_Button_MouseEnter_1 : falseConstant_Universally_Button_MouseEnter_2;
                falseConstant_Universally_Button.MouseEnter += showOthers ? falseConstant_Universally_Button_MouseEnter_2 : falseConstant_Universally_Button_MouseEnter_1;
                equal_TrueFormula_Button.MouseEnter -= showOthers ? equal_TrueFormula_Button_MouseEnter_1 : equal_TrueFormula_Button_MouseEnter_2;
                equal_TrueFormula_Button.MouseEnter += showOthers ? equal_TrueFormula_Button_MouseEnter_2 : equal_TrueFormula_Button_MouseEnter_1;
                notEqual_FalseFormula_Button.MouseEnter -= showOthers ? notEqual_FalseFormula_Button_MouseEnter_1 : notEqual_FalseFormula_Button_MouseEnter_2;
                notEqual_FalseFormula_Button.MouseEnter += showOthers ? notEqual_FalseFormula_Button_MouseEnter_2 : notEqual_FalseFormula_Button_MouseEnter_1;
                lessThan_Boolean_Button.MouseEnter -= showOthers ? lessThan_Boolean_Button_MouseEnter_1 : lessThan_Boolean_Button_MouseEnter_2;
                lessThan_Boolean_Button.MouseEnter += showOthers ? lessThan_Boolean_Button_MouseEnter_2 : lessThan_Boolean_Button_MouseEnter_1;
                lessThanOrEqual_Integer_Button.MouseEnter -= showOthers ? lessThanOrEqual_Integer_Button_MouseEnter_1 : lessThanOrEqual_Integer_Button_MouseEnter_2;
                lessThanOrEqual_Integer_Button.MouseEnter += showOthers ? lessThanOrEqual_Integer_Button_MouseEnter_2 : lessThanOrEqual_Integer_Button_MouseEnter_1;
                greaterThan_ZeroOrOne_Button.MouseEnter -= showOthers ? greaterThan_ZeroOrOne_Button_MouseEnter_1 : greaterThan_ZeroOrOne_Button_MouseEnter_2;
                greaterThan_ZeroOrOne_Button.MouseEnter += showOthers ? greaterThan_ZeroOrOne_Button_MouseEnter_2 : greaterThan_ZeroOrOne_Button_MouseEnter_1;
                greaterThanOrEqual_NaturalNumber_Button.MouseEnter -= showOthers ? greaterThanOrEqual_NaturalNumber_Button_MouseEnter_1 : greaterThanOrEqual_NaturalNumber_Button_MouseEnter_2;
                greaterThanOrEqual_NaturalNumber_Button.MouseEnter += showOthers ? greaterThanOrEqual_NaturalNumber_Button_MouseEnter_2 : greaterThanOrEqual_NaturalNumber_Button_MouseEnter_1;
                divisor_PositiveInteger_Button.MouseEnter -= showOthers ? divisor_PositiveInteger_Button_MouseEnter_1 : divisor_PositiveInteger_Button_MouseEnter_2;
                divisor_PositiveInteger_Button.MouseEnter += showOthers ? divisor_PositiveInteger_Button_MouseEnter_2 : divisor_PositiveInteger_Button_MouseEnter_1;
                notDivisor_IntegerInterval_Button.MouseEnter -= showOthers ? notDivisor_IntegerInterval_Button_MouseEnter_1 : notDivisor_IntegerInterval_Button_MouseEnter_2;
                notDivisor_IntegerInterval_Button.MouseEnter += showOthers ? notDivisor_IntegerInterval_Button_MouseEnter_2 : notDivisor_IntegerInterval_Button_MouseEnter_1;
                conjunction_WeakestPrecond_Button.MouseEnter -= showOthers ? conjunction_WeakestPrecond_Button_MouseEnter_1 : conjunction_WeakestPrecond_Button_MouseEnter_2;
                conjunction_WeakestPrecond_Button.MouseEnter += showOthers ? conjunction_WeakestPrecond_Button_MouseEnter_2 : conjunction_WeakestPrecond_Button_MouseEnter_1;
                disjunction_AssignmentProgram_Button.MouseEnter -= showOthers ? disjunction_AssignmentProgram_Button_MouseEnter_1 : disjunction_AssignmentProgram_Button_MouseEnter_2;
                disjunction_AssignmentProgram_Button.MouseEnter += showOthers ? disjunction_AssignmentProgram_Button_MouseEnter_2 : disjunction_AssignmentProgram_Button_MouseEnter_1;
                negation_SkipProgram_Button.MouseEnter -= showOthers ? negation_SkipProgram_Button_MouseEnter_1 : negation_SkipProgram_Button_MouseEnter_2;
                negation_SkipProgram_Button.MouseEnter += showOthers ? negation_SkipProgram_Button_MouseEnter_2 : negation_SkipProgram_Button_MouseEnter_1;
                implication_AbortProgram_Button.MouseEnter -= showOthers ? implication_AbortProgram_Button_MouseEnter_1 : implication_AbortProgram_Button_MouseEnter_2;
                implication_AbortProgram_Button.MouseEnter += showOthers ? implication_AbortProgram_Button_MouseEnter_2 : implication_AbortProgram_Button_MouseEnter_1;
                summation_Separator_Button.MouseEnter -= showOthers ? summation_Separator_Button_MouseEnter_1 : summation_Separator_Button_MouseEnter_2;
                summation_Separator_Button.MouseEnter += showOthers ? summation_Separator_Button_MouseEnter_2 : summation_Separator_Button_MouseEnter_1;

                // Refresh the controls.
                controlsEnabledRefresh();
            };

            latexFormulaTextBox.MouseDown += (sender, e) => RunAction_Click(sender, e, nonModificationAction);

            newFormulaButton.Click += (sender, e) => EditorCreateNew_Click(sender, e, editorChanged);
            prevFormulaButton.Click += (sender, e) => PreviousEditor_Click(sender, e, editorChanged);
            nextFormulaButton.Click += (sender, e) => NextEditor_Click(sender, e, editorChanged);
            deleteFormulaButton.Click += (sender, e) => EditorDelete_Click(sender, e, editorChanged);

            changeLayoutButton.Click += (sender, e) => RunAction_Click(sender, e, changeBetweenControls);
            moveRightButton.Click += (sender, e) => MoveLeft_Click(sender, e, nonModificationAction);
            moveLeftButton.Click += (sender, e) => MoveRight_Click(sender, e, nonModificationAction);
            backSpaceButton.Click += (sender, e) => Backspace_Click(sender, e, modificationAction);

            //summation_Separator_Button.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Summation, modificationAction);
            additionButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Addition, modificationAction);
            subtractionButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Subtraction, modificationAction);
            multiplicationButton.Click += (sender, e) => LatexCommand_Click(sender, e, LatexCommand.Multiplication, modificationAction);

            addSymbolButton.Click += (sender, e) => RunAction_Click(sender, e, addSymbolAction);

            // Change the current editor.
            editorChanged();

            // Change to the basic controls.
            changeBetweenControls();
        }

        private void SetupMenus()
        {
            stateSpaceMenu.Checked = model.EditorMode == CurrentEditorMode.StateSpaceEditor;
            formulaMenu.Checked = model.EditorMode == CurrentEditorMode.FormulaEditors;
            implyMenu.Checked = model.EditorMode == CurrentEditorMode.ImplyEditors;
            resultMenu.Checked = model.EditorMode == CurrentEditorMode.None;
        }



        private void ResetMenuItem_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Biztosan alaphelyzetbe szeretné állítani a programot?" + Environment.NewLine +
                "A legutóbbi mentés óta készült változtatások elvesznek, ezért a megtartásukhoz " +
                "javasolt egy újbóli mentésnek az elkészítése.",
                "Implikációs állítások belátása",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                model = new SimImplyModel();

                StateSpaceView();
                SetupMenus();
            }
        }

        private async void LoadMenuItem_Click(object? sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await model.LoadAsync(openFileDialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        "Betöltése sikertelen!" + Environment.NewLine +
                        "Hibás az elérési út, vagy a fájlformátum.",
                        "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error
                    );

                    model = new SimImplyModel();
                }
            }
        }

        private async void SaveMenuItem_Click(object? sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await model.SaveAsync(saveFileDialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        "Mentése sikertelen!" + Environment.NewLine +
                        "Hibás az elérési út, vagy a könyvtár nem írható.",
                        "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
                }
            }
        }

        private void ExitMenuItem_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Biztosan ki szeretne lépni?", "Implikációs állítások belátása",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void StateSpaceMenu_Click(object? sender, EventArgs e)
        {
            if (model.EditorMode != CurrentEditorMode.StateSpaceEditor)
            {
                StateSpaceView();
                SetupMenus();
            }
        }

        private void FormulaMenu_Click(object? sender, EventArgs e)
        {
            if (model.EditorMode != CurrentEditorMode.FormulaEditors)
            {
                Text = "Implikációs állítások - Formulák szerkesztése";

                bool changeTheView = model.EditorMode != CurrentEditorMode.ImplyEditors;

                if (changeTheView)
                {
                    FormulaView();
                }
                else
                {
                    model.EditorMode = CurrentEditorMode.FormulaEditors;

                    tableLayoutPanel.Controls[1].Text = "Új formula hozzáadása";
                    tableLayoutPanel.Controls[2].Text = "Elõzõ formula szerkesztése";
                    tableLayoutPanel.Controls[3].Text = "Következõ formula szerkesztése";
                    tableLayoutPanel.Controls[4].Text = "Formula törlése";

                    counterLabel.Text = $"1 / {model.CurrentEditorListCount}";

                    if (modificationAction is not null)
                    {
                        modificationAction();
                    }
                }

                SetupMenus();
            }
        }

        private void ImplyMenu_Click(object? sender, EventArgs e)
        {
            if (model.EditorMode != CurrentEditorMode.ImplyEditors)
            {
                Text = "Implikációs állítások - Következtetések szerkesztése";

                bool changeTheView = model.EditorMode != CurrentEditorMode.FormulaEditors;

                if (changeTheView)
                {
                    FormulaView();
                }

                model.EditorMode = CurrentEditorMode.ImplyEditors;

                tableLayoutPanel.Controls[1].Text = "Új következtetés hozzáadása";
                tableLayoutPanel.Controls[2].Text = "Elõzõ következtetés szerkesztése";
                tableLayoutPanel.Controls[3].Text = "Következõ következtetés szerkesztése";
                tableLayoutPanel.Controls[4].Text = "Következtetés törlése";

                counterLabel.Text = $"1 / {model.CurrentEditorListCount}";

                if (modificationAction is not null)
                {
                    modificationAction();
                }

                SetupMenus();
            }
        }

        private void EditorCreateNew_Click(object? sender, EventArgs e, Action action)
        {
            if (model.EditorMode is CurrentEditorMode.FormulaEditors or CurrentEditorMode.ImplyEditors)
            {
                bool added = model.FormulaOrImplyEditorCreateNew();

                if (added)
                {
                    action();
                }
            }
        }

        private void NextEditor_Click(object? sender, EventArgs e, Action action)
        {
            if (model.EditorMode is CurrentEditorMode.FormulaEditors or CurrentEditorMode.ImplyEditors)
            {
                bool moved = model.FormulaOrImplyEditorNext();

                if (moved)
                {
                    action();
                }
            }
        }

        private void PreviousEditor_Click(object? sender, EventArgs e, Action action)
        {
            if (model.EditorMode is CurrentEditorMode.FormulaEditors or CurrentEditorMode.ImplyEditors)
            {
                bool moved = model.FormulaOrImplyEditorPrevious();

                if (moved)
                {
                    action();
                }
            }
        }

        private void EditorDelete_Click(object? sender, EventArgs e, Action action)
        {
            if (model.EditorMode is CurrentEditorMode.FormulaEditors or CurrentEditorMode.ImplyEditors)
            {
                DialogResult result = MessageBox.Show(
                    "Biztosan törölni szeretné a teljes kifejezést?", "Implikációs állítások belátása",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

                bool deleted = result == DialogResult.Yes && model.FormulaOrImplyEditorRemove();

                if (deleted)
                {
                    action();
                }
            }
        }

        private void MoveLeft_Click(object? sender, EventArgs e, Action action)
        {
            model.MoveLeft();
            action();
        }

        private void MoveRight_Click(object? sender, EventArgs e, Action action)
        {
            model.MoveRight();
            action();
        }

        private void Backspace_Click(object? sender, EventArgs e, Action action)
        {
            model.Remove();
            action();
        }

        private void LatexCommand_Click(object? sender, EventArgs e, LatexCommand command, Action action)
        {
            model.Add(command);
            action();
        }

        //private void LeftParentheses_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.LeftParentheses);
        //    action();
        //}

        //private void RightParentheses_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.RightParentheses);
        //    action();
        //}

        //private void Addition_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Addition);
        //    action();
        //}

        //private void Subtraction_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Subtraction);
        //    action();
        //}

        //private void Multiplication_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Multiplication);
        //    action();
        //}

        //private void Equal_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Equal);
        //    action();
        //}

        //private void NotEqual_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.NotEqual);
        //    action();
        //}

        //private void LessThan_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.LessThan);
        //    action();
        //}

        //private void GreaterThan_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.GreaterThan);
        //    action();
        //}

        //private void LessThanOrEqual_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.LessThanOrEqual);
        //    action();
        //}

        //private void GreaterThanOrEqual_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.GreaterThanOrEqual);
        //    action();
        //}

        //private void Divisor_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Divisor);
        //    action();
        //}

        //private void NotDivisor_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.NotDivisor);
        //    action();
        //}

        //private void Conjunction_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Conjunction);
        //    action();
        //}

        //private void Disjunction_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Disjunction);
        //    action();
        //}

        //private void Implication_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Implication);
        //    action();
        //}

        //private void TrueConstant_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.TrueConstant);
        //    action();
        //}

        //private void FalseConstant_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.FalseConstant);
        //    action();
        //}

        //private void TrueFormula_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.TrueFormula);
        //    action();
        //}

        //private void FalseFormula_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.FalseFormula);
        //    action();
        //}

        //private void NotEvaluableFormula_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.NotEvaluableFormula);
        //    action();
        //}

        //private void Abort_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Abort);
        //    action();
        //}

        //private void Skip_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Skip);
        //    action();
        //}

        //private void Boolean_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Boolean);
        //    action();
        //}

        //private void Integer_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Integer);
        //    action();
        //}

        //private void NaturalNumber_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.NaturalNumber);
        //    action();
        //}

        //private void PositiveInteger_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.PositiveInteger);
        //    action();
        //}

        //private void Negation_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Negation);
        //    action();
        //}

        //private void ZeroOrOne_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.ZeroOrOne);
        //    action();
        //}

        //private void Assignment_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Assignment);
        //    action();
        //}

        //private void IntegerInterval_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.IntegerInterval);
        //    action();
        //}

        //private void VariableDeclaration_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.VariableDeclaration);
        //    action();
        //}

        //private void ArrayVariable_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.ArrayVariable);
        //    action();
        //}

        //private void ArrayVariableDeclaration_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.ArrayVariableDeclaration);
        //    action();
        //}

        //private void SymbolDeclaration_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.SymbolDeclaration);
        //    action();
        //}

        //private void ChiFunction_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.ChiFunction);
        //    action();
        //}

        //private void BetaFunction_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.BetaFunction);
        //    action();
        //}

        //private void Imply_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Imply);
        //    action();
        //}

        //private void UniversallyQuantifiedFormula_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.UniversallyQuantifiedFormula);
        //    action();
        //}

        //private void ExistentiallyQuantifiedFormula_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.ExistentiallyQuantifiedFormula);
        //    action();
        //}

        //private void Summation_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.Summation);
        //    action();
        //}

        //private void WeakestPrecondition_Click(object? sender, EventArgs e, Action action)
        //{
        //    currentEditor.Add(LatexCommand.WeakestPrecondition);
        //    action();
        //}

        private void RunAction_Click(object? sender, EventArgs e, Action action)
        {
            action();
        }

        private void ChangeStatusLabel(object? sender, EventArgs e, string description, string? latexCode = null)
        {
            descriptionLabel.Visible = true;
            descriptionTextLabel.Visible = true;
            descriptionTextLabel.Text    = description;

            bool showLatexCode = latexCode is not null;

            descriptionTextLabel.BorderSides =
                showLatexCode ? ToolStripStatusLabelBorderSides.Right : ToolStripStatusLabelBorderSides.None;

            latexLabel.Visible     = showLatexCode;
            latexCodeLabel.Visible = showLatexCode;

            if (showLatexCode)
            {
                latexCodeLabel.Text = latexCode;
            }
        }

        private void HideStatusLabels(object? sender, EventArgs e)
        {
            descriptionLabel.Visible = false;
            descriptionTextLabel.Visible = false;

            latexLabel.Visible = false;
            latexCodeLabel.Visible = false;
        }
    }
}