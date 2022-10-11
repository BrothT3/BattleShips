using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BattleShips
{
    public class SpriteRenderer : Component
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Origin { get; set; }
        public int OffSetY { get; set; } = 0;
        public int OffSetX { get; set; } = 0;
        public float Rotation { get; set; } = 0;
        public override void Start()
        {
            Origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
        }

        /// <summary>
        /// Sets the sprite of a component through the instance of a SpriteRenderer
        /// </summary>
        /// <param name="spriteName"></param>
        public void SetSprite(string spriteName)
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Vector2(GameObject.Transform.Position.X + OffSetX, GameObject.Transform.Position.Y + OffSetY), null, Color.White, Rotation, Origin, 1, SpriteEffects.None, 1);
        }
    }
}
