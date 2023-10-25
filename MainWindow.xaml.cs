using RejillaGridRedimensionable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        }

        public void addPersona()
        {
            Persona nuevaPersona = new Persona
            {
                Nombre = textBoxNombre.Text,
                Apellidos = textBoxApellidos.Text,
                Direccion = textBoxDireccion.Text,
                Edad = int.Parse(textBoxEdad.Text)
            };

            listaPersonas.Add(nuevaPersona);

            textBoxNombre.Text = string.Empty;
            textBoxApellidos.Text = string.Empty;
            textBoxDireccion.Text = string.Empty;
            textBoxEdad.Text = string.Empty;
        }

        private ObservableCollection<Persona> listaPersonas = new ObservableCollection<Persona>();
        public MainWindow()
        {
            InitializeComponent();
            dataGrid.ItemsSource = listaPersonas;
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
                    btnAceptar.Content = "Modificar";

                    void btnAceptar_Click(object sender, RoutedEventArgs e)
                    {
                        textBoxNombre.Text = persona.Nombre;
                        textBoxApellidos.Text = persona.Apellidos;
                        textBoxDireccion.Text = persona.Direccion;
                        textBoxEdad.Text = persona.Edad.ToString();
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
            btnAceptar.Content = "Aceptar";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
                var personaSeleccionada = dataGrid.SelectedItem as Persona;
                if (personaSeleccionada != null)
                {
                listaPersonas.Remove(personaSeleccionada);
                    resetear();
                }
            }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.B)
            {
                resetear();
                e.Handled = true;
            }
        }
    }
}
