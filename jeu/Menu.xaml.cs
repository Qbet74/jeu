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

namespace jeu
{
    /// <summary>
    /// Logique d'interaction pour Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public string niveauDifficulte;

        public Menu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(niveauDifficulte))
            {
                MessageBox.Show("Sélectionnez une difficulté.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.SetNiveauDifficulte(niveauDifficulte);
                this.DialogResult = true;
                this.Close();
  
            }
        }

        private void BoutonFacile_Click(object sender, RoutedEventArgs e)
        {
            niveauDifficulte = "Facile";
        }

        private void BoutonNormal_Click(object sender, RoutedEventArgs e)
        {
            niveauDifficulte = "Normal";
        }

        private void BoutonDifficile_Click(object sender, RoutedEventArgs e)
        {
            niveauDifficulte = "Difficile";
        }
    }
}
