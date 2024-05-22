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
    public partial class Form3 : Form
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=lesly1;Database=MuseosMx";

        public Form3()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 formaSiguiente = new Form4();
            formaSiguiente.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtIdNacionalidad.Text, out int id) && !string.IsNullOrWhiteSpace(txtTipoNacionalidad.Text))
            {
                UpdateNacionalidad(id, txtTipoNacionalidad.Text);
                LoadNacionalidades();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un identificador de nacionalidad válido y un tipo de nacionalidad.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTipoNacionalidad.Text))
            {
                InsertNacionalidad(txtTipoNacionalidad.Text);
                LoadNacionalidades();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un tipo de nacionalidad.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtIdNacionalidad.Text, out int id))
            {
                DeleteNacionalidad(id);
                LoadNacionalidades();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una nacionalidad válida para eliminar.");
            }
        }

        private void LoadNacionalidades()
        {
            DataTable dataTable = GetNacionalidades();
            dataGridView1.DataSource = dataTable;
            dataGridView1.Refresh();
        }

        private void InsertNacionalidad(string tipoNacionalidad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spInsertNacionalidad(@TipoNacionalidad)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TipoNacionalidad", tipoNacionalidad);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar nacionalidad: " + ex.Message);
            }
        }

        private DataTable GetNacionalidades()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            DataTable dataTable = new DataTable();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spGetNacionalidades()", connection))
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
                MessageBox.Show("Error al obtener nacionalidades: " + ex.Message);
            }
            return dataTable;
        }

        private void UpdateNacionalidad(int idNacionalidad, string tipoNacionalidad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spUpdateNacionalidad(@IdNacionalidad, @TipoNacionalidad)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdNacionalidad", idNacionalidad);
                        command.Parameters.AddWithValue("@TipoNacionalidad", tipoNacionalidad);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar nacionalidad: " + ex.Message);
            }
        }

        private void DeleteNacionalidad(int idNacionalidad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spDeleteNacionalidad(@IdNacionalidad)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdNacionalidad", idNacionalidad);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar nacionalidad: " + ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                txtIdNacionalidad.Text = row.Cells["IdNacionalidad"].Value.ToString();
                txtTipoNacionalidad.Text = row.Cells["TipoNacionalidad"].Value.ToString();
            }

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadNacionalidades();

        }
    }
}