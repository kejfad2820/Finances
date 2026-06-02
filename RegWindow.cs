using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppExam1
{
    public partial class RegWindow : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AutoService.mdf;Integrated Security=True";

        public RegWindow()
        {
            InitializeComponent();
            comboRoleReg.Items.Clear();
            comboRoleReg.Items.Add("Управляющий");
            comboRoleReg.Items.Add("Бухгалтер");
            comboRoleReg.Items.Add("Мастер-приемщик");
            comboRoleReg.Items.Add("Механик");
        }

        public void Registration()
        {
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string name = textNameReg.Text.Trim();
                string login = textLoginReg.Text.Trim();
                string password = textPassReg.Text.Trim();
                string role = comboRoleReg.Text.Trim();
                string hashedLogin, hashedPassword;
                int idRole;
                int branch_id = 1;

                if(name != "" || login != "" || password != "" || role != "")
                {
                    if (login.Length >= 6)
                    {
                        if (password.Length >= 8)
                        {
                            if(role != "")
                            {
                                hashedLogin = Hash(login);
                                hashedPassword = Hash(password);

                                string sqlCheck = "SELECT 1 FROM Employees WHERE Login = @hashedLogin";

                                SqlCommand check = new SqlCommand(sqlCheck, conn);

                                check.Parameters.AddWithValue("@hashedLogin", hashedLogin);

                                object result = check.ExecuteScalar();

                                if (result == null || result == DBNull.Value)
                                {
                                    string sqlSelectRole = "SELECT Id FROM Roles WHERE Name = @role";
                                    SqlCommand selectRole = new SqlCommand(sqlSelectRole, conn);

                                    selectRole.Parameters.AddWithValue("@role", role);

                                    SqlDataReader reader = selectRole.ExecuteReader();

                                    reader.Read();

                                    idRole = reader.GetInt32(0);

                                    reader.Close();

                                    string sqlReg = "INSERT INTO Employees (Name, Login, Password, Role_id, Branch_id) VALUES(@name, @hashedLogin, @hashedPassword, @idRole, @branch_id)";
                                    SqlCommand reg = new SqlCommand(sqlReg, conn);

                                    reg.Parameters.AddWithValue("@name", name);
                                    reg.Parameters.AddWithValue("@hashedLogin", hashedLogin);
                                    reg.Parameters.AddWithValue("@hashedPassword", hashedPassword);
                                    reg.Parameters.AddWithValue("@idRole", idRole);
                                    reg.Parameters.AddWithValue("@branch_id", branch_id);

                                    reg.ExecuteNonQuery();

                                    MessageBox.Show("Успешная регистрация", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Такой пользователь уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Выберите роль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }
                        }
                        else
                        {
                            MessageBox.Show("Пароль не может быть меньше 8-ми символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Логин не может быть меньше 6-ти символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Все поля должны быть заполнены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                conn.Close();
            }
        }

        public static string Hash(string input)
        {
            var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = sha256.ComputeHash(bytes);

            var sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);

            return sb.ToString();
        }

        private void buttonReg_Click(object sender, EventArgs e)
        {
            Registration();
        }
    }
}
