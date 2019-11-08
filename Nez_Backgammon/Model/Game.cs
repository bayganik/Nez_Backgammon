
using System;
using System.Threading;
using System.Windows.Forms;
using StartGame = Backgammon.game.StartGame;
using Dice = Backgammon.model.Dice;
using Player = Backgammon.model.Player;
using Board = Backgammon.view.Board;
using BoardAnimation = Backgammon.view.BoardAnimation;

namespace Backgammon.controller
{

    public class Game : ThreadStart
    {

        private sbyte currPlayer; // player indicator ('0' for player 1 (/human), '1'
                                  // for player 2 (/computer)
        private Player[] players;
        private StartGame frame; // the Main object
        private bool winnerCheck; // winner check variable
        private bool rolled; // dice roll check variable

        private Dice realDice; // the real dice of the game
        private bool firstroll; // used for the first two dice rolls (initial
                                // player check)
        private bool gameStarted; // indicates whether a game has been started
        private Thread gameThread; // thread used to balance the flow of the program
        private sbyte chk; // used to indicate whether a player has won
        private int winner = 0;

        /// <summary>
        /// 1 parameter constructor for the Game object.
        /// </summary>
        /// <param name="frame">
        ///            : the Main class JFrame </param>

        public Game(StartGame frame)
        {
            // initialize all the Game variables
            this.frame = frame;
            this.winnerCheck = false;
            this.rolled = false;
            this.players = new Player[2];
            this.firstroll = true;
            this.gameStarted = false;
            players[0] = new Player(1, "You", this);
            players[1] = new Computer(2, this);
        } // end of Game

        /// <summary>
        /// Exits the current game.
        /// 
        /// </summary>
        public void exitCurrent()
        {
            // reset the dice, interrupt any rolls and set the firstroll variable to
            // true
            frame.DiceAnimation.showExtra(false);
            frame.DiceAnimation.cancelAnimatedRoll();
            firstroll = true;

            // interrupt the game thread and end any player turns
            gameThread.Interrupt();

            players[0].endTurn();
            players[1].endTurn();
            winnerCheck = false;

            StartGame.easyMenuItem.Enabled = true;
            StartGame.mediumMenuItem.Enabled = true;
            StartGame.hardMenuItem.Enabled = true;
        } // end of exitCurrent

        /// <summary>
        /// Initializes and starts the thread.
        /// 
        /// It is called when the new game button is pressed
        /// </summary>
        public void startGame()
        {
            gameThread = new Thread(this, "GameThread");
            StartGame.easyMenuItem.Enabled = false;
            StartGame.mediumMenuItem.Enabled = false;
            StartGame.hardMenuItem.Enabled = false;
            gameThread.Start();
        }

        /// <summary>
        /// Runs the thread.
        /// 
        /// Catches any thread exceptions, runs the infinite game loop
        /// </summary>

        public void run()
        {
            this.gameStarted = true;
            this.EnabledCheckers = false;
            try
            {
                setInitial();
            }
            catch (InterruptedException)
            {
                return;
            }
            catch (ExecutionException)
            {
                return;
            }
            catch (CancellationException)
            {
                return;
            }
            rolled = currPlayer == 1;
            frame.RollButton.Enabled = currPlayer == 0;

            while (!winnerCheck)
            {
                //			StartGame.easyMenuItem.setEnabled(false);
                //			StartGame.mediumMenuItem.setEnabled(false);
                //			StartGame.hardMenuItem.setEnabled(false);
                while (rolled)
                {

                    if (firstroll)
                    {
                        firstroll = false;
                        if (this.CurrentPlayer.Num == 1)
                        {
                            frame.RollButton.Enabled = true;
                            continue;
                        }
                        continue;
                    }
                    rolled = false;
                    if (this.CurrentPlayer.Num == 1)
                    {

                        this.rollDice((sbyte)3);
                        this.CurrentPlayer.startTurn();
                        frame.RollButton.Enabled = false;
                        frame.PassButton.Enabled = false;
                        while (!this.Dice.AllDicesUsed)
                        {
                            if (rolled)
                            {
                                break;
                            }
                            if (hasWon())
                            {
                                winnerCheck = true;
                                break;
                            }
                            if (!frame.BoardAnimation.haveAnyMove(this.Dice))
                            {
                                frame.PassButton.Enabled = true;
                            }
                            // else if()
                            try
                            {
                                Thread.Sleep(1500);
                            }
                            catch (InterruptedException e)
                            {
                                Console.WriteLine(e.ToString());
                                Console.Write(e.StackTrace);
                            }
                        }
                        this.CurrentPlayer.endTurn();
                        frame.RollButton.Enabled = false;
                        frame.PassButton.Enabled = false;
                        rolled = true;
                    }
                    else
                    {
                        if (hasWon())
                        {
                            winnerCheck = true;
                            break;
                        }
                        this.rollDice((sbyte)3);
                        long time = DateTimeHelper.CurrentUnixTimeMillis();
                        //System.out.println("started");
                        ((Computer)this.CurrentPlayer).play();
                        System.out.println("done in "
        + (System.currentTimeMillis() - time)
        + " ms (including graphical move)");
                        rolled = false;
                        frame.getRollButton().setEnabled(true);// enable dice again
                    }
                    chk = changeTurn(); // next turn
                }
                // perform the check every 900 ms
                if (hasWon())
                {
                    winnerCheck = true;
                    break;
                }
                try
                {
                    Thread.sleep(900);
                }
                catch (InterruptedException e)
                {
                    System.err.println("Thread interrupted");
                    return;
                }
            }
            winnerDialog();

        }// end of run
        public void winnerDialog()
        {
            String winnerName = null;

            if (winner == 1)
            {
                winnerName = "You";
            }
            else
            {
                winnerName = "Computer";
            }
            JOptionPane.showMessageDialog(frame,
                     winnerName + " WON :)");
            this.exitCurrent();
        }
        /*
 * get methods
 */

        public Player getCurrentPlayer()
        {
            return this.players[currPlayer];
        }

        public Player getPlayer(byte b)
        {
            return this.players[b];
        }

        public Dice getDice()
        {
            return this.realDice;
        }

        public void setDice(Dice d)
        {
            this.realDice = d;
        }

        public boolean isFirstRoll()
        {
            return this.firstroll;
        }

        public boolean isRunning()
        {
            return gameStarted;
        }

        public Board getCurrentBoard()
        {
            return this.frame.getBoardAnimation().getCurrentBoard();
        }

        public BoardAnimation getBoardAnimation()
        {
            return this.frame.getBoardAnimation();
        }
    }
}

