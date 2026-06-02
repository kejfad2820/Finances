using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlTypes;

namespace WindowsFormsAppExam1
{
    public partial class AuthWindow : Form
    {
        public static int idRole;
        string connectionString = @"Data Source = (localdb)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\AutoService.mdf; Integrated Security = True";

        public AuthWindow()
        {
            InitializeComponent();
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                Debug.WriteLine("DB is connected");
                conn.Close();
            }
        }

        public int CheckRole(string login)
        {
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string loginHashed = login;

                string sqlRole = "SELECT Role_id FROM Employees WHERE Login = @loginHashed";

                SqlCommand selectRole = new SqlCommand(sqlRole, conn);

                selectRole.Parameters.AddWithValue("@loginHashed", loginHashed);

                SqlDataReader reader = selectRole.ExecuteReader();

                reader.Read();

                int idRole = reader.GetInt32(0);

                reader.Close();

                conn.Close();

                return idRole;
            }
        }

        public void Auth()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                try 
                { 
                    string loginHash = Hash(textLoginAuth.Text);
                    string passHash = Hash(textPassAuth.Text);

                    SqlCommand check_data = new SqlCommand("SELECT 1 FROM Employees WHERE Login = @loginHash AND Password = @passHash", conn);
                    check_data.Parameters.AddWithValue("@loginHash", loginHash);
                    check_data.Parameters.AddWithValue("@passHash", passHash);
                    object check_id = check_data.ExecuteScalar();

                    if (check_id == null || check_id == DBNull.Value)
                    {
                        MessageBox.Show("Неверный ввод", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        int value = (int)check_id;
                        if(value == 1)
                        {
                            MessageBox.Show($"Успешный вход, {textLoginAuth.Text}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            OpenWindow(CheckRole(loginHash));
                        }
                        else
                        {
                            MessageBox.Show("Неверный ввод", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Неверный ввод", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                conn.Close();
            }
        }

        public static string Hash(string input)
        {
            if(input == null) throw new ArgumentNullException("input");

            var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = sha256.ComputeHash(bytes);

            var sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);

            return sb.ToString();
        }

        public void OpenWindow(int idRole)
        {
            MainWindow mainForm = new MainWindow(idRole);

            this.Hide();

            mainForm.ShowDialog();
            
            this.Close();
        }

        private void buttonAuth_Click(object sender, EventArgs e)
        {
            Auth();
        }
    }
}
