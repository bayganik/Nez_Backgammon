using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using Nez.Tiled;
using Nez.Sprites;
using Nez.Textures;

using Nez_Backgammon.ECS.Components;
using Nez_Backgammon.ECS.Systems;
using Nez_Backgammon.Models;

namespace Nez_Backgammon.Scenes
{
    public class MainScene : BaseScene
    {

        //
        // Game Board (to be passed around)
        //
        public int TotalNumOfStacks { get; set; }
        public BGBoard GameBoard { get; set; }
        public int[] DiceRoll { get; set; }
        public Dictionary<int, int> LegalMoves { get; set; }
        public bool WhiteCanMove { get; set; }
        public bool WhiteGraveYard { get; set; }
        public bool BlackGraveYard { get; set; }
        public bool GameEnded { get; set; }
        public int PlayerWon { get; set; }                              //0=white, 1=black
        public int WhoseTurn { get; set; }                              //0=white, 1=black 
        //public ExpectiMinimax Eminmax { get; set; }

        //
        // Stacks of checkers (first 24 for game, 1 white graveyard, 1 black grave yard, 1 white collection, 1 black collection)
        //
        public Entity[] GameStacks;

        SceneResolutionPolicy policy;
        NezSpriteFont font;
        Entity TextEntity;

        Entity Play;
        Entity FForward;
        Entity FBackward;
        Entity Pause;
        //
        // Mouse var, so we can track what it clicks on
        //
        Entity MouseCursor;                 // mouse cursor to track (could have an image!)
        Entity BoardImage;                  // backgammon board is the background 
        Vector2 PosStacks;                  // location of each stack entity

        UICanvas UIC;                       // UI canvas to hold buttons & lables

