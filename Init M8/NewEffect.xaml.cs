using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Init_M8
{
    /// <summary>
    /// Interaction logic for NewEffect.xaml
    /// </summary>
    public partial class NewEffect : Window
    {
        public delegate void giveeffect(string name, int duration, character target, character source);
        event giveeffect effectgiver;
        public NewEffect(giveeffect method, character target, character source, List<character> characters)
        {
            effectgiver += method;
            InitializeComponent();
            TargetBox.ItemsSource = characters;
            SourceBox.ItemsSource = characters;
            TargetBox.SelectedItem = target;
            SourceBox.SelectedItem = source;
            Keyboard.Focus(namebox);
        }

        void addClick(object sender, RoutedEventArgs args)
        {
            try
            {
                string name = namebox.Text;
                int duration = Convert.ToInt32(DuraBox.Text);
                effectgiver.Invoke(name, duration, TargetBox.SelectedItem as character, SourceBox.SelectedItem as character);
                this.Close();
            }
            catch
            {

            }
        }

        void cancelClick(object sender, RoutedEventArgs args)
        {
            this.Close();
        }
    }
}
