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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ModEffect : Window
    {
        public delegate void refresh();
        event refresh refresher;
        Status changee;
        public ModEffect(Status _changee, List<character> characters, refresh method)
        {
            InitializeComponent();
            refresher += method;
            changee = _changee;
            namebox.Text = changee.name;
            DuraBox.Text = changee.duration.ToString();
            TargetBox.ItemsSource = characters;
            SourceBox.ItemsSource = characters;
            TargetBox.SelectedItem = changee.target;
            SourceBox.SelectedItem = changee.source;
            Keyboard.Focus(namebox);
        }

        void addClick(object sender, RoutedEventArgs args)
        {
            try
            {
                string name = namebox.Text;
                int duration = Convert.ToInt32(DuraBox.Text);
                changee.update(name, duration, TargetBox.SelectedItem as character, SourceBox.SelectedItem as character);
                refresher.Invoke();
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

