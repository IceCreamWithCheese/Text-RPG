using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Лучшее_текстовое_РПГ_всех_времен_и_народов
{
    static class Open
    {
        public static void open_new(MainWindow current_window,int file_num)
        {
            // генерация новой игры
            Game_Window g = new Game_Window();
            g.filepath = current_window.filepath;
            g.file_num = file_num;
            g.room_name.Content = "Слот# " + file_num;
            g.Show();
            current_window.Close();

            File.WriteAllText(current_window.filepath, "");
        }

        public static void open_new(Game_Window current_window)
        {
            // генерация новой игры
            Game_Window g = new Game_Window();
            g.filepath = current_window.filepath;
            g.file_num = current_window.file_num;
            g.room_name.Content = "Слот# " + current_window.file_num;
            g.Show();
            current_window.Close();

            File.WriteAllText(current_window.filepath, "");
        }



        public static void open_old(MainWindow current_window, int file_num)
        {
            try
            {
                // открыть сохранённую игру
                Game_Window g = new Game_Window(current_window.filepath);
                g.filepath = current_window.filepath;
                g.file_num = file_num;
                g.room_name.Content = "Слот# " + file_num;
                g.Show();
                current_window.Close();
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Увы, файл похерился. Может начнешь заново?", "", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    open_new(current_window,file_num);
                }
            }
            
        }

        public static void open_old(Game_Window current_window)
        {
            try
            {
                // открыть сохранённую игру
                Game_Window g = new Game_Window(current_window.filepath);
                g.filepath = current_window.filepath;
                g.file_num = current_window.file_num;
                g.room_name.Content = "Слот# " + current_window.file_num;
                g.Show();
                current_window.Close();
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Увы, файл похерился. Может начнешь заново?", "", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    open_new(current_window);
                }
            }
        }



        public static void open_menu(Game_Window current_window)
        {
            MainWindow m = new MainWindow();
            m.Show();
            current_window.Close();
        }


    }
}
