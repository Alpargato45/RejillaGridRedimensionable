using RejillaGridRedimensionable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static RejillaGridRedimensionable.MainWindow;


namespace RejillaGridRedimensionable
{
    
    public partial class MainWindow : Window
    {
        public class Persona
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Direccion { get; set; }
            public int Edad { get; set; }
            public int Hijos { get; set; }
            public String Altura { get; set; }

            public String Fecha { get; set; }
        }

        public void addPersona()
        {
            Persona nuevaPersona = new Persona
            {
                Nombre = textBoxNombre.Text,
                Apellidos = textBoxApellidos.Text,
                Direccion = textBoxDireccion.Text,
                Edad = int.Parse(textBoxEdad.Text),
                Hijos = (int)SliderHijos.Value,
                Altura = (string)TxtAltura.Content,
                Fecha = escogerFecha.Text.ToString(),
            };

            listaPersonas.Add(nuevaPersona);

            resetear();
        }

        public static RoutedCommand MyCommand = new RoutedCommand();

        private ObservableCollection<Persona> listaPersonas = new ObservableCollection<Persona>();
        public MainWindow()
        {
            InitializeComponent();
            dataGrid.ItemsSource = listaPersonas;
            MyCommand.InputGestures.Add(new KeyGesture(Key.B, ModifierKeys.Alt));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                var personaSeleccionada = dataGrid.SelectedItem as Persona;
                if (personaSeleccionada != null)
                {
                    
                    personaSeleccionada.Nombre = textBoxNombre.Text;
                    personaSeleccionada.Apellidos = textBoxApellidos.Text;
                    personaSeleccionada.Direccion = textBoxDireccion.Text;
                    if (int.TryParse(textBoxEdad.Text, out int edad))
                    {
                        personaSeleccionada.Edad = edad;
                    }
                    personaSeleccionada.Hijos = (int)SliderHijos.Value;
                    personaSeleccionada.Altura =(String) TxtAltura.Content;
                    personaSeleccionada.Fecha = escogerFecha.Text;

                    dataGrid.SelectedItem = null;
                    dataGrid.Items.Refresh();
                    resetear();
                }
            }
            else
            {

                string errorMessage = "";

                if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
                {
                    errorMessage += "ERROR. El campo 'Nombre' está vacío.\n";
                }

                if (string.IsNullOrWhiteSpace(textBoxApellidos.Text))
                {
                    errorMessage += "ERROR. El campo 'Apellidos' está vacío.\n";
                }

                if (string.IsNullOrWhiteSpace(textBoxDireccion.Text))
                {
                    errorMessage += "ERROR. El campo 'Dirección' está vacío.\n";
                }

                if (!int.TryParse(textBoxEdad.Text, out int edad))
                {
                    errorMessage += "ERROR. 'Edad' no es un número válido.\n";
                }
                if (string.IsNullOrWhiteSpace(escogerFecha.Text))
                {
                    errorMessage += "ERROR. No has seleccionado tu fecha de nacimiento.\n";
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    textoError.Content = errorMessage;
                }
                else
                {
                    textoError.Content = "";
                    addPersona();
                }
            }

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                var persona = dataGrid.SelectedItem as Persona;

                if (persona != null)
                {
                    textBoxNombre.Text = persona.Nombre;
                    textBoxApellidos.Text = persona.Apellidos;
                    textBoxDireccion.Text = persona.Direccion;
                    textBoxEdad.Text = persona.Edad.ToString();
                    SliderHijos.Value = persona.Hijos;
                    TxtAltura.Content = persona.Altura;
                    escogerFecha.Text = persona.Fecha;
                    btnAceptar.Content = "Modificar";

                    void btnAceptar_Click(object sender, RoutedEventArgs e)
                    {
                        textBoxNombre.Text = persona.Nombre;
                        textBoxApellidos.Text = persona.Apellidos;
                        textBoxDireccion.Text = persona.Direccion;
                        textBoxEdad.Text = persona.Edad.ToString();
                        SliderHijos.Value = persona.Hijos;
                        TxtAltura.Content = persona.Altura;
                        escogerFecha.Text = persona.Fecha;
                    }
                }
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            resetear();
        }

        public void resetear()
        {
            textBoxNombre.Text = "";
            textBoxApellidos.Text = "";
            textBoxDireccion.Text = "";
            textBoxEdad.Text = "";
            SliderHijos.Value = 0;
            TxtAltura.Content = 170;
            escogerFecha.Text = "";

            btnAceptar.Content = "Aceptar";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var personaSeleccionada = dataGrid.SelectedItem as Persona;
            if (personaSeleccionada != null)
            {
                MessageBoxResult result = MessageBox.Show("¿De verdad quieres borrar esta fila?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    listaPersonas.Remove(personaSeleccionada);
                    resetear();
                }
            }
        }
        private void MyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            resetear();
        }

        private void CheckBoxHijos_Checked(object sender, RoutedEventArgs e)
        {
            SliderHijos.IsEnabled = true;
        }

        private void RepeatMas_Click(object sender, RoutedEventArgs e)
        {
            string labelText =(String) TxtAltura.Content;

            if (int.TryParse(labelText, out int numero))
            {
                if(numero <= 230)
                {
                    numero += 1;
                    TxtAltura.Content = numero.ToString();
                }else
                {
                    textoError.Content = "Altura Máxima Alcanzada";
                }
            }
        }

        private void RepeatMenos_Click(object sender, RoutedEventArgs e)
        {
            string labelText = (String)TxtAltura.Content;

            if (int.TryParse(labelText, out int numero))
            {
                if (numero >= 65)
                {
                    numero -= 1;
                    TxtAltura.Content = numero.ToString();
                }
                else
                {
                    textoError.Content = "Altura Mínima Alcanzada";
                }
            }
        }
    }
}
