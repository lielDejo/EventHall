using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WebApiClient;
using EventModels;
using System;
using System.Collections.Generic;

namespace HallEventAdmin
{
    public class MeetingInfo
    {
        public string Id { get; set; }
        public string HallName { get; set; }         // שם האולם
        public string UserFullName { get; set; }     // שם המשתמש
        public string DateMeet { get; set; }
        public string Hour { get; set; }
        public string ReasonMeet { get; set; }
        public string MeetingSummary { get; set; }
    }

    public partial class Window4 : Window
    {
        public ObservableCollection<MeetingInfo> Meetings { get; set; }
        private string adminId;
        private Window mainWindow;
        public Window4(Window mainW, string idUser, string userName)
        {
            mainWindow = mainW;
            InitializeComponent();
            adminId = idUser;
            UserNameTextBlock.Text = userName;
            Meetings = new ObservableCollection<MeetingInfo>();
            LoadMeetingsFromServer();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.Close();
        }

        private async void LoadMeetingsFromServer()
        {
            WebClient<List<Meet>> webClient = new WebClient<List<Meet>>();
            webClient.Schema = "http";
            webClient.Port = 5232;
            webClient.Host = "localhost";
            webClient.Path = "api/Manager/GetListOfMeets";
            webClient.AddParam("hallID", adminId);
            
            try
            {
                List<Meet> meetingsFromServer = await webClient.Get();
                if (meetingsFromServer != null)
                {
                    Meetings.Clear();
                    string hallName = "";
                    string userName = "";
                    foreach (Meet meeting in meetingsFromServer)
                    {
                        MeetingInfo newMeet = new MeetingInfo();
                        newMeet.Id = meeting.Id;
                        newMeet.DateMeet = meeting.DateMeet;
                        newMeet.ReasonMeet = meeting.ReasonMeet;
                        newMeet.Hour = meeting.Hour;
                        newMeet.MeetingSummary = meeting.MeetingSummary;
                        WebClient<string> webClient1 = new WebClient<string>();
                        webClient1.Schema = "http";
                        webClient1.Port = 5232;
                        webClient1.Host = "localhost";
                        webClient1.Path = "api/ragister/getNameHallById";
                        webClient1.AddParam("id", meeting.HallId);
                        hallName = await webClient1.Get();

                        WebClient<string> webClient2 = new WebClient<string>();
                        webClient2.Schema = "http";
                        webClient2.Port = 5232;
                        webClient2.Host = "localhost";
                        webClient2.Path = "api/Manager/getNameUserbyId";
                        webClient2.AddParam("id", meeting.UserId);
                        userName = await webClient2.Get();
                       
                        newMeet.UserFullName = userName;
                        newMeet.HallName = hallName;

                        Meetings.Add(newMeet);
                    }
                    MeetingsDataGrid.ItemsSource = Meetings;
                }
                else
                {
                    MessageBox.Show("לא נמצאו פגישות עבור מנהל זה.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בטעינת הפגישות מהשרת: {ex.Message}");
            }
        }

        private void futureMeets_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Meetings != null)
                {
                    var emptyMeetings = new List<MeetingInfo>();
                    foreach (var meeting in Meetings)
                    {
                        if (!string.IsNullOrWhiteSpace(meeting.MeetingSummary))
                        {
                            emptyMeetings.Add(meeting);
                        }
                    }

                    foreach (var meeting in emptyMeetings)
                    {
                        Meetings.Remove(meeting);
                    }

                    MeetingsDataGrid.ItemsSource = Meetings;
                }
                else
                {
                    MessageBox.Show("לא נמצאו פגישות עתידיות עבור מנהל זה.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בטעינת הפגישות העתידיות: {ex.Message}");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // חזרה לחלון הראשי או הקודם
            Window2 back = new Window2(mainWindow, adminId, UserNameTextBlock.Text);
            back.Show();
            this.Close();
        }
        private void allMeets_Click(object sender, RoutedEventArgs e)
        {
            LoadMeetingsFromServer();
        }

        private async void EditMeet_Click(object sender, RoutedEventArgs e)
        {
            MeetingsDataGrid.CommitEdit(DataGridEditingUnit.Row, true);

            if (sender is Button btn && btn.DataContext is MeetingInfo meet_)
            {
                Meet meet = new Meet();
                meet.DateMeet = meet_.DateMeet;
                meet.ReasonMeet = meet_.ReasonMeet;
                meet.Hour = meet_.Hour;
                meet.MeetingSummary = meet_.MeetingSummary;
                meet.Id = meet_.Id;
                meet.UserId = "0";
                meet.HallId = "0";
                WebClient<Meet> webClient = new WebClient<Meet>();
                webClient.Schema = "http";
                webClient.Port = 5232;
                webClient.Host = "localhost";
                webClient.Path = "api/Manager/UpDateMeet";

                try
                {
                    bool isWork = await webClient.Post(meet);
                    if (isWork)
                    {
                        MessageBox.Show($"הפגישה עודכנה בהצלחה.");
                    }
                    else { throw new Exception(); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"שגיאה בעדכון הפגישה: {ex.Message}");
                }
            }
        }

        private async void DeleteMeet_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is MeetingInfo meet_)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"האם אתה בטוח שברצונך למחוק את הפגישה '{meet_.ReasonMeet}'?",
                    "אישור מחיקה",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    //פה אפשר לשלוח בקשת DELETE לשרת
                    Meet meet = new Meet();
                    meet.DateMeet = " ";
                    meet.ReasonMeet = " ";
                    meet.Hour = " ";
                    meet.MeetingSummary = " ";
                    meet.Id = meet_.Id;
                    meet.UserId = " ";
                    meet.HallId = " ";
                    WebClient<Meet> webClient = new WebClient<Meet>();
                    webClient.Schema = "http";
                    webClient.Port = 5232;
                    webClient.Host = "localhost";
                    webClient.Path = "api/Manager/DelMeet";
                    bool isWork = await webClient.Post(meet);

                    if (isWork)
                    {
                        // מציאת הפגישה המתאימה מתוך הרשימה לפי שעה ותאריך (או קריטריונים אחרים)
                        var meetingToRemove = Meetings.FirstOrDefault(m =>
                            m.DateMeet == meet_.DateMeet &&
                            m.Hour == meet_.Hour &&
                            m.ReasonMeet == meet_.ReasonMeet);

                        if (meetingToRemove != null)
                        {
                            Meetings.Remove(meetingToRemove);
                        }

                        MessageBox.Show($"הפגישה נמחקה .");
                    }
                    else
                        MessageBox.Show($"שגיאה במחיקת האולם");
                }
            }
        }
    }
}
