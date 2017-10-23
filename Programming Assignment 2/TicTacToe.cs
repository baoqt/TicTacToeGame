using System;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

/// <summary>
/// Name: Bao Tran
/// Redid: 818209256
/// Date: 9.26.17
/// This assignment was completed from: Memory, MS website for file I/O, and Newtonsoft website for JSON write
/// </summary>
namespace Programming_Assignment_2
{
    class TicTacToe
    {
        private int currentPlayer;
        private string victor;
        private bool victoryFlag;
        private bool tieFlag;
        public int[,] board;

        /// <summary>
        /// Default constructor:
        /// auto starts with player 1,
        /// resets all flags,
        /// creates a blank board,
        /// and clears the json file that holds file game results
        /// </summary>
        public TicTacToe()                                                                                                                          
        {
            currentPlayer = 1;
            victor = string.Empty;
            victoryFlag = false;
            tieFlag = false;
            board = new int[3, 3] { { 0, 0, 0 },                                                                                                    
                                    { 0, 0, 0 },
                                    { 0, 0, 0 } };
            File.WriteAllText(@"game.json", "");                                                                                                    
        }

        /// <summary>
        /// Function does the main menu logic
        /// </summary>
        public void DisplayMainMenu()                                                                                                               
        {
            /// <summary>
            /// Attemp to play from file.
            /// Exit if file exists, otherwise display appropriate
            /// error and continue to main menu draw
            /// </summary>
            try
            {
                FileGame();
                Environment.Exit(0);                                                                                                                
            }                                                                                                                                       
            catch (FileNotFoundException)
            {
                ClearGame();
                Console.SetCursorPosition(18, 6);
                Console.WriteLine("game.txt not found");
                Console.ReadKey();
                ClearGame();
            }
            catch (ArgumentOutOfRangeException)
            {
                ClearGame();
                Console.SetCursorPosition(18, 6);
                Console.WriteLine("Please trim game.txt");
                Console.ReadKey();
                ClearGame();
            }

            int cursorLocation = 0;
            ClearGame();
            ConsoleKey inputKey = 0;

            /// <summary>
            /// While on the main menu, pressing esc exits the program
            /// </summary>
            while (inputKey != ConsoleKey.Escape)                                                                                                   
            {
                DrawMenus("Main Menu", cursorLocation);
                inputKey = Console.ReadKey(true).Key;
                if (inputKey == ConsoleKey.Enter)
                {
                    /// <summary>
                    /// Resets all flags and board
                    /// </summary>
                    ResetBoard();
                    currentPlayer = 1;                                                                                                              
                    victor = string.Empty;
                    victoryFlag = false;
                    tieFlag = false;
                    ClearGame();

                    /// <summary>
                    /// 0 for regular 2 player game, should have just done enum
                    /// </summary>
                    if (cursorLocation == 0)
                    {
                        string userInput = string.Empty;
                        int moveCounter = 0;
                        ChooseStartingPlayer("2 player");
                        ClearGame();

                        while (!victoryFlag && moveCounter < 9)
                        {
                            DisplayBoard();                                                                                                         
                            userInput = Console.ReadLine();                                                                                         
                            PlaceMarker(userInput);                                                                                                 
                            moveCounter++;
                        }

                        /// <summary>
                        /// Board is filled but no winner
                        /// </summary>
                        if (victor == string.Empty && moveCounter == 9)                                                                             
                        {
                            tieFlag = true;
                            DisplayBoard();
                            DisplayVictor();
                            AppendToJson();
                            ClearGame();
                        }
                        /// <summary>
                        /// Regular win
                        /// </summary>
                        if (victor != string.Empty)
                        {
                            DisplayBoard();
                            DisplayVictor();                                                                                                       
                            ClearGame();                                                                                                            
                        }
                    }
                    /// <summary>
                    /// 1 to try file game again
                    /// </summary>
                    else if (cursorLocation == 1)                                                                                                   
                    {
                        try
                        {
                            FileGame();
                        }
                        catch (FileNotFoundException)
                        {
                            ClearGame();
                            Console.SetCursorPosition(18, 6);
                            Console.WriteLine("game.txt not found");
                            Console.ReadKey();
                            ClearGame();
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            ClearGame();
                            Console.SetCursorPosition(18, 6);
                            Console.WriteLine("Please trim game.txt");
                            Console.ReadKey();
                            ClearGame();
                        }
                    }
                    /// <summary>
                    /// 2 for AI game
                    /// </summary>
                    else if (cursorLocation == 2)                                                                                                   
                    {
                        ChooseStartingPlayer("AI game");
                        ClearGame();
                        Console.SetCursorPosition(18, 6);
                        Console.WriteLine("Not yet implemented");
                        Console.ReadKey();
                        ClearGame();

                    }
                }
                /// <summary>
                /// Moving the cursor
                /// </summary>
                else if (((inputKey == ConsoleKey.UpArrow) || (inputKey == ConsoleKey.LeftArrow)) 
                       && (cursorLocation != 0))
                {
                    cursorLocation--;
                }
                else if (((inputKey == ConsoleKey.DownArrow) || (inputKey == ConsoleKey.RightArrow))
                       && (cursorLocation != 2))
                {
                    cursorLocation++;
                }
                ClearGame();
            }

            ClearGame();
            Console.SetCursorPosition(0, 0);
            return;
        }

