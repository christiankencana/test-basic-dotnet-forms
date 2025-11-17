using System;
using System.Drawing;
using System.Windows.Forms;

namespace test_basic_dotnet_forms
{
    public class CustomDialog : Form
    {
        private TextBox txtInput;
        private Button btnOK;
        private Button btnCancel;
        private Label lblPrompt;

        public string UserInput { get; private set; }

        public CustomDialog()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Custom Input Dialog";
            this.Size = new Size(350, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Label
            lblPrompt = new Label
            {
                Text = "Please enter your input:",
                Location = new Point(20, 20),
                Size = new Size(300, 20)
            };

            // TextBox
            txtInput = new TextBox
            {
                Location = new Point(20, 45),
                Size = new Size(290, 20)
            };

            // OK Button
            btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(140, 75),
                Size = new Size(80, 25)
            };
            btnOK.Click += BtnOK_Click;

            // Cancel Button
            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(230, 75),
                Size = new Size(80, 25)
            };

            // Add controls
            this.Controls.AddRange(new Control[] { lblPrompt, txtInput, btnOK, btnCancel });
            
            // Set accept and cancel buttons
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            UserInput = txtInput.Text;
        }
    }
}       