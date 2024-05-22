using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABC_Csharp
{
    public partial class Form5 : Form
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=lesly1;Database=MuseosMx";

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }



        private void LoadUsers()
        {
            DataTable dataTable = GetUsers();
            dataGridView1.DataSource = dataTable;
            dataGridView1.Refresh();
        }

        private void InsertUser(string nombre, string user, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spInsertUser(@Nombre, @User, @Password)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@User", user);
                        command.Parameters.AddWithValue("@Password", password);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar usuario: " + ex.Message);
            }
        }

        private DataTable GetUsers()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            DataTable dataTable = new DataTable();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spGetUsers()", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener usuarios: " + ex.Message);
            }
            return dataTable;
        }

        private void UpdateUser(int id, string nombre, string user, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spUpdateUser(@Id, @Nombre, @User, @Password)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@User", user);
                        command.Parameters.AddWithValue("@Password", password);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar usuario: " + ex.Message);
            }
        }

        private void DeleteUser(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spDeleteUser(@Id)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar usuario: " + ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtUser.Text = row.Cells["User"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text) && !string.IsNullOrWhiteSpace(txtUser.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                InsertUser(txtNombre.Text, txtUser.Text, txtPassword.Text);
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtId.Text, out int id) && !string.IsNullOrWhiteSpace(txtNombre.Text) && !string.IsNullOrWhiteSpace(txtUser.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                UpdateUser(id, txtNombre.Text, txtUser.Text, txtPassword.Text);
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido y complete todos los campos.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtId.Text, out int id))
            {
                DeleteUser(id);
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un ID válido para eliminar.");
            }
        }
    }
}