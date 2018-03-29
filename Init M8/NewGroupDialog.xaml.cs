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
    /// Interaction logic for NewGroupDialog.xaml
    /// </summary>
    public partial class NewGroupDialog : Window
    {
        public delegate void givelist(List<player> names);
        event givelist listgiver;
        List<player> players;
        player chosen;
        public NewGroupDialog(givelist method)
        {
            chosen = null;
            listgiver += method;
            players = new List<player>();
            InitializeComponent();
            Keyboard.Focus(namebox);
        }

        void addClick(object sender, RoutedEventArgs args)
        {
            try
            {
                string name = namebox.Text;
                int health = Convert.ToInt32(healthBox.Text);
                if (chosen == null)
                {
                    players.Add(new player(name, health));
                }
                else
                {
                    chosen.name = name;
                    chosen.health = health;
                    chosen = null;
                    addButton.Content = "Add";
                }
                namebox.Text = "";
                healthBox.Text = "";
                Keyboard.Focus(namebox);
                memberListView.ItemsSource = players;
                refreshWindow();
            }
            catch
            {
                healthBox.Text = "Needs to be only numbers";
                Keyboard.Focus(healthBox);
            }

        }

        void MemberSelected(object sender, RoutedEventArgs args)
        {
            chosen = memberListView.SelectedItem as player;
            namebox.Text = chosen.name;
            healthBox.Text = chosen.health.ToString();
            addButton.Content = "Save Change";
        }

        void refreshWindow()
        {
            memberListView.Items.Refresh();
            Height = 135 + (20 * players.Count);
        }

        void doneClick(object sender, RoutedEventArgs args)
        {
            listgiver.Invoke(players);
            this.Close();
        }

    }
    public class player
    {
        public string name { get; set; }
        public int health { get; set; }
        public player(string _name, int _health)
        {
            name = _name;
            health = _health;
        }
        public override string ToString()
        {
            return "Name: " + this.name + " Health: " + this.health;
        }
    }
}
