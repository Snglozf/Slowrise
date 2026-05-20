using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class Player
{
    Texture2D tex;
    Texture2D bulletTex;

    public Vector2 Pos;
    public Vector2 Velocity;
    

    public int HP = 10;
    public int MaxHP = 100;

    int frameWidth;
    int frameHeight;
    int currentFrame = 0;
    int totalFrames = 10;

    float animationTimer = 0f;
    float animationSpeed = 0.25f;

    bool facingRight = true;

    public bool isJumping = false;
    public int Direction = 1;

    public List<Bullet> Bullets = new List<Bullet>();
    private KeyboardState oldState;

    public Rectangle PlayerHitBox => new Rectangle((int)Pos.X, (int)Pos.Y, 52, 68);

    public bool IsGrounded { get; internal set; }

    public Player(Texture2D t, Texture2D bt, Vector2 p)
    {
        tex = t;
        bulletTex = bt;
        Pos = p;

        frameWidth = tex.Width / totalFrames;
        frameHeight = tex.Height;
    }

    public void Update(List<Platform> platforms, GameTime gameTime)
    {
        KeyboardState ks = Keyboard.GetState();

        if (ks.IsKeyDown(Keys.A))
        {
            Pos.X -= 5;
            Direction = -1;
            facingRight = false;

            foreach (var p in platforms)
            {
                if (PlayerHitBox.Intersects(p.HitBox))
                    Pos.X = p.HitBox.Right;
            }
        }

        if (ks.IsKeyDown(Keys.D))
        {
            Pos.X += 5;
            Direction = 1;
            facingRight = true;

            foreach (var p in platforms)
            {
                if (PlayerHitBox.Intersects(p.HitBox))
                    Pos.X = p.HitBox.Left - PlayerHitBox.Width;
            }
        }

        if (Pos.X < 0) Pos.X = 0;
        if (Pos.X > 800 - PlayerHitBox.Width) Pos.X = 800 - PlayerHitBox.Width;

        Velocity.Y += 0.6f;
        Pos.Y += Velocity.Y;

        if (Pos.Y < 0)
        {
            Pos.Y = 0;
            Velocity.Y = 0;
        }

        foreach (var p in platforms)
        {
            if (PlayerHitBox.Intersects(p.HitBox))
            {
                if (Velocity.Y > 0)
                {
                    Pos.Y = p.HitBox.Top - PlayerHitBox.Height;
                    Velocity.Y = 0;
                    isJumping = false;
                }
                else if (Velocity.Y < 0)
                {
                    Pos.Y = p.HitBox.Bottom;
                    Velocity.Y = 0;
                }
            }
        }

        if (ks.IsKeyDown(Keys.W) && !isJumping)
        {
            Velocity.Y = -13f;
            isJumping = true;
        }

        if (ks.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
        {
            Bullets.Add(new Bullet(
                bulletTex,
                new Vector2(Pos.X + 10, Pos.Y + 15),
                new Vector2(12 * Direction, 0)
            ));
            
        }

        foreach (var b in Bullets)
        
            b.Update();

        Bullets.RemoveAll(b => !b.Active || b.Pos.X > 800 || b.Pos.X < 0);
        

        UpdateAnimation(gameTime, ks);

        oldState = ks;
    }

    private void UpdateAnimation(GameTime gameTime, KeyboardState ks)
    {
        bool isMoving =
            ks.IsKeyDown(Keys.A) ||
            ks.IsKeyDown(Keys.D) ||
            ks.IsKeyDown(Keys.Left) ||
            ks.IsKeyDown(Keys.Right);

        if (isMoving)
        {
            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer >= animationSpeed)
            {
                currentFrame++;

                if (currentFrame >= totalFrames)
                    currentFrame = 0;

                animationTimer = 0f;
            }
        }
        else
        {
            currentFrame = 0;
        }
    }

   public void Draw(SpriteBatch sb)
{
    SpriteEffects effect = facingRight
        ? SpriteEffects.None
        : SpriteEffects.FlipHorizontally;

    Rectangle sourceRect = new Rectangle(currentFrame * frameWidth,0,frameWidth,frameHeight);

   
    Rectangle drawRect = new Rectangle((int)Pos.X - 50,(int)Pos.Y - 105,200,260 );

    sb.Draw(tex,drawRect,sourceRect,Color.White,0f,Vector2.Zero,effect,0f);

    foreach (var b in Bullets)
        b.Draw(sb);
}
}