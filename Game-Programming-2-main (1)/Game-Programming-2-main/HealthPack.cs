using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class HealthPack
{
    public Texture2D Tex;
    public Rectangle HitBox;
    public bool Active = true;

    int currentFrame = 0;
    int totalFrames = 6;

    float animationTimer = 0f;
    float animationSpeed = 0.15f;

    int frameWidth;
    int frameHeight;

    public HealthPack(Texture2D tex, Rectangle rect)
    {
        Tex = tex;
        HitBox = rect;

        frameWidth = Tex.Width / totalFrames;
        frameHeight = Tex.Height;
    }

    public void Update(GameTime gameTime)
    {
        if (!Active) return;

        animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (animationTimer >= animationSpeed)
        {
            currentFrame++;

            if (currentFrame >= totalFrames)
                currentFrame = 0;

            animationTimer = 0f;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!Active) return;

        Rectangle sourceRect = new Rectangle(
            currentFrame * frameWidth,
            0,
            frameWidth,
            frameHeight
        );

        spriteBatch.Draw(
            Tex,
            HitBox,
            sourceRect,
            Color.White
        );
    }
}