        /// <summary>
        /// Function allows starting player to be chosen
        /// or rolls random between the two players
        /// </summary>
        /// <param name="gameType">Game type to change menu text</param>
        private void ChooseStartingPlayer(string gameType)                                                                                          
        {                                                                                                                                           
            int cursorLocation = 0;
            ConsoleKey inputKey = 0;
            while (inputKey != ConsoleKey.Escape)
            {
                DrawMenus(gameType, cursorLocation);

                inputKey = Console.ReadKey(true).Key;
                if (inputKey == ConsoleKey.Enter)
                {
                    if (cursorLocation == 0)
                    {
                        currentPlayer = 1;
                        return;
                    }
                    else if (cursorLocation == 1)
                    {
                        currentPlayer = 2;
                        return;
                    }
                    else if (cursorLocation == 2)
                    {
                        /// <summary>
                        /// Rolls a random int between 0 and 1,
                        /// shifts it to 1 or 2
                        /// </summary>
                        Random RNG = new Random();
                        currentPlayer = RNG.Next(2) + 1;                                                                                     
                        return;
                    }
                }
                else if (((inputKey == ConsoleKey.UpArrow) || (inputKey == ConsoleKey.LeftArrow))
                           && (cursorLocation != 0))
                {
                    cursorLocation--;
                }
                else if (((inputKey == ConsoleKey.DownArrow) || (inputKey == ConsoleKey.RightArrow))
                       && (cursorLocation != 2))
                {
                    cursorLocation++;
                }
                ClearGame();
            }
            /// <summary>
            /// No actual victory but set victoryFlag to true
            /// to skip the game since esc was pressed to return
            /// </summary>
            victoryFlag = true;
        }

