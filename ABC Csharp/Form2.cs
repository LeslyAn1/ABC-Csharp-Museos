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
    public partial class Form2 : Form
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=lesly1;Database=MuseosMx";

        public Form2()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 formaSiguiente = new Form3();
            formaSiguiente.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombreMuseo.Text))
            {
                InsertMuseo(txtNombreMuseo.Text);
                LoadMuseos();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un nombre de museo.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtIdMuseo.Text, out int id) && !string.IsNullOrWhiteSpace(txtNombreMuseo.Text))
            {
                UpdateMuseo(id, txtNombreMuseo.Text);
                LoadMuseos();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un identificador de museo válido y un nombre de museo.");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtIdMuseo.Text, out int id))
            {
                DeleteMuseo(id);
                LoadMuseos();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un museo válido para eliminar.");
            }
        }

        private void LoadMuseos()
        {
            DataTable dataTable = GetMuseos();
            dataGridView1.DataSource = dataTable;
            dataGridView1.Refresh();
        }

        private void InsertMuseo(string nombreMuseo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spInsertMuseo(@NombreMuseo)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NombreMuseo", nombreMuseo);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar museo: " + ex.Message);
            }
        }

        private DataTable GetMuseos()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            DataTable dataTable = new DataTable();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spGetMuseos()", connection))
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
                MessageBox.Show("Error al obtener museos: " + ex.Message);
            }
            return dataTable;
        }

        private void UpdateMuseo(int idMuseo, string nombreMuseo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spUpdateMuseo(@IdMuseo, @NombreMuseo)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdMuseo", idMuseo);
                        command.Parameters.AddWithValue("@NombreMuseo", nombreMuseo);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar museo: " + ex.Message);
            }
        }

        private void DeleteMuseo(int idMuseo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spDeleteMuseo(@IdMuseo)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdMuseo", idMuseo);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar museo: " + ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                txtIdMuseo.Text = row.Cells["IdMuseo"].Value.ToString();
                txtNombreMuseo.Text = row.Cells["NombreMuseo"].Value.ToString();
            }

        }
}
