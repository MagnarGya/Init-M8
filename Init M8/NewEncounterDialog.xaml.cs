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
    /// Interaction logic for NewEncounterDialog.xaml
    /// </summary>
    public partial class NewEncounterDialog : Window
    {
        public delegate void givelist(List<character> characters);
        event givelist listgiver;
        List<player> playerlist;
        List<player> oldnames;
        List<character> characters;
        character chosen;
        public NewEncounterDialog(List<player> players, givelist method)
        {
            this.characters = new List<character>();
            this.playerlist = new List<player>(players);
            oldnames = new List<player>(players);
            listgiver += method;
            chosen = null;
            InitializeComponent();
            refreshView();
        }
        void refreshView()
        {
            memberListView.Items.Refresh();
            if (chosen == null)
            {
                if (playerlist.Count != 0)
                {
                    string name = playerlist[0].name;
                    int health = playerlist[0].health;
                    playerlist.RemoveAt(0);
                    namebox.Text = name;
                    healthBox.Text = health.ToString();
                    Keyboard.Focus(initBox);
                }
                else
                {
                    namebox.Text = "";
                    healthBox.Text = "";
                    Keyboard.Focus(namebox);
                }
                initBox.Text = "";
            }
            else
            {
                namebox.Text = chosen.name;
                initBox.Text = chosen.initiative.ToString();
                healthBox.Text = chosen.health.ToString();
                addButton.Content = "Save Change";
                Keyboard.Focus(namebox);
            }
            Height = 155 + (20 * characters.Count);
        }

        void doneClick(object sender, RoutedEventArgs args)
        {
            listgiver.Invoke(characters);
            playerlist = oldnames;
            this.Close();
        }

        void addClick(object sender, RoutedEventArgs args)
        {
            try
            {
                string name = namebox.Text;
                int initiative = Convert.ToInt32(initBox.Text);
                int health = Convert.ToInt32(healthBox.Text);
                if (chosen == null)
                {
                    characters.Add(new character(name, initiative, health));
                }
                else
                {
                    chosen.name = name;
                    chosen.initiative = initiative;
                    chosen.health = health;
                    addButton.Content = "Add";
                    chosen = null;
                }
                memberListView.ItemsSource = characters;
                refreshView();
            }
            catch
            {

            }
        }

        void selectionChanged(object sender, RoutedEventArgs args)
        {
            try
            {
                string name = namebox.Text;
                int health = Convert.ToInt32(healthBox.Text);
                playerlist.Insert(0, new player(name, health));
            }
            catch { }
            chosen = memberListView.SelectedItem as character;
            refreshView();
        }
    }
}
