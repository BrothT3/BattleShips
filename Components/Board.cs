using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class Board : Component
    {

        private int cellCount;
        private int cellSizeX;
        private int cellSizeY;
        private int yOffSet;
        private bool isHovering;

        public List<Cell> board;
        public Dictionary<Point, Cell> cells = new Dictionary<Point, Cell>();


        public Board(int cellcount, int cellsizex, int cellsizey, int yOffSet)
        {
            this.cellCount = cellcount;
            this.cellSizeX = cellsizex;
            this.cellSizeY = cellsizey;
            this.yOffSet = yOffSet;
            cells = CreateCells();
            board = CreateBoard();
        }
        public override void Awake()
        {

        }

        public override void Update(GameTime gameTime)
        {

            foreach (Cell cell in cells.Values)
            {
                cell.Update(gameTime);
            }
            foreach (Cell cell in board)
            {
                cell.Update(gameTime);
            }
        }
        public  Rectangle BoardSpace
        {
            get
            {
                return new Rectangle((int)GameObject.Transform.Position.X, (int)GameObject.Transform.Position.Y + yOffSet * cellSizeY, cellCount * cellSizeX, cellCount * cellSizeY);
            }
        }
       
        public override void Draw(SpriteBatch spriteBatch)
        {

            foreach (Cell cell in cells.Values)
            {
                cell.Draw(spriteBatch);
            }


        }
        public override void Start()
        {

            foreach (Cell cell in cells.Values)
            {
                cell.topLine = new Rectangle(cell.Position.X * cell.width, cell.Position.Y * cell.height, cell.width, 1);
                cell.bottomLine = new Rectangle(cell.Position.X * cell.width, (cell.Position.Y * cell.height) + cell.height, cell.width, 1);
                cell.rightLine = new Rectangle((cell.Position.X * cell.width) + cell.width, cell.Position.Y * cell.height, 1, cell.height);
                cell.leftLine = new Rectangle(cell.Position.X * cell.width, cell.Position.Y * cell.height, 1, cell.height);
            }
        }

        public List<Cell> CreateBoard()
        {
            List<Cell> board = new List<Cell>();

            for (int x = 0; x < cellCount; x++)
            {
                for (int y = 0; y < cellCount; y++)
                {
                    board.Add(new Cell(new Point(x, (y + yOffSet)), cellSizeX, cellSizeY));
                }

            }
            return board;
        }
        Dictionary<Point, Cell> CreateCells()
        {
            Dictionary<Point, Cell> board = new Dictionary<Point, Cell>();
            for (int y = 0; y < cellCount; y++)
            {
                for (int x = 0; x < cellCount; x++)
                {
                    board.Add(new Point(x, y), new Cell(new Point(x, (y + yOffSet)), (int)cellSizeX, cellSizeY));
                }
            }
            return board;
        }
    }
    public class Cell
    {
        public Texture2D pixel;
        private Point position;
        public int height;
        public int width;
        public bool isHovering;
        public bool IsOccupied;
        public Vector2 cellVector;
        public Point Position { get => position; set => position = value; }
        public MouseState mstate { get; set; }

        public Rectangle topLine;

        public Rectangle bottomLine;

        public Rectangle rightLine;

        public Rectangle leftLine;
        public Rectangle cellSquare
        {
            get
            {
                return new Rectangle(Position.X * width, Position.Y * height, width, height);
            }
        }

        public Cell(Point position, int width, int height)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            cellVector = new Vector2((position.X + 1) * width - (width / 2), (position.Y + 1) * height - (height));
            IsOccupied = false;
        }
        public void LoadContent()
        {
            topLine = new Rectangle(Position.X * width, Position.Y * height, width, 1);
            bottomLine = new Rectangle(Position.X * width, (Position.Y * height) + height, width, 1);
            rightLine = new Rectangle((Position.X * width) + width, Position.Y * height, 1, height);
            leftLine = new Rectangle(Position.X * width, Position.Y * height, 1, height);
        }
        public void Update(GameTime gameTime)
        {
            mstate = Mouse.GetState();
            isHovering = false;

            if (cellSquare.Intersects(new Rectangle(mstate.X, mstate.Y, 2, 2)))
            {
                isHovering = true;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.DarkBlue * 0.2f;
            if (isHovering)
            {
                color = Color.Red * 0.9f;
            }

            spriteBatch.Draw(GameWorld.Instance.pixel, cellSquare, color);

            spriteBatch.Draw(GameWorld.Instance.pixel, topLine, Color.Black);
            spriteBatch.Draw(GameWorld.Instance.pixel, bottomLine, Color.Black);
            spriteBatch.Draw(GameWorld.Instance.pixel, rightLine, Color.Black);
            spriteBatch.Draw(GameWorld.Instance.pixel, leftLine, Color.Black);


        }
    }

}




