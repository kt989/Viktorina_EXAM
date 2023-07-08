// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

class Program
{
    static List<User> reader_user(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs, Encoding.UTF8);
        List<User> users = new List<User>();
        string[] buffer_array;
        User buffer_user;
        for(int i= 0; !sr.EndOfStream;++i) 
        {
             //Console.WriteLine($"{i}. \t{sr.ReadLine()}\n"); //проверочный код
            
            buffer_array = sr.ReadLine().Split();
            buffer_user = new User(buffer_array[0], StringToDate(buffer_array[1]), StringToLogin(buffer_array[2]), StringToPassword(buffer_array[3]));
            users.Add(buffer_user);
        }
        sr.Close();
        fs.Close();
        return users;
    }

    static public void GetRegistration()
    {
        Console.Clear();
        Console.WriteLine("Регистрация:\n\nКак Вас зовут? ");
        string user_name = Console.ReadLine();

        Console.WriteLine("\nВведите Вашу дату рождения: ");
        string[] date_array = Console.ReadLine().Split('.');
        Date birthday_user = new Date(Convert.ToInt32(date_array[0]), Convert.ToInt32(date_array[1]), Convert.ToInt32(date_array[2]));
        if (!birthday_user.date_check())
        {
            do
            {
                Console.WriteLine("Дата рождения введена не корректно. Повторите ввод: ");
                date_array = Console.ReadLine().Split('.');
                birthday_user.set_day(Convert.ToInt32(date_array[0]));
                birthday_user.set_month(Convert.ToInt32(date_array[1]));
                birthday_user.set_year(Convert.ToInt32(date_array[2]));
            } while (!birthday_user.date_check());
        };
        Console.WriteLine("\nПридумайте логин: ");
        Login login_new = new Login(Console.ReadLine());
        while (login_new.login_control())
        {
            Console.WriteLine("Такой логин уже существует, придумайте другой: ");
            login_new.set_login(Console.ReadLine());
        }
        Console.WriteLine("\nПридумайте пароль:" +
            "\n(Пароль должен быть не менее 8 знаков и состоять из латинских букв и цифр)\n");
        Password new_password = new Password(Console.ReadLine());
        while (!new_password.check_password())
        {
            Console.WriteLine("\nПароль должен быть не менее 8 знаков и состоять из латинских букв и цифр\n");
            new_password.set_password(Console.ReadLine());
        }
        User new_user = new User(user_name, birthday_user, login_new, new_password);
        string path = "users.txt";
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        fs.Close();
        new_user.user_writer(path);
        //reader(path);
        Console.WriteLine($"Регистрация пройдена! Для продолжения нажмите Enter");
        ConsoleKey key = Console.ReadKey().Key;
        if (key == ConsoleKey.Enter)
        {
            Console.Clear();
        }
    }
    static public bool EnterByLogin(string login)
    {
        User old_user = new User();
        old_user.set_login(StringToLogin(login));
        string path = "users.txt";
        List<User> users = reader_user(path);
        bool check = false;

        foreach (User user in users)
        {
            if (user.get_login().ToString().Equals(old_user.get_login().ToString()))
            {
                check = true;
                old_user = user;
                Console.WriteLine("\n");
                break;
            }
        }
        return check;
    }

    static public bool EnterByPassword(string login)
    {
        Console.Write("Введите пароль: ");
        User old_user = new User();
        old_user.set_login(StringToLogin(login));
        old_user.set_password(StringToPassword(Console.ReadLine()));
        string path = "users.txt";
        List<User> users = reader_user(path);
        bool check = false;
        foreach (User user in users)
        {
            if ((user.get_login().ToString().Equals(old_user.get_login().ToString()))
                &&(user.get_password().ToString().Equals(old_user.get_password().ToString())))
            {
                check = true;
                old_user = user;
                Console.WriteLine("\n");
                break;
            }
        }
        return check;
    }
    static public List<string> FileReader(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs, Encoding.UTF8);
        List<string> questions = new List<string>();
        string[] buffer_array;
        for (int i = 0; !sr.EndOfStream; ++i)
        {
            //Console.WriteLine($"{i}. \t{sr.ReadLine()}\n"); //проверочный код
            questions.Add(sr.ReadLine());
        }
       // foreach(var question in questions) { Console.WriteLine(question); }

        sr.Close();
        fs.Close();
        return questions;
    }
    /*static public void QuestionWriter(List<string> questions, int numer)
    {
        for(int  i=0+numer*4; i<4 + numer * 4; i++)
        {
            Console.WriteLine(questions[i]);
        }
    }*/
    static public int ValueAnswer(List<string> questions, int numer, ref int marks, int index_questions)
    {
        Console.Clear();
        Console.WriteLine("\nВопрос " + (numer + 1));
        Console.WriteLine("\n" + questions[numer * 4]);
        Console.WriteLine("\n->" + questions[numer * 4 + 1]);
        Console.WriteLine("\n  " + questions[numer * 4 + 2]);
        Console.WriteLine("\n  " + questions[numer * 4 + 3]);
        int index_answer = 1;
        ConsoleKey key = Console.ReadKey().Key;
        while (key != ConsoleKey.Enter) //работа меню, пока не нажат Enter, стрелка двигается вверх-вниз
        {
            if (key == ConsoleKey.DownArrow && index_answer == 1)
            {
                Console.Clear();
                Console.WriteLine("\nВопрос " + (numer + 1));
                Console.WriteLine("\n" + questions[numer * 4]);
                Console.WriteLine("\n  " + questions[numer * 4 + 1]);
                Console.WriteLine("\n->" + questions[numer * 4 + 2]);
                Console.WriteLine("\n  " + questions[numer * 4 + 3]);
                index_answer = 2;
                key = Console.ReadKey().Key;
                continue;
            }
            if (key == ConsoleKey.UpArrow && index_answer == 1)
            {
                Console.Clear();
                Console.WriteLine("\nВопрос " + (numer + 1));
                Console.WriteLine("\n" + questions[numer * 4]);
                Console.WriteLine("\n  " + questions[numer * 4 + 1]);
                Console.WriteLine("\n  " + questions[numer * 4 + 2]);
                Console.WriteLine("\n->" + questions[numer * 4 + 3]);
                index_answer = 3;
                key = Console.ReadKey().Key;
                continue;
            }
            if (key == ConsoleKey.DownArrow && index_answer == 2)
            {
                Console.Clear();
                Console.WriteLine("\nВопрос " + (numer + 1));
                Console.WriteLine("\n" + questions[numer * 4]);
                Console.WriteLine("\n  " + questions[numer * 4 + 1]);
                Console.WriteLine("\n  " + questions[numer * 4 + 2]);
                Console.WriteLine("\n->" + questions[numer * 4 + 3]);
                index_answer = 3;
                key = Console.ReadKey().Key;
                continue;
            }
            if (key == ConsoleKey.UpArrow && index_answer == 2)
            {
                Console.Clear();
                Console.WriteLine("\nВопрос " + (numer + 1));
                Console.WriteLine("\n" + questions[numer * 4]);
                Console.WriteLine("\n->" + questions[numer * 4 + 1]);
                Console.WriteLine("\n  " + questions[numer * 4 + 2]);
                Console.WriteLine("\n  " + questions[numer * 4 + 3]);
                index_answer = 1;
                key = Console.ReadKey().Key;
                continue;
            }
            if (key == ConsoleKey.DownArrow && index_answer == 3)
            {
                Console.Clear();
                Console.WriteLine("\nВопрос " + (numer + 1));
                Console.WriteLine("\n" + questions[numer * 4]);
                Console.WriteLine("\n->" + questions[numer * 4 + 1]);
                Console.WriteLine("\n  " + questions[numer * 4 + 2]);
                Console.WriteLine("\n  " + questions[numer * 4 + 3]);
                index_answer = 1;
                key = Console.ReadKey().Key;
                continue;
            }
            if (key == ConsoleKey.UpArrow && index_answer == 3)
            {
                Console.Clear();
                Console.WriteLine("\nВопрос " + (numer + 1));
                Console.WriteLine("\n" + questions[numer * 4]);
                Console.WriteLine("\n  " + questions[numer * 4 + 1]);
                Console.WriteLine("\n->" + questions[numer * 4 + 2]);
                Console.WriteLine("\n  " + questions[numer * 4 + 3]);
                index_answer = 2;
                key = Console.ReadKey().Key;
                continue;
            }
        }
        count_marks(marks, numer, index_answer, index_questions);
        return index_answer;
    }
    static public int count_marks(int marks, int numer, int index_answer, int index_questions)
    {
        FileStream fs = new FileStream("history answer.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs, Encoding.UTF8);
        List<int> answers = new List<int>();
        string[] buffer_answer = sr.ReadLine().Split(); 
        for (int i = 0; !sr.EndOfStream; ++i)
        {
            Console.WriteLine("Вопрос " + i + "  Ответ" + buffer_answer[i]);
        }

        sr.Close();
        fs.Close();
        
        return marks;
    }
    static public Date StringToDate(string date)
    {
        Date buffer = new Date();
        string[] buffer_array = new string[3];
        buffer_array = date.Split('.');
        buffer.set_day(Convert.ToInt32(buffer_array[0]));
        buffer.set_month(Convert.ToInt32(buffer_array[1]));
        buffer.set_year(Convert.ToInt32(buffer_array[2]));
        return buffer;
    }
    static public Password StringToPassword(string password)
    {
        Password buffer = new Password(password);
        return buffer;
    }
    static public Login StringToLogin(string login)
    {
        Login buffer = new Login(login);
        return buffer;
    }

    public class Login // Класс Логин
    {
        private string login;
        public void set_login(string login) { this.login = login;}
        public string get_login() { return this.login; }
        public Login()
        {
            this.login = "Login";
        }

        public Login(string login)
        {
            this.login = login;
        }

        public void add_login(List<string> users_login)//Добавление логина в список
        { 
        users_login.Add(login);
        }
        public bool login_control() //проверка логина, есть ли он в списке
        {
            bool check_login = false;
            string path = "users.txt";
            List<User> users = reader_user(path);
            foreach (var i in users)
                {
                    //i.show_user();
                    if (this.login.Equals(i.get_login().ToString())) { check_login = true; break; }
                }
            return check_login;
        }

        public string ToString()
        {
            return this.login;
        }
    }

    public class Password
    {
        private string password;
        public void set_password(string password) { this.password = password; }
        public string get_password() { return this.password; }
        public Password()
        {
            this.password= "password";
        }
        public Password(string password)
        {
            this.password = password;
        }

        public bool check_password()
        {
            char[] pass_char = password.ToCharArray();
            if (pass_char.Length < 8) return false;
            else
            {
                foreach (char i in pass_char)
                {
                    if (Char.IsDigit(i)) continue;
                    else if (Char.IsLetter(i))
                    {
                        if ((i >= 'A' && i <= 'Z')|| (i >= 'a' && i <= 'z')) continue;
                        else return false;
                        
                    }
                    else return false;
                }
            } 
            return true;
        }

        public string ToString()
        {
            return password;
        }
    }  //Класс Пароль
    public class Date //Класс дата
    {
        private int day, month, year;
        public void set_day(int day)
        {
            if (day>0&&day<32) this.day = day;
            else this.day = 0;
        }
        public int get_day(){ return this.day; }
        public void set_month(int month)
        {
            if (month > 0 && month < 13) this.month = month;
            else this.month = 0;
        }
        public int get_month() { return this.month; }
        public void set_year(int year)
        {
            if (year > 1925 && year < 2023) this.year = year;
            else this.year = 0;
        }
        public int get_year() { return this.year; }

        public Date()
        {
            set_day(1);
            set_month(1);
            set_year(2000);
        }
        public Date(int day, int month, int year)
        {
            set_day(day);
            set_month(month);
            set_year(year);
        }
        public void show_date()
        {
            Console.WriteLine($"{day}.{month}.{year}");
        }
        public bool date_check()
        {
            if (this.day == 0 || this.month == 0 || this.year == 0) return false;
            else return true;
        }
        public void date_writer()
        {
            string path_to_file = @"C:\Users\Катюша\source\repos\Viktorina_EXAM\users_information.txt";
            File.AppendAllText(path_to_file, $"{day}.{month}.{year}");
        }
        public string ToString()
        {
            return (Convert.ToString(day)+"."+Convert.ToString(month)+"."+Convert.ToString(year));
        }
    }
    public class User //Класс Пользователь
    {
        private string name;
        public void set_name(string name) { this.name = name; }
        public string get_name() {return this.name;}
        private Date date;
        public void set_date(Date date) { this.date = date; }
        public Date get_date() { return this.date; }
        private Login login;
        public void set_login(Login login) { this.login = login; }
        public Login get_login() { return this.login; }

        private Password password;
        public void set_password(Password password) { this.password = password; }
        public Password get_password() { return this.password; }

        public User()
        {
            this.name = "Name";
            this.date = new Date();
            this.login = new Login();
            this.password = new Password();
        }
        public User (string name, Date date, Login login, Password password)
        {
            this.name = name;
            this.date = date;
            this.login = login;
            this.password = password;
        }
        public void show_user()
        {
            Console.WriteLine($"Name: {this.name}  Birthday: {this.date.ToString()}  " +
                $"Login: {this.login.ToString()}  Password: {this.password.ToString()}");
        }

        public string UserToString()
        {
            return (name +" "+ date.ToString() + " " + login.ToString() + " " + password.ToString() + "\n");
        }
        public void user_writer(string path)
        {
            FileStream fs= new FileStream(path, FileMode.Append, FileAccess.Write);
            string data = UserToString();
            byte[] writeBytes = Encoding.Default.GetBytes(data);
            fs.Write(writeBytes);
            fs.Close();   
        }
        /*public void user_reader(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] readBytes = new byte[(int)fs.Length];
            fs.Read(readBytes, 0, readBytes.Length);
            string data = Encoding.Default.GetString(readBytes);
            Console.WriteLine("\n\nСчитываем с файла\n\n" + data);
            fs.Close();
        }*/

    }

    public  static void Main(string[] args)
    {
        Console.WriteLine("\nУвлекательная викторина!\n\nПриветствуем! Вы у нас впервые? ");
        Console.WriteLine("->Да\n  Нет");
        bool check_answer = true;
        ConsoleKey key = Console.ReadKey().Key;
        while (key != ConsoleKey.Enter) //работа меню, пока не нажат Enter, стрелка двигается вверх-вниз
        {
            if ((key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow) && check_answer == true)
            {
                Console.Clear();
                Console.WriteLine("\nУвлекательная викторина!\n\nПриветствуем! Вы у нас впервые? ");
                Console.WriteLine("  Да\n->Нет");
                check_answer = !(check_answer);
                key = Console.ReadKey().Key;
                continue;
            }
            else if ((key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow) && check_answer == false)
            {
                Console.Clear();
                Console.WriteLine("\nУвлекательная викторина!\n\nПриветствуем! Вы у нас впервые? ");
                Console.WriteLine("->Да\n  Нет");
                check_answer = !(check_answer);
                key = Console.ReadKey().Key;
                continue;
            }
        }; // Меню

        if (check_answer) //Регистрация
        {
            GetRegistration();
        }
        else
        {
            Console.Clear();
            Console.Write("\nВведите логин: ");
            string user_login= Console.ReadLine();
            bool check = EnterByLogin(user_login); //Вход по логину
            while (!check) //Если введен неправильный логин
            {
                    Console.Clear();
                    Console.WriteLine("\nВведен неверный логин.\n");
                    Console.WriteLine("\n->Повторить ввод\n  Пройти регистрацию");
                    check_answer = true;
                    key = Console.ReadKey().Key;
                    while (key != ConsoleKey.Enter) //работа меню, пока не нажат Enter, стрелка двигается вверх-вниз
                    {
                        if ((key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow) && check_answer == true)
                        {
                            Console.Clear();
                            Console.WriteLine("\nВведен неверный логин.\n");
                            Console.WriteLine("\n  Повторить ввод\n->Пройти регистрацию");
                            check_answer = !(check_answer);
                            key = Console.ReadKey().Key;
                            continue;
                        }
                        else if ((key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow) && check_answer == false)
                        {
                            Console.Clear();
                            Console.WriteLine("\nВведен неверный логин.\n");
                            Console.WriteLine("\n->Повторить ввод\n Пройти регистрацию");
                            check_answer = !(check_answer);
                            key = Console.ReadKey().Key;
                            continue;
                        }
                    }
                if (check_answer) 
                {
                    Console.Clear();
                    Console.Write("\nВведите логин: ");
                    user_login = Console.ReadLine();
                    EnterByLogin(user_login); 
                }
                else { GetRegistration(); check = true; break; }//Возврат в регистрацию
            }

            while(!EnterByPassword(user_login)) //Проверка совпадения логина и пароля
            {
                Console.WriteLine("\nПароль не корректен. Повторите ввод пароля!\n");
            }
        }
        Console.Clear() ;//Выбор темы викторины
        Console.WriteLine("\nНачинаем викторину! Выберете тему: \n");
        Console.WriteLine("\n->История");
        Console.WriteLine("\n  География");
        Console.WriteLine("\n  Смешанная тема");

        int index_answer = 1;
        key = Console.ReadKey().Key;
        while (key != ConsoleKey.Enter) //работа меню, пока не нажат Enter, стрелка двигается вверх-вниз
        {
            if (key==ConsoleKey.DownArrow && index_answer==1)
            {
                Console.Clear();
                Console.WriteLine("\nНачинаем викторину! Выберете тему: \n");
                Console.WriteLine("\n  История");
                Console.WriteLine("\n->География");
                Console.WriteLine("\n  Смешанная тема");
                index_answer = 2;
                key = Console.ReadKey().Key;
                continue;
            }
            if (key == ConsoleKey.UpArrow && index_answer == 1)
            {
                Console.Clear();
                Console.WriteLine("\nНачинаем викторину! Выберете тему: \n");
                Console.WriteLine("\n  История");
                Console.WriteLine("\n  География");
                Console.WriteLine("\n->Смешанная тема");
                index_answer = 3;
                key = Console.ReadKey().Key;
                continue;
            }
            if (key == ConsoleKey.DownArrow && index_answer == 2)
            {
                Console.Clear();
                Console.WriteLine("\nНачинаем викторину! Выберете тему: \n");
                Console.WriteLine("\n  История");
                Console.WriteLine("\n  География");
                Console.WriteLine("\n->Смешанная тема");
                index_answer = 3;
                key = Console.ReadKey().Key;
                continue;
            }
            if (key == ConsoleKey.UpArrow && index_answer == 3)
            {
                Console.Clear();
                Console.WriteLine("\nНачинаем викторину! Выберете тему: \n");
                Console.WriteLine("\n  История");
                Console.WriteLine("\n->География");
                Console.WriteLine("\n  Смешанная тема");
                index_answer = 2;
                key = Console.ReadKey().Key;
                continue;
            }
            if (key == ConsoleKey.DownArrow && index_answer == 3)
            {
                Console.Clear();
                Console.WriteLine("\nНачинаем викторину! Выберете тему: \n");
                Console.WriteLine("\n->История");
                Console.WriteLine("\n  География");
                Console.WriteLine("\n  Смешанная тема");
                index_answer = 1;
                key = Console.ReadKey().Key;
                continue;
            }
            if (key == ConsoleKey.UpArrow && index_answer == 2)
            {
                Console.Clear();
                Console.WriteLine("\nНачинаем викторину! Выберете тему: \n");
                Console.WriteLine("\n->История");
                Console.WriteLine("\n  География");
                Console.WriteLine("\n  Смешанная тема");
                index_answer = 1;
                key = Console.ReadKey().Key;
                continue;
            }
            
        }
        List<string> questions;
        int marks = 0; //баллы за правильные ответы
        switch(index_answer)
        {
            case 1:
                {
                    Console.Clear();
                    questions=FileReader("history.txt");
                    int numer = 0;
                    for (int i = 0;i<questions.Count();i++)
                    {
                        index_answer = ValueAnswer(questions, numer, ref marks, 1);
                        i += 3;
                        numer++;

                    }
                    
                    break;
                }
            case 2:
                {
                    Console.Clear();
                    questions=FileReader("geography.txt");
                    int numer = 0;
                    for (int i = 0; i < questions.Count(); i++)
                    {
                        index_answer = ValueAnswer(questions, numer, ref marks, 2);
                        i += 3;
                        numer++;

                    }
                    break;
                }
            case 3:
                {
                    Console.Clear();
                    questions = FileReader("mix.txt");
                    Random random = new Random();

                    int numer = random.Next(0,40);
                    for (int i = 0; i < questions.Count(); i++)
                    {
                        Console.WriteLine("Random question is " + numer);
                        Console.ReadLine();
                        index_answer = ValueAnswer(questions, numer, ref marks, 3);
                        i += 3;
                        numer = random.Next(0, 40); 

                    }
                    break;
                }
        }


    }
}
