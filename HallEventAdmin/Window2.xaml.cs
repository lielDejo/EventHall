using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WebApiClient;
using EventModels;
using System.Windows.Documents;
using System;
using System.IO;
using System.Net;

namespace HallEventAdmin
{
    public partial class Window2 : Window
    {
        public ObservableCollection<EventHall> EventHalls { get; set; }
        private string idUser;
        private Window mainWindow;

        public Window2(Window mainW, string iduser, string userName)
        {
            mainWindow = mainW;
            InitializeComponent();
            idUser = iduser;
            UserNameTextBlock.Text = userName;
            EventHalls = new ObservableCollection<EventHall>(); // אתחול של רשימה
            LoadEventHallsFromServer();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // לדוגמה: לשאול אם המשתמש בטוח
            MessageBoxResult result = MessageBox.Show("האם אתה בטוח שברצונך לצאת?", "אישור יציאה", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // מונע סגירה
            }

            mainWindow.Close();
        }

        private async void LoadEventHallsFromServer()
        {
            WebClient<List<EventHall>> webClient = new WebClient<List<EventHall>>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localhost";
            webClient.Path = "api/Manager/GetHallsByAdmin";
            webClient.AddParam("manageId", idUser);

            try
            {
                List<EventHall> halls = await webClient.Get();
                if (halls != null)
                {
                    EventHalls.Clear(); // נוודא שהרשימה ריקה לפני הוספת נתונים חדשים
                    foreach (var hall in halls)
                    {
                        WebClient<List<Image_>> webClient2 = new WebClient<List<Image_>>();
                        webClient2.Schema = "http";
                        webClient2.Port = 5232;
                        webClient2.Host = "localhost";
                        webClient2.Path = "api/ragister/GetImagesOfHall";
                        webClient2.AddParam("Id", hall.Id);

                        hall.HallImage = await webClient2.Get();
                        hall.OwnerId = idUser;
                        EventHalls.Add(hall);
                    }
                    EventHallsDataGrid.ItemsSource = EventHalls;
                }
                else
                {
                    MessageBox.Show("לא נמצאו אולמות למשתמש זה.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בטעינת הנתונים מהשרת: {ex.Message}");
            }
        }

        private void AddNewHall_Click(object sender, RoutedEventArgs e)
        {
            Window3 window3 = new Window3(mainWindow, idUser, UserNameTextBlock.Text);
            window3.Show();
            this.Hide();
        }
        private void viewListOfMeets_Click(object sender, RoutedEventArgs e)
        {
            Window4 window4 = new Window4(mainWindow, idUser, UserNameTextBlock.Text);
            window4.Show();
            this.Hide();
        }

        private async void EditHall_Click(object sender, RoutedEventArgs e)
        {
            EventHallsDataGrid.CommitEdit(DataGridEditingUnit.Row, true);

            if (sender is Button btn && btn.DataContext is EventHall hall)
            {

                WebClient<EventHall> webClient = new WebClient<EventHall>();
                webClient.Schema = "http";
                webClient.Port = 5232;
                webClient.Host = "localhost";
                webClient.Path = "api/Manager/UpDateHall";

                try
                {
                    bool isWork = await webClient.Post(hall);
                    if (isWork)
                    {
                        MessageBox.Show($"האולם '{hall.HallName}' עודכן בהצלחה.");
                    }
                    else { throw new Exception(); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"שגיאה בעדכון האולם: {ex.Message}");
                }
            }
        }

        private async void DeleteHall_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is EventHall selectedHall)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"האם אתה בטוח שברצונך למחוק את האולם '{selectedHall.HallName}'?",
                    "אישור מחיקה",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    //פה אפשר לשלוח בקשת DELETE לשרת
                    WebClient<EventHall> webClient = new WebClient<EventHall>();
                    webClient.Schema = "http";
                    webClient.Port = 5232;
                    webClient.Host = "localhost";
                    webClient.Path = "api/Manager/DeleteHall";
                    bool isWork = await webClient.Post(selectedHall);
                    if (isWork)
                    {
                        EventHalls.Remove(selectedHall);
                        MessageBox.Show($"האולם '{selectedHall.HallName}' נמחק .");
                    }
                    else
                        MessageBox.Show($"שגיאה במחיקת האולם");
                }
            }
        }
        private async void logOut_Click(object sender, RoutedEventArgs e)
        {
            Window1 mainWindow = new Window1();
            mainWindow.Show();
            this.Close();
        }
        private async void AddImageToHall_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is EventHall currentHall)
            {
                var dlg = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                    Multiselect = true
                };

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    List<FileStream> streams = new List<FileStream>();
                    List<Image_> images = new List<Image_>();

                    try
                    {
                        foreach (var filename in dlg.FileNames)
                        {
                            FileInfo imageFile = new FileInfo(filename);
                            var newImage = new Image_
                            {
                                ImageName = imageFile.Name, // לדוגמה: hall1.jpg
                                ImageAddress = filename,    // הנתיב המלא
                                HallId = currentHall.Id
                            };

                            currentHall.HallImage.Add(newImage);
                            images.Add(newImage);

                            // פותח את הקובץ לקריאה
                            var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                            streams.Add(stream);
                        }
                        
                        WebClient<List<Image_>> webClient = new WebClient<List<Image_>>();
                        webClient.Schema = "http";
                        webClient.Port = 5232;
                        webClient.Host = "localhost";
                        webClient.Path = "api/Manager/addImages";
                        bool isWork = await webClient.Post(images, streams);
                        if (isWork)
                        {
                            MessageBox.Show("האולם עודכן בהצלחה!\n\n");
                            EventHallsDataGrid.Items.Refresh();
                        }
                        else
                        { MessageBox.Show("שגיאה בהוספת תמונה!\n\n"); }
                        
                    }
                    finally
                    {
                        // סגירת כל הזרמים
                        foreach (var stream in streams)
                        {
                            stream.Close();
                        }
                    }
                }
            }
        }



    }
}
