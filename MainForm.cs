using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using test_basic_dotnet_forms.Data;
using test_basic_dotnet_forms.Models;

namespace test_basic_dotnet_forms
{
    public partial class MainForm : Form
    {
        private DatabaseHelper db;
        private Person selectedPerson = null;

        public MainForm()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required", "Warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email is required", "Warning");
                return;
            }

            var person = new Person
            {
                Name = txtName.Text,
                Email = txtEmail.Text
            };

            db.Add(person);

            LoadData();
            ClearForm();
            MessageBox.Show("Data saved!", "Success");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstPeople.SelectedItem == null)
            {
                MessageBox.Show("Please select an item to edit", "Warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required", "Warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email is required", "Warning");
                return;
            }

            var person = (Person)lstPeople.SelectedItem;
            person.Name = txtName.Text;
            person.Email = txtEmail.Text;

            db.Update(person);

            LoadData();
            ClearForm();
            MessageBox.Show("Data updated!", "Success");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstPeople.SelectedItem == null)
            {
                MessageBox.Show("Please select an item", "Warning");
                return;
            }

            var result = MessageBox.Show("Delete this item?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                var person = (Person)lstPeople.SelectedItem;
                db.Delete(person.Id);
                LoadData();
                MessageBox.Show("Data deleted!", "Success");
            }
        }

        private void LoadData()
        {
            lstPeople.DataSource = null;
            lstPeople.DataSource = db.GetAll();
            lstPeople.SelectedIndexChanged += lstPeople_SelectedIndexChanged;
        }

        private void lstPeople_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPeople.SelectedItem != null)
            {
                var person = (Person)lstPeople.SelectedItem;
                txtName.Text = person.Name;
                txtEmail.Text = person.Email;
            }
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtName.Focus();
        }
    }
}
