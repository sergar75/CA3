using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
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
//using TicTacToeClient;
using TicTacToeLibrary;

namespace WA1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Свойство для изображения игрока
        public string ImageSource { get; set; }

        // Игровое поле 'X' или 'O', или пустая клетка
        public static char[] board = new char[9];
        public Button[] buttons; 

        public char Symbol { get; set; }
        public Client client;

        public MainWindow()
        {
            InitializeComponent();
          
            client = new Client("192.168.0.115",5555);
            client.Play();
            Symbol = client.Symbol;
            // Получаем от клиентской части крести или нолик играем
            ImageSource = Symbol == 'X' ? "images/krest.png" : "images/nol.png";
            board = client.GetBoard();
            buttons = new Button[] { Button0_0, Button0_1, Button0_2,
                                     Button1_0, Button1_1, Button1_2,
                                     Button2_0, Button2_1, Button2_2};

        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            //    button.Content = user.GetImage();


            if (button.Content == null)
            {
                // рисуем

                button.Content = GetImage();
                button.IsEnabled = false;

                int row = int.Parse(button.Name.Substring(button.Name.Length - 3, 1));
                int column = int.Parse(button.Name.Substring(button.Name.Length - 1, 1));


                // отправляем индекс клиентской части
             client.SendIndex(button.TabIndex);

            }
            board = client.GetBoard();
            //ShowBoard();

        }
        public Image GetImage()
        {
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(ImageSource, UriKind.Relative));
            return img;
        }

        public void ShowBoard()
        {
            for (int i = 0; i<8; i++) {
                    switch (board[i])
                    {
                        case 'x' : 
                            buttons[i].IsEnabled = false;
                        break;
                        case 'o': ImageSource = "images/nol.png";
                                  buttons[i].IsEnabled = false;
                        break;
                        case '-': ImageSource = null;
                            break;
                    }
                buttons[i].Content = GetImage();

            }

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            board = client.GetBoard();
        }
    }
}
