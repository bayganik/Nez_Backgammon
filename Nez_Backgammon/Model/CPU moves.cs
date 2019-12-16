

public class Game : ThreadStart
{
    public Game(StartGame frame)
    {
        players[0] = new Player(1, "You", this);
        players[1] = new Computer(2, this);
    }
}

public class Computer : Player
{
    private Expectiminimax emm;

    public Computer(int pn) : base(pn, "Computer")
    {
        emm = new Expectiminimax();
    }
    public void play()
    {
        GameState init = new GameState(this.Game.CurrentBoard, this.Game.Dice);
        GameState res = emm.chooseMove(init);
        if (res == null) // no moves to play
        {
            this.Game.Dice.useAllDices();
            return;
        }
        calcMoves(this.Game.CurrentBoard.BoardLocation, res.board, this.Game.Dice);
        this.Game.Dice.useAllDices();
        Console.WriteLine(res); // print the result of the algorithm in the
                                // console
    }
    /// <summary>
    /// Find which moves took place at init GameState that produced res GameState
    /// Perform those moves
    /// </summary>
    /// <param name="init">
    ///            initial GameState </param>
    /// <param name="init">
    ///            result GameState </param>
    private void calcMoves(int[] init, int[] dest, Dice dice)
    {
        List<int> from = new List<int>();
        List<int> to = new List<int>();
        if (init[25] != dest[25])
        {
            int counter = dest[25] - init[25];
            while (counter > 0)
            {
                from.Add(25);
                counter--;
            }
        }
        calcFromTo(init, dest, from, to);
        calcMoves(dice, from, to, dest);
        if (to.Count > 0)
        {
            foreach (int i in to)
            {
                if (init[i] > 0) // Player was hit during a multiple move
                {
                    this.Game.BoardAnimation.CurrentBoard.sendToBar(i);
                }

            }
        }
        if (from.Count > 0) // gathering move for Computer
        {
            foreach (int i in from)
            {
                this.Game.BoardAnimation.removeChecker(i);
            }
        }
    }
}

public class GameState
{
    internal int[] board;
    internal int[] dice;
    private double score;

    public GameState(int[] board, int[] dice)
    {
        this.board = board.Clone();
        if (dice[0] < dice[1])
        {
            this.setDice(dice[0], dice[1]);
        }
        else
        {
            this.setDice(dice[1], dice[0]);
        }
    }
    //JAVA TO C# CONVERTER TODO TASK: The following line could not be converted:
    // player = 2 for computer
    //
    public List<GameState> getChildren(int player)
    {
        int diceLeft = (this.dice[0] == this.dice[1] ? 4 : 2);
        if (!this.hasGyard(this.board, player))
        {
            List<GameState> children = new List<GameState>(30);
            this.getChildren(children, player, 0, diceLeft - 1, this.board);
            this.setDice(dice[1], dice[0]); // swap dice
            this.getChildren(children, player, 0, diceLeft - 1, this.board);
            if (!children.Empty)
            {
                return children;
            }
            else // cannot find children,will check for partial moves only
            {
                if (diceLeft == 2)
                {
                    diceLeft--;
                    this.getChildren(children, player, 0, diceLeft - 1, this.board);
                    this.setDice(dice[1], dice[0]);
                    this.getChildren(children, player, 0, diceLeft - 1, this.board);
                    return children;
                }
                else // doubles
                {
                    while (diceLeft > 0)
                    {
                        this.getChildren(children, player, 0, diceLeft - 1, this.board);
                        this.setDice(dice[1], dice[0]);
                        this.getChildren(children, player, 0, diceLeft - 1, this.board);
                        if (!children.Empty)
                        {
                            return children;
                        }
                        diceLeft--;
                    }
                }
            }
            return children;
        }
        else
        {
            List<GameState> children = new List<GameState>(20);
            int[][] noGy = null;
            if (Math.Abs(board[this.gyard(player)]) >= 2 || diceLeft == 4)
            {
                noGy = new int[1][]; // at most two possible boards when moving
            }
            // out of graveyard
            else
            {
                noGy = new int[2][];
            }
            int op = player == 1 ? -1 : 1;
            int moveLoc = 0;
            if (diceLeft == 4) // doubles
            {
                noGy[0] = this.board.clone();
                while (this.hasGyard(noGy[0], player) && diceLeft != 0)
                {
                    moveLoc = this.countFrom(player) + (op * this.dice[--diceLeft]);
                    if (this.isValid(player, this.board[moveLoc]))
                    {
                        this.move(player, noGy[0], this.gyard(player), moveLoc);
                    }
                    else // can't place a checker from graveyard,return
                    {
                        return children;
                    }
                }
                this.getChildren(children, player, 0, diceLeft - 1, noGy[0]);
                return children;
            }
            else
            {
                if (Math.Abs(board[this.gyard(player)]) < 2)
                {
                    moveLoc = this.countFrom(player) + (op * this.dice[0]);
                    if (this.isValid(player, this.board[moveLoc]))
                    {
                        noGy[0] = this.board.clone();
                        this.move(player, noGy[0], this.gyard(player), moveLoc);
                        diceLeft--;
                        this.setDice(dice[1], dice[0]);
                        this.getChildren(children, player, 0, diceLeft - 1, noGy[0]);
                        this.setDice(dice[1], dice[0]);
                        diceLeft++;
                    }
                    moveLoc = this.countFrom(player) + (op * this.dice[1]);
                    if (this.isValid(player, this.board[moveLoc]))
                    {
                        diceLeft--;
                        noGy[1] = this.board.clone();
                        this.move(player, noGy[1], this.gyard(player), moveLoc);
                        this.getChildren(children, player, 0, diceLeft - 1, noGy[1]);
                    }
                    return children;
                }
                else
                {
                    noGy[0] = this.board.clone();
                    moveLoc = this.countFrom(player) + (op * this.dice[0]);
                    if (this.isValid(player, noGy[0][moveLoc]))
                    {
                        this.move(player, noGy[0], this.gyard(player), moveLoc);
                    }
                    moveLoc = this.countFrom(player) + (op * this.dice[1]);
                    if (this.isValid(player, noGy[0][moveLoc]))
                    {
                        this.move(player, noGy[0], this.gyard(player), moveLoc);
                    }
                    children.add(this);
                    return children;
                }

            }
        }
    }
}