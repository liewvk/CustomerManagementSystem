using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace CustomerManagementSystem
{
    public partial class Form1 : Form
    {
        private DataTable customerTable = new DataTable();

        private bool IsValidPhoneNumber(string phone)
        {
            foreach (char c in phone)
            {
                if (!char.IsDigit(c) && c != ' ' && c != '+' && c != '-')
                {
                    return false;
                }
            }

            return true;
        }
        private bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private bool CustomerIdExists(string customerId)
        {
            foreach (DataRow row in customerTable.Rows)
            {
                if (row["Customer ID"].ToString().Equals(customerId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        private void ClearInputFields()
        {
            txtCustomerId.Clear();
            txtCustomerName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtCity.Clear();
            cmbCustomerType.SelectedIndex = -1;

            txtCustomerId.Focus();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to exit?";

            if (customerTable.Rows.Count > 0)
            {
                message = "You have customer records that are not saved permanently.\n\nAre you sure you want to exit?";
            }

            DialogResult result = MessageBox.Show(message,
                                                  "Confirm Exit",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        private void lblCustomerType_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            if (searchText == "")
            {
                MessageBox.Show("Please enter a customer name to search.",
                                "Missing Search Text",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtSearch.Focus();
                return;
            }

            DataView view = customerTable.DefaultView;
            view.RowFilter = $"[Customer Name] LIKE '%{searchText}%'";

            if (view.Count == 0)
            {
                MessageBox.Show("No matching customer found.",
                                "Search Result",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbCustomerType.Items.Add("Regular");
            cmbCustomerType.Items.Add("VIP");
            cmbCustomerType.Items.Add("Corporate");
            cmbCustomerType.Items.Add("Student");
            cmbCustomerType.Items.Add("Others");

            cmbCustomerType.SelectedIndex = -1;

            customerTable.Columns.Add("Customer ID", typeof(string));
            customerTable.Columns.Add("Customer Name", typeof(string));
            customerTable.Columns.Add("Phone Number", typeof(string));
            customerTable.Columns.Add("Email", typeof(string));
            customerTable.Columns.Add("City", typeof(string));
            customerTable.Columns.Add("Customer Type", typeof(string));

            dgvCustomers.DataSource = customerTable;

            txtCustomerId.Focus();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string customerId = txtCustomerId.Text.Trim();
            string customerName = txtCustomerName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string city = txtCity.Text.Trim();

            if (customerId == "")
            {
                MessageBox.Show("Please enter the Customer ID.",
                                "Missing Customer ID",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtCustomerId.Focus();
                return;
            }

            if (CustomerIdExists(customerId))
            {
                MessageBox.Show("This Customer ID already exists. Please enter a different Customer ID.",
                                "Duplicate Customer ID",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtCustomerId.Focus();
                return;
            }

            if (customerName == "")
            {
                MessageBox.Show("Please enter the Customer Name.",
                                "Missing Customer Name",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtCustomerName.Focus();
                return;
            }

            if (phone == "")
            {
                MessageBox.Show("Please enter the phone number.",
                                "Missing Phone Number",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtPhone.Focus();
                return;
            }

            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Please enter a valid phone number.",
                                "Invalid Phone Number",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtPhone.Focus();
                return;
            }

            if (email == "")
            {
                MessageBox.Show("Please enter the email address.",
                                "Missing Email",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtEmail.Focus();
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.",
                                "Invalid Email",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtEmail.Focus();
                return;
            }

            if (city == "")
            {
                MessageBox.Show("Please enter the city.",
                                "Missing City",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtCity.Focus();
                return;
            }

            if (cmbCustomerType.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a customer type.",
                                "Missing Customer Type",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                cmbCustomerType.Focus();
                return;
            }

            string customerType = cmbCustomerType.SelectedItem.ToString();

            customerTable.Rows.Add(customerId, customerName, phone, email, city, customerType);

            ClearInputFields();

            MessageBox.Show("Customer added successfully.",
                            "Customer Added",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

        }

        private void btnClearInput_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];

            txtCustomerId.Text = row.Cells["Customer ID"].Value.ToString();
            txtCustomerName.Text = row.Cells["Customer Name"].Value.ToString();
            txtPhone.Text = row.Cells["Phone Number"].Value.ToString();
            txtEmail.Text = row.Cells["Email"].Value.ToString();
            txtCity.Text = row.Cells["City"].Value.ToString();
            cmbCustomerType.Text = row.Cells["Customer Type"].Value.ToString();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to update.",
                                "No Customer Selected",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                return;
            }

            string customerId = txtCustomerId.Text.Trim();
            string customerName = txtCustomerName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string city = txtCity.Text.Trim();

            if (customerId == "")
            {
                MessageBox.Show("Please enter the Customer ID.",
                                "Missing Customer ID",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtCustomerId.Focus();
                return;
            }

            if (customerName == "")
            {
                MessageBox.Show("Please enter the Customer Name.",
                                "Missing Customer Name",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtCustomerName.Focus();
                return;
            }

            if (phone == "")
            {
                MessageBox.Show("Please enter the phone number.",
                                "Missing Phone Number",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtPhone.Focus();
                return;
            }

            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Please enter a valid phone number.",
                                "Invalid Phone Number",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtPhone.Focus();
                return;
            }

            if (email == "")
            {
                MessageBox.Show("Please enter the email address.",
                                "Missing Email",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtEmail.Focus();
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.",
                                "Invalid Email",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtEmail.Focus();
                return;
            }

            if (city == "")
            {
                MessageBox.Show("Please enter the city.",
                                "Missing City",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtCity.Focus();
                return;
            }

            if (cmbCustomerType.Text == "")
            {
                MessageBox.Show("Please select a customer type.",
                                "Missing Customer Type",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                cmbCustomerType.Focus();
                return;
            }

            int rowIndex = dgvCustomers.SelectedRows[0].Index;

            customerTable.Rows[rowIndex]["Customer ID"] = customerId;
            customerTable.Rows[rowIndex]["Customer Name"] = customerName;
            customerTable.Rows[rowIndex]["Phone Number"] = phone;
            customerTable.Rows[rowIndex]["Email"] = email;
            customerTable.Rows[rowIndex]["City"] = city;
            customerTable.Rows[rowIndex]["Customer Type"] = cmbCustomerType.Text;

            ClearInputFields();

            MessageBox.Show("Customer updated successfully.",
                            "Customer Updated",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to delete.",
                                "No Customer Selected",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete the selected customer?",
                                                  "Confirm Delete",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int rowIndex = dgvCustomers.SelectedRows[0].Index;

                customerTable.Rows.RemoveAt(rowIndex);

                ClearInputFields();

                MessageBox.Show("Customer deleted successfully.",
                                "Customer Deleted",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }

        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (customerTable.Rows.Count == 0)
            {
                MessageBox.Show("There are no customer records to clear.",
                                "Empty Customer List",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to clear all customer records?",
                                                  "Confirm Clear All",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                customerTable.Rows.Clear();

                ClearInputFields();

                MessageBox.Show("All customer records have been cleared.",
                                "Customer Records Cleared",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }

        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            customerTable.DefaultView.RowFilter = "";
            txtSearch.Clear();
            txtSearch.Focus();

        }
    }
}
