using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FloodFillTest
{

    public struct Player
    {
       public int ID;
        public Player(int id) { ID = id; }
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public int[,] Grid;
        public int WIDTH; 
        public int HEIGHT;  

        Texture2D texture;

        int CellSize = 20;

        public List<Player> players;
        public int currentPlayer;
       
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            players = new List<Player>();
            
        }

        
        protected override void Initialize()
        {
            texture = Content.Load<Texture2D>("cp437T");
            WIDTH = GraphicsDevice.Viewport.Width / 10 ;
            HEIGHT = GraphicsDevice.Viewport.Height / 10;
            
            Grid = new int[WIDTH, HEIGHT];

            ClearGrid();

            players.Add(new Player(219));
            players.Add(new Player(4));

            base.Initialize();
        }

       
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

       
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update();
            if (InputHandler.IsKeyPressed(Keys.Escape))
                Exit();


            if (InputHandler.IsMouseRButtonPressed())
            {
                Point targetCell = ConvertToGridSpace(InputHandler.MousePosition);
                FillGrid(targetCell, players[currentPlayer].ID);
            }

            if (InputHandler.WasKeyPressed(Keys.Enter)) { AutomaticFloodFill(players[currentPlayer].ID); }
            if (InputHandler.WasKeyPressed(Keys.Back)) { ClearGrid(); }

            if (InputHandler.WasKeyPressed(Keys.Down))
            {
                currentPlayer++;
                if (currentPlayer > players.Count-1)
                {
                    currentPlayer = 0;
                }
            }

            base.Update(gameTime);
        }

        private void ClearGrid()
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    Grid[x, y] = 0;
                }
            }
        }

        private void FillGrid(Point targetCell, int Glyph)
        {
            if (IsInsideGrid(targetCell)) 
                Grid[targetCell.X, targetCell.Y] = Glyph; 
        }

        private void AutomaticFloodFill(int Glyph)
        {
            int[,] midlertidig = new int[WIDTH, HEIGHT];

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    midlertidig[x, y] = Grid[x, y];
                }
            }


            // Pass 0. Sletter andre brukeres celler.
            for (int x = 1; x < WIDTH - 1; x++)
            {
                for (int y = 1; y < HEIGHT - 1; y++)
                {
                    if (Grid[x, y] != Glyph) { Grid[x, y] = 0; }
                }
            }

            // Pass 1. Fyller fra tomme kanter. Setter alle tomme ruter til -1
            for ( int x = 0; x < WIDTH; x++)
            {
                FloodFillPoint(x, 0, -1);
                FloodFillPoint(x, HEIGHT, -1);
            }
            for (int y = 0; y < HEIGHT; y++)
            {
                FloodFillPoint(0, y, -1);
                FloodFillPoint(WIDTH, y, -1);
            }

            // Pass 2. Fyller alle -1 ruter tilbake til 0

            for (int x = 1; x < WIDTH-1; x++)
            {
                for (int y = 1; y < HEIGHT-1; y++)
                {
                    if (Grid[x,y] == -1) { Grid[x, y] = 0; }
                    else { Grid[x, y] = Glyph; }   
                }
            }

            // Pass 3. Fyller tilbake andre brukeres celler

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    if (Grid[x,y] == 0 && midlertidig[x,y] == 0) { continue; }
                    if (Grid[x,y] == 0 && midlertidig[x,y] != 0)
                                      { Grid[x, y] = midlertidig[x, y]; }
                }
            }


        }

        void FloodFillPoint(int x, int y, int value)
        {
            Queue<Point> Frontier = new Queue<Point>();
            List<Point> Visited = new List<Point>();
            Point Origin = new Point(x, y);

            Frontier.Enqueue(Origin);
            Visited.Add(Origin);

            while (Frontier.Count > 0 )
            {
                Point current = Frontier.Dequeue();
                foreach (Point p in GetNeighbors(current))
                {
                    if ( Visited.Contains(p)) { continue; }
                    if (Grid[p.X, p.Y] != 0 ) { continue; }
                    Grid[p.X, p.Y] = value;
                    Frontier.Enqueue(p);
                    Visited.Add(p);
                }
            }

        }

        private Point ConvertToGridSpace(Point mousePosition)
        {
            return new Point(mousePosition.X / CellSize,
                             mousePosition.Y / CellSize);
        }

        

        private IEnumerable<Point> GetNeighbors(Point center)
        {
            Point returnPoint;
            for (int localX = - 1; localX < 2; localX++)
            {
                for (int localY = - 1; localY < 2; localY++)
                {
                    if (Math.Abs(localX) == Math.Abs(localY)) { continue; }

                    returnPoint = new Point(localX + center.X, localY + center.Y);

                    if (IsInsideGrid(returnPoint))

                    yield return returnPoint;
                }
            }
        }

        private bool IsInsideGrid(Point point)
        {
            if (point.X < 1 ) { return false;}
            if ( point.Y < 1 ) { return false; }
            if ( point.X >= WIDTH-1) { return false; }
            if (point.Y >= HEIGHT-1) { return false; }

            return true;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            int x_offset, y_offset;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    {
                        x_offset = Grid[x, y] % 16;
                        y_offset = Grid[x, y] / 16;

                        spriteBatch.Draw(texture, new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize),
                            new Rectangle(x_offset * 10, y_offset * 10, 10, 10), Color.White);
                    }
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
