using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PROJECT // Kendi proje adın (namespace) neyse onunla değiştirebilirsin
{
    public class Enemy
    {
        public Texture2D tex;
        public Texture2D bulletTex;
        public Vector2 Pos;
        public Vector2 Velocity;
        public int HP = 150;
        public bool Active = true;

        private float shootTimer = 0f;
        private float shootInterval = 1.2f;
        private static Random rnd = new Random();
        private float jumpTimer = 0f;
        public bool IsBattleStarted = false;

        int frameWidth;
        int frameHeight;
        int currentFrame = 0;
        int totalFrames = 8;
        float animationTimer = 0f;
        float animationSpeed = 0.12f;
        bool facingRight = true;

        int enemyWidth = 120;
        int enemyHeight = 160;

        public List<Bullet> Bullets = new List<Bullet>();

        public Rectangle HitBox => new Rectangle((int)Pos.X, (int)Pos.Y, enemyWidth, enemyHeight);

        public Enemy(Texture2D t, Texture2D bt, Vector2 p)
        {
            tex = t;
            bulletTex = bt;
            Pos = p;

            frameWidth = tex.Width / totalFrames;
            frameHeight = tex.Height;
        }

        public void Update(GameTime gameTime, Vector2 playerPos, List<Platform> platforms)
        {
            if (HP <= 0) { Active = false; return; }

            Velocity.Y += 0.5f;
            Pos.Y += Velocity.Y;

            foreach (var p in platforms)
            {
                if (this.HitBox.Intersects(p.HitBox))
                {
                    if (Velocity.Y > 0)
                    {
                        if (p.HitBox.Top > 500)
                        {
                            Pos.Y = p.HitBox.Top - enemyHeight + 50;
                            Velocity.Y = 0;
                        }
                    }
                }
            }

            if (playerPos.Y > 450)
            {
                IsBattleStarted = true;
            }

            bool isMoving = false;

            if (IsBattleStarted)
            {
                if (Pos.X < playerPos.X - 60)
                {
                    Pos.X += 1.5f;
                    facingRight = true;
                    isMoving = true;
                }
                else if (Pos.X > playerPos.X + 60)
                {
                    Pos.X -= 1.5f;
                    facingRight = false;
                    isMoving = true;
                }

                if (isMoving)
                {
                    animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (animationTimer >= animationSpeed)
                    {
                        animationTimer = 0f;
                        currentFrame++;

                        if (currentFrame >= totalFrames)
                            currentFrame = 0;
                    }
                }
                else
                {
                    currentFrame = 0;
                }

                jumpTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (jumpTimer >= 1.0f)
                {
                    if (Velocity.Y == 0 && rnd.Next(0, 100) < 30)
                    {
                        Velocity.Y = -10f;
                    }
                    jumpTimer = 0f;
                }

                shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shootTimer >= shootInterval)
                {
                    Shoot(playerPos);
                    shootTimer = 0f;
                }
            }

            foreach (var b in Bullets) b.Update();
            Bullets.RemoveAll(b => !b.Active);
        }

        private void Shoot(Vector2 target)
        {
            Vector2 enemyCenter = new Vector2(Pos.X + enemyWidth / 2, Pos.Y + enemyHeight / 2);
            Vector2 playerCenter = new Vector2(target.X + 25, target.Y + 25);
            Vector2 direction = playerCenter - enemyCenter;

            if (direction != Vector2.Zero)
                direction.Normalize();

            Bullets.Add(new Bullet(
                bulletTex,
                new Vector2(enemyCenter.X, enemyCenter.Y - 20),
                direction * 8f
            ));

            
        }

        public void Draw(SpriteBatch sb)
        {
            if (!Active) return;

            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            Rectangle destRect = new Rectangle((int)Pos.X, (int)Pos.Y, enemyWidth, enemyHeight);
            SpriteEffects flip = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            sb.Draw(tex, destRect, sourceRect, Color.White, 0f, Vector2.Zero, flip, 0f);

            foreach (var b in Bullets) b.Draw(sb);
        }
    }
}