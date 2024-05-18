using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int numeroUsuarios) || numeroUsuarios <= 0)
            {
                MessageBox.Show("Ingrese un número válido de usuarios.");
                textBox1.Focus();
                return;
            }

            if (!float.TryParse(textBox2.Text, out float anchoBandaCanal) || anchoBandaCanal <= 0)
            {
                MessageBox.Show("Ingrese un ancho de banda válido para el canal.");
                textBox2.Focus();
                return;
            }

            if (!float.TryParse(textBox3.Text, out float anchoBandaGuarda) || anchoBandaGuarda <= 0)
            {
                MessageBox.Show("Ingrese un ancho de banda válido para la guarda.");
                textBox3.Focus();  
                return;
            }

            if (!float.TryParse(textBox4.Text, out float frecuenciaCanal) || frecuenciaCanal <= 0)
            {
                MessageBox.Show("Ingrese una frecuencia válida para el canal.");
                textBox4.Focus();
                return;
            }

            DatosMultiplexacion datos = new DatosMultiplexacion
            {
                NumeroUsuarios = numeroUsuarios,
                AnchoBandaCanal = anchoBandaCanal,
                AnchoBandaGuarda = anchoBandaGuarda,
                FrecuenciaCentralCanal1 = frecuenciaCanal
            };

            // Calcular resultados
            float anchoBandaTotal = CalcularAnchoBandaTotal(datos);
            float tasaKbpsTotal = CalcularTasaKbpsTotal(datos);
            float tasaKbpsTotalSinGuarda = CalcularTasaKbpsTotalSinGuarda(anchoBandaTotal);

            // Mostrar resultados
            richTextBox1.Clear();
            richTextBox1.AppendText("Datos ingresados:\n");
            richTextBox1.AppendText($"Número de usuarios: {datos.NumeroUsuarios}\n");
            richTextBox1.AppendText($"Ancho de banda por canal: {datos.AnchoBandaCanal} kHz\n");
            richTextBox1.AppendText($"Ancho de banda de guarda: {datos.AnchoBandaGuarda} kHz\n");
            richTextBox1.AppendText($"Frecuencia del canal: {datos.FrecuenciaCentralCanal1} MHz\n\n");

            for (int i = 1; i <= datos.NumeroUsuarios; i++)
            {
                float frecuenciaPortadoraUsuarioActual = CalcularFrecuenciaPortadoraUsuario(datos, i);
                richTextBox1.AppendText($"Frecuencia portadora del usuario {i}: {frecuenciaPortadoraUsuarioActual:F6} MHz\n");
            }

            richTextBox1.AppendText($"\nAncho de banda total: {anchoBandaTotal:F2} kHz\n");
            richTextBox1.AppendText($"Número total de Kbps que se transmiten: {tasaKbpsTotal:F2} kbps\n");
            richTextBox1.AppendText($"Número total de Kbps que podrían transmitirse utilizando todo el ancho de banda: {tasaKbpsTotalSinGuarda:F2} kbps\n");
            LimpiarFormulario();
        }
        private float CalcularFrecuenciaPortadoraUsuario(DatosMultiplexacion datos, int numeroUsuario)
        {
            return datos.FrecuenciaCentralCanal1 + (numeroUsuario - 1) * (datos.AnchoBandaCanal + datos.AnchoBandaGuarda) / 1000.0f;
        }

        private float CalcularAnchoBandaTotal(DatosMultiplexacion datos)
        {
            return 2 * (datos.AnchoBandaCanal / 2) + (datos.NumeroUsuarios - 1) * (datos.AnchoBandaCanal + datos.AnchoBandaGuarda);
        }

        private float CalcularTasaKbpsUsuario()
        {
            return 100.0f; // Tasa fija de transmisión por usuario en kbps
        }

        private float CalcularTasaKbpsTotal(DatosMultiplexacion datos)
        {
            return datos.NumeroUsuarios * CalcularTasaKbpsUsuario();
        }

        private float CalcularTasaKbpsTotalSinGuarda(float anchoBandaTotal)
        {
            return anchoBandaTotal * 0.5f; // Utilizando toda la banda, relación 0.5 bps/Hz
        }
        private void LimpiarFormulario()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox1.Focus(); 
        }

        public struct DatosMultiplexacion
        {
            public int NumeroUsuarios;
            public float AnchoBandaCanal;
            public float AnchoBandaGuarda;
            public float FrecuenciaCentralCanal1;
        }
    }
}