        //public ImageButton PlayButton { get; set; }
        public TextButton BlackDiceBtn { get; set; }
        public TextButton WhiteDiceBtn { get; set; }
        public Label Msg { get; set; }
        public Entity CheckerBeingDragged { get; set; }
        public bool Dragging { get; set; }
        public MainScene()
        {
            policy = Scene.SceneResolutionPolicy.ExactFit;
        }
        public override void Initialize()
        {
            base.Initialize();
            //font = new NezSpriteFont(Content.Load<SpriteFont>("Arial"));
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // Text entity with component (Game name label)
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            TextEntity = CreateEntity("txt");
            TextEntity.Transform.Position = new Vector2(100, 20);
            TextEntity.Transform.Scale = new Vector2(2, 2);
            var txt = new TextComponent(Graphics.Instance.BitmapFont, "Backgammon Test", new Vector2(0, 0), Color.White);
           // txt.SetFont(font);
            TextEntity.AddComponent(txt);

            font = new NezSpriteFont(Content.Load<SpriteFont>("Arial"));

            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // put a Canvas entity on upper right hand side for UI
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            Entity uiCanvas = CreateEntity("ui-canvas");
            UIC = uiCanvas.AddComponent(new UICanvas());

            WhiteDiceBtn = UIC.Stage.AddElement(new TextButton("White Roll Dice", Skin.CreateDefaultSkin()));
            WhiteDiceBtn.SetPosition(800f, 10f);
            WhiteDiceBtn.SetSize(90f, 30f);
            WhiteDiceBtn.OnClicked += WhiteDiceBtn_OnClicked;

            BlackDiceBtn = UIC.Stage.AddElement(new TextButton("Black Roll Dice", Skin.CreateDefaultSkin()));
            BlackDiceBtn.SetPosition(900f, 10f);
            BlackDiceBtn.SetSize(90f, 30f);
            BlackDiceBtn.OnClicked += BlackDiceBtn_OnClicked;
           

            //Msg = UIC.Stage.AddElement(new Label("Label Msg"));
            //Msg.SetPosition(800f, 90f);
            //Msg.SetSize(100f, 50f);

            //ImageButtonStyle stl = new ImageButtonStyle();
            //PlayButton = UIC.Stage.AddElement(new ImageButton(stl));
            //PlayButton.SetPosition(800f, 120);

            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // mouse entity (used for tracking of clicks)
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            MouseCursor = CreateEntity("mouse");
            MouseCursor.AddComponent(new BoxCollider(15,15));
            MouseCursor.AddComponent(new MouseComponent());
            
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // BoardImage is the wooden backgammon board
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            BoardImage = CreateEntity("backgammonboard", new Vector2(0, 0));
            BoardImage.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("BKBoard")).SetRenderLayer(100));
            BoardImage.GetComponent<SpriteRenderer>().SetOrigin(new Vector2(0, 0));
            BoardImage.SetPosition(new Vector2(100, 50));

            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // positions all 28 stacks on the screen (board)
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            Entity ent;
            TotalNumOfStacks = 28;
            GameEnded = false;                          //true = game has ended
            PlayerWon = 0;                              // 0=human, 1=computer
            GameStacks = new Entity[TotalNumOfStacks];
            //
            // Stacks bottom right
            //
            PosStacks = new Vector2(860, 510);
            for (int i = 0; i < 6; i++)
            {
                
                ent = CreateEntity("stack" + i.ToString(), PosStacks);
                PosStacks = PosStacks - new Vector2(62, 0);
                ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                ent.Tag = i;                //location of the stack on the board
                GameStacks[i] = ent;
            }
            //
            // Stacks bottom left
            //
            PosStacks = new Vector2(450, 510);
            for (int i = 6; i < 12; i++)
            {

                ent = CreateEntity("stack" + i.ToString(), PosStacks);
                PosStacks = PosStacks - new Vector2(62, 0);

                //ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")).SetRenderLayer(-99));
                ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                ent.Tag = i;                //location of the stack on the board
                GameStacks[i] = ent;
            }
            //
            // Stacks top left
            //
            PosStacks = new Vector2(140, 190);
            for (int i = 12; i < 18; i++)
            {

                ent = CreateEntity("stack" + i.ToString(), PosStacks);
                PosStacks = PosStacks + new Vector2(62, 0);

                //ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
                ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                ent.Tag = i;                //location of the stack on the board
                GameStacks[i] = ent;
            }
            //
            // Stacks top right
            //
            PosStacks = new Vector2(550, 190);
            for (int i = 18; i < 24; i++)
            {

                ent = CreateEntity("stack" + i.ToString(), PosStacks);
                PosStacks = PosStacks + new Vector2(62, 0);

                //ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
                ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                ent.Tag = i;                //location of the stack on the board
                GameStacks[i] = ent;
            }
            //
            // Graveyard stack for white = 24
            //
            PosStacks = new Vector2(500, 510);
            ent = CreateEntity("stack 24" , PosStacks);
            PosStacks = PosStacks - new Vector2(62, 0);

            ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
            ent.AddComponent(new StackComponent() { StackID = 24 });
            ent.Tag = 24;                //location of the stack on the board
            GameStacks[24] = ent;
            //
            // Graveyard stack for black = 25
            //
            PosStacks = new Vector2(500, 190);
            ent = CreateEntity("stack 25", PosStacks);
            PosStacks = PosStacks - new Vector2(62, 0);

            ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
            ent.AddComponent(new StackComponent() { StackID = 25 });
            ent.Tag = 25;                //location of the stack on the board
            GameStacks[25] = ent;
            //
            // Collector Stack for white = 26
            //
            PosStacks = new Vector2(950, 510);
            ent = CreateEntity("stack 26", PosStacks);
            PosStacks = PosStacks - new Vector2(62, 0);

            ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
            ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
            ent.AddComponent(new StackComponent() { StackID = 26 });
            ent.Tag = 26;                //location of the stack on the board
            GameStacks[26] = ent;
            //
            // Collector Stack for black = 27
            //
            PosStacks = new Vector2(950, 190);
            ent = CreateEntity("stack 27", PosStacks);
            PosStacks = PosStacks - new Vector2(62, 0);

            ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
            ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
            ent.AddComponent(new StackComponent() { StackID = 27 });
            ent.Tag = 27;                //location of the stack on the board
            GameStacks[27] = ent;
            //
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // Systems to process our requests
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            //
            this.AddEntityProcessor(new MouseClickSystem(new Matcher().All(typeof(MouseComponent))));
            this.AddEntityProcessor(new BoardDispSystem(new Matcher().All(typeof(StackComponent))));
            this.AddEntityProcessor(new CheckerDragSystem(new Matcher().All(typeof(DragComponent))));
            this.AddEntityProcessor(new CheckTurnEOGSystem());

