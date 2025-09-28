using Evaluator.Core;

namespace Evaluator.UI.Windows
{
    public partial class Form1 : Form
    {
        private TextBox inputBox;
        private Button evalButton;
        private Label resultLabel;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Evaluator - Calculadora";
            this.Width = 400;
            this.Height = 200;

            inputBox = new TextBox
            {
                Left = 20,
                Top = 20,
                Width = 250
            };

            evalButton = new Button
            {
                Text = "Evaluar",
                Left = 280,
                Top = 18,
                Width = 80
            };
            evalButton.Click += EvalButton_Click;

            resultLabel = new Label
            {
                Left = 20,
                Top = 60,
                Width = 340,
                Height = 30,
                Text = "Resultado: "
            };

            this.Controls.Add(inputBox);
            this.Controls.Add(evalButton);
            this.Controls.Add(resultLabel);
        }

        private void EvalButton_Click(object? sender, EventArgs e)
        {
            try
            {
                string expression = inputBox.Text;
                double result = ExpressionEvaluator.Evaluate(expression);
                resultLabel.Text = $"Resultado: {result}";
            }
            catch (Exception ex)
            {
                resultLabel.Text = $"Error: {ex.Message}";
            }
        }
    }
}
