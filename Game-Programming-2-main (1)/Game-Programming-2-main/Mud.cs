using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PROJECT
{
    public class Mud
    {
        public Rectangle HitBox;
        Texture2D texture;

        public Mud(Texture2D texture, Rectangle rect)
        {
            this.texture = texture;
            this.HitBox = rect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, HitBox, Color.White);
        }
    }
}