        /// <summary>
        /// Function draws different menus
        /// </summary>
        /// <param name="menuType">Menu type to change menu text</param>
        /// <param name="cursorLocation">Cursor location to draw where cursor is</param>
        private void DrawMenus(string menuType, int cursorLocation)
        {
            if (menuType == "Main Menu")
            {
                switch (cursorLocation)                                                                                                             
                {                                                                                                                           
                    case 0:
                        Console.WriteLine("                     TicTacToe Game\n" +
                                          "\n" +
                                          "                   [x] Live 2 Player\n" +
                                          "\n" +
                                          "                   [ ] Read from File\n" +
                                          "\n" +
                                          "                   [ ] AI Game\n" +
                                          "\n" +
                                          "                 Use Arrow Keys to move\n" +
                                          "\n" +
                                          "                 Press Enter to choose\n" +
                                          "\n" +
                                          "                   Press Esc to quit");
                        break;
                    case 1:
                        Console.WriteLine("                     TicTacToe Game\n" +
                                          "\n" +
                                          "                   [ ] Live 2 Player\n" +
                                          "\n" +
                                          "                   [x] Read from File\n" +
                                          "\n" +
                                          "                   [ ] AI Game\n" +
                                          "\n" +
                                          "                 Use Arrow Keys to move\n" +
                                          "\n" +
                                          "                 Press Enter to choose\n" +
                                          "\n" +
                                          "                   Press Esc to quit");
                        break;
                    case 2:
                        Console.WriteLine("                     TicTacToe Game\n" +
                                          "\n" +
                                          "                   [ ] Live 2 Player\n" +
                                          "\n" +
                                          "                   [ ] Read from File\n" +
                                          "\n" +
                                          "                   [x] AI Game\n" +
                                          "\n" +
                                          "                 Use Arrow Keys to move\n" +
                                          "\n" +
                                          "                 Press Enter to choose\n" +
                                          "\n" +
                                          "                   Press Esc to quit");
                        break;
                }
            }
            else if (menuType == "2 player")
            {
                switch (cursorLocation)
                {
                    case 0:
                        Console.WriteLine("                  Choose Starting Player\n" +
                                          "\n" +
                                          "                   [x] Player 1\n" +
                                          "\n" +
                                          "                   [ ] Player 2\n" +
                                          "\n" +
                                          "                   [ ] Random\n" +
                                          "\n" +
                                          "                 Use Arrow Keys to move\n" +
                                          "\n" +
                                          "                 Press Enter to choose\n" +
                                          "\n" +
                                          "                   Press Esc to quit");
                        break;
                    case 1:
                        Console.WriteLine("                  Choose Starting Player\n" +
                                          "\n" +
                                          "                   [ ] Player 1\n" +
                                          "\n" +
                                          "                   [x] Player 2\n" +
                                          "\n" +
                                          "                   [ ] Random\n" +
                                          "\n" +
                                          "                 Use Arrow Keys to move\n" +
                                          "\n" +
                                          "                 Press Enter to choose\n" +
                                          "\n" +
                                          "                   Press Esc to quit");
                        break;
                    case 2:
                        Console.WriteLine("                  Choose Starting Player\n" +
                                          "\n" +
                                          "                   [ ] Player 1\n" +
                                          "\n" +
                                          "                   [ ] Player 2\n" +
                                          "\n" +
                                          "                   [x] Random\n" +
                                          "\n" +
                                          "                 Use Arrow Keys to move\n" +
                                          "\n" +
                                          "                 Press Enter to choose\n" +
                                          "\n" +
                                          "                   Press Esc to quit");
                        break;
                }
            }
            else if (menuType == "AI game")
            {
                switch (cursorLocation)
                {
                    case 0:
                        Console.WriteLine("                  Choose Starting Player\n" +
                                          "\n" +
                                          "                   [x] Player\n" +
                                          "\n" +
                                          "                   [ ] AI\n" +
                                          "\n" +
                                          "                   [ ] Random\n" +
                                          "\n" +
                                          "                 Use Arrow Keys to move\n" +
                                          "\n" +
                                          "                 Press Enter to choose\n" +
                                          "\n" +
                                          "                   Press Esc to quit");
                        break;
                    case 1:
                        Console.WriteLine("                  Choose Starting Player\n" +
                                          "\n" +
                                          "                   [ ] Player\n" +
                                          "\n" +
                                          "                   [x] AI\n" +
                                          "\n" +
                                          "                   [ ] Random\n" +
                                          "\n" +
                                          "                 Use Arrow Keys to move\n" +
                                          "\n" +
                                          "                 Press Enter to choose\n" +
                                          "\n" +
                                          "                   Press Esc to quit");
                        break;
                    case 2:
                        Console.WriteLine("                  Choose Starting Player\n" +
                                          "\n" +
                                          "                   [ ] Player\n" +
                                          "\n" +
                                          "                   [ ] AI\n" +
                                          "\n" +
                                          "                   [x] Random\n" +
                                          "\n" +
                                          "                 Use Arrow Keys to move\n" +
                                          "\n" +
                                          "                 Press Enter to choose\n" +
                                          "\n" +
                                          "                   Press Esc to quit");
                        break;
                }
            }
        }

