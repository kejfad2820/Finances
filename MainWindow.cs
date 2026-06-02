using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace WindowsFormsAppExam1
{
    public partial class MainWindow : Form
    {
        public static int idRole;

        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AutoService.mdf;Integrated Security=True";
        public MainWindow(int idRole)
        {
            InitializeComponent();
            OpenPanel(idRole);
        }

        public void OpenPanel(int idRole)
        {
            panelAdmin.Hide();
            panelAdmin.Enabled = false;
            panelBooker.Hide();
            panelBooker.Enabled = false;
            panelManager.Hide();
            panelManager.Enabled = false;
            panelMaster.Hide();
            panelMaster.Enabled = false;
            panelMechanic.Hide();
            panelMechanic.Enabled = false;

            switch(idRole)
            {
                case 1:
                    panelAdmin.Show();
                    panelAdmin.Enabled = true;
                    break;
                case 2:
                    panelManager.Show();
                    panelManager.Enabled = true;
                    break;
                case 3:
                    panelBooker.Show();
                    panelBooker.Enabled = true;
                    break;
                case 4:
                    panelMaster.Show();
                    panelMaster.Enabled = true;
                    break;
                case 5:
                    panelMechanic.Show();
                    panelMechanic.Enabled = true;
                    break;
            }
        }

        public void WriteEmployees()
        {
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM Employees";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;

                conn.Close();
            }
        }

        public void ExitMainWindow()
        {
            AuthWindow formAuth = new AuthWindow();
            this.Hide();
            formAuth.ShowDialog();
            this.Close();
        }

        private void buttonReg_Click(object sender, EventArgs e)
        {
            RegWindow formReg = new RegWindow();
            formReg.ShowDialog();
        }

        private void buttonExitAdmin_Click(object sender, EventArgs e)
        {
            ExitMainWindow();
        }

        private void buttonExitMaster_Click(object sender, EventArgs e)
        {
            ExitMainWindow();
        }

        private void buttonExitMechanic_Click(object sender, EventArgs e)
        {
            ExitMainWindow();
        }

        private void buttonExitBooker_Click(object sender, EventArgs e)
        {
            ExitMainWindow();
        }

        private void buttonExitManager_Click(object sender, EventArgs e)
        {
            ExitMainWindow();
        }

        private void buttonStorage_Click(object sender, EventArgs e)
        {
            StoragesWindowAdmin storagesWindow = new StoragesWindowAdmin();
            this.Hide();
            storagesWindow.ShowDialog(); 
            this.Close();
        }

        private void buttonEmployees_Click(object sender, EventArgs e)
        {
            WriteEmployees();
        }
    }
}
