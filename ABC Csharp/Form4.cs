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
    public partial class Form4 : Form
    {

        private string connectionString = "Host=localhost;Username=postgres;Password=lesly1;Database=MuseosMx";

        public Form4()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadVisitantes();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 formaSiguiente = new Form5();
            formaSiguiente.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int id))
            {
                DeleteVisitante(id);
                LoadVisitantes();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un visitante válido para eliminar.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int id) && !string.IsNullOrWhiteSpace(txtTipoVisitante.Text))
            {
                UpdateVisitante(id, txtTipoVisitante.Text);
                LoadVisitantes();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un identificador de visitante válido y un tipo de visitante.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTipoVisitante.Text))
            {
                InsertVisitante(txtTipoVisitante.Text);
                LoadVisitantes();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un tipo de visitante.");
            }
        }


        private void LoadVisitantes()
        {
            DataTable dataTable = GetVisitantes();
            dataGridView1.DataSource = dataTable;
            dataGridView1.Refresh();
        }

        private void InsertVisitante(string tipoVisitante)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spInsertVisitante(@TipoVisitante)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TipoVisitante", tipoVisitante);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar visitante: " + ex.Message);
            }
        }

        private DataTable GetVisitantes()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            DataTable dataTable = new DataTable();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spGetVisitantes()", connection))
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
                MessageBox.Show("Error al obtener visitantes: " + ex.Message);
            }
            return dataTable;
        }

        private void UpdateVisitante(int idVisitante, string tipoVisitante)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spUpdateVisitante(@IdVisitante, @TipoVisitante)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdVisitante", idVisitante);
                        command.Parameters.AddWithValue("@TipoVisitante", tipoVisitante);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar visitante: " + ex.Message);
            }
        }

        private void DeleteVisitante(int idVisitante)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("CALL spDeleteVisitante(@IdVisitante)", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdVisitante", idVisitante);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar visitante: " + ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                textBox1.Text = row.Cells["IdVisitante"].Value.ToString();
                txtTipoVisitante.Text = row.Cells["TipoVisitante"].Value.ToString();
            }

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
