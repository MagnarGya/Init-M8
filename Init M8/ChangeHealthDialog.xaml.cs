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

namespace Init_M8 { 
    /// <summary>
    /// Interaction logic for ChangeHealthDialog.xaml
    /// </summary>
    public partial class ChangeHealthDialog : Window
    {
        public delegate void refresh();
        event refresh refresher;
        character character;
        public ChangeHealthDialog(character _character, refresh method)
        {
            refresher += method;
            this.character = _character;
            InitializeComponent();
            nameBlock.Text = "Name: " + character.name;
            healthBox.Text = character.health.ToString();
        }

        public void SaveClick(object sender, RoutedEventArgs args)
        {
            try
            {
                int health = Convert.ToInt32(healthBox.Text);
                character.health = health;
                refresher.Invoke();
                this.Close();
            }
            catch
            {
                healthBox.Text = "This field should only contain numbers";
            }
        }

        public void CancelClick(object sender, RoutedEventArgs args)
        {
            refresher.Invoke();
            this.Close();
        }
    }
}
