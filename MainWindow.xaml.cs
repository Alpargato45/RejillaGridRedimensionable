using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RejillaGridRedimensionable
{
    public partial class MainWindow : Window
    {
        private class Persona
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Direccion { get; set; }
            public int Edad { get; set; }
            public int Hijos { get; set; }
            public String Altura { get; set; }
            public String Fecha { get; set; }
            public List<Persona> listaHijos { get; set; } = new List<Persona>();
        }

        public void addPersona()
        {
            Persona nuevaPersona = new Persona
            {
                Nombre = textBoxNombre.Text,
                Apellidos = textBoxApellidos.Text,
                Direccion = textBoxDireccion.Text,
                Edad = int.Parse(textBoxEdad.Text),
                Hijos = Convert.ToInt32(SliderHijos.Value),
                Altura = TxtAltura.Content.ToString(),
                Fecha = escogerFecha.Text.ToString()
            };
            foreach (string nombreHijo in ListBoxHijos.Items.OfType<string>())
            {
                nuevaPersona.listaHijos.Add(new Persona { Nombre = nombreHijo });
            }

            listaPersonas.Add(nuevaPersona);
            MostrarTreeView();
            resetear();
        }

        private void MostrarTreeView()
        {
            // Limpiar el TreeView antes de agregar nuevos elementos
            treeView.Items.Clear();

            foreach (var persona in listaPersonas)
            {
                TreeViewItem personaNode = new TreeViewItem();
                personaNode.Header = persona.Nombre;

                foreach (var hijo in persona.listaHijos)
                {
                    TreeViewItem hijoNode = new TreeViewItem();
                    hijoNode.Header = hijo.Nombre;

                    personaNode.Items.Add(hijoNode);
                }

                treeView.Items.Add(personaNode);
            }
        }

        public static RoutedCommand MyCommand = new RoutedCommand();

        private ObservableCollection<Persona> listaPersonas = new ObservableCollection<Persona>();
        public MainWindow()
        {
            InitializeComponent();
            dataGrid.ItemsSource = listaPersonas;
            MyCommand.InputGestures.Add(new KeyGesture(Key.B, ModifierKeys.Alt));
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) { }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            List<string> lista = new List<string>();

            if (SliderHijos.Value > 0 && ListBoxHijos.Items.OfType<string>().Count() != SliderHijos.Value)
            {
                MessageBox.Show("Por favor, ingresa los nombres de todos los hijos antes de agregar/modificar una persona.");
                return;
            }
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
                    personaSeleccionada.Altura = (String)TxtAltura.Content;
                    personaSeleccionada.Fecha = escogerFecha.Text;
                    personaSeleccionada.listaHijos.Clear();
                    foreach (string nombreHijo in ListBoxHijos.Items.OfType<string>())
                    {
                        personaSeleccionada.listaHijos.Add(new Persona { Nombre = nombreHijo });
                    }
                    MostrarTreeView();

                    dataGrid.SelectedItem = null;
                    dataGrid.Items.Refresh();
                    resetear();
                }
            }
            else
            {
                string errorMessage = "";

                void AddError(string fieldName, string errorMessageFormat, params object[] args)
                {
                    if (string.IsNullOrWhiteSpace(fieldName))
                    {
                        errorMessage += string.Format("ERROR. " + errorMessageFormat + "\n", args);
                    }
                }
                AddError(textBoxNombre.Text, "El campo 'Nombre' está vacío.");
                AddError(textBoxApellidos.Text, "El campo 'Apellidos' está vacío.");
                AddError(textBoxDireccion.Text, "El campo 'Dirección' está vacío.");

                int edad;
                if (!int.TryParse(textBoxEdad.Text, out edad))
                {
                    AddError("", "'Edad' no es un número válido.");
                }
                AddError(escogerFecha.Text, "No has seleccionado tu fecha de nacimiento.");

                int hijosValue = (int)SliderHijos.Value;
                if (ListBoxHijos.Items.Count != hijosValue)
                {
                    AddError(hijosValue.ToString(), "El número de elementos en la lista de hijos no coincide con el valor del Slider ({0}).", hijosValue);
                }
                textoError.Content = string.IsNullOrEmpty(errorMessage) ? "" : errorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
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
                    foreach (var hijo in persona.listaHijos)
                    {
                        ListBoxHijos.Items.Add(hijo.Nombre);
                    }
                    btnAceptar.Content = "Modificar";
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
            TextBoxHijos.Text = "";
            ListBoxHijos.ItemsSource = null;
            ListBoxHijos.Items.Clear();
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
                    MostrarTreeView();
                }
            }
        }

        private void MyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            resetear();
        }

        private void RepeatMas_Click(object sender, RoutedEventArgs e)
        {
            string labelText = (String)TxtAltura.Content;
            if (int.TryParse(labelText, out int numero))
            {
                if (numero < 230)
                {
                    numero += 1;
                    TxtAltura.Content = numero.ToString();
                }
                else
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
                if (numero > 65)
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SliderHijos.IsEnabled = true;
            GroupBoxHijos.Visibility = Visibility.Visible;
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            SliderHijos.IsEnabled = false;
            SliderHijos.Value = 0;
            GroupBoxHijos.Visibility = Visibility.Hidden;
        }

        private void btnHijos_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxHijos.Items.Count >= SliderHijos.Value)
            {
                MessageBox.Show("No se puede añadir más hijos. Número máximo alcanzado.");
            }
            else if (TextBoxHijos.Text == "")
            {
                MessageBox.Show("No se puede añadir. El campo está vacío.");
            }
            else
            {
                ListBoxHijos.Items.Add(TextBoxHijos.Text);
                TextBoxHijos.Text = string.Empty;
            }
        }
        private void ListBoxHijos_SelectionChanged(object sender, SelectionChangedEventArgs e) {}
        private void TextBoxHijos_TextChanged(object sender, TextChangedEventArgs e) { }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ListBoxHijos.SelectedItem != null)
            {
                ListBoxHijos.Items.Remove(ListBoxHijos.SelectedItem);
                ListBoxHijos.Items.Refresh();
            }
        }
        private void CambiarFondoNegro_Click(object sender, RoutedEventArgs e)
        {
            FondoApp.Background = new SolidColorBrush(Colors.Black);
        }
        private void CambiarFondoBeige_Click(object sender, RoutedEventArgs e)
        {
            FondoApp.Background = new SolidColorBrush(Colors.Beige);
        }
        private void CambiarFondoBlanco_Click(object sender, RoutedEventArgs e)
        {
            FondoApp.Background = new SolidColorBrush(Colors.White);
        }
    }
}