using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_basic_dotnet_forms
{
    public partial class MainForm : Form
    {
        private List<Person> people = new List<Person>();
        private Timer progressTimer;
        
        public MainForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Initialize ComboBox
            cmbGender.Items.AddRange(new string[] { "Male", "Female", "Other" });
            cmbGender.SelectedIndex = 0;

            // Initialize DataGridView
            InitializeDataGridView();

            // Initialize Timer for Progress Bar
            progressTimer = new Timer();
            progressTimer.Interval = 100;
            progressTimer.Tick += ProgressTimer_Tick;

            // Set default date
            dtpBirthDate.Value = DateTime.Now.AddYears(-20);
        }

        private void InitializeDataGridView()
        {
            dgvData.AutoGenerateColumns = false;
            
            dgvData.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                DataPropertyName = "Name", 
                HeaderText = "Name", 
                Width = 150 
            });
            
            dgvData.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                DataPropertyName = "Email", 
                HeaderText = "Email", 
                Width = 180 
            });
            
            dgvData.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                DataPropertyName = "Gender", 
                HeaderText = "Gender", 
                Width = 80 
            });
            
            dgvData.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                DataPropertyName = "Age", 
                HeaderText = "Age", 
                Width = 50 
            });
            
            dgvData.Columns.Add(new DataGridViewCheckBoxColumn 
            { 
                DataPropertyName = "IsSubscribed", 
                HeaderText = "Subscribed", 
                Width = 80 
            });
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validation
            if (!ValidateInput())
                return;

            // Create new person
            var person = new Person
            {
                Name = txtName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Gender = cmbGender.SelectedItem.ToString(),
                BirthDate = dtpBirthDate.Value,
                IsSubscribed = chkSubscribe.Checked,
                Status = rbStudent.Checked ? "Student" : "Professional"
            };

            // Add to list
            people.Add(person);
            
            // Update ListBox
            lstItems.Items.Add($"{person.Name} - {person.Email}");
            
            // Update DataGridView
            RefreshDataGridView();
            
            // Clear form
            ClearForm();
            
            // Update progress bar
            UpdateProgressBar();
            
            // Update status
            toolStripStatusLabel.Text = $"Added: {person.Name} | Total: {people.Count}";
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an item to remove.", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int index = lstItems.SelectedIndex;
            
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to remove '{people[index].Name}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                people.RemoveAt(index);
                lstItems.Items.RemoveAt(index);
                RefreshDataGridView();
                UpdateProgressBar();
                toolStripStatusLabel.Text = $"Item removed | Total: {people.Count}";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (people.Count == 0)
            {
                MessageBox.Show("No data to clear.", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to clear all data?",
                "Confirm Clear",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                people.Clear();
                lstItems.Items.Clear();
                RefreshDataGridView();
                progressBar.Value = 0;
                toolStripStatusLabel.Text = "All data cleared";
            }
        }

        private void btnShowDialog_Click(object sender, EventArgs e)
        {
            using (var dialog = new CustomDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show($"You entered: {dialog.UserInput}", "Dialog Result", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtEmail.Clear();
            cmbGender.SelectedIndex = 0;
            dtpBirthDate.Value = DateTime.Now.AddYears(-20);
            chkSubscribe.Checked = false;
            rbStudent.Checked = true;
            txtName.Focus();
        }

        private void RefreshDataGridView()
        {
            dgvData.DataSource = null;
            dgvData.DataSource = people.ToList();
        }

        private void UpdateProgressBar()
        {
            progressBar.Maximum = 100;
            progressBar.Value = Math.Min(people.Count * 10, 100);
            
            if (!progressTimer.Enabled && progressBar.Value > 0)
            {
                progressTimer.Start();
            }
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            // Animate progress bar
            if (progressBar.Value < progressBar.Maximum)
            {
                progressBar.Value += 1;
            }
            else
            {
                progressTimer.Stop();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearForm();
            toolStripStatusLabel.Text = "New form ready";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }

    // Person Model Class
    public class Person
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsSubscribed { get; set; }
        public string Status { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
