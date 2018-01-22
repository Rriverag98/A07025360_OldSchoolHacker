using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour
{

    [SerializeField] AudioClip playerLost;
    static AudioSource audioSrc;

    const string menuHint = "You can type menu at any time.";
    //These arrays will hold the passwords for each level
    string[] passwordLevel1 = { "book", "class", "teacher", "room", "hour" };
    string[] passwordLevel2 = { "cashier", "department", "payment", "electronics" };
    string[] passwordLevel3 = { "dossier", "international", "security" };
    int level;
    int tries = 3;
    //enumerated tipe to represent the different states, variable is declared to hold the 
    //current game state
    enum GameState { MainMenu, Password, Win, Lost };     
    GameState currentScreen = GameState.MainMenu;   
    string password;                                
    bool changePassword = true;
    // Use this for initialization
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        ShowMainMenu(); 
    }
    
    void Update()
    {

    }
    
    void ShowMainMenu()
    {
        //  We clear the screen
        Terminal.ClearScreen();

        //  We show the menu
        Terminal.WriteLine("What will you hack today?");
        Terminal.WriteLine("");
        Terminal.WriteLine("1. Town´s College");
        Terminal.WriteLine("2. City´s Super Center");
        Terminal.WriteLine("3. NSA Server (You only have 3 tries)");
        Terminal.WriteLine("");
        Terminal.WriteLine("Option?");

        currentScreen = GameState.MainMenu;
    }

    void OnUserInput(string input)
    {
        //if user inputs menu then main menu is shown
        if (input == "menu")
        {
            ShowMainMenu();
        }
        else if (input == "quit" || input == "close" || input == "exit") // if user inputs quit or closed 
        {
            Terminal.WriteLine("Please close the browser's tab");
            Application.Quit();
        }
        //if the user inputs something different to menu,quit or close, the input will be
        //handled depending the gamestate.
        else if (currentScreen == GameState.MainMenu)
        {
            RunMainMenu(input);
        }
        //if GameState is password, then we must call checkPassword().
        else if (currentScreen == GameState.Password)
        {
            CheckPassword(input);
        }
    }
    
    private void CheckPassword(string input)
    { 
        if (input == password)
        {
            changePassword = true;
            DisplayWinScreen();
        }
        else if (tries == 1)
        {
            DisplayLoseScreen();
        } else
        {
            AskForPassword();
        }
    }

    private void DisplayLoseScreen()
    {
        currentScreen = GameState.Lost;
        Terminal.ClearScreen();
        Terminal.WriteLine(@"
  ___         _          _ _ 
 | _ )_  _ __| |_ ___ __| | |
 | _ \ || (_-<  _/ -_) _` |_|
 |___/\_,_/__/\__\___\__,_(_)");
        Terminal.WriteLine("You failed to hack the NSA server");
        audioSrc.clip = playerLost;
        audioSrc.Play();
        Terminal.WriteLine(menuHint);
    }
    
    private void DisplayWinScreen()
    {
        currentScreen = GameState.Win;
        Terminal.ClearScreen();
        ShowLevelReward();
        Terminal.WriteLine(menuHint);
    }
    
    private void ShowLevelReward()
    {
        switch (level)
        {
            case 1:
                Terminal.WriteLine("Have a book...");
                Terminal.WriteLine(@"
    _______
   /      //
  /      //
 /______//
(______(/
                ");
                break;
            case 2:
                Terminal.WriteLine("Grab a lime!");
                Terminal.WriteLine(@"
     /
  __|__
 /     \
|       )
 \_____/
                ");
                break;
            case 3:
                Terminal.WriteLine("Greetings...");
                Terminal.WriteLine(@"
 _ __   ___  __ _
| '_ \ / __|/ _` |
| | | |\__ \ (_| |
|_| |_||___)\__,_|
                ");
                Terminal.WriteLine("Welcome to the NSA server");
                break;
            default:
                Debug.LogError("Invalid level reached.");
                break;
        }
    }

    void sevenPt()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine(@"
 ____     _     ___               
|__  | __| |_  |   \ _ _ ___ _ __ 
  / / '_ \  _| | |) | '_/ _ \ '_ \
 /_/| .__/\__| |___/|_| \___/ .__/
    |_|                     |_|   
                ");
        Terminal.WriteLine("You made 1,570 computer systems crash");
        Terminal.WriteLine("and caused a 7 point drop in the NYSE");
        Terminal.WriteLine("Here´s your $45,000 fine");
        Terminal.WriteLine(menuHint);
    }
    
    void RunMainMenu(string input)
    {
        //Validate that input is valid
        bool isValidInput = (input == "1") || (input == "2") || (input == "3");
        //If it is then we convert the input into an int and assign it to level
        //and call askForPassword();
        input = input.ToLower();
        if (isValidInput)
        {
            level = int.Parse(input);
            AskForPassword();
        }
        else if (input == "007")  //if the input is invalid, check if its an easter egg
        {
            Terminal.WriteLine("Please enter a valid level, Mr. Bond");
        }
        else if (input == "zero cool")  //if the input is invalid, check if its an easter egg
        {
            sevenPt();
        }
        else
        {
            Terminal.WriteLine("Enter a valid level");
        }
    }
    
    private void AskForPassword()
    {
        switch (level)
        {
            case 3:
                //set currentScreen as GameState password
                currentScreen = GameState.Password;
                //Clear Screen
                Terminal.ClearScreen();
                tries--;
                //Call setRandomPassord to set a password
                if (changePassword)
                {
                    tries = 3;
                    SetRandomPassword();
                }
                Terminal.WriteLine("Enter your password. Hint: " + password.Anagram());
                Terminal.WriteLine("You have " + (tries) + " tries left");
                //show menuHint
                Terminal.WriteLine(menuHint);
                break;

            default:
                //set currentScreen as GameState password
                currentScreen = GameState.Password;
                //Clear Screen
                Terminal.ClearScreen();
                //Call setRandomPassord to set a password
                if (changePassword)
                {
                    SetRandomPassword();
                }
                Terminal.WriteLine("Enter your password. Hint: " + password.Anagram());
                //show menuHint
                Terminal.WriteLine(menuHint);
                break;

        }
    }

    private void SetRandomPassword()
    {
        //set changePassword as false so that the password wont change until solved
        changePassword = false;
        switch (level)
        {
            case 1:
                password = passwordLevel1[UnityEngine.Random.Range(0, passwordLevel1.Length)];
                break;
            case 2:
                password = passwordLevel2[UnityEngine.Random.Range(0, passwordLevel2.Length)];
                break;
            case 3:
                password = passwordLevel3[UnityEngine.Random.Range(0, passwordLevel3.Length)];
                break;
            default:
                Debug.LogError("Hmmm...Invalid level...This shouldn´t be possible");
                changePassword = true;
                break;
        }
    }
}