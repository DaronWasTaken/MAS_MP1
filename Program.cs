// See https://aka.ms/new-console-template for more information

using System.Text.Json;

namespace MP1;

public static class Program
{
    private static List<User>? _users;
    private const string FilePath = "userData.txt";
    public static void Main()
    {
        _users = ReadFromFile(FilePath);
        var isEnded = false;
        Console.WriteLine("Hi there! What would you like to do?");
        while (!isEnded)
        {
            Console.WriteLine("1) Add a user");
            Console.WriteLine("2) Delete the user");
            Console.WriteLine("3) Print the users");
            Console.WriteLine("4) Exit the program");

            var expression = Console.ReadLine();

            switch (expression)
            {
                case "1":
                    AddUser();
                    break;
                case "2":
                    DeleteUser();
                    break;
                case "3":
                    PrintUsers();
                    break;
                case "4":
                    isEnded = true;
                    break;
                default:
                    Console.WriteLine("I'm not sure what you want me to do!");
                    break;
            }
        }
    }

    private static void PrintUsers()
    {
        _users?.ForEach(user => Console.WriteLine($"User: {user.Username}"));
    }

    private static void DeleteUser()
    {
        Console.WriteLine("Which user should be deleted: ");
        var input = Console.ReadLine();
        var foundUser = _users?.Find(user => user.Username == input);
        if (foundUser != null) _users?.Remove(foundUser);
        WriteToFile(FilePath, SerializeUsers(_users));
    }

    private static void AddUser()
    {
        Console.WriteLine("What should the user be called?");
        var username = Console.ReadLine();
        _users?.Add(new User { Username = username });
        
        if (_users == null) 
            return;
        
        var fileContent = SerializeUsers(_users);
        WriteToFile(FilePath, fileContent);
    }

    private static string SerializeUsers(List<User>? users)
    {
        return JsonSerializer.Serialize(users);
    }

    private static List<User>? DeserializeUsers(string jsonString)
    {
        return JsonSerializer.Deserialize<List<User>>(jsonString);
    }

    private static void WriteToFile(string filePath, string fileContent)
    {
        File.WriteAllText(filePath, fileContent);
    }

    private static List<User>? ReadFromFile(string filePath)
    {
        try
        {
            var fileContent = File.ReadAllText(filePath);
            return DeserializeUsers(fileContent);
        }
        catch (FileNotFoundException e)
        {
            File.WriteAllText(FilePath, null);
            return new List<User>();
        }
    }
}