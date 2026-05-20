using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PROJECT
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        Level level;
        Texture2D pixel;
        Texture2D backgroundTexture;
        Texture2D DoorTexture;
        Texture2D healthTexture;
        Texture2D spikeTexture;
        Texture2D mudTexture;
        Texture2D playerTexture;
        Texture2D enemyTexture;
        int heartFrameWidth;
        int heartFrameHeight;
       
        bool wasOnMud = false;

        SpriteFont myFont;
        int levelNum = 1;

      
        double spikeDamageTimer = 0;
        bool wasOnSpike = false;

        enum State { Menu, Play, Over, Win }
        State gameState = State.Menu;
        State previousState = State.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            myFont = Content.Load<SpriteFont>("MyFont");
            DoorTexture = Content.Load<Texture2D>("door");
            healthTexture = Content.Load<Texture2D>("Hearts");
            heartFrameWidth = 16;
            heartFrameHeight = 16;
            spikeTexture = Content.Load<Texture2D>("spike");
            mudTexture = Content.Load<Texture2D>("mud");
            playerTexture = Content.Load<Texture2D>("playerr");
            enemyTexture = Content.Load<Texture2D>("enemy");

            try { backgroundTexture = Content.Load<Texture2D>("background"); }
            catch { }

          
            SoundManager.LoadContent(Content);
        }

        void LoadLevel(int n)
        {
            level = new Level();

            Vector2 spawnPos;

            if (n == 1)
                spawnPos = new Vector2(50, 350);
            else if (n == 2)
                spawnPos = new Vector2(50, 80);
            else
                spawnPos = new Vector2(50, 80);

            if (player == null)
            {
                player = new Player(playerTexture, pixel, spawnPos);
                player.HP = 10;
            }
            else
            {
                player.Pos = spawnPos;
            }

            if (n == 1)
            {
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 430, 260, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(460, 480, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(580, 480, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(660, 390, 140, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(250, 270, 50, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(360, 290, 260, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 230, 180, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(100, 110, 250, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(350, 110, 80, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(520, 110, 130, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(690, 110, 110, 30)));

                level.Door = new Door(DoorTexture, new Rectangle(720, 10, 113, 125));

                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(150, 380, 40, 40)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(500, 220, 40, 40)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(600, 60, 40, 40)));

                level.Spikes.Add(new Spike(spikeTexture, new Rectangle(0, 560, 800, 40)));
            }
            else if (n == 2)
            {
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 150, 130, 30)));
               
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 520, 180, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(260, 480, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(360, 420, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(450, 360, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(500, 240, 40, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(620, 360, 180, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(620, 150, 180, 30)));

                level.Door = new Door(DoorTexture, new Rectangle(730, 50, 113, 125));

                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(280, 420, 40, 40)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(650, 300, 40, 40)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(0, 450, 40, 40)));

                level.Spikes.Add(new Spike(spikeTexture, new Rectangle(0, 560, 800, 40)));
            }
            else if (n == 3)
            {
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 150, 150, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(280, 220, 150, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(460, 320, 180, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(750, 320, 100, 30)));
                level.Platforms.Add(new Platform(pixel, new Rectangle(0, 530, 800, 70)));
                level.Muds.Add(new Mud(mudTexture, new Rectangle(638, 320, 125, 35)));

                level.Enemy = new Enemy(enemyTexture, pixel, new Vector2(600, 480));

                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(320, 170, 40, 40)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(580, 270, 40, 40)));
                level.HealthPacks.Add(new HealthPack(healthTexture, new Rectangle(700, 270, 40, 40)));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Escape)) Exit();
          
            if (ks.IsKeyDown(Keys.R)) 
            {
                gameState = State.Menu;
    
                if (player != null) 
                {
                    player.HP = 10; 
                }
            }

            if (gameState == State.Menu)
            {
                if (ks.IsKeyDown(Keys.Enter))
                {
                    levelNum = 1;
                    LoadLevel(levelNum); 
                    gameState = State.Play;
                }
            }
            else if (gameState == State.Play)
            {
                player.Update(level.Platforms, gameTime);
                bool isOnMud = false;

                foreach (var m in level.Muds)
                {
                    Rectangle playerBox = player.PlayerHitBox;
                    Rectangle mudBox = m.HitBox;

                    if (playerBox.Intersects(mudBox))
                    {
                        isOnMud = true;
                        Rectangle overlap = Rectangle.Intersect(playerBox, mudBox);
                        
                        bool fromTop = playerBox.Bottom - overlap.Height <= mudBox.Top + 5;

                        if (fromTop)
                        {
                            player.Pos.Y = mudBox.Top - player.PlayerHitBox.Height;
                            player.Velocity.Y = 0;
                            player.isJumping = false;
                        }
                        else
                        {
                            if (playerBox.Center.X < mudBox.Center.X)
                                player.Pos.X -= overlap.Width;
                            else
                                player.Pos.X += overlap.Width;
                        }

                        if (!wasOnMud)
                        {
                            player.HP -= 2;
                            spikeDamageTimer = 0;
                        }

                        break;
                    }
                }

                wasOnMud = isOnMud;
                spikeDamageTimer += gameTime.ElapsedGameTime.TotalSeconds;

                bool isOnSpike = false;

                foreach (var s in level.Spikes)
                {
                    if (player.PlayerHitBox.Intersects(s.HitBox))
                    {
                        isOnSpike = true;

                        if (!wasOnSpike) 
                        {
                            player.HP -= 2;
                            spikeDamageTimer = 0;
                        }

                        if (spikeDamageTimer >= 0.8f)
                        {
                            player.HP -= 2;
                            spikeDamageTimer = 0;
                        }

                        break;
                    }
                }

                wasOnSpike = isOnSpike;

                foreach (var hp in level.HealthPacks)
                {
                    hp.Update(gameTime);
                    if (hp.Active && player.PlayerHitBox.Intersects(hp.HitBox))
                    {
                        player.HP += 10;
                        if (player.HP > 100)
                            player.HP = 100;

                        hp.Active = false;
                    }
                }

                if (level.Enemy != null)
                {
                    level.Enemy.Update(gameTime, player.Pos, level.Platforms);

                    foreach (var bullet in level.Enemy.Bullets)
                    {
                        if (bullet.Active && player.PlayerHitBox.Intersects(bullet.HitBox))
                        {
                            player.HP -= 10; 
                            bullet.Active = false;
                        }
                    }

                    foreach (var bullet in player.Bullets)
                    {
                        if (bullet.Active && level.Enemy.Active && bullet.HitBox.Intersects(level.Enemy.HitBox))
                        {
                            level.Enemy.HP -= 5; 
                            bullet.Active = false;
                        }
                    }

                    if (!level.Enemy.Active || level.Enemy.HP <= 0)
                    {
                        gameState = State.Win;
                    }
                }

                if (level.Door != null && player.PlayerHitBox.Intersects(level.Door.HitBox))
                {
                    levelNum++;

                    if (levelNum > 3)
                        gameState = State.Win;
                    else
                        LoadLevel(levelNum);
                }

                if (player.HP <= 0 || player.Pos.Y > 600)
                    gameState = State.Over;
            }

            // Durum Değişikliklerine Göre Müzik Yönetimi (Sadece 3 müzik için)
            if (gameState != previousState)
            {
                if (gameState == State.Menu)
                {
                    SoundManager.StopMusic(); // Menüye dönülürse müziği kes
                }
                else if (gameState == State.Play)
                {
                    SoundManager.PlayMusic("Audio/level_music", true); // Döngüsel çalsın
                }
                else if (gameState == State.Over)
                {
                    SoundManager.PlayMusic("Audio/lose_music", false); // Bir kere çalsın
                }
                else if (gameState == State.Win)
                {
                    SoundManager.PlayMusic("Audio/win_music", false); // Bir kere çalsın
                }

                previousState = gameState;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if (backgroundTexture != null)
                spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 800, 600), Color.White);

            if (gameState == State.Menu)
            {
                spriteBatch.Draw(pixel, new Rectangle(0, 0, 800, 600), Color.Black * 0.6f);
                spriteBatch.DrawString(myFont, "SLOWRISE", new Vector2(300, 150), Color.Yellow);
                spriteBatch.DrawString(myFont, "PRESS ENTER TO START", new Vector2(230, 250), Color.White);
                spriteBatch.DrawString(myFont, "PRESS R TO EXIT", new Vector2(260, 350), Color.White);
            }
            else if (gameState == State.Play)
            {
                foreach (var s in level.Spikes)
                    s.Draw(spriteBatch);

                foreach (var hp in level.HealthPacks)
                    hp.Draw(spriteBatch);

                level.Draw(spriteBatch);
                player.Draw(spriteBatch);
                
                foreach (var m in level.Muds)
                    m.Draw(spriteBatch);

                if (level.Enemy != null && level.Enemy.Active)
                {
                    int maxHp = 150;
                    Vector2 barPos = new Vector2(level.Enemy.Pos.X, level.Enemy.Pos.Y - 30);
                    spriteBatch.Draw(pixel, new Rectangle((int)barPos.X, (int)barPos.Y, 60, 8), Color.Black * 0.6f);

                    int width = (level.Enemy.HP * 60) / maxHp;
                    if (width < 0) width = 0;
                    if (width > 60) width = 60;

                    spriteBatch.Draw(pixel, new Rectangle((int)barPos.X, (int)barPos.Y, width, 8), Color.Red);
                }

                int hpBarWidth = (player.HP * 300) / 100;

                if (hpBarWidth > 300) hpBarWidth = 300;
                if (hpBarWidth < 0) hpBarWidth = 0;

                spriteBatch.Draw(pixel, new Rectangle(10, 10, 300, 25), Color.Black * 0.5f);
                spriteBatch.Draw(pixel, new Rectangle(10, 10, hpBarWidth, 25), Color.Red);
                spriteBatch.DrawString(myFont, "HP: " + player.HP + "/100", new Vector2(15, 10), Color.White);
                spriteBatch.DrawString(myFont, "LEVEL: " + levelNum, new Vector2(650, 10), Color.Gold);
            }
            else if (gameState == State.Over)
            {
                spriteBatch.DrawString(myFont, "GAME OVER", new Vector2(300, 250), Color.White);
            }
            else if (gameState == State.Win)
            {
                spriteBatch.DrawString(myFont, "YOU WIN!", new Vector2(300, 250), Color.Gold);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}