            GameBoard = new BGBoard();          //GameBoard initiated for start of game
            Dragging = false;                   //Are we dragging a White checker?
            WhiteCanMove = false;               //if true the Mouse Clicks are allowed to move White checkers
            WhiteGraveYard = false;             //no white checkers hits
            WhoseTurn = 0;                      //white plays first
            //
            // We have a GameBoard, use it to create checker entities
            //
            Fill_GameBoard_Stacks();
            //UpdateStacksFromGameBoard();
            /*
             *  Game starts when WHITE player presses the [Roll Dice] button
             *  
             */

        }
        public int TestGraveYardForCheckers(int graveYardStackNum)
        {
            return Math.Abs(GameBoard.DispBoard[graveYardStackNum]);
            //
            // Test to see if there are checkers in grave yard 
            //
            //Entity stack = GameStacks[graveYardStackNum];
            //StackComponent sc = stack.GetComponent<StackComponent>();
            //return sc.CheckersInStack.Count();
        }
        public void DropChecker2NewPosition(Entity _dropStack)
        {
            //
            // Test the checker for its legal move locations (called/set by CheckerDragSystem)
            // values are 0 - 23 and if value is negative, then a single black checker is sitting in there
            // remove the dice roll that brought us to this drop location (set it to zero)
            //
            foreach (KeyValuePair<int, int> _myMoves in LegalMoves)
            {

                if (_dropStack.Tag == Math.Abs(_myMoves.Value))      //if dropped stack is equal to a legal move
                {
                    if (_myMoves.Value < 0)                          //black checker got hit
                    {
                        Entity _blackGraveYard = GameStacks[25];
                        StackComponent sc = _dropStack.GetComponent<StackComponent>();
                        Entity _blackchkerEntity = sc.CheckersInStack[0];   //get the black checker
                        sc.CheckersInStack.RemoveAt(0);                     //Remove black checker from original stack
                        DropChecker(_blackGraveYard, _blackchkerEntity);    //black moves to graveyard
                    }
                    //
                    // drop the white checker being dragged
                    //
                    DropChecker(_dropStack);                        //found the correct location
                    DiceRoll[_myMoves.Key] = 0;                     //remove the dice value
                    RefreshWhiteDiceValues();
                    return;
                }
            }
            //
            // legal moves didn't work
            //
            DropChecker2PreviousPosition();
        }
        public bool WhiteCanMoveFromGraveYard()
        {
            bool result = false;
            int loc = 0;

            for (int j = 0; j < DiceRoll.Length; j++)
            {
                if (DiceRoll[j] == 0)                           //dice roll is used
                    continue;

                loc = 24 - DiceRoll[j];
                if (Math.Abs(GameBoard.DispBoard[loc]) <= 1)     //move than 1 black checker
                    result = true;
            }

            return result;
        }
        private void DropChecker(Entity _dropStack, Entity _checker)
        {
            //
            // Drop a checker onto a stack
            //
            StackComponent sc = _dropStack.GetComponent<StackComponent>();
            sc.CheckersInStack.Add(_checker);

            UpdateGameBoardFromStacks();
        }
        private void DropChecker(Entity _dropStack)
        {
            //
            // Drop is good, put checker in its new stack
            //
            StackComponent sc = _dropStack.GetComponent<StackComponent>();
            sc.CheckersInStack.Add(CheckerBeingDragged);

            UpdateGameBoardFromStacks();

            ClearDragChecker();
        }
        public void DropChecker2PreviousPosition()
        {
            //
            // Drop failed, put checker back in its original stack
            //
            DragComponent dc = CheckerBeingDragged.GetComponent<DragComponent>();
            Entity FromEnt = dc.FromStack;
            StackComponent sc = FromEnt.GetComponent<StackComponent>();
            sc.CheckersInStack.Add(CheckerBeingDragged);

            ClearDragChecker();
        }
        public void ClearDragChecker()
        {
            DragComponent dc = CheckerBeingDragged.GetComponent<DragComponent>();
            CheckerBeingDragged.RemoveComponent(dc);
        }
        private void Fill_GameBoard_Stacks()
        {
            int fanout = 3;
            int chkNumBlack = 0;
            int chkNumWhite = 0;
            SpriteRenderer sp;
            //
            // There are 25 entities with Stacks of Checkers
            //
            for (int i = 0; i < TotalNumOfStacks; i++)
            {
                switch (i)
                {
                    case int n when (n <= 11):               //bottom of board
                        fanout = 3;
                        break;
                    case int n when (n >= 12 && n <= 23):    //top of board
                        fanout = 4;
                        break;
                    case 24:
                        fanout = 4;     //downward (white graveyard)
                        break;
                    case 25:
                        fanout = 3;     //upward (black graveyard)
                        break;
                    case 26:
                        fanout = 3;     //upward (white collector)
                        break;
                    case 27:
                        fanout = 4;     //downward (black collector)
                        break;
                }
                //
                // Get a stack to fill with Checker entitites (using the GameBoard)
                //
                Entity stackLocation = GameStacks[i];        
                StackComponent sc = stackLocation.GetComponent<StackComponent>();
                sc.FannedDirection = fanout;
                sc.CheckersInStack.Clear();

                int _totalchkers = Math.Abs(GameBoard.DispBoard[i]);
                for (int j = 0; j < _totalchkers; j++)
                {
                    Entity _checker = CreateEntity("Checker" + i.ToString());
                    
                    CheckerComponent cc = new CheckerComponent();
                    cc.CName = "Checker" + i.ToString();

                    if (GameBoard.DispBoard[i] < 0)
                    {
                        sp = new SpriteRenderer(Content.Load<Texture2D>("BlackChecker"));
                        cc.IsWhite = false;
                        chkNumBlack -= 1;
                        _checker.Tag = chkNumBlack;
                    }
                    else
                    {
                        sp = new SpriteRenderer(Content.Load<Texture2D>("WhiteChecker"));
                        cc.IsWhite = true;
                        chkNumWhite += 1;
                        _checker.Tag = chkNumWhite;
                    }
                    _checker.AddComponent(sp);     //add sprite component

                    cc.HoldingStack = sc;
                    cc.CheckerFace = sp.Sprite;
                    _checker.AddComponent(cc);      //add check component

                    sc.CheckersInStack.Add(_checker);
                }

            }
        }
        public void EndOfGame()
        {
            //
            // Exit button is pressed
            //
            TextEntity.Transform.Position = new Vector2(350, 400);
            var txt = TextEntity.GetComponent<TextComponent>();
            txt.RenderLayer = -100;
            if (PlayerWon == 0)
                txt.SetText("GAME IS OVER ! White Player Won.");
            else
                txt.SetText("GAME IS OVER ! Black Player Won.");

            txt.SetColor(Color.White);
        }
        private void MoveBlackGraveYard2Board()
        {
            int hit = -1;
            int place = -1;
            int loc = 0;
            int diceLoc = -1;
            //    
            // Must seat the grave yard checkers first
            //
            int gyCnt = TestGraveYardForCheckers(25);          //if true, then grave yard checkers go first
            if (gyCnt <= 0)
            {
                BlackGraveYard = false;
                //WhiteCanMove = true;
                return;
            }

            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznz
            // Hit a single WHITE checker
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznz
            for (int j = 0; j < DiceRoll.Length; j++)
            {
                if (DiceRoll[j] == 0)                           //dice roll is used
                    continue;

                loc = DiceRoll[j] - 1;
                if (GameBoard.DispBoard[loc] == 1)             //a single white  checker
                {
                    if (hit < 0)
                    {
                        hit = loc;                              //take the first location of the hit
                        diceLoc = j;                            //dice used
                    }
                    //
                    // always hit the lowest location first
                    //
                    if (loc < hit)
                    {
                        hit = loc;
                        diceLoc = j;                            //dice used
                    }
                                                  
                }
            }
            //
            // If a hit is possible, fix the board 
            //
            if (hit >= 0)
            {
                GameBoard.DispBoard[hit] = -1;              //place black checker on board
                GameBoard.DispBoard[25] += 1;               //take one off of black grave yard
                GameBoard.DispBoard[24] += 1;               //put one into white grave yard
                DiceRoll[diceLoc] = 0;                          //dice roll is used

                return;
            }

            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznz
            // Land on single BLACK checker
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznz
            place = -1;
            for (int j = 0; j < DiceRoll.Length; j++)
            {
                if (DiceRoll[j] == 0)                           //dice roll is used
                    continue;

                loc = DiceRoll[j] - 1;
                if (BlackCheckerSingle(loc))                    //a black single checker
                {
                    if (place < 0)
                    {
                        place = loc;                            //take the first location of the hit
                        diceLoc = j;                            //dice used
                        break;

                    }
                }
            }

            if (place >= 0)
            {
                GameBoard.DispBoard[25] += 1;              //take one off of black grave yard
                GameBoard.DispBoard[place] += -1;          //place black checker on board
                DiceRoll[diceLoc] = 0;

                return;
            }
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznz
            // Land on a stack of BLACK checkers
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznz
            place = -1;
            for (int j = 0; j < DiceRoll.Length; j++)
            {
                if (DiceRoll[j] == 0)                           //dice roll is used
                    continue;

                loc = DiceRoll[j] - 1;
                if (BlackCheckerStack(loc))        //a black single checker
                {
                    if (place < 0)
                    {
                        place = loc;                            //take the first location of the hit
                        diceLoc = j;                            //dice used
                        break;

                    }
                }
            }

            if (place >= 0)
            {
                GameBoard.DispBoard[25] += 1;                   //take one off of black grave yard
                GameBoard.DispBoard[place] += -1;              //place black checker on board
                DiceRoll[diceLoc] = 0;

                return;
            }
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznz
            // Place on EMPTY location
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznz
            place = -1;
            for (int j = 0; j < DiceRoll.Length; j++)
            {
                if (DiceRoll[j] == 0)                           //dice roll is used
                    continue;

                loc = DiceRoll[j] - 1;
                if (EmptyCheckerStack(loc))        //a black single checker
                {
                    if (place < 0)
                    {
                        place = loc;                            //take the first location of the hit
                        diceLoc = j;                            //dice used
                        break;

                    }
                }
            }

            if (place >= 0)
            {
                GameBoard.DispBoard[25] += 1;                   //take one off of black grave yard
                GameBoard.DispBoard[place] += -1;              //place black checker on board
                DiceRoll[diceLoc] = 0;

                return;
            }
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznz
            // BLACK cannot move from graveyard to board, it loses its turn
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznz
            BlackGraveYard = true;
            WhiteCanMove = true;
        }
        private bool EmptyCheckerStack(int _boardLoc)
        {
            //
            // stack is empty
            //
            if (GameBoard.DispBoard[_boardLoc] == 0)
                return true;

            return false;
        }
        private bool BlackCheckerStack(int _boardLoc)
        {
            //
            // stack of black checkers (2 or more)
            //
            if (GameBoard.DispBoard[_boardLoc] < 0)
            {
                if (Math.Abs(GameBoard.DispBoard[_boardLoc]) > 1)        //a black stack of checkers
                    return true;
            }
            return false;
        }
        private bool BlackCheckerSingle(int _boardLoc)
        {
            //
            // single black checker
            //
            if (GameBoard.DispBoard[_boardLoc] == -1)        //a black single checker
                return true;

            return false;
        }
        private void BlackDiceBtn_OnClicked(Button button)
        {
            //
            // Not black turn yet
            //
            if (WhiteCanMove)
                return;

            Roll_The_Dice();
            //DiceRoll = new int[4];
            //DiceRoll[0] = 1;
            //DiceRoll[1] = 1;
            //DiceRoll[2] = 1;
            //DiceRoll[3] = 1;


            DispBlackDiceValues();                               //screen display
            WhiteCanMove = false;                                   //white cannot move
            BlackGraveYard = false;

            int gyCnt = TestGraveYardForCheckers(25);          //if true, then grave yard checkers go first
            for (int i = 0; i < gyCnt; i++)
            {
                MoveBlackGraveYard2Board();                        //must sit from GraveYard first
            }
            //
            // Either we have checkers in graveyard and lose the turn
            // or
            // We need the AI to give us the moves
            //
            if (BlackGraveYard)
            {
                //
                // Black has checkers in graveyard that it cannot move
                //
                WhiteCanMove = true;
                UpdateStacksFromGameBoard();
                return;
            }
            //znznznznznznznznznznznznznznznznzn
            // AI needs to make a move
            //znznznznznznznznznznznznznznznznzn
            //
            // Reverse sort of the dice.  This will put all the used dice at end
            //
            Array.Reverse(DiceRoll);
            int diceLeft = 0;
            for (int i = 0; i < DiceRoll.Length; i++)          // how many real dice move do we have
            {
                if (DiceRoll[i] > 0)
                    diceLeft += 1;
            }
            if (diceLeft > 0)
            {
                int[] dice2Play = new int[diceLeft];
                diceLeft = 0;

                for (int i = 0; i < DiceRoll.Length; i++)          // how many real dice move do we have
                {
                    if (DiceRoll[i] > 0)
                    {
                        dice2Play[diceLeft] = DiceRoll[i];
                        diceLeft += 1;
                    }
                        
                }
                //
                // AI move
                //
                GameState gs = new GameState(GameBoard);
                int[] newBoard = gs.GetCPUMove(dice2Play);
                if (newBoard.Length < 28)
                {
                    throw new Exception("this board is too small");
                }
                UpdateGameBoardFromCPU(newBoard);
            }

            //
            // Update stacks using GameBoard.DispBoard
            //
            UpdateStacksFromGameBoard();
        }
        private void WhiteDiceBtn_OnClicked(Button button)
        {
            //
            // White player is the only person to click on this button
            //
            Roll_The_Dice();

            RefreshWhiteDiceValues();
            //
            // Set the flag so MouseClickSystem will allow moves
            //

            //WhiteGraveYard = TestGraveYardForCheckers(24);          //if true, then grave yard checkers go first

            UpdateGameBoardFromStacks();
        }
        public void Roll_The_Dice()
        {
            //
            // random integer between min (inclusive) and max (exclusive)
            //
            int _dice1 = Nez.Random.Range(1, 7);
            int _dice2 = Nez.Random.Range(1, 7);

            //_dice1 = 1;
            //_dice2 = 1;
            //
            // if a double, then we have 4 chances to move
            //
            if (_dice1 == _dice2)
            {
                DiceRoll = new int[4];
                DiceRoll[0] = _dice1;
                DiceRoll[1] = _dice1;
                DiceRoll[2] = _dice1;
                DiceRoll[3] = _dice1;
            }
            else
            {
                DiceRoll = new int[2];
                DiceRoll[0] = _dice1;
                DiceRoll[1] = _dice2;
            }
        }
        public void DispBlackDiceValues()
        {
            //
            // Test WHITE rolls to see if more dice moves available
            //
            int cnt = 0;
            for (int i = 0; i < DiceRoll.Length; i++)
            {
                if (DiceRoll[i] == 0)
                    cnt++;
            }
            if (cnt == DiceRoll.Length)
            {
                WhiteCanMove = true;               //all dice is used
                WhoseTurn = 0;                      //white turn
            }
            else
                WhiteCanMove = false;                //black still moves left

            var txt = TextEntity.GetComponent<TextComponent>();
            txt.RenderLayer = -100;

            txt.SetText("Black Dice Roll: " + DiceRoll[0].ToString() + " - " + DiceRoll[1].ToString() + "   " + DiceRoll.Count().ToString());
            txt.SetColor(Color.White);


        }
        public void RefreshWhiteDiceValues()
        {
            //
            // Test WHITE rolls to see if more dice moves available
            //
            int cnt = 0;
            for (int i=0; i < DiceRoll.Length; i++)
            {
                if (DiceRoll[i] == 0)
                    cnt++;
            }
            if (cnt == DiceRoll.Length)
            {
                WhiteCanMove = false;               //all dice is used
                WhoseTurn = 1;                      //black turn
            } 
            else
                WhiteCanMove = true;                //white still moves left

            //TextEntity.Transform.Position = new Vector2(100, 20);
            var txt = TextEntity.GetComponent<TextComponent>();
            txt.RenderLayer = -100;

            txt.SetText("White Dice Roll: " + DiceRoll[0].ToString() + " - " + DiceRoll[1].ToString() + "   " + DiceRoll.Count().ToString());
            txt.SetColor(Color.White);

            
        }
        public void UpdateGameBoardFromStacks()
        {
            Entity ent;
            Entity checker;

            for (int i = 0; i < TotalNumOfStacks; i++)
            {
                GameBoard.DispBoard[i] = 0;
                ent = GameStacks[i];
                StackComponent sc = ent.GetComponent<StackComponent>();
                if (sc.CheckersInStack.Count() > 0)
                {
                    checker = sc.CheckersInStack[0];

                    if (checker.Tag > 0)
                        GameBoard.DispBoard[i] = sc.CheckersInStack.Count();
                    else
                        GameBoard.DispBoard[i] = sc.CheckersInStack.Count() * -1;
                }
            }
        }
        public void UpdateStacksFromGameBoard()
        {
            Entity entGS;

            for (int i = 0; i < TotalNumOfStacks; i++)
            {
                entGS = GameStacks[i];
                StackComponent sc = entGS.GetComponent<StackComponent>();
                if (sc.CheckersInStack.Count != 0)
                {
                    //
                    // removal of all checker entities from a stack
                    //
                    foreach(Entity chk in sc.CheckersInStack)
                    {
                        chk.RemoveAllComponents();
                        chk.Destroy();
                    }
                }
            }

            Fill_GameBoard_Stacks();
        }
        public void UpdateGameBoardFromCPU(int[] _board)
        {
            GameBoard.DispBoard = (int [])_board.Clone();
        }
    }
}