        /// <summary>
        /// Function does the logic for playing from file.
        /// Is called from a try catch block so not needed inside.
        /// Done that way because I wanted it to exit if successful the first time,
        /// but not any other times
        /// </summary>
        private void FileGame()
        {
            int stringIndex = 0;
            int moveCounter = 0;
            string[] lines = System.IO.File.ReadAllLines(@"game.txt");
            while (stringIndex < lines.Length)
            {
                moveCounter = 0;
                ResetBoard();
                victor = String.Empty;
                victoryFlag = false;

                if (lines[stringIndex].Substring(0, 2) == "P1")
                {
                    currentPlayer = 1;
                }
                else
                {
                    currentPlayer = 2;
                }
                while ((stringIndex < lines.Length) && !victoryFlag && moveCounter < 9)
                {
                    /// <summary>
                    /// End of game flag
                    /// </summary>
                    if (lines[stringIndex] == "0000000000")
                    {
                        victoryFlag = true;                                                                                     
                        break;                                                                                                     
                    }
                    /// <summary>
                    /// Check for wrong order moves
                    /// </summary>
                    else if (((currentPlayer == 1) && (lines[stringIndex].Substring(0, 2) == "P1") ||                 
                              (currentPlayer == 2) && (lines[stringIndex].Substring(0, 2) == "P2")) && (moveCounter < 9))
                    {
                        //DisplayBoard();
                        /// <summary>
                        /// Increment move counter if valid move
                        /// </summary>
                        if (PlaceMarker(lines[stringIndex].Substring(4, 2)))                                                   
                        {
                            moveCounter++;                                                                                               
                        }                                                                                                                
                        Thread.Sleep(10);
                        //DisplayBoard();
                    }
                    /// <summary>
                    /// Increment through the file regardless of cheat
                    /// </summary>
                    stringIndex++;                                                                                                  
                }

                if (victor != string.Empty)
                {
                    victoryFlag = true;
                    //DisplayBoard();
                    //ClearError();
                    //DisplayVictorNoPrompt();
                    /// <summary>
                    /// Scroll until end game marker or end file
                    /// </summary>
                    while ((stringIndex < lines.Length) && (lines[stringIndex] != "0000000000"))
                    {
                        stringIndex++;
                    }
                }
                else if ((victor == string.Empty) && (moveCounter == 9))                                                                
                {
                    victoryFlag = true;
                    tieFlag = true;
                    //ClearError();
                    //DisplayVictorNoPrompt();
                    while ((stringIndex < lines.Length) && (lines[stringIndex] != "0000000000"))                                            
                    {
                        stringIndex++;
                    }
                }
                /// <summary>
                /// Forfeitted
                /// </summary>
                else if (victor == string.Empty)                                                                             
                {
                    victoryFlag = true;
                    victor = (1 + (currentPlayer % 2)).ToString();
                    //ClearError();
                    //DisplayVictorNoPrompt();
                    while ((stringIndex < lines.Length) && (lines[stringIndex] != "0000000000"))                                           
                    {
                        stringIndex++;
                    }
                }
                stringIndex++;
                AppendToJson();                                                                                                          
            }
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Function displays the board for 2 player mode
        /// </summary>
        private void DisplayBoard()                                                                                                        
        {
            ClearBoard();                                                                                                                          

                Console.WriteLine($" {board[0, 0]} | {board[0, 1]} | {board[0, 2]}   [A]       Player {currentPlayer}'s turn!\n" +               
                                  $"-----------\n" +
                                  $" {board[1, 0]} | {board[1, 1]} | {board[1, 2]}   [B]       Enter \"///\" to forfeit\n" +
                                  $"-----------\n" +
                                  $" {board[2, 0]} | {board[2, 1]} | {board[2, 2]}   [C]       Enter selection: \n\n" +
                                  $"[0] [1] [2]\n");

            Console.SetCursorPosition(40, 4);                                                                                                
        }                                                                                                                                    
        
        /// <summary>
        /// Function places markers based on user input
        /// and returns wheter the move was valid or not.
        /// Row/Column indexes for user selection
        /// Flags are:
        /// 0 for first index out of bounds
        /// 1 for second index out of bounds
        /// 2 for wrong size input
        /// 3 for already filled square
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool PlaceMarker(string value)                                                                                                   
        {                                                                                                                                         
            int row = 0;                                                                                                                          
            int column = 0;                                                                                                                      
            bool flag0 = false;                                                                                                                 
            bool flag1 = false;                                                                                                                  
            bool flag2 = false;                                                                                                                  
            bool flag3 = false;                                                                                                                  

            ClearError();                                                                                                                       
            if (value.Length == 2)
            {
                /// <summary>
                /// Double switch case, one after another, didn't work well so
                /// had to do an if else statement to be able to raise all 
                /// applicable flags
                /// </summary>
                if (Char.ToUpper(value[0]) == 'A')                                                                                             
                {                                                                                                                                 
                    row = 0;                                                                                                                     
                }
                else if (Char.ToUpper(value[0]) == 'B')
                {
                    row = 1;
                }
                else if (Char.ToUpper(value[0]) == 'C')
                {
                    row = 2;
                }
                else
                {
                    flag0 = true;                                                                                                      
                }

                if (value[1] == '0')
                {
                    column = 0;
                }
                else if (value[1] == '1')
                {
                    column = 1;
                }
                else if (value[1] == '2')
                {
                    column = 2;
                }
                else
                {
                    flag1 = true;                                                                                                           
                }
            }                    
            /// <summary>
            /// Player forfeitted, give win to other player
            /// </summary>
            else if (value == "///")                                                                                                          
            {
                victoryFlag = true;                                                                                                  
                victor = (1 + (currentPlayer % 2)).ToString();
                return false;
            }
            else if (value == "")
            {
                return false;                                                                                                       
            }
            else
            {
                flag2 = true;                                                                                                                      
            }

            if (!flag0 && !flag1 && !flag2)                                                                                                  
            {
                if (board[row, column] == 0)                                                                                                 
                {
                    board[row, column] = currentPlayer;
                    if (currentPlayer == 1)                                                                                                     
                    {
                        currentPlayer = 2;
                    }
                    else
                    {
                        currentPlayer = 1;
                    }
                }
                else
                {
                    flag3 = true;
                }
            }

            /// <summary>
            /// Logic checks for rows with similar markers that aren't 0
            /// Gives a win if true
            /// </summary>
            if (((board[0, 0] == board[0, 1]) && (board[0, 0] == board[0, 2])) && (board[0, 0] != 0) ||                                         
                ((board[0, 0] == board[1, 0]) && (board[0, 0] == board[2, 0])) && (board[0, 0] != 0) ||                                          
                ((board[0, 0] == board[1, 1]) && (board[0, 0] == board[2, 2])) && (board[0, 0] != 0))
            {
                victor = board[0, 0].ToString();
                victoryFlag = true;
            }
            else if ((board[0, 2] == board[1, 2]) && (board[0, 2] == board[2, 2]) && (board[0, 2] != 0) || 
                     (board[0, 2] == board[1, 1]) && (board[0, 2] == board[2, 0]) && (board[0, 2] != 0))
            {
                victor = board[0, 2].ToString();
                victoryFlag = true;
            }
            else if ((board[1, 0] == board[1, 1]) && (board[1, 0] == board[1, 2]) && (board[1, 0] != 0))
            {
                victor = board[1, 0].ToString();
                victoryFlag = true;
            }
            else if ((board[0, 1] == board[1, 1]) && (board[0, 1] == board[2, 1]) && (board[0, 1] != 0))
            {
                victor = board[0, 1].ToString();
                victoryFlag = true;
            }
            else if ((board[2, 0] == board[2, 1]) && (board[2, 0] == board[2, 2]) && (board[2, 0] != 0))
            {
                victor = board[2, 0].ToString();
                victoryFlag = true;
            }

            DisplayError(flag0, flag1, flag2, flag3);     
            /// <summary>
            /// Return true if no flags raised
            /// </summary>
            return !(flag0 || flag1 || flag2 || flag3);                                                                               
        }

        /// <summary>
        /// Function places cursor at selection entry area.
        /// Doesn't actually save any code but makes it look
        /// better in my opinion
        /// </summary>
        private void ReplaceCursorForGame()                                                                                               
        {
            Console.SetCursorPosition(40, 4);                                                                                                   
        }                                                                                                                                         

        /// <summary>
        /// Function clears the board area, preparing
        /// it for a menu redraw. Definitely saves a lot
        /// of code
        /// </summary>
        private void ClearBoard()                                                                                                                 
        {                                                                                                                                 
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n");
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Function resets the 3x3 board array, preparing
        /// it for a new game.
        /// </summary>
        private void ResetBoard()                                                                                                         
        {
            int rowIndex = 0;
            int columnIndex = 0;

            for (rowIndex = 0; rowIndex < 3; rowIndex++)
            {
                for (columnIndex = 0; columnIndex < 3; columnIndex++)
                {
                    board[rowIndex, columnIndex] = 0;
                }
            }
        }

        /// <summary>
        /// Function displays a winner if there is one
        /// and waits to exit current game into main menu.
        /// </summary>
        private void DisplayVictor()                                                                                                          
        {                                                                                                                                         
            if (!tieFlag)
            {
                Console.SetCursorPosition(0, 8);
                Console.WriteLine($"{victor} is the winner!");
                Console.WriteLine("Press enter to continue.");
                Console.Read();
            }
            else if (tieFlag)
            {
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("Tie!");
                Console.WriteLine("Press enter to continue.");
                Console.Read();
            }
        }

        /// <summary>
        /// Function prints out winner without prompt to
        /// exit. Not used if file games doesn't print into text gui
        /// </summary>
        private void DisplayVictorNoPrompt()                                                                                            
        {                                                                                                                                      
            if (!tieFlag)
            {
                Console.SetCursorPosition(0, 8);
                Console.WriteLine($"{victor} is the winner!");
            }
            else if (tieFlag)
            {
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("Tie!");
            }
        }

        /// <summary>
        /// Function prints out all errors into error area
        /// </summary>
        /// <param name="flag0">First index out of bounds</param>
        /// <param name="flag1">Second index out of bounds</param>
        /// <param name="flag2">Input wrong size</param>
        /// <param name="flag3">Square already filled</param>
        private void DisplayError(bool flag0, bool flag1, bool flag2, bool flag3)                                                         
        {                                                                                                                                    
            if (flag0)
            {
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("Invalid input, first index valid range: A - C");
                ReplaceCursorForGame();
            }

            if (flag1)
            {
                Console.SetCursorPosition(0, 9);
                Console.WriteLine("Invalid input, second index valid range: 0 - 2");
                ReplaceCursorForGame();
            }

            if (flag2)
            {
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("Invalid length");
                ReplaceCursorForGame();
            }
            if (flag3)
            {
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("Square already filled");
                ReplaceCursorForGame();
            }
        }

        /// <summary>
        /// Function clears error area for new messages
        /// </summary>
        private void ClearError()                                                                                                                
        {
            Console.SetCursorPosition(0, 7);
            Console.WriteLine("                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n");
            ReplaceCursorForGame();
        }

        /// <summary>
        /// Function clears the whole game area, preparing
        /// it for menu redraws
        /// </summary>
        private void ClearGame()                                                                                                                 
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   \n" +
                              "                                                                                   ");
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Function appends current board state to
        /// JSON file.
        /// </summary>
        public void AppendToJson()                                                                                                                
        {
            //string toJson = string.Empty;

            //toJson = JsonConvert.SerializeObject($" {board[0, 0]} | {board[0, 1]} | {board[0, 2]}   [A]");
            //File.AppendAllText(@"game.json", toJson);
            //File.AppendAllText(@"game.json", "\n");
            //toJson = JsonConvert.SerializeObject("-----------");
            //File.AppendAllText(@"game.json", toJson);
            //File.AppendAllText(@"game.json", "\n");
            //toJson = JsonConvert.SerializeObject($" {board[1, 0]} | {board[1, 1]} | {board[1, 2]}   [B]");
            //File.AppendAllText(@"game.json", toJson);
            //File.AppendAllText(@"game.json", "\n");
            //toJson = JsonConvert.SerializeObject("-----------");
            //File.AppendAllText(@"game.json", toJson);
            //File.AppendAllText(@"game.json", "\n");
            //toJson = JsonConvert.SerializeObject($" {board[2, 0]} | {board[2, 1]} | {board[2, 2]}   [C]");
            //File.AppendAllText(@"game.json", toJson);
            //File.AppendAllText(@"game.json", "\n");
            //File.AppendAllText(@"game.json", "\n");
            //toJson = JsonConvert.SerializeObject("[0] [1] [2]");
            //File.AppendAllText(@"game.json", toJson);
            //File.AppendAllText(@"game.json", "\n");
            //File.AppendAllText(@"game.json", "\n");
            //if (victor != string.Empty)                                                                                                             /// Depending on tie, forfeit, or regular win
            //{
            //    toJson = JsonConvert.SerializeObject($"{victor} is the winner!");
            //}
            //else if (tieFlag)
            //{
            //    toJson = JsonConvert.SerializeObject($"Tie!");            
            //}
            //File.AppendAllText(@"game.json", toJson);
            //File.AppendAllText(@"game.json", "\n");
            //toJson = JsonConvert.SerializeObject("     *     ");
            //File.AppendAllText(@"game.json", toJson);
            //File.AppendAllText(@"game.json", "\n");
            //File.AppendAllText(@"game.json", "\n");                                                                                               /// Nicer formatting

            File.AppendAllText(@"game.json", JsonConvert.SerializeObject(this));                                                                    /// Single line formatting
            File.AppendAllText(@"game.json", "\n\n");
        }

        /// <summary>
        /// Function returns current board state
        /// as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()                                                                                             
        {
            return ($" {board[0, 0]} | {board[0, 1]} | {board[0, 2]}   [A]\n" +
                    $"-----------\n" +
                    $" {board[1, 0]} | {board[1, 1]} | {board[1, 2]}   [B]\n" +
                    $"-----------\n" +
                    $" {board[2, 0]} | {board[2, 1]} | {board[2, 2]}   [C]\n\n" +
                    $"[0] [1] [2]\n");
        }
    }
}
