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
    /// Логика взаимодействия для Game_Window.xaml
    /// </summary>

    public partial class Game_Window : Window
    {

        /* =================================================================================== */
        /* =================================================================================== */
        /* =================================================================================== */
        
        

        Matrix matrix = new Matrix();
        Player player = new Player();
        Rectangle box = new Rectangle();
        Saver saver = new Saver();
        public Rectangle[,] map_blocks;
        public string filepath;
        public int file_num;

        


        




        public Game_Window()
        {

            InitializeComponent();



            new_game();
            go_to_menu.Focus();
            take_item.IsEnabled = false;

            

            void new_game()
            {

                //приветствие
                
                show_list.Items.Add("Знакомство");
                show_list.Items.Add("----------------------------------------------");

                string introduction = " Ты - ученик чародея. \nПомогаешь старым колдунам с их дурацкими просьбами. Каждый день. \nНаконец твой сэнсей подошел к тебе и сказал: \n- Я дам тебе отгул, если ты проведаешь всех моих старых друзей и передашь им от меня 'Привет'. Ступай. \nНа-ка тебе еще на дорожку маны чтоб добраться. Смотри не растрать по ветру!!!"; 
                add_result(introduction);

                string goal = "- Ну и еще рецептик салата \n'Жабье рёбрышко' спроси. Уж очень хочется...  \nдобудь этот салат и НЕДЕЛЮ будешь бездельничать. нет...месяц!";
                add_result(goal);

                matrix.generate_obj();

               

                create_map();

                create_pos_for_player();
                // место в рюкзаке - 60% от колдунов
                player.bag_size = Convert.ToInt32(player.incomplete_missions * 0.60);
                bag_size_txt.Content += player.bag_size + "";


                check_bag();
                show_way();



                

                /* используемые методы: */

                void create_pos_for_player()
                {
                    player.incomplete_missions = matrix.witches.Count;

                    points_txt.Text = player.points + "";
                    incomplete_missions_txt.Text = player.incomplete_missions + "";
                    int x;
                    int y;
                    Random r = new Random();
                    do
                    {
                        x = r.Next(0, matrix.matrix_size);
                        y = r.Next(0, matrix.matrix_size);
                    }
                    while (matrix.show_arr(x, y) != 0);
                    player.set_x(x);
                    player.set_y(y);
                    create_box();
                }
            }

        }

        public Game_Window(string filepath)
        {
            InitializeComponent();





            create_map();


            //приветствие
            show_list.Items.Add("Эй! Чего вернулся?");
            show_list.Items.Add("----------------------------------------------");


            
            to_download();


            incomplete_missions_txt.Text = player.incomplete_missions + "";
            points_txt.Text = player.points + "";
            moves_txt.Text = player.moves + "";
            bag_size_txt.Content += player.bag_size + "";

            go_to_menu.Focus();



            create_box();


            check_bag();
            show_way();

            



            void to_download()
            {

                Saver saver = JsonConvert.DeserializeObject<Saver>(File.ReadAllText(filepath));


                matrix.matrix_size = saver.matrix_size;
                matrix.arr_matrix = saver.arr_matrix;
                matrix.witches = saver.witches;
                matrix.items = saver.items;
                player.bag_size = saver.bag_size;
                if (saver.bag.Count != 0)
                {
                    for (int i = 0; i < saver.bag.Count; i++)
                    {
                        bag.Items.Add(saver.bag[i]);
                    }
                    bag.SelectedIndex = bag.Items.Count - 1;
                }


                for (int i = 0; i < matrix.matrix_size; i++)
                {
                    for (int j = 0; j < matrix.matrix_size; j++)
                    {
                        switch (saver.map_blocks[i, j])
                        {
                            case -1:
                                map_blocks[i, j].Fill = Brushes.Black;
                                break;
                            case 1:
                                map_blocks[i, j].Fill = marked;
                                break;
                            case 10:
                                map_blocks[i, j].Fill = Brushes.Red;
                                break;
                            case 11:
                                map_blocks[i, j].Fill = Brushes.ForestGreen;
                                break;
                            case 2:
                                map_blocks[i, j].Fill = Brushes.DarkBlue;
                                break;
                            default:
                                map_blocks[i, j].Fill = Brushes.White;
                                break;
                        }


                    }
                }

                player.incomplete_missions = saver.incomplete_missions;
                player.points = saver.points;
                player.moves = saver.moves;
                player.set_x(saver.my_x);
                player.set_y(saver.my_y);




            }
        }




        public void create_map()
        {
            map_blocks = new Rectangle[matrix.matrix_size, matrix.matrix_size];
            for (int i = 0; i < matrix.matrix_size; i++)
            {
                for (int j = 0; j < matrix.matrix_size; j++)
                {
                    map_blocks[i, j] = new Rectangle();
                    map_blocks[i, j].Fill = Brushes.Black;
                    map_blocks[i, j].Opacity = 20;

                    Grid.SetColumn(map_blocks[i, j], i);
                    Grid.SetRow(map_blocks[i, j], j);
                    map.Children.Add(map_blocks[i, j]);
                }
            }
        }

        /* Мне нужно сохранить:
              
             
             * arr_matrix (array)
             * map_blocks (array)
             * witches (list)
             * items (list)
             * bag (listbox)
             * incomplete_missions 
               points 
               moves (variables)
             */


        public class Saver
        {
            public int matrix_size;
            public int[,] arr_matrix;
            public List<Matrix.Witch> witches = new List<Matrix.Witch>();
            public List<Matrix.Item> items = new List<Matrix.Item>();
            public List<string> bag = new List<string>();
            public int[,] map_blocks;

            public int bag_size;
            public int incomplete_missions;
            public int points;
            public int moves;
            public int my_x;
            public int my_y;
        }



        public void to_save()
        {


            Saver saver = new Saver();
            saver.matrix_size = matrix.matrix_size;
            saver.arr_matrix = matrix.arr_matrix;
            saver.witches = matrix.witches;
            saver.items = matrix.items;
            saver.bag_size = player.bag_size;
            if (bag.Items.Count != 0)
            {
                for (int i = 0; i < bag.Items.Count; i++)
                {
                    saver.bag.Add(bag.Items.GetItemAt(i).ToString());
                }
            }


            saver.map_blocks = new int[matrix.matrix_size, matrix.matrix_size];
            // -1 - не пройден, 1 - пустой, 10 - колдун(не завершенный), 11 - колдун(завершенный), 2 - предмет
            for (int i = 0; i < matrix.matrix_size; i++)
            {
                for (int j = 0; j < matrix.matrix_size; j++)
                {
                    if (map_blocks[i, j].Fill == Brushes.Black)
                    {
                        saver.map_blocks[i, j] = -1;
                    }
                    else if (map_blocks[i, j].Fill == Brushes.Red)
                    {
                        saver.map_blocks[i, j] = 10;
                    }
                    else if (map_blocks[i, j].Fill == Brushes.ForestGreen)
                    {
                        saver.map_blocks[i, j] = 11;
                    }
                    else if (map_blocks[i, j].Fill == Brushes.DarkBlue)
                    {
                        saver.map_blocks[i, j] = 2;
                    }
                    else
                    {
                        saver.map_blocks[i, j] = 1;
                    }
                }
            }

            saver.incomplete_missions = player.incomplete_missions;
            saver.points = player.points;
            saver.moves = player.moves;
            saver.my_x = player.show_x();
            saver.my_y = player.show_y();

            // сохранение для слота-файлика
            string content = JsonConvert.SerializeObject(saver);

            File.WriteAllText(filepath, content);




            // сохранения для меню-файлика

            using (StreamWriter sw = File.AppendText(MainWindow.menu_file))
            {
                Savespot s = new Savespot();
                s.name = "Слот: " + file_num;
                s.number = file_num;
                s.movies = player.moves;
                s.points = player.points;
                string menu_content = JsonConvert.SerializeObject(s);
                sw.WriteLine(menu_content);
            }

            /*
            MainWindow.Label_name = FindName("name_" + file_num) as Label;
            MainWindow.Label_name.Content = file_num;

            MainWindow.Label_movies = FindName("movies_" + file_num) as Label;
            MainWindow.Label_movies.Content = player.moves;

            MainWindow.Label_points = FindName("points_" + file_num) as Label;
            MainWindow.Label_points.Content = player.points;
            */

            /*
            string[] changing_slots = new string[MainWindow.spots_count];

            changing_slots = File.ReadAllText(MainWindow.menu_file).Split('\n');

            for (int i = 0; i < changing_slots.Length; i++)
            {
                if (i == file_num - 1)
                {
                    changing_slots[i] = "Слот " + file_num + "_" + player.moves + "_" + player.points;
                }
            }
            File.WriteAllText(MainWindow.menu_file, string.Join("\n", changing_slots));

            */


        }


















        public void create_box()
        {

            box.Fill = Brushes.Yellow;
            Grid.SetColumn(box, player.show_x());
            Grid.SetRow(box, player.show_y());
            map.Children.Add(box);


        }

        /* КЛАСС:МАТРИЦА НАЧАЛСЯ */
        public class Matrix
        {
            public List<Matrix.Witch> witches = new List<Matrix.Witch>();
            public List<Matrix.Item> items = new List<Matrix.Item>();

            //имя, тип_предмета, разговор {inro(2),just(3),bad(4),success(5)}
            string[,] arr_witches =
            {
                { "Травник","овощь","Привет, милок. Чего говоришь надо? Рецепт? Ох много я всяких рецептов знаю... Но память мне изменяет сейчас. Сбегай чтоли витаминок с огорода мне принеси...для памяти :)","Где витаминки???","Нет.Нет. И нет.","Ну...милок. Спасибо тебе. Помню петрушку класть надо... ну и корень лемминга" },
                { "Берсерк","шкура","А ничего тебе больше не надо?! Ты даже приготовить его не сможешь. Думаешь сможешь? Так иди и овец чтоли подстриги.","Поди ищешь луг?","Что за хлам ты притащил?!","Вот это смельчак! Беру свои слова обратно! Богатырь!!! Положи в салат ногти 3-х годовых гномов. Ммм...вкуснятина!" },
                { "Оккультист","свиток","Привет земной человек. Чего пожаловал в эти земли? Ооо...славный салатик. Я бы помог тебе с рецептом, но ищу свой хиромантский свиток чтоб погадать на ручке одной симпатичной соседке :). Поможешь найти?","Ты смотрел в библиотеке?","Это не свиток. Ты читать умеешь кстати?","Спасибо, дружище. У меня будет славный вечер ^-^. Отвари русалочьи плавники - это один из главных ингридиентов." },
                { "Жрец","паста","Здравствуй, неколдун. Кого почитаешь высшим божеством? Ах...Перун так Перун. Не мне тебя переучивать. Куда идешь, чего ищешь? 'Жабье рёбрышко' говоришь? Знавал я такой в молодости. Найди мне пламенный эликсир для чистки зубов, тогда и поговорим, сын мой.","Снова здорова, мои зубы просто зудят. Как там поиски?","Этим даже ботинки чистить нельзя, не то что зубы!","Секретный ингридиент - рёбрышко соседки-ведьмы. Приятного аппетита!" },
                { "Шаман","питомец","Привет путник! Что забыл на колдуньих холмах? Ах, друг, салатик - это святое! Помоги мне питомца моего найти: маленький, умненький, любит есть мышей. Только под ноги смотри, не дай Демиург наступишь на него! Не поздоровится ;)","Ну что ж, путник. Смотрю цел, значит на зверька моего не наткнулся!","Это не мой :(","Ой, какой миленький. Привет потеряшечка! Весь в листьях, негодник! Спасибо тебе, добрый человек. Помню касатку надо положить. Сухопутную. Хромую." },
                { "Колдун на мели","еда","Дратути, чудак. Куда путь держишь? Ааааа...ну понятно...дай мне нормальной еды и я расскажу тебе то, о чем забыли все остальные колдуны!","Руки у тебя пусты. А у меня язык - нем.","Этот хлам мне не нужен. Тащи другой ;)","Ооо...это то, что мне нужно. Пасиба, пасиба! Заправь салат сметанкой, будет круто." },

            };
            string[,] arr_items =
            {
                { "морковь","овощь" },
                { "орехи","орех" },
                { "антисклерозин","лекарство" },
                { "волчья шкура","шкура" },
                { "кабанья шкура","шкура" },
                { "бараньи рога","рога" },
                { "книга заклинаний","книга" },
                { "свиток хироманта","свиток" },
                { "нумерологический свиток","другой свиток" },
                { "свиток астронома","другой свиток" },
                { "зубная паста со вкусом \nперца чили","паста" },
                { "эликсир из серцевины \nогненной крапивы","не паста" },
                { "зубная паста \nсо вкусом мяты","другая паста" },
                { "ёжик","питомец" },
                { "пёс","другой питомец" },
                { "утка","другой питомец" },
                { "медведь","другой питомец" },
                { "заяц","другой питомец" },
                { "лиса","другой питомец" },
                { "кошка","питомец" },
                { "бараньи ножки","еда" },
                { "сочная курочка","еда" },
                { "гороховый суп","еда" },
                { "уха","еда" },
                { "окрошка","еда" },
            };


            /* РАБОТА С МАТРИЦЕЙ */

            public int matrix_size = 30;
            // 0-пусто, 1-колдун, 2-предмет
            public int[,] arr_matrix;
            public int show_arr(int x, int y)
            {
                return arr_matrix[x, y];
            }
            public int[] create_pos(string type)
            {
                int x;
                int y;
                Random r = new Random();
                do
                {
                    x = r.Next(0, matrix_size);
                    y = r.Next(0, matrix_size);
                }
                while (arr_matrix[x, y] != 0);

                if (type == "witch")
                {
                    arr_matrix[x, y] = 1;
                }
                if (type == "item")
                {
                    arr_matrix[x, y] = 2;
                }
                return new int[] { x, y };
            }

            public void take_item(int x, int y, int id)
            {
                if (arr_matrix[x, y] == 2)
                {
                    arr_matrix[x, y] = 0;
                    items[id].set_x(-4);
                    items[id].set_y(-4);
                }
            }

            public int put_item(int x, int y, int id)
            {
                if (arr_matrix[x, y] == 0)
                {
                    arr_matrix[x, y] = 2;
                    items[id].set_x(x);
                    items[id].set_y(y);
                    return 0;
                }

                else if (arr_matrix[x, y] == 1)
                {
                    int witch_id = search_in_witches(x, y);
                    // пытаюсь выполнить миссию
                    witches[witch_id].complete_mission(items[id].kind);
                    if (witches[witch_id].show_mission() == true)
                    {
                        return 1;
                    }
                    return -4;
                }
                return 2;
            }


            
            public class Witch
            {
                public int x;
                public int y;
                public bool mission = false;
                public void new_pos(int[] pos)
                {
                    x = pos[0];
                    y = pos[1];
                }
                public int[] show_pos()
                {
                    return new int[] { x, y };
                }
                public int show_x()
                {
                    return x;
                }
                public int show_y()
                {
                    return y;
                }
                public string name;
                //вид предмета            
                public string kind;


                string talk_sign = " говорит: ";

                public string talk_end()
                {
                    return name + talk_sign + "Опять ты :) \n Ничем больше помочь не могу.";
                }
                

                public string intro_msg;
                public string introduction_talk()
                {
                    return name + talk_sign + intro_msg;
                }
                public string just_msg;
                public string just_talk()
                {
                    return name + talk_sign + just_msg;
                }
                public string bad_msg;
                public string bad_talk()
                {
                    return name + talk_sign + bad_msg;
                }
                public string success_msg;
                public string success_talk()
                {
                    return name + talk_sign + success_msg;
                }





                //дать колдуну предмет
                public void complete_mission(string item_kind)
                {
                    if (kind == item_kind)
                    {
                        mission = true;
                    }
                }
                public bool show_mission()
                {
                    return mission;
                }
                public bool search_witch(int x, int y)
                {

                    if (this.x == x && this.y == y)
                    {
                        return true;
                    }
                    return false;
                }

            }




            public class Item
            {
                public int x;
                public int y;
                public string name;
                public string kind;

                /* для смены позиции */
                public void new_pos(int[] pos)
                {
                    x = pos[0];
                    y = pos[1];
                }
                public int[] show_pos()
                {
                    return new int[] { x, y };
                }
                public int show_x()
                {
                    return x;
                }
                public int show_y()
                {
                    return y;
                }
                public void set_x(int x)
                {
                    this.x = x;
                }
                public void set_y(int y)
                {
                    this.y = y;
                }

                /* для поиска предмета */
                // по координатам
                public bool search_item(int x, int y)
                {

                    if (this.x == x && this.y == y)
                    {
                        return true;
                    }
                    return false;
                }
                // по имени
                public bool search_item(string name)
                {
                    if (this.name == name)
                    {
                        return true;
                    }
                    return false;
                }

            }

            public void generate_obj()
            {
                Random r = new Random();
                //тут нужно добавить массив с разными id колдунов - СПИСОК АЙДИШНИКОВ
                //последний айди колдуна
                int last_id = arr_witches.GetUpperBound(0);
                //как минимум должно быть 5 колдуна, максимум - 6
                bool original = false;
                int[] arr_id = new int[r.Next(5, 7)];
                arr_id[0] = r.Next(0, last_id + 1);
                for (int i = 1; i < arr_id.Length; i++)
                {
                    while (original == false)
                    {
                        arr_id[i] = r.Next(0, last_id + 1);
                        original = true;
                        for (int j = 0; j < i; j++)
                        {
                            if (arr_id[i] == arr_id[j])
                            {
                                original = false;
                                break;
                            }
                        }
                    }
                    original = false;
                }
                //уже сформирован лист со случайными айдишниками


                arr_matrix = new int[matrix_size, matrix_size];
                //генерируем колдунов
                for (int i = 0; i < arr_id.Length; i++)
                {
                    Witch human = new Witch();
                    human.name = arr_witches[arr_id[i], 0];
                    human.kind = arr_witches[arr_id[i], 1];
                    //нужен метод для интерактивного общения
                    human.intro_msg = arr_witches[arr_id[i], 2];
                    human.just_msg = arr_witches[arr_id[i], 3];
                    human.bad_msg = arr_witches[arr_id[i], 4];
                    human.success_msg = arr_witches[arr_id[i], 5];



                    human.new_pos(create_pos("witch"));
                    //генерируем предмет
                    int id = -1;
                    while (id == -1)
                    {
                        int x = r.Next(0, arr_items.GetUpperBound(0) + 1);
                        //выбирает случайный айдишник предмета и проверяет нужен ли он колдуну
                        if (arr_items[x, 1] == human.kind)
                        {
                            id = x;
                        }
                    }
                    
                    Item item = new Item();
                    item.name = arr_items[id, 0];
                    item.kind = arr_items[id, 1];
                    item.new_pos(create_pos("item"));
                    items.Add(item);

                    
                    witches.Add(human);
                   
                }
                // генерация ненужных предметов
                int id_excess = -1;
                int d = r.Next(3, 6);
                for (int j = 0; j < d; j++)
                {
                    id_excess = -1;
                    while (id_excess == -1)
                    {
                        int x = r.Next(0, arr_items.GetUpperBound(0) + 1);
                        for (int k = 0; k < items.Count; k++)
                        {
                            if (arr_items[x, 0] != items[k].name)
                            {
                                id_excess = x;
                            }
                        }
                    }
                    Item excess_item = new Item();
                    excess_item.name = arr_items[id_excess, 0];
                    excess_item.kind = arr_items[id_excess, 1];
                    excess_item.new_pos(create_pos("item"));
                    items.Add(excess_item);
                }
            }
            /* КЛАСС:МАТРИЦА ЗАКОНЧИЛСЯ */








            /* ЗАПРОСЫ-ПОИСКОВИКИ ДЛЯ КЛАСС:МАТРИЦА */
            public int search_in_items(int x, int y)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].search_item(x, y) == true)
                    {
                        return i;
                    }
                }
                return -4;
            }
            public int search_in_items(string name)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].search_item(name) == true)
                    {
                        return i;
                    }
                }
                return -4;
            }

            public int search_in_witches(int x, int y)
            {
                for (int i = 0; i < witches.Count; i++)
                {
                    if (witches[i].search_witch(x, y) == true)
                    {
                        return i;
                    }
                }
                return -4;
            }
        }



        /* КЛАСС:ИГРОК НАЧАЛСЯ */
        public class Player
        {
            public int incomplete_missions = 0;
            public int points = 2005;
            public int moves = -1;

            public int bag_size;


            //передвижения героя
            int my_x;
            public void set_x(int x)
            {
                my_x = x;
            }

            public int show_x()
            {
                return my_x;
            }

            int my_y;
            public void set_y(int y)
            {
                my_y = y;
            }
            public int show_y()
            {
                return my_y;
            }

            string say_where(string where)
            {
                return "Ты идёшь: " + where /*+ "\n" + "Позиция: " + show_x() + " . " + show_y()*/;
            }


            public string left()
            {
                my_x--;
                return say_where("на запад");
            }
            public string right()
            {
                my_x++;
                return say_where("на восток");
            }
            public string up()
            {
                my_y--;
                return say_where("на север");
            }
            public string down()
            {
                my_y++;
                return say_where("на юг");
            }

        }
        /* КЛАСС:ИГРОК ЗАКОНЧИЛСЯ */



        /* ЗАПРОСЫ-ПОИСКОВИКИ ДЛЯ КЛАСС:ИГРОК */
        public string search_in_x(int sign)
        {
            int x = player.show_x();
            int y = player.show_y();
            for (int i = 1; i < 4; i++)
            {
                x += i * sign;
                /* поиск по 3 клеткам по дороге */
                // если клетка впереди не пройдена
                if (x >= 0 && x < matrix.matrix_size && map_blocks[x, y].Fill == Brushes.Black)
                {
                    // если в клетке есть колдун/предмет
                    if (matrix.show_arr(x, y) == 1)
                    {
                        return "Впереди человек.";
                    }
                    else if (matrix.show_arr(x, y) == 2)
                    {
                        return "Кажется на дороге что-то лежит... Подойди поближе.";
                    }
                }
            }
            return "";
        }
        public string search_in_y(int sign)
        {
            int x = player.show_x();
            int y = player.show_y();
            for (int i = 1; i < 4; i++)
            {
                y += i * sign;
                /* поиск по 3 клеткам по дороге */
                // если клетка впереди не пройдена
                if (y >= 0 && y < matrix.matrix_size && map_blocks[x, y].Fill == Brushes.Black)
                {
                    // если в клетке есть колдун/предмет
                    if (matrix.show_arr(x, y) == 1)
                    {
                        return "Впереди человек.";
                    }
                    else if (matrix.show_arr(x, y) == 2)
                    {
                        return "Кажется на дороге что-то лежит...";
                    }
                }
            }
            return "";
        }











        public string add_item()
        {
            int id = matrix.search_in_items(player.show_x(), player.show_y());
            if (id >= 0)
            {

                matrix.take_item(player.show_x(), player.show_y(), id);
                bag.Items.Add(matrix.items[id].name);
                bag.SelectedIndex = bag.Items.Count - 1;

                int x = player.show_x();
                int y = player.show_y();

                
                check_map();
                check_bag();
                take_item.IsEnabled = false;


                return "Предмет успешно добавлен.";
            }
            return "Хех...Не балуй!";
        }

        public string delete_item()
        {
            

            int x = player.show_x();
            int y = player.show_y();


            string select_name = bag.SelectedItem + "";
            int id = matrix.search_in_items(select_name);

            int id_witch = matrix.search_in_witches(x, y);
            if (id != -4)
            {

                int result = matrix.put_item(player.show_x(), player.show_y(), id);
                switch (result)
                {
                    case 0:
                        bag.Items.RemoveAt(bag.SelectedIndex);
                        bag.SelectedIndex = bag.Items.Count - 1;                        
                       
                        check_bag();
                        put_item.IsEnabled = false;
                        check_map();

                        return "Предмет выкинут.";


                    case 2:
                        return "Там уже есть предмет, чувак.";
                    case 1:
                        bag.Items.RemoveAt(bag.SelectedIndex);
                        bag.SelectedIndex = bag.Items.Count - 1;
                        

                        change_points(+700);
                        reduce_mission();

                        
                        check_bag();
                        put_item.IsEnabled = false;
                        check_map(true);

                        /* фразочка-концовочка! */
                        //колдун говорит: правильный предмет (миссия выполнена)                      
                        return matrix.witches[id_witch].success_talk();

                    case -4:

                        change_points(-300);

                        // колдун говорит: неправильный предмет                     
                        return matrix.witches[id_witch].bad_talk();
                }
            }
            return "# Подсказка: попробуй выделить предмет #";
        }

        //-----------                 ГЛОБАЛЬНЫЕ ПЕРЕМЕННЫЕ            ------------------------------
        public void change_points(int i)
        {
            player.points = player.points + i;
            points_txt.Text = player.points + "";
            if (player.points <= 0)
            {
                game_over();
            }
        }

        public void reduce_mission()
        {
            player.incomplete_missions--;
            incomplete_missions_txt.Text = player.incomplete_missions + "";
            if(player.incomplete_missions==0)
            {
                game_win();
            }
        }

        public void do_step()
        {

            player.moves++;
            change_points(-1);
            moves_txt.Text = player.moves + "";
        }

        //----------------------------------------------------------------------------------




        Brush marked = new SolidColorBrush(Color.FromArgb(255, 20, 20, 20));

        // проверка: пустой ячейки/ячейки с предметом
        public void check_map()
        {

            int x = player.show_x();
            int y = player.show_y();
            if (matrix.show_arr(x, y) == 0)
            {
                map_blocks[x, y].Fill = marked;
            }
            if (matrix.show_arr(x, y) == 2)
            {
                map_blocks[x, y].Fill = Brushes.DarkBlue;
            }



        }

        // проверка: колдун пройденный/непройденный
        public void check_map(bool mission)
        {
            int x = player.show_x();
            int y = player.show_y();
            if (matrix.show_arr(x, y) == 1)
            {

                if (mission)
                {
                    map_blocks[x, y].Fill = Brushes.ForestGreen;
                    put_item.IsEnabled = false;
                }
                else
                {
                    map_blocks[x, y].Fill = Brushes.Red;
                }

            }
        }




        /* ОТЛАВЛИВАЕТ ПУТЬ */
        public void show_way()
        {
            do_step();
            

            int x = player.show_x();
            int y = player.show_y();
            int el = matrix.show_arr(x, y);

            Grid.SetColumn(box, x);
            Grid.SetRow(box, y);

            switch (el)
            {
                case 0:
                    
                    put_item.Content = "Выкинуть предмет (E)";
                    check_bag();
                    take_item.IsEnabled = false;
                    check_map();
                    break;
                case 1:
                    int id = matrix.search_in_witches(x, y);
                    add_result("Ты нашёл колдуна");
                    if(matrix.witches[id].show_mission()==true)
                    {
                        check_bag();
                        put_item.IsEnabled = false;
                        add_result(matrix.witches[id].talk_end()); //...колдун уже давно зеленый 

                    }
                    else
                    {


                        // добавить проверку не явл ли ячейка неоткрытой
                        if(map_blocks[x,y].Fill==Brushes.Black)
                        {
                            //...знакомство с красным
                            add_result(matrix.witches[id].introduction_talk());
                            Thread.Sleep(300);

                        }
                        else
                        {
                            //...повторная встреча с красным
                            add_result(matrix.witches[id].just_talk());

                        }
                        
                        

                    }
                    check_bag();
                    take_item.IsEnabled = false;
                    put_item.Content = "Предложить предмет (E)";              
                    check_map(matrix.witches[id].show_mission());



                    break;
                case 2:
                    int id_item = matrix.search_in_items(x, y);

                    add_result("ООО... " + matrix.items[id_item].name + " :)");
                    check_bag();
                    check_map();
                    put_item.IsEnabled = false;
                    put_item.Content = "Выкинуть предмет (E)";
                    
                    
                    break;
            }



            switch (x)
            {
                case 0:
                    left.IsEnabled = false;
                    break;
                case 29:
                    rigth.IsEnabled = false;
                    break;
                default:
                    left.IsEnabled = true;
                    rigth.IsEnabled = true;
                    break;
            }


            switch (y)
            {
                case 0:
                    up.IsEnabled = false;
                    break;
                case 29:
                    down.IsEnabled = false;
                    break;
                default:
                    up.IsEnabled = true;
                    down.IsEnabled = true;
                    break;
            }
            //чтобы при блокировки активной кнопки фокус не терялся
            go_to_menu.Focus();
        }

        public void check_bag()
        {

            /* есть че в рюкзаке? */

            // пустой рюкзак?
            if (bag.Items.Count == 0)
            {
                put_item.IsEnabled = false;
            }
            else
            {
                put_item.IsEnabled = true;
            }

            // полный рюкзак?
            if (bag.Items.Count >= player.bag_size)
            {
                take_item.IsEnabled = false;
            }
            else
            {
                take_item.IsEnabled = true;
            }



        }

        public void game_over()
        {
            //код для перезагрузки
            map.Children.Clear();
            game.Visibility = Visibility.Collapsed;
            end.Visibility = Visibility.Visible;
            back_to_menu.Focus();
            switch(player.incomplete_missions)
            {
                case 1:
                    shame_msg.Content = "эх, проиграл как дурак";
                    break;
                case 2:
                case 3:
                    shame_msg.Content = "(с позором)";
                    break;
                case 4:
                    shame_msg.Content = "(с очень большим позором)";
                    break;

                default:
                    shame_msg.Content = "ну кто так играет?!";
                    break;

            }

        }

        public void game_win()
        {
            game.Visibility = Visibility.Collapsed;
            happiend.Visibility = Visibility.Visible;
        }








        // КОНСОЛЬКА
        //******************************************************************************************************
        //для подсчета сообщений в консольке
        int some_file_num = 0;
        public void add_result(string text)
        {
            if (text == "")
            {
                return;
            }
            // сократить консольку (автоматически)
            if (show_list.Items.Count > 70)
            {
                while (show_list.Items.Count > 20)
                {
                    show_list.Items.RemoveAt(0);
                }
            }
            //ширина консольки
            int len_show = 36;

            if (text.Length > len_show)
            {
                //индекс последнего символа в строке
                int id_last = len_show;
                if (text.IndexOf("\n") != -1)
                {
                    id_last = text.IndexOf("\n") + len_show;
                }
                while (id_last < text.Length)
                {

                    if (text.Substring(id_last, 1) == " ")
                    {
                        text = text.Insert(id_last+1, "\n");
                    }
                    else
                    {
                        int new_last = id_last - 1;
                        while (text.Substring(new_last, 1) != " ")
                        {
                            new_last = new_last - 1;
                        }
                        text = text.Insert(new_last+1, "\n");
                    }

                    id_last += len_show;
                }

            }
            if (text != Convert.ToString(show_list.Items[show_list.Items.Count - 2]))
            {
                show_list.Items.Add(text);
                some_file_num++;
                show_list.Items.Add("------------------ USE: " + some_file_num + "------------------------");
                show_list.ScrollIntoView(show_list.Items.GetItemAt(show_list.Items.Count - 1));
            }
        }
        //********************************************************************************************







        /*
        // ДЛЯ РАЗРАБОТЧИКА
        private void show_matrix_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < matrix.witches.Count; i++)
            {
                check1.Text += matrix.witches[i].name + "\n";
            }
            check1.Text += "Всего колдунов:" + matrix.witches.Count + "\n";

            for (int i = 0; i < matrix.items.Count; i++)
            {
                check1.Text += matrix.items[i].name + "\n";
            }
           
            check1.Text += "Всего предметов:" + matrix.items.Count + "\n";

            //ПОКАЗЫВАЕТ В КОНСОЛЬКЕ ПАРУ КОЛДУН - ПРЕДМЕТ (КАКИЕ ЕСТЬ В МАТРИЦЕ)
            for (int i = 0; i < matrix.witches.Count; i++)
            {
                check1.Text += matrix.witches[i].check_txt + "\n\n\n";
            }

            



        }
        */




        // ---------------    ДВИЖЕНИЯ ГЕРОЯ  ---------------------//
        private void up_Click(object sender, RoutedEventArgs e)
        {
            add_result(player.up());
            add_result(search_in_y(-1));
            show_way();


        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            add_result(player.down());
            add_result(search_in_y(+1));
            show_way();

        }

        private void left_Click(object sender, RoutedEventArgs e)
        {
            add_result(player.left());
            add_result(search_in_x(-1));
            show_way();

        }

        private void rigth_Click(object sender, RoutedEventArgs e)
        {
            add_result(player.right());
            add_result(search_in_x(+1));
            show_way();

        }





        //---------------   КНОПКИ РЮКЗАКА   -------------------------
        private void take_item_Click(object sender, RoutedEventArgs e)
        {
            add_result(add_item());
        }

        private void put_item_Click(object sender, RoutedEventArgs e)
        {
            add_result(delete_item());
        }






        /* ВЫХОД */
        private void go_to_menu_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Точно выйти?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result== MessageBoxResult.Yes)
            {
                MainWindow m = new MainWindow();
                m.Show();
                this.Close();
            }


        }


        public int time = 100;

        /* БИНДЫ */
        private void game_KeyDown(object sender, KeyEventArgs e)
        {
            // для выделения предмета в рюкзаке
            if (e.Key == Key.Up && bag.SelectedIndex > 0)
            {
                bag.SelectedIndex--;                
            }

            if (e.Key == Key.Down && bag.SelectedIndex != -1)
            {
                bag.SelectedIndex++;
            }

            if (e.Key == Key.Left && bag.SelectedIndex != -1)
            {
                bag.SelectedIndex--;
            }

            if (e.Key == Key.Right && bag.SelectedIndex != -1)
            {
                bag.SelectedIndex++;
            }

            // бинды для взять/выкинуть

            if (e.Key == Key.Q && take_item.IsEnabled)
            {
                take_item_Click(take_item, null);
            }

            if (e.Key == Key.E && put_item.IsEnabled)
            {
                put_item_Click(put_item, null);
            }

            
            // бинды для перевижения
            if (e.Key == Key.W && up.IsEnabled)
            {
                up_Click(up, null);
                Thread.Sleep(time);
            }

            if (e.Key == Key.S && down.IsEnabled)
            {
                down_Click(down, null);
                Thread.Sleep(time);
            }

            if (e.Key == Key.A && left.IsEnabled)
            {
                left_Click(left, null);
                Thread.Sleep(time);
            }

            if (e.Key == Key.D && rigth.IsEnabled)
            {
                rigth_Click(rigth, null);
                Thread.Sleep(time);
            }


            if (e.Key == Key.Escape)
            {
                go_to_menu_Click(go_to_menu, null);
            }

            if (e.Key == Key.F5)
            {
                save_Click(save, null);
            }

            if (e.Key == Key.Delete)
            {
                delete_slot_Click(delete_slot, null);
            }

        }


        private void save_Click(object sender, RoutedEventArgs e)
        {
            to_save();
            add_result("Настройки сохранены!");

        }

        private void delete_slot_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Точно удалить весь прогресс?", "", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (result == MessageBoxResult.No || result == MessageBoxResult.None)
            {
                return;
            }

            Open.open_new(this);

            // удаление меню-файлика

            // отсчет в листе с 0, а номера сейвспотов начинаются с 1



            /*
            using (StreamReader sr = File.OpenText(MainWindow.menu_file))
            {
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.IndexOf(Convert.ToChar(file_num)) != -1)
                    {

                    }
                }

            }
            */

            int id = MainWindow.search_in_saves(file_num);
            MainWindow.savespots.RemoveAt(id);

            File.WriteAllText(MainWindow.menu_file, "");

            using (StreamWriter sw = File.AppendText(MainWindow.menu_file))
            {
                for(int i=0;i<MainWindow.savespots.Count;i++)
                {
                    string menu_content = JsonConvert.SerializeObject(MainWindow.savespots[i]);
                    sw.WriteLine(menu_content);
                }
                
            }


            /*
            MainWindow.Label_name = FindName("name_" + file_num) as Label;
            MainWindow.Label_name.Content = "";

            MainWindow.Label_movies = FindName("movies_" + file_num) as Label;
            MainWindow.Label_movies.Content = "";

            MainWindow.Label_points = FindName("points_" + file_num) as Label;
            MainWindow.Label_points.Content = "";
            */
            /*
            string[] changing_slots = new string[MainWindow.spots_count + 1];
            changing_slots = File.ReadAllText(MainWindow.menu_file).Split('\n');
            for (int i = 1; i < changing_slots.Length; i++)
            {
                if (i == file_num)
                {
                    changing_slots[i] = "Не открыта__";
                }
            }
            File.WriteAllText(MainWindow.menu_file, string.Join("\n", changing_slots));
            */
        }







        /* ОКОШКО С ПРОИГРЫШЕМ */
        private void end_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                back_to_menu_Click(back_to_menu, null);
            }
        }


        private void back_to_menu_Click(object sender, RoutedEventArgs e)
        {
            Open.open_menu(this);
        }
        /*--------------------*/

        
    }
}
