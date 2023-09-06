namespace adventure;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Text Adventure!");
        Hero hero = new Hero();
        while (hero.Location != "quit") // Game-loop, after each location the game comes back here and goes to the next
        {
            if (hero.Location == "newgame")
            {
                NewGame(hero);
            }

            else if (hero.Location == "tableroom")
            {
                TableRoom(hero);
            }
            else if (hero.Location == "corridor")
            {
                Corridor(hero);
            }
            else if (hero.Location == "lockedRoom")
            {
                LockedRoom(hero);
            }
            else if (hero.Location == "thirdRoom")
            {
                ThirdRoom(hero);
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
        Console.Clear();
        do
        {
            foreach (var item in choices) // Prints out the new items to look more nice
            {
                // TODO fix grammar 1, 2 or 3
                itemList += item;
                if (choices.IndexOf(item) == choices.Count-2) // Add an or when it's the 2nd to last item
                {
                    itemList += " or ";
                }
                else if (choices.IndexOf(item) + 1 != choices.Count) // Prints out comma unless it's the last item
                {
                    itemList += ", ";
                }
            }
            newItem = Ask($"Which item would you like out of {itemList}?");
            if (!choices.Contains(newItem)) // If the answer is not in "choices" it asks again
            {
                Console.WriteLine("Please choose one of the given items");
            }
        } while (!AskYesOrNo($"Are you sure you would like {newItem}?") && !choices.Contains(newItem)); // You have to say yes to wanting the item and it needs to be a valid item to move on
        Console.WriteLine($"You pick up the {newItem}");
        hero.Items.Add(newItem);
        Console.WriteLine("You pick up the key");

        hero.Location = "corridor";
    }

    static void Corridor(Hero hero)
    {
        Console.Clear();
        Console.WriteLine("You exit the room and find yourself standing in a dark " + 
                          "hallway. You can either enter another room on your right " + 
                          "side, or continue down the hallway on your left.");
        answer = Console.Readline();
        if (answer == "right")
        {
            if (hero.Items.Contains("key"))
            {
                hero.Location = "lockedRoom";
                hero.Items.Remove("key");
            }
            else
            {
                hero.Location = "thirdRoom";
            }
        }
        else if (answer == "left")
        {
            hero.Location = "thirdRoom";
        }
    }

    static void LockedRoom(Hero hero)
    {
        Console.Clear();
        Console.WriteLine("Inside the locked room " +
                          "you find a shiny sword!");
        if (AskYesOrNo("Do you want it instead of " +
                       "your wooden sword? "))
        {
            hero.Items.Remove("woodensword");
            hero.Items.Add("shinysword");
        }

        hero.Location = "thirdRoom";
    }

    static void ThirdRoom(Hero hero)
    {
        Console.Clear();
        Console.WriteLine("On the floor before you lies a lifeless corpse.\n" +
                          "Its hand is clasped around something shiny.\n");
        if (AskYesOrNo("Would you like to loot the body?"))
        {
            if (RollD6() >= 3)
            {
                Console.WriteLine("A warm feeling spreads over your body.");
                hero.Items.Add("blessed amulet");
            }
            else
            {
                Console.WriteLine("A cold shiver runs down your spine.");
                hero.Items.Add("cursed amulet");
            }
        }
        Console.WriteLine("You leave the corpse and continue into the \n" +
                          "next room.");
        Console.Read();
        hero.Location = "backOutside"
    }

    static int RollD6()
    {
        return new Random().Next() % 6 + 1;
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