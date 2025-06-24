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
using WebApiClient;


namespace HallEventAdmin
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // אם הטקסט הוא "Username" אז נמחק אותו
            if (UsernameTextBox.Text == "Username")
            {
                UsernameTextBox.Text = "";
                UsernameTextBox.Foreground = System.Windows.Media.Brushes.Black; // הופכים את הצבע לשחור
            }
        }

        // פונקציה כאשר ה-TextBox מאבד פוקוס
        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // אם הטקסט ריק, מחזירים את הטקסט "Username" בצבע אפור
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                UsernameTextBox.Text = "Username";
                UsernameTextBox.Foreground = System.Windows.Media.Brushes.Gray; // צבע אפור
            }
        }

        // פונקציה ללחיצה על כפתור ההתחברות
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            WebClient<string> webClient = new WebClient<string>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localHost";
            webClient.Path = "api/Guest/LoginAdmin"; ///לבדוק
            webClient.AddParam("userName", username);
            webClient.AddParam("password", password);


            string iduser = await webClient.Get();
            if (iduser != "")
            {
                Window2 window2 = new Window2(this, iduser, username);
                window2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("שם משתמש או סיסמה אינם נכונים.");
            }
        }
        // פונקציה שמבצעת חיפוש במסד נתונים
        private bool CheckUserCredentials(string username, string password)
        {
            // קוד לחיבור למסד נתונים והחזרת True אם יש התאמה, אחרת False
            return username == "admin" && password == "1234"; // רק לדוגמה! לשאול את אלכס
        }
        // פונקציה לסגירת החלון
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // סגירת החלון
        }
    }
}
