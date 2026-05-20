using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;   

namespace PROJECT
{
    public class Spike
    {
        public Rectangle HitBox;
        Texture2D texture;
        
        // TAŞ MEKANİĞİ İÇİN EKLEDİĞİMİZ DEĞİŞKEN:
        // Başlangıçta false, taş üzerine düşerse true yapacağız.
        public bool IsCovered { get; set; } = false;

        public Spike(Texture2D texture, Rectangle rect)
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