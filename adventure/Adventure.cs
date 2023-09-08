using System.Security.Principal;

namespace adventure;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Text Adventure!"); 
        //Initiates the hero and the boss
        Hero hero = new Hero();
        Enemy boss = new Enemy("Minotaur", 250, 10);
        while (hero.Location != "quit") // Game-loop, after each location the game comes back here and goes to the next
        {
            Console.WriteLine("Press Enter to continue"); //makes sure no text is cleaned away too fast since rooms clear on entry
            Console.ReadLine();
            
            if (hero.Location == "newGame")
            {
                NewGame(hero);
            }
            else if (hero.Location == "tableRoom")
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
            else if (hero.Location == "exitRoom")
            {
                ExitRoom(hero);
            }
            else if (hero.Location == "backOutside")
            {
                BackOutside(hero, boss);
            }
            else if (hero.Location == "bossFight")
            {
                BossFight(hero, boss);
            }
            else if (hero.Location == "win")
            {
                Win(hero);
            }
            else if (hero.Location == "lose")
            {
                Lose(hero);
            }
            else if (hero.Location == "gameOver")
            {
                GameOver(hero, boss);
            }
            else
            {
                Console.Error.WriteLine($"You forgot to implement {hero.Location}!");
            }
        }
    }

    static void NewGame(Hero hero) //lets user input a name for the hero and then starts the adventure
    {
        Console.Clear();
        string name;
        do
        {
            name = Ask("What is your name?");
        } while (!AskYesOrNo($"So, {name} is your name?"));

        hero.Name = name;
        hero.Location = "tableRoom";
    }
    
    static void TableRoom(Hero hero)
    {
        Console.Clear();
        hero.Items.Add("Wooden Sword");
        Console.WriteLine("You are equipped with one wooden sword, and your task " +
                          "is to slay the monster at the end of the adventure. " +
                          "In front of you is a stone table with two items on it, " +
                          "a knife and a key. " +
                          "You can only pick up one of these items.");
        
        string answer;
        do //Loops until the player chooses one of the three options, then add to inventory as necessary
        {
            answer = Ask("Would you like the knife, the key or neither?");
            if (answer == "the knife" || answer == "knife")
            {
                if (AskYesOrNo("Are you sure you'd like the knife?"))
                {
                    hero.Items.Add("knife");
                    Console.WriteLine("You pick up the knife");
                    hero.Location = "corridor";
                }
            }
            else if (answer == "the key" || answer == "key")
            {
                if (AskYesOrNo("Are you sure you'd like the key?"))
                {
                    hero.Items.Add("key");
                    Console.WriteLine("You pick up the key");
                    hero.Location = "corridor";
                }
            }
            else if (answer == "neither")
            {
                if (AskYesOrNo("Are you sure you want neither of them?"))
                {
                    Console.WriteLine("you choose to leave both items behind");
                    hero.Location = "corridor";
                }
            }
            else
            {
            Console.WriteLine("Please choose a valid option");
            }
        } while (hero.Location == "tableRoom"); //loops until the hero leaves the room by picking a valid option
    }

    static void Corridor(Hero hero)
    {
        Console.Clear();
        Console.WriteLine("You exit the room and find yourself standing in a dark " + 
                          "hallway. You can either enter another room on your right " + 
                          "side, or continue down the hallway on your left.");
        
        if (AskYesOrNo("Would you like to check the door on the right side?")) //option to check an extra room
        {
            Console.WriteLine("You find yourself in front of a locked door");
            if (hero.Items.Contains("key")) //lets the player enter if they have the key
            {
                Console.WriteLine("Using your key you open the locked room and enter it");
                hero.Items.Remove("key");
                hero.Location = "lockedRoom";
            }
            else
            {
                Console.WriteLine("You cant open the door since you dont have the Key...");
                Console.WriteLine("You continue down the corridor on your left");
                hero.Location = "thirdRoom";
            }
        }
        
        
    }
    static void LockedRoom(Hero hero)
    {
        Console.Clear();
        Console.WriteLine("Inside the locked room " +
                          "you find a shiny sword!");
        
        if (AskYesOrNo("Do you want it instead of your wooden sword?")) // Lets the player exchange their old sword for a better one
        {
            hero.Items.Remove("woodensword");
            hero.Items.Add("shinysword");
            Console.WriteLine("You pick up the shiny sword and leave your old wooden sword behind");
        }
        Console.WriteLine("you exit the locked room and continue down the corridor");
        hero.Location = "thirdRoom";
    }

    static void ThirdRoom(Hero hero)
    {
        Console.Clear();
        Console.WriteLine("You enter the room at the end of the corridor.\n" +
                          "On the floor before you lies a lifeless corpse.\n" +
                          "Its hand is clasped around something shiny.\n");
        
        if (AskYesOrNo("Would you like to loot the body?")) //looting the body has a chance of giving a beneficial item
        {
            Console.Clear();
            if (RollD6() >= 3) //67% chance to get the beneficial item
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
        Console.WriteLine("You leave the corpse and continue into the next room.");
        hero.Location = "exitRoom";
    }

    static void ExitRoom(Hero hero)
    {
        bool doorOpen = false;
        Console.Clear();
        Console.WriteLine("You enter the room");
        Console.WriteLine("You notice a big door at the opposite wall and a lever in the corner");
        
        string answer;
        do // the player has to solve the puzzle in the room before continuing
        {
            answer = Ask("Do you want to investigate the door or the lever?");
            if (answer == "the door" || answer == "door")
            {
                Console.Clear();
                if (doorOpen)
                {
                    Console.WriteLine("You open the door and exit the dungeon");
                    hero.Location = "backOutside";
                }
                else
                {
                    Console.WriteLine("The door seems to be locked");
                    Console.WriteLine("you return to the start of the room");
                }
            }
            else if (answer == "the lever" || answer == "lever")
            {
                Console.Clear();
                Console.WriteLine("You walk over to the lever");
                if (AskYesOrNo("Would you like to pull it?"))
                {
                    Console.WriteLine("You hear a loud click sound coming from the door");
                    doorOpen = !doorOpen; //pulling multiple times will lock and unlock the door
                }
                else
                {
                    Console.WriteLine("you decide not to pull the lever");
                }
                Console.WriteLine("you return to the start of the room");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("please pick a valid choice");
            }
        } while (hero.Location == "exitRoom"); //loops until they manage to unlock and use the door
    }

    static void BackOutside(Hero hero, Enemy boss)
    {
        Console.Clear();
        Console.WriteLine("As you exit the dungeon you notice a minotaur charging towards you");
        if (hero.Items.Contains("knife")) //the player gets a chance to damage the boss in advance
        {
            if (AskYesOrNo("Would you like to throw your knife at the charging minotaur?"))
            {
                Console.WriteLine("You hit it in its eye!");
                hero.Items.Remove("knife");
                boss.Health -= 200;
            }
        }
        hero.Location = "bossFight";
    }

    static void BossFight(Hero hero, Enemy boss)
    {
        Console.Clear();
        Console.WriteLine($"You have {hero.Health}/100 HP"); //writes the hero's exact hp and a rough estimate for the boss
        if (boss.Health>=250/2)
        {
            Console.WriteLine($"The {boss.Name} is furious");
        }
        else if (boss.Health>=50)
        {
            Console.WriteLine($"The {boss.Name} has started bleeding here and there");
        }
        else
        {
            Console.WriteLine($"The {boss.Name} is barely standing");
        }
        
        string bossAttack = "";
        if (boss.TurnCounter%2 == 0) // The boss attacks every other round
        {
            int bossMove = RollD6() % 3 + 1; // The boss randomly uses 3 different attacks
            switch (bossMove)
            {
                case 1:
                    Console.WriteLine($"The {boss.Name} is preparing to Slam the ground");
                    bossAttack = "Slam";
                    break;
                case 2:
                    Console.WriteLine($"The {boss.Name} is preparing to Swipe your legs");
                    bossAttack = "Swipe";
                    break;
                case 3:
                    Console.WriteLine($"The {boss.Name} is preparing to Punch");
                    bossAttack = "Punch";
                    break;
            }
        }
        else
        {
            Console.WriteLine($"The {boss.Name} is charging up its next attack");
        }
        
        string move = Ask("Would you like to attack, dodge, jump or parry?"); //the player gets to choose what to do
        while (!(move == "attack" || move == "dodge" || move == "jump" || move == "parry")) //checks so that the action is valid before continuing
        {
            Console.WriteLine("Please choose one of the given options");
            move = Ask("Would you like to attack, dodge, jump or parry?");
        }

        if (move == "attack")
        {
            Console.WriteLine("You strike with your sword");
            
            if (hero.Items.Contains("woodensword"))
            {
                boss.Health -= 5;
            }
            else if (hero.Items.Contains("shinysword"))
            {
                boss.Health -= 25;
            }
        }

        if (boss.Health <= 0) //check if the boss is dead before it attacks
        {
            Console.WriteLine($"You've bested The {boss.Name}");
            hero.Location = "win";
            return;
        }

        if (bossAttack == "Slam" && move == "dodge") // If the player uses the correct response, they take no damage
        {
            Console.WriteLine("You dodged the attack");
        }
        else if (bossAttack == "Swipe" && move == "jump")
        {
            Console.WriteLine("You jumped over the attack");
        }
        else if (bossAttack == "Punch" && move == "parry")
        {
            Console.WriteLine("You parried its punch");
        }
        else if (bossAttack != "")
        {
            if (bossAttack == "Slam")
            {
                Console.WriteLine($"The {boss.Name} slams its club into you, you take {2 * boss.Damage} damage.");
                hero.Health -= 2 * boss.Damage;
            }
            else if (bossAttack == "Swipe")
            {
                Console.WriteLine($"The {boss.Name} swipes your legs with its club, you take {boss.Damage/2} damage.");
                hero.Health -= boss.Damage/2;
            }
            else
            {
                Console.WriteLine($"The {boss.Name} punches you in the face, you take {boss.Damage} damage.");
                hero.Health -= boss.Damage;
            }
        }
        
        if (hero.Health <= 0) // check if the hero is dead
        {
            if (hero.Items.Contains("blessed amulet")) //if you have the blessed amulet you get another chance
            {
                hero.Health = 50;
                Console.WriteLine("As you're dying you feel the amulet reinvigorate you");
            }
            else
            {
                Console.WriteLine($"You were slain by The {boss.Name}");
                hero.Location = "lose";
            }
        }
        boss.TurnCounter++; //increment the turn counter to track boss attack
    }

    static void Win(Hero hero)
    {
        Console.Clear();
        Console.WriteLine($"Congratulations {hero.Name}, you have conquered the dungeon!!!");
        hero.Location = "gameOver";
    }
    
    static void Lose(Hero hero)
    {
        Console.Clear();
        Console.WriteLine("You never escaped the dungeon, you lose...");
        hero.Location = "gameOver";
    }

    static void GameOver(Hero hero, Enemy boss) //lets the player choose to play again
    {
        if (AskYesOrNo("Would you like to play again?")) // if yes reset the player and boss as necessary
        {
            hero.Health = 100;
            hero.Items = new List<string>();
            hero.Location = "newGame";

            boss.Health = 250;
        }
        else
        {
            hero.Location = "quit";
        }
    }

    static int RollD6() // returns a random number between 1 and 6
    {
        return new Random().Next() % 6 + 1;
    }
    static bool AskYesOrNo(string question) //lets the player confirm a question/option/answer
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
            Console.WriteLine("Please answer yes or no");
        }
    }
    static string Ask(string question) //asks a question and returns the answer trimmed and in lowercase
    {
        string response;
        do
        {
            Console.WriteLine(question);
            response = Console.ReadLine().Trim().ToLower();
        } while (response == "");

        return response;
    }
}

class Hero //the hero, saves relevant data
{
    public string Name = "";
    public int Health = 100;
    public List<string> Items = new List<string>();
    public string Location = "newGame";
}

class Enemy //generic enemy class, currently just used for the boss
{
    public string Name;
    public int Health;
    public int Damage;
    public int Armor;
    public int TurnCounter = 0;

    public Enemy(string Name, int Health, int Damage, int Armor = 0) //armor is optional
    {
        this.Name = Name;
        this.Health = Health;
        this.Damage = Damage;
        this.Armor = Armor;
    }
}