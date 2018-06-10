using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Init_M8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<character> characterlist;
        List<character> orgCharList;
        List<player> playerList;
        List<Status> statuslist;
        bool isInitializing;
        int turn;
        int round;
        public MainWindow()
        {
            isInitializing = true;
            InitializeComponent();
            turn = 0;
            round = 0;
            advanceTurn.IsEnabled = false;
            newEncounter.IsEnabled = false;
            StatusManipList.Visibility = Visibility.Collapsed;
            statuslist = new List<Status>();
            StatusManipList.ItemsSource = statuslist;
            isInitializing = false;
        }

        public void CloseBtn(object sender, RoutedEventArgs args)
        {
            this.Close();
        }

        void sortList()
        {
            SinglyLinkedNode first = null;
            foreach (character character in characterlist)
            {
                if (first == null)
                {
                    first = new SinglyLinkedNode(character, null);
                }
                else
                {
                    if (character.initiative > first.character.initiative)
                    {
                        first = new SinglyLinkedNode(character, first);
                    }
                    else
                    {
                        first.insert(character);
                    }
                }
            }
            characterlist = new List<character>();
            first.getList(characterlist);
            InitiativeList.ItemsSource = characterlist;
        }

        void AdvanceTurn(object sender, RoutedEventArgs args)
        {
            character last = characterlist[0];
            characterlist.RemoveAt(0);
            characterlist.Add(last);
            characterlist.First().progress();
            turn++;
            round = (turn / characterlist.Count) + 1;
            refreshView();
        }

        void NewGroup(object sender, RoutedEventArgs args)
        {
            NewGroupDialog ngd = new NewGroupDialog(getNames);
            ngd.ShowDialog();


        }

        void SaveGroup(object sender, RoutedEventArgs args)
        {
            //Something with the playerlist
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName ="Group_name.tpk";
            if (sfd.ShowDialog() == true)
            {
                String output = "";
                foreach (player p in playerList){
                    output = output + p.name + ":" + p.health + '\n';
                    
                }
                output = output.TrimEnd('\n');
                File.WriteAllText(sfd.FileName, output);
            }
        }

        void loadGroup(object sender, RoutedEventArgs args)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                String file = File.ReadAllText(ofd.FileName);
                String[] characters = file.Split('\n');
                List<player> tempList = new List<player>();
                foreach(String c in characters)
                {
                    String[] cargs = c.Split(':');
                    tempList.Add(new player(cargs[0], int.Parse(cargs[1]), int.Parse(cargs[2])));
                }
                newEncounter.IsEnabled = true;
                playerList = tempList;
            }
        }

        void getNames(List<player> players)
        {
            playerList = players;
            newEncounter.IsEnabled = true;
            advanceTurn.IsEnabled = false;
        }
        void addEffect(object sender, RoutedEventArgs args)
        {
            Button button = sender as Button;
            character target = button.DataContext as character;

            NewEffect ne = new NewEffect(addEffect, target, characterlist.First(), characterlist);
            ne.ShowDialog();
        }

        void changeHealth(object sender, RoutedEventArgs args)
        {
            Button button = sender as Button;
            character target = button.DataContext as character;
            ChangeHealthDialog chd = new ChangeHealthDialog(target, refreshView);
            chd.ShowDialog();
        }

        private void NewEncounter(object sender, RoutedEventArgs e)
        {
            NewEncounterDialog ned = new NewEncounterDialog(playerList, getInitList);
            ned.ShowDialog();
        }

        void getInitList(List<character> characters)
        {
            turn = 0;
            characterlist = characters;
            orgCharList = new List<character>(characters);
            CharacterList.ItemsSource = orgCharList;
            advanceTurn.IsEnabled = true;
            statuslist = new List<Status>();
            StatusManipList.ItemsSource = statuslist;
            sortList();
            refreshView();
        }

        void addToCombat(object sender, RoutedEventArgs args)
        {
            character senderchar = (sender as Button).DataContext as character;
            bool exists = false;
            foreach (character ch in characterlist)
            {
                if (ch.name.Contains(senderchar.name))
                {
                    exists = true;
                    break;
                }
            }
            if (exists)
            {
                int i = 0;
                while (exists)
                {
                    i++;
                    exists = false;
                    foreach (character ch in characterlist)
                    {
                        if (ch.name.Contains(senderchar.name + i))
                        {
                            exists = true;
                        }
                    }
                }
                characterlist.Add(new character((senderchar.name + i), senderchar.initiative, senderchar.health, senderchar.armor));
            }
            else
            {
                characterlist.Add(new character(senderchar.name, senderchar.initiative, senderchar.health, senderchar.armor));
            }
            character first = characterlist.First();
            sortList();
            while (characterlist.First() != first)
            {
                turn--;
                AdvanceTurn(sender, args);
            }


            refreshView();
        }

        void removeFromCombat(object sender, RoutedEventArgs args)
        {
            character senderchar = (sender as Button).DataContext as character;
            characterlist.Remove(senderchar);
            refreshView();
        }

        void changeListView(object sender, RoutedEventArgs args)
        {
            if (!isInitializing)
            {
                ComboBox cb = sender as ComboBox;
                if (cb.SelectedIndex == 1)
                {
                    CharacterList.Visibility = Visibility.Collapsed;
                    StatusManipList.Visibility = Visibility.Visible;
                }
                else if (cb.SelectedIndex == 0)
                {
                    CharacterList.Visibility = Visibility.Visible;
                    StatusManipList.Visibility = Visibility.Collapsed;
                }
            }

        }

        

        void addEffect(string name, int duration, character target, character source)
        {
            Status newStatus = new Status(name, duration, target, source);
            target.inflictEffect(newStatus);
            source.causeEffect(newStatus);

            statuslist.Add(newStatus);
            refreshView();
        }
        void ModifyStatus(object sender, RoutedEventArgs args)
        {
            Status sendee = (sender as Button).DataContext as Status;
            ModEffect me = new ModEffect(sendee, characterlist, refreshView);
            me.Show();
        }

        void refreshView()
        {
            foreach (character character in characterlist)
            {
                character.refresh();
            }
            InitiativeList.Items.Refresh();
            CharacterList.Items.Refresh();
            StatusManipList.Items.Refresh();
            TurnaRound.Text = "Round: " + round;
        }
    }

    public class SinglyLinkedNode
    {
        public character character { get; set; }
        public SinglyLinkedNode node { get; set; }
        public SinglyLinkedNode(character _char, SinglyLinkedNode _node)
        {
            character = _char;
            node = _node;
        }
        character getNext()
        {
            if (node != null)
            {
                return node.character;
            }
            return null;
        }

        public void insert(character _char)
        {
            if (node != null)
            {
                if (_char.initiative > node.character.initiative)
                {
                    SinglyLinkedNode newnode = new SinglyLinkedNode(_char, node);
                    node = newnode;
                }
                else
                {
                    node.insert(_char);
                }
            }
            else
            {
                node = new SinglyLinkedNode(_char, node);
            }
        }
        public void getList(List<character> characterlist)
        {
            characterlist.Add(character);
            if (node != null)
            {
                node.getList(characterlist);
            }
        }
    }

    public class character
    {
        public string name { get; set; }
        public int initiative { get; set; }
        public int health { get; set; }
        public int armor { get; set; }

        public string status { get; set; }

        private List<Status> statusList;
        private List<Status> causeList;
        public character(string _name, int _initiative, int _health, int _armor)
        {
            name = _name;
            initiative = _initiative;
            health = _health;
            status = "";
            armor = _armor;
            statusList = new List<Status>();
            causeList = new List<Status>();
        }


        public void inflictEffect(Status _status)
        {
            statusList.Add(_status);
        }

        public void causeEffect(Status _status)
        {
            causeList.Add(_status);
        }

        public void removeInflict(Status effect)
        {
            statusList.Remove(effect);
        }
        public void removeCause(Status effect)
        {
            causeList.Remove(effect);
        }
        public void progress()
        {
            foreach (Status stat in causeList)
            {
                stat.progress();
            }


        }
        public void refresh()
        {
            status = "";
            foreach (Status stat in statusList)
            {
                if (stat.duration > 0)
                {
                    status += ", " + stat.name + ": " + stat.duration + " turns";
                }
            }
            try
            {
                status = status.Substring(2);
            }
            catch
            {
                status = "";
            }
        }
        public override string ToString()
        {
            return "Name: " + name + " Health: " + health + " Initiative: " + initiative + " Armor: " + armor;
        }
    }
    public class Status
    {
        public string name { get; set; }
        public int duration { get; set; }
        public character source { get; set; }
        public character target { get; set; }
        public Status(string _name, int _duration, character _target, character _source)
        {
            name = _name;
            duration = _duration;
            source = _source;
            target = _target;
        }
        public void progress()
        {
            duration = duration - 1;
        }
        public void update(string _name, int _duration, character _target, character _source)
        {
            name = _name;
            duration = _duration;
            target.removeInflict(this);
            _target.inflictEffect(this);
            target = _target;

            source.removeCause(this);
            _source.causeEffect(this);
            source = _source;
        }
    }
}
