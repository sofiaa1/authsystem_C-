using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;
using System;
using System.Text.RegularExpressions;

class Program
{
    static MySqlConnection connection = new MySqlConnection("server=localhost; username=root; port=4000; database=authsystem;password=");

    public static void login()
    {
        string login, password;

        Console.WriteLine("Login");

        Console.Write("Login: ");
        login = Console.ReadLine();

        Console.Write("Password: ");
        password = Console.ReadLine();

        string searchUser = "SELECT * FROM users WHERE login = @value1";
        MySqlCommand searchUserCommand = new MySqlCommand(searchUser, connection);
        searchUserCommand.Parameters.AddWithValue("@value1", login);

        try
        {
            MySqlDataReader userData = searchUserCommand.ExecuteReader();

            if (userData.Read())
            {
                if (password == userData["password"].ToString())
                { 
                    int userOptions;
                    Console.WriteLine("-> ");
                    userOptions = Convert.ToInt32(Console.ReadLine());


                    while (true)
                    {
                        Console.Clear();
                        switch (userOptions)
                        {
                            case 1:
                                myAccount(login);
                                break;
                            case 2:
                                otherAccounts(login);
                                break;
                            case 3:
                                return;
                            default:
                                Console.WriteLine("Option does not exisr!");
                                break;
                        }
                        Console.WriteLine("User menu");
                        Console.WriteLine("1. My account\n2. Other accounts\n3. Log out");
                        Console.WriteLine("-> ");
                        userOptions = Convert.ToInt32(Console.ReadLine());
                    }

                }
                else
                {
                    Console.WriteLine("Wrong credentials!");
                }
            }
            else
            {
                Console.WriteLine("Wrong credentials!");
            }

            userData.Close();
        }
        catch (MySqlException e)
        {
            Console.WriteLine("Wrong credentials");
        }
    }



    public static void myAccount(string login)
    {
        string accountSelect = "SELECT * FROM users WHERE login = @v1";
        MySqlCommand accountSelectComamnd = new MySqlCommand(accountSelect, connection);
        accountSelectComamnd.Parameters.AddWithValue("v1", login);
        MySqlDataReader accountReader = accountSelectComamnd.ExecuteReader();

        while (accountReader.Read())
        {
            Console.WriteLine("---Account---");
            Console.WriteLine($"Login: {accountReader["login"].ToString()}");
            Console.WriteLine($"Email: {accountReader["email"].ToString()}");
        }

        Console.Write("Enter any charater to exit -> ");
        char x = Console.ReadLine()[0];
    }

    public static void otherAccounts(string login)
    {

    }




    public static void register()
    {
        string login, password, email;
        Console.WriteLine("---Register---");
        Console.Write("Login: ");
        login = Console.ReadLine();

        while (!Regex.IsMatch(login, @"^(?=.*[a-zA-Z])[a-zA-Z0-9]{4,20}$"))
        {
            Console.Write("Login does not correspond to the rules. Retry!");
            Console.Write("New login: ");
            login = Console.ReadLine();
        }

        Console.Write("Password: ");
        password = Console.ReadLine();
        while (!Regex.IsMatch(password, @"^[A-Za-z0-9!@#$.]{8,200}$"))
        {
            Console.Write("Password does not correspond to the rules. Retry!");
            Console.Write("New password: ");
            password = Console.ReadLine();
        }

        Console.Write("Email: ");
        email = Console.ReadLine();
        while (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            Console.Write("Email does not correspond to the rules. Retry!");
            Console.Write("New email: ");
            email = Console.ReadLine();
        }

        string createUser = "INSERT INTO users (login, password, email) VALUES (@v1, @v2, @v3)";
        MySqlCommand createUserCommand = new MySqlCommand(createUser, connection);
        createUserCommand.Parameters.AddWithValue("@v1", login);
        createUserCommand.Parameters.AddWithValue("@v2", password);
        createUserCommand.Parameters.AddWithValue("@v3", email);

        try
        {
            connection.Open();
            int rowsAffected = createUserCommand.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("User was created");
            }
            else
            {
                Console.WriteLine("Something went wrong...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }




    public static void Main(string[] args)
    {
        int optionMain;

        Console.WriteLine("~Welcome to Social Club~");
        Console.WriteLine("1. Login\n2. Register\n3. Close");
        Console.Write("-> ");
        optionMain = Convert.ToInt32(Console.ReadLine());
        while (true)
        {
            Console.Clear();

            switch (optionMain)
            {
                case 1:
                    login();
                    break;
                case 2:
                    register();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("!---Option does not exist---!");
                    break;
            }
            Console.WriteLine("1. Login\n2. Register\n3. Close");
            Console.Write("-> ");
            optionMain = Convert.ToInt32(Console.ReadLine());
        }
    }
}
