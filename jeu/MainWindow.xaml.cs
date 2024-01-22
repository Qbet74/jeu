using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace jeu
{
    public partial class MainWindow : Window
    {
        private bool menuDebut = true;
        // mettre pause  

        private bool Estenpause = false;
        // liste des éléments rectangles  

        private List<Rectangle> itemsToRemove = new List<Rectangle>();

        // crée une nouvelle instance de la classe dispatch timer  

        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private ImageBrush skinjoueur = new ImageBrush();

        private ImageBrush policeskin = new ImageBrush();

        private ImageBrush pieceskin = new ImageBrush();

        // vitesse par défaut de l'ennemi  

        private int vitessepolice = 2;

        // vitesse du joueur  

        private int VitesseJoueur = 2;

        private ImageBrush map = new ImageBrush();

        private bool goLeft, goRight, goUp, goDown = false;
        // booléens pour aller à gauche et à droite  et en haut et en bas
        private int Vie = 5;
        // variable des vie
        private int compteur = 0;
        //indicateur des points
        readonly int MORT = 0;
        // indicateur de mort
        readonly int PENALITEVIE = 1;
        // Indicateur de la penaliter de degat
        bool testpolice = true;
        // test du deplacement police
        bool testpolice2 = true;
        // test du deplacement police
        bool testpolice3 = true;
        // test du deplacement police
        bool testpolice4 = true;
        // test du deplacement police
        bool testpolice5 = true;
        // test du deplacement police


        public MainWindow()
        {
            InitializeComponent();

            Menu menu = new Menu();
            menu.ShowDialog();

            myCanvas.Focus();
            // lie le timer du répartiteur à un événement appelé moteur de jeu gameengine  
            dispatcherTimer.Tick += MoteurdeJeu;
            // rafraissement toutes les 16 milliseconds  
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(16);
            // lancement du timer  
            dispatcherTimer.Start();
            // chargement de l’image du joueur   
            skinjoueur.ImageSource = new BitmapImage(new
            Uri(AppDomain.CurrentDomain.BaseDirectory + "Image/Voiture.png"));
            // assignement de skin du joueur au rectangle associé  
            joueur.Fill = skinjoueur;
            map.ImageSource = new BitmapImage(new
            Uri(AppDomain.CurrentDomain.BaseDirectory + "Image/ville.jpg"));
            // assignement de skin du joueur au rectangle associé  
            ville.Fill = map;
            policeskin.ImageSource = new BitmapImage(new
             Uri(AppDomain.CurrentDomain.BaseDirectory + "Image/police1.png"));
            // assignement de skin de police au rectangle associé  
            police.Fill = policeskin;
            police2.Fill = policeskin;
            police3.Fill = policeskin;
            police4.Fill = policeskin;
            police5.Fill = policeskin;

            police_decoration2.Fill = policeskin;
            police_decoration1.Fill = policeskin;

            pieceskin.ImageSource = new BitmapImage(new
                Uri(AppDomain.CurrentDomain.BaseDirectory + "Image/piece.png"));
            // assignement de skin des piece au rectangle associé  
            Piece1.Fill = pieceskin;
            Piece2.Fill = pieceskin;
            Piece3.Fill = pieceskin;
            Piece4.Fill = pieceskin;
            Piece5.Fill = pieceskin;
            Piece6.Fill = pieceskin;
            Piece7.Fill = pieceskin;
            Piece8.Fill = pieceskin;
            Piece9.Fill = pieceskin;
            Piece10.Fill = pieceskin;

        }


        internal void SetNiveauDifficulte(string niveauDifficulte)
        {
            switch (niveauDifficulte)
            {
                case "Facile":
                    vitessepolice = 2;
                    break;

                case "Normal":
                    vitessepolice = 4;
                    break;

                case "Difficile":
                    vitessepolice = 6;
                    break;
            }
        }
        private void Colision(Rect joueur)
        {
            foreach (var colision in myCanvas.Children.OfType<Rectangle>().Where(r => r.Name.StartsWith("colision")))
            {
                Rect colisionRect = new Rect(Canvas.GetLeft(colision), Canvas.GetTop(colision), colision.Width, colision.Height);

                if (joueur.IntersectsWith(colisionRect))
                {
                    if (Vie > MORT)
                    {
                        Pertevie();
                    }
                    else
                    {
                        dispatcherTimer.Stop();
                        MessageBox.Show("Perdu", "Collision avec un obstacle", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
            foreach (var police in myCanvas.Children.OfType<Rectangle>().Where(r => r.Name.StartsWith("police")))
            {
                Rect colisionRectpolice = new Rect(Canvas.GetLeft(police), Canvas.GetTop(police), police.Width, police.Height);
                if (joueur.IntersectsWith(colisionRectpolice))
                {
                    if (Vie > MORT)
                    {
                        Pertevie();
                    }
                    else
                    {
                        dispatcherTimer.Stop();
                        MessageBox.Show("Perdu", "Collision avec un obstacle", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }

            }
            foreach (var piece in myCanvas.Children.OfType<Rectangle>().Where(r => r.Name.StartsWith("Piece")).ToList())
            {
                Rect colisionpiece = new Rect(Canvas.GetLeft(piece), Canvas.GetTop(piece), piece.Width, piece.Height);
                if (joueur.IntersectsWith(colisionpiece) && compteur >= 9)
                {
                    dispatcherTimer.Stop();
                    MessageBox.Show("Gagner", "Vous avez recuperer toute les pieces", MessageBoxButton.OK, MessageBoxImage.None);

                }
                else
                {
                    if (joueur.IntersectsWith(colisionpiece))
                    {
                        compteur = compteur + 1;
                        myCanvas.Children.Remove(piece);

                    }
                }
            }

        }

        private void Pertevie()
        {
            int[] coordonneex = { 40, 690, 680, 60 };
            int[] coordonneey = { 690, 690, 30, 40 };
            Random aleatoire = new Random();
            int pointreaparetion = aleatoire.Next(0, 3);
            Vie = Vie - PENALITEVIE;
            Canvas.SetLeft(joueur, coordonneex[pointreaparetion]);
            Canvas.SetTop(joueur, coordonneey[pointreaparetion]);
        }
        private void CanvasKeyIsDown(object sender, KeyEventArgs e)
        {
            // on gère les booléens gauche et droite en fonction de l’appui de la touche  
            if (e.Key == Key.Left)
            {
                goLeft = true;
            }
            if (e.Key == Key.Right)
            {
                goRight = true;
            }
            if (e.Key == Key.Up)
            {
                goUp = true;
            }
            if (e.Key == Key.Down)
            {
                goDown = true;
            }
            if (e.Key == Key.P || e.Key == Key.Escape)
            {
                DeclanchePause();
            }
            if(e.Key == Key.Tab)
            {
                 dispatcherTimer.Stop();
                    MessageBox.Show("Gagner", "Vous avez recuperer toute les pieces", MessageBoxButton.OK, MessageBoxImage.None);

            }
        }


        private void CanvasKeyIsUp(object sender, KeyEventArgs e)
        {
            // on gère les booléens gauche et droite en fonction du relâchement de la touche  
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }

            if (e.Key == Key.Right)
            {
                goRight = false;
            }

            if (e.Key == Key.Up)
            {
                goUp = false;
            }
            if (e.Key == Key.Down)
            {
                goDown = false;
            }
        }

        private void Mouvementjoueur()
        {
            // déplacement à gauche et droite de vitessePlayer avec vérification des limites de fenêtre gauche et droite  
            if (goRight && Canvas.GetLeft(joueur) + joueur.Width < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) + VitesseJoueur);

            }
            if (goLeft && Canvas.GetLeft(joueur) > 0)
            {
                Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) - VitesseJoueur);
            }
            if (goUp && Canvas.GetTop(joueur) + joueur.Height < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(joueur, Canvas.GetTop(joueur) - VitesseJoueur);
            }
            if (goDown && Canvas.GetTop(joueur) > 0)
            {
                Canvas.SetTop(joueur, Canvas.GetTop(joueur) + VitesseJoueur);
            }
            if (goRight == true)
            {
                RotateTransform rotateTransform1 =
                new RotateTransform(0);
                joueur.RenderTransform = rotateTransform1;
            }
            if (goLeft == true)
            {
                RotateTransform rotateTransform1 =
                 new RotateTransform(180);
                joueur.RenderTransform = rotateTransform1;
            }
            if (goUp == true)
            {
                RotateTransform rotateTransform1 =
                new RotateTransform(270);
                joueur.RenderTransform = rotateTransform1;
            }
            if (goDown == true)
            {
                RotateTransform rotateTransform1 =
                new RotateTransform(90);
                joueur.RenderTransform = rotateTransform1;
            }


        }
        private void MouvementPolice()
        {



            if (Canvas.GetTop(police) < 230 && testpolice == true)
            {
                Canvas.SetTop(police, Canvas.GetTop(police) + vitessepolice);
                RotateTransform rotateTransform1 =
                new RotateTransform(0);
                police.RenderTransform = rotateTransform1;
            }
            else
            {
                Canvas.SetTop(police, Canvas.GetTop(police) - vitessepolice);
                RotateTransform rotateTransform1 =
                new RotateTransform(180);
                police.RenderTransform = rotateTransform1;
                if (Canvas.GetTop(police) > 40)
                {
                    testpolice = false;
                }
                else
                {
                    testpolice = true;
                }

            }
            if (Canvas.GetTop(police2) < 230 && testpolice2 == true)
            {
                Canvas.SetTop(police2, Canvas.GetTop(police2) + vitessepolice);
                RotateTransform rotateTransform1 =
               new RotateTransform(0);
                police2.RenderTransform = rotateTransform1;
            }
            else
            {

                Canvas.SetTop(police2, Canvas.GetTop(police2) - vitessepolice);
                RotateTransform rotateTransform1 =
               new RotateTransform(180);
                police2.RenderTransform = rotateTransform1;
                if (Canvas.GetTop(police2) > 40)
                {
                    testpolice2 = false;
                }
                else
                {
                    testpolice2 = true;
                }
            }
            if (Canvas.GetTop(police3) < 605 && testpolice3 == true)
            {
                Canvas.SetTop(police3, Canvas.GetTop(police3) + vitessepolice);
                RotateTransform rotateTransform1 =
               new RotateTransform(0);
                police3.RenderTransform = rotateTransform1;
            }
            else
            {

                Canvas.SetTop(police3, Canvas.GetTop(police3) - vitessepolice);
                RotateTransform rotateTransform1 =
               new RotateTransform(180);
                police3.RenderTransform = rotateTransform1;
                if (Canvas.GetTop(police3) > 445)
                {
                    testpolice3 = false;
                }
                else
                {
                    testpolice3 = true;
                }
            }
            if (Canvas.GetTop(police4) > 417 && testpolice4 == true)
            {
                Canvas.SetTop(police4, Canvas.GetTop(police4) - vitessepolice);
                RotateTransform rotateTransform1 =
               new RotateTransform(180);
                police4.RenderTransform = rotateTransform1;
            }
            else
            {

                Canvas.SetTop(police4, Canvas.GetTop(police4) + vitessepolice);
                RotateTransform rotateTransform1 =
               new RotateTransform(0);
                police4.RenderTransform = rotateTransform1;
                if (Canvas.GetTop(police4) < 626)
                {
                    testpolice4 = false;
                }
                else
                {
                    testpolice4 = true;
                }
            }
            //261 27
            if (Canvas.GetLeft(police5) < 260 && testpolice5 == true)
            {
                Canvas.SetLeft(police5, Canvas.GetLeft(police5) + vitessepolice);
                RotateTransform rotateTransform1 =
               new RotateTransform(270);
                police5.RenderTransform = rotateTransform1;
            }
            else
            {

                Canvas.SetLeft(police5, Canvas.GetLeft(police5) - vitessepolice);
                RotateTransform rotateTransform1 =
               new RotateTransform(90);
                police5.RenderTransform = rotateTransform1;
                if (Canvas.GetLeft(police5) > 30)
                {
                    testpolice5 = false;
                }
                else
                {
                    testpolice5 = true;
                }
            }


        }
        private void DeclanchePause()

        {
            Estenpause = !Estenpause;
            // Affiche ou masque le menu pause en fonction de l'état de pause 
            pauseGrid.Visibility = Estenpause ? Visibility.Visible : Visibility.Collapsed;
            // Met en pause ou reprend le DispatcherTimer en fonction de l'état de pause 
            if (Estenpause)
            {
                dispatcherTimer.Stop();
            }
            else
            {
                dispatcherTimer.Start();
            }
        }



        private void MoteurdeJeu(object sender, EventArgs e)
        {
            // Create a rectangle for the joueur for collision detection 
            Rect joueurRect = new Rect(Canvas.GetLeft(joueur), Canvas.GetTop(joueur), joueur.Width, joueur.Height);
            Mouvementjoueur();
            MouvementPolice();
            Colision(joueurRect);

        }
    }
}