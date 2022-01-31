using EDMIProject.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDMIProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadData()
        {
            try
            {
                EDMIDBContext db = new EDMIDBContext();
                List<Employees> data = new List<Employees>();
                data = db.Employees.ToList();
                dataGridView1.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void saveData()
        {
            try
            {
                EDMIDBContext db = new EDMIDBContext();
                Employees data = new Employees();
                if (!string.IsNullOrEmpty(txtID.Text))
                    data = db.Employees.Where(y => y.EmployeeId == Convert.ToInt32(txtID.Text)).FirstOrDefault();
                data.EmployeeName = textBox1.Text;
                data.EmployeeEmail = textBox2.Text;
                data.EmployeeAddress = textBox3.Text;

                if (string.IsNullOrEmpty(txtID.Text)) db.Employees.Add(data);
                var result = db.SaveChanges();

                if (result > 0)
                {
                    MessageBox.Show("Data has been saved in the database.", "Save");
                }
                else
                {
                    MessageBox.Show("Failed to execute the query", "error");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void updateData()
        {
            try
            {
                EDMIDBContext db = new EDMIDBContext();
                Employees data = new Employees();
                if (!string.IsNullOrEmpty(txtID.Text))
                    data = db.Employees.Where(y => y.EmployeeId == Convert.ToInt32(txtID.Text)).FirstOrDefault();
                data.EmployeeName = textBox1.Text;
                data.EmployeeEmail = textBox2.Text;
                data.EmployeeAddress = textBox3.Text;

                if (string.IsNullOrEmpty(txtID.Text)) db.Employees.Add(data);
                var result = db.SaveChanges();

                if (result > 0)
                {
                    MessageBox.Show("Data has been updated in the database.", "Update");
                }
                else
                {
                    MessageBox.Show("Failed to execute the query", "error");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void deleteData()
        {
            try
            {
                EDMIDBContext db = new EDMIDBContext();
                Employees data = new Employees();
                if (!string.IsNullOrEmpty(txtID.Text))
                {
                    data = db.Employees.Where(y => y.EmployeeId == Convert.ToInt32(txtID.Text)).FirstOrDefault();
                    db.Employees.Remove(data);
                }
                var result = db.SaveChanges();

                if (result > 0)
                {
                    MessageBox.Show("Data has been deleted in the database.", "Delete");
                }
                else
                {
                    MessageBox.Show("Failed to execute the query", "error");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private static DataTable GetDataTableFromCSVFile(string csvfilePath)
        {
            DataTable csvData = new DataTable();
            using (TextFieldParser csvReader = new TextFieldParser(csvfilePath))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = true;

                //Read columns from CSV file, remove this line if columns not exits  
                string[] colFields = csvReader.ReadFields();

                foreach (string column in colFields)
                {
                    DataColumn datecolumn = new DataColumn(column);
                    datecolumn.AllowDBNull = true;
                    csvData.Columns.Add(datecolumn);
                }

                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();
                    //Making empty value as null
                    for (int i = 0; i < fieldData.Length; i++)
                    {
                        if (fieldData[i] == "")
                        {
                            fieldData[i] = null;
                        }
                    }
                    csvData.Rows.Add(fieldData);
                }
            }
            return csvData;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveData();
            loadData();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteData();
            loadData();

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            txtID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtID.Text = string.Empty;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "csv",
                Filter = "csv files (*.csv)|*.csv",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = GetDataTableFromCSVFile(textBox4.Text);
                EDMIDBContext db = new EDMIDBContext();
                Employees data = new Employees();
                bool isnew = false;
                foreach (DataRow dtRow in dt.Rows)
                {
                    data = new Employees();
                    isnew = false;
                    if (!string.IsNullOrEmpty(dtRow[0].ToString()))
                        data = db.Employees.Where(y => y.EmployeeId == Convert.ToInt32(dtRow[0].ToString())).FirstOrDefault();
                    if (data == null)
                    {
                        data = new Employees();
                        isnew = true;
                    }
                    else
                    {
                        isnew = false;
                    }
                    data.EmployeeName = dtRow[1].ToString();
                    data.EmployeeEmail = dtRow[2].ToString();
                    data.EmployeeAddress = dtRow[3].ToString();

                    if (isnew) db.Employees.Add(data);
                    db.SaveChanges();

                   

                }
                loadData();
                MessageBox.Show("Data has been updated in the database.", "Update");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Import CSV File", MessageBoxButtons.OK,
  MessageBoxIcon.Error);
            }
        }
    }
}
