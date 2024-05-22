using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;



namespace ABC_Csharp
{
    public partial class Form1 : Form

    {
        private string connectionString = "Host=localhost;Username=postgres;Password=lesly1;Database=MuseosMx";

        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 formaSiguiente = new Form2();
            formaSiguiente.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadEstados();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombreEstado.Text))
            {
                InsertEstado(txtNombreEstado.Text);
                LoadEstados();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un nombre de estado.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtIdEstado.Text, out int id) && !string.IsNullOrWhiteSpace(txtNombreEstado.Text))
            {
                UpdateEstado(id, txtNombreEstado.Text);
                LoadEstados();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un identificador de estado válido y un nombre de estado.");
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtIdEstado.Text, out int id))
            {
                DeleteEstado(id);
                LoadEstados();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un estado válido para eliminar.");
            }
        }


        private void LoadEstados()
        {
            DataTable dataTable = GetEstados();
            dataGridView1.DataSource = dataTable;
            dataGridView1.Refresh();
        }


        private void InsertEstado(string nombreEstado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spInsertEstado(@NombreEstado)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NombreEstado", nombreEstado);
                        command.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar estado: " + ex.Message);
            }
        }

        private DataTable GetEstados()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            DataTable dataTable = new DataTable();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spGetEstados()", connection))
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
                MessageBox.Show("Error al obtener estados: " + ex.Message);
            }
            return dataTable;
        }

        private void UpdateEstado(int idEstado, string nombreEstado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spUpdateEstado(@IdEstado, @NombreEstado)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEstado", idEstado);
                        command.Parameters.AddWithValue("@NombreEstado", nombreEstado);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar estado: " + ex.Message);
            }
        }

        private void DeleteEstado(int idEstado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spDeleteEstado(@IdEstado)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEstado", idEstado);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar estado: " + ex.Message);
            }
        } 

            private void dataGridView1_SelectionChanged(object sender, EventArgs e)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = dataGridView1.SelectedRows[0];
                    txtIdEstado.Text = row.Cells["IdEstado"].Value.ToString();
                    txtNombreEstado.Text = row.Cells["NombreEstado"].Value.ToString();
                }
            }
    }
}
