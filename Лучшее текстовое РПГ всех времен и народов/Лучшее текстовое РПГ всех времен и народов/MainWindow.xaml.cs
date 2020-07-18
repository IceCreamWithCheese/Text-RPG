using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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



namespace Лучшее_текстовое_РПГ_всех_времен_и_народов
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /* Объявление ссылок на элементы управления */
        public Label Label_name;
        public Label Label_movies;
        public Label Label_points;


        public static List<Savespot> savespots = new List<Savespot>();
        public static int search_in_saves(int number)
        {
            for(int i=0;i<savespots.Count;i++)
            {
                if(savespots[i].search_by_number(number))
                {
                    return i;
                }
            }
            return -4;
        }


        /* КОНФИГУРАЦИЯ */
        static string save_path = AppDomain.CurrentDomain.BaseDirectory + @"\SaveDirectory";
        public static string menu_file = save_path + @"\menu" + ".json";
        public string filepath;
        public static int spots_count = 3;




        private void main_Loaded(object sender, RoutedEventArgs e)
        {           


            if (!Directory.Exists(save_path))
            {
                Directory.CreateDirectory(save_path);
                
            }
            if (!File.Exists(menu_file))
            {
                File.Create(menu_file).Close();
                /*
                using (StreamWriter writer = new StreamWriter(menu_file))
                {
                    writer.WriteLine("Не открыта__");
                    writer.WriteLine("Не открыта__");
                    writer.WriteLine("Не открыта__");
                }*/
                
                


            }


            else
            {

                

                string s = "";
                using (StreamReader sr = File.OpenText(menu_file))
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        Savespot save = JsonConvert.DeserializeObject<Savespot>(s);
                        savespots.Add(save);
                    }
                        
                }
                for(int i=1; i<=spots_count;i++)
                {
                    Label_name = FindName("name_" + i) as Label;
                    Label_name.Content = "Пустой слот";

                    Label_movies = FindName("movies_" + i) as Label;
                    Label_movies.Content = "";

                    Label_points = FindName("points_" + i) as Label;
                    Label_points.Content = "";

                    for (int j = 0; j < savespots.Count; j++)
                    {

                        if (i == savespots[j].number)
                        {
                            Label_name = FindName("name_" + savespots[j].number) as Label;
                            Label_name.Content = savespots[j].name;

                            Label_movies = FindName("movies_" + savespots[j].number) as Label;
                            Label_movies.Content = savespots[j].movies;

                            Label_points = FindName("points_" + savespots[j].number) as Label;
                            Label_points.Content = savespots[j].points;
                            break;
                        }                        
                    }
                }
                

                /*
                string[,] slots = new string[spots_count,3];
                var s1 = File.ReadAllText(menu_file).Split('\n');

                for(int i=0;i<spots_count;i++)
                {
                    var str = s1[i].Split('_');
                    for(int j=0;j<spots_count;j++)
                    {
                        slots[i, j] = str[j];
                    }
                }

                
                for(int i=1;i<=spots_count;i++)
                {
                    Label_name = FindName("name_" + i) as Label;
                    Label_name.Content = slots[i, 0];

                    Label_movies = FindName("movies_" + i) as Label;
                    Label_movies.Content = slots[i, 1];

                    Label_points = FindName("points_" + i) as Label;
                    Label_points.Content = slots[i, 2];
                }
                */



                /*
                name_1.Content = slots[0, 0];
                movies_1.Content = slots[0, 1];
                points_1.Content = slots[0, 2];

                name_2.Content = slots[1, 0];
                movies_2.Content = slots[1, 1];
                points_2.Content = slots[1, 2];


                name_3.Content = slots[2, 0];
                movies_3.Content = slots[2, 1];
                points_3.Content = slots[2, 2];
                */
            }
            

            for (int i = 1; i <= spots_count; i++)
            {
                filepath = save_path + @"\savefile_" + i + ".json";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Close();
                }
            }


            room_name.Content = "Хранилище слотов";


        }

        public void open_slot (int file_num)
        {                     
            filepath = save_path + @"\savefile_" + file_num + ".json";


            if (File.ReadAllText(filepath) == "")
            {
                Open.open_new(this,file_num);
            }
            else
            {
                Open.open_old(this,file_num);                
            }

            slot_1.Focus();
            
            
            
        }

        

        












        private void slot_1_Click(object sender, RoutedEventArgs e)
        {
            open_slot(1);
            
        }
        private void slot_2_Click(object sender, RoutedEventArgs e)
        {
            open_slot(2);
        }

        private void slot_3_Click(object sender, RoutedEventArgs e)
        {
            open_slot(3);
        }

        private void main_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Escape)
            {
                //выход из приложения
                Application.Current.Shutdown();
            }


            if(e.Key==Key.NumPad1 || e.Key == Key.D1)
            {
                slot_1_Click(slot_1, null);
            }

            if (e.Key == Key.NumPad2 || e.Key == Key.D2)
            {
                slot_2_Click(slot_2, null);
            }

            if (e.Key == Key.NumPad3 || e.Key == Key.D3)
            {
                slot_3_Click(slot_3, null);
            }

        }

    }
}
