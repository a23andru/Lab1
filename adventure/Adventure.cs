namespace adventure;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Text Adventure!");
        Hero hero = new Hero();
        while (hero.Location != "quit")
        {
            if (hero.Location == "newgame")
            {
                NewGame(hero);
            }

            else if (hero.Location == "tableroom")
            {
                TableRoom(hero);
            }
            else
            {
                Console.Error.WriteLine($"You forgot to implement {hero.Location}!");
            }
        }
    }

    static void NewGame(Hero hero)
    {
        Console.Clear();
        string name = "";
        do
        {
            name = Ask("What is your name?");
        } while (!AskYesOrNo($"So, {name} is your name?"));

        hero.Name = name;
        hero.Location = "tableroom";
    }
    static void TableRoom(Hero hero)
    {
        Console.Clear();
        hero.Items.Add("Wooden Sword");
        Console.WriteLine("You are equipped with one wooden sword, and your task " +
                          "is to slay the monster at the end of the adventure. " +
                          "In front of you is a stone table with two items on it, " +
                          "a knife and a key." +
                          "You can only pick up one of these items.");
        List<string> options = new List<string>();
        options.Add("knife");         options.Add("key");        options.Add("neither");
        AddNewItem(options, hero);
        
    }

    static void AddNewItem(List<string> choices, Hero hero)
    {
        string newItem = "";
        string itemList = "";
        do
        {
            foreach (var item in choices)
            {
                // TODO fix grammar 1, 2 or 3
                itemList += item;
                if (choices.IndexOf(item) == choices.Count)
                {
                    itemList += " or ";
                }
                else if (choices.IndexOf(item) + 1 != choices.Count)
                {
                    itemList += ", ";
                }
            }
            newItem = Ask("Which item would you like out of " + itemList + "?");
            if (!choices.Contains(newItem))
            {
                Console.WriteLine("Please choose one of the given items");
            }
        } while (!AskYesOrNo($"Are you sure you would like {newItem}") && !choices.Contains(newItem));
        Console.WriteLine($"You pick up the {newItem}");
        hero.Items.Add(newItem);

        hero.Location = "corridor";
    }
    static bool AskYesOrNo(string question)
    {
        while (true)
        {
            string response = Ask(question).ToLower();
            switch (response)
            {
                case "yes":
                case "ok":
                    return true;
                case "no":
                    return false;
            }
        }
    }
    static string Ask(string question)
    {
        string response;
        do
        {
            Console.WriteLine(question);
            response = Console.ReadLine().Trim();
        } while (response == "");

        return response;
    }
}

class Hero
{
    public string Name = "";
    public int Health = 100;
    public List<string> Items = new List<string>();
    public string Location = "newgame";
}