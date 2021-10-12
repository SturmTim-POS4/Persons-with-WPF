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

namespace Persons_with_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<Person> people = new();

        private void DeleteSection(object sender, RoutedEventArgs e)
        {
            if (People.SelectedItem != null)
            {
                people.RemoveAt(People.SelectedIndex);
                People.Items.RemoveAt(People.SelectedIndex);
            }
        }

        private void AddPerson(object sender, RoutedEventArgs e)
        {
            string firstName = app_Firstname.Text;
            string lastName = app_Lastname.Text;

            DateTime birthday = app_Birthday.DisplayDate;
            bool isMale = Male.IsChecked is true;
            bool driversLicence = app_HasDriversLicence.IsChecked is true;

            people.Add(new Person(firstName, lastName, birthday, isMale, driversLicence));
            People.Items.Add(new Person(firstName, lastName, birthday, isMale, driversLicence));
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "Open CSV File",
                Filter = "CSV files|*.csv",
                InitialDirectory = @"C:\"
            };

            List<Person> tempPeople = new();
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                tempPeople.AddRange(File.ReadAllText(openFileDialog.FileName).Split("\n").Where(item => !item.Equals("")).Select(item => Person.Parse(item)));
                people.AddRange(tempPeople);
                People.Items.Clear();
                foreach(Person person in tempPeople)
                {
                    People.Items.Add(person);
                }
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Title = "Save CSV File",
                Filter = "CSV files|*.csv",
                InitialDirectory = @"C:\"
            };

            if (saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                StringBuilder csvString = new();
                foreach (Person person in people)
                {
                    csvString.Append($"{ person.ToCsvString()}\n");
                }
                File.WriteAllText(saveFileDialog.FileName, csvString.ToString());
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void People_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (People.SelectedItem != null)
            {
                Person tempPerson = people[People.SelectedIndex];
                app_Firstname.Text = tempPerson.FirstName;
                app_Lastname.Text = tempPerson.LastName;
                app_Birthday.SelectedDate = tempPerson.Birthdate;
                Male.IsChecked = tempPerson.IsMale;
                Female.IsChecked = !tempPerson.IsMale;
                app_HasDriversLicence.IsChecked = tempPerson.HasDriversLicence;
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AutoFill != null)
            {
                activateSearch();

            }
        }

        private void filter_fName_Click(object sender, RoutedEventArgs e)
        {
            if (AutoFill != null)
            {
                activateSearch();

            }
        }

        private void activateSearch()
        {
            AutoFill.Items.Clear();
            if (!Search.Text.ToString().Equals(""))
            {

                Dictionary<string, List<Person>> searchFor = new Dictionary<string, List<Person>>();
                searchFor.Add($"{Search.Text.ToString()}", people);
                if (filter_fName.IsChecked is true)
                {
                    List<Person> filtered = searchFor.SelectMany(item => item.Value).Where(item => item.FirstName.StartsWith(searchFor.Keys.ToList()[0])).ToList();
                    List<string> names = new();
                    foreach (Person person in filtered)
                    {
                        if (!names.Contains(person.FirstName))
                        {
                            AutoFill.Items.Add($"{person.FirstName} ({searchFor.SelectMany(item => item.Value).Where(item => item.FirstName.Equals(person.FirstName)).ToList().Count})");
                            names.Add(person.FirstName);
                        }
                    }
                }
                else
                {
                    List<Person> filltered = searchFor.SelectMany(item => item.Value).Where(item => item.LastName.StartsWith(searchFor.Keys.ToList()[0])).ToList();
                    List<string> names = new();
                    foreach (Person person in filltered)
                    {
                        if (!names.Contains(person.LastName))
                        {
                            AutoFill.Items.Add($"{person.LastName} ({searchFor.SelectMany(item => item.Value).Where(item => item.LastName.Equals(person.LastName)).ToList().Count})");
                            names.Add(person.LastName);
                        }
                    }
                }
            }
        }
    }
}
