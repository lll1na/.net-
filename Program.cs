using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using System;

enum ItemType { Coin, Bomb }

//Pelintila
enum GameState { Playing, GameOver }

//Taivaasta putoava esine
struct Item
{
    public Rectangle Rect;
    public Color Color;
    public ItemType Type;
    public bool Active;
}

class Game
{
   
    const int ScreenWidth = 800;
    const int ScreenHeight = 600;

    const float PlayerScale = 2.0f;

    const float ItemSize = 30f;
    const float ItemSpeed = 250f;
    const float SpawnInterval = 0.8f;
    const float BombSpawnChance = 0.3f; // 30% Pommin räjähdys


    const int CoinScore = 10;

    
    const int HudFontSize = 25;

    Texture2D playerTex;
    Sound collectSound;
    Sound explodeSound;

    Rectangle player;
    List<Item> items = new List<Item>();
    Random rng = new Random();

    int score;
    int lives;
    float spawnTimer;
    GameState state;

    //Resurssien lataus ja alkuasetukset
    public void Load()
    {
        playerTex = Raylib.LoadTexture("player.png");
        Raylib.SetTextureFilter(playerTex, TextureFilter.Point);

        collectSound = Raylib.LoadSound("collect.wav");
        explodeSound = Raylib.LoadSound("explode.wav");

        StartGame();
    }

    //Kaikkien muuttujien nollaaminen uutta peliä varten
    void StartGame()
    {
        score = 0;
        lives = 3;
        spawnTimer = 0;
        items.Clear();
        state = GameState.Playing;

        // pelaaja näytön alareunaan keskelle
        float playerW = playerTex.Width * PlayerScale;
        float playerH = playerTex.Height * PlayerScale;
        player = new Rectangle(
            ScreenWidth / 2f - playerW / 2f,
            ScreenHeight - playerH - 10,
            playerW,
            playerH
        );
    }

    //Pelilogiikan päivitys joka ruudulla
    public void Update()
    {
        if (state == GameState.GameOver)
        {
            
            if (Raylib.IsKeyPressed(KeyboardKey.R))
                StartGame();
            return;
        }

        float dt = Raylib.GetFrameTime();

        MovePlayer();
        SpawnItems(dt);
        UpdateItems(dt);
    }

    //Pelaajan liike hiiren mukaan
    void MovePlayer()
    {
        player.X = Raylib.GetMouseX() - player.Width / 2;

        //Älä mene näytön reunojen yli
        if (player.X < 0) player.X = 0;
        if (player.X > ScreenWidth - player.Width) player.X = ScreenWidth - player.Width;
    }

    //Uusien esineiden spawnaus ajanjakson kautta
    void SpawnItems(float dt)
    {
        spawnTimer += dt;
        if (spawnTimer < SpawnInterval) return;

        spawnTimer = 0;

        // Выбрать тип предмета случайно
        ItemType type = rng.NextSingle() < BombSpawnChance ? ItemType.Bomb : ItemType.Coin;

        items.Add(new Item
        {
            Rect = new Rectangle(rng.Next(0, (int)(ScreenWidth - ItemSize)), -ItemSize, ItemSize, ItemSize),
            Type = type,
            Color = (type == ItemType.Coin) ? Color.Gold : Color.Red,
            Active = true
        });
    }

    //Kaikkien kohteiden päivitys: liike, törmäykset, poisto
    void UpdateItems(float dt)
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            Item item = items[i];

            //Liikuta esinettä alas
            item.Rect.Y += ItemSpeed * dt;

            // Tarkista törmäys vain aktiivisille kohteille
            if (item.Active && Raylib.CheckCollisionRecs(player, item.Rect))
            {
                item.Active = false;
                OnItemCollected(item);
            }

            // Poista esine, jos se on tullut näytön ulkopuolelle tai on epäaktiivinen
            if (!item.Active || item.Rect.Y > ScreenHeight)
            {
                items.RemoveAt(i);
                continue;
            }

            items[i] = item;
        }
    }

    // Reaktio esineen valintaan
    void OnItemCollected(Item item)
    {
        if (item.Type == ItemType.Coin)
        {
            score += CoinScore;
            Raylib.PlaySound(collectSound);
        }
        else if (item.Type == ItemType.Bomb)
        {
            lives--;
            Raylib.PlaySound(explodeSound);

            if (lives <= 0)
                state = GameState.GameOver;
        }
    }

    // Koko kehyksen piirto
    public void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.DarkBlue);

        if (state == GameState.Playing)
            DrawGame();
        else
            DrawGameOver();

        Raylib.EndDrawing();
    }

    // Pelin näytön piirtäminen
    void DrawGame()
    {
        DrawItems();
        DrawPlayer();
        DrawHud();
    }

    // Kaikkien aktiivisten esineiden piirtäminen
    void DrawItems()
    {
        foreach (var item in items)
        {
            if (!item.Active) continue;

            if (item.Type == ItemType.Coin)
            {
               
                int cx = (int)(item.Rect.X + item.Rect.Width / 2);
                int cy = (int)(item.Rect.Y + item.Rect.Height / 2);
                Raylib.DrawCircle(cx, cy, item.Rect.Width / 2 - 3, item.Color);
            }
            else
            {
          
                Raylib.DrawRectangleRec(item.Rect, item.Color);
            }
        }
    }

    
    void DrawPlayer()
    {
        if (playerTex.Id > 0)
        {
            Vector2 pos = new Vector2(player.X, player.Y);
            Raylib.DrawTextureEx(playerTex, pos, 0.0f, PlayerScale, Color.White);
        }
        else
        {
            // Varavaihtoehto, jos tekstuuri ei lataudu
            Raylib.DrawRectangleRec(player, Color.Green);
        }
    }

    
    void DrawHud()
    {
        Raylib.DrawText($"Score: {score}", 10, 10, HudFontSize, Color.White);
        Raylib.DrawText($"Lives: {lives}", 10, 40, HudFontSize, Color.Red);
    }

    // Pelin loppunäytön piirto
    void DrawGameOver()
    {
        int cx = ScreenWidth / 2;
        int cy = ScreenHeight / 2;

        Raylib.DrawText("GAME OVER", cx - 100, cy - 20, 40, Color.White);
        Raylib.DrawText($"Final Score: {score}", cx - 80, cy + 30, 20, Color.Gray);
        Raylib.DrawText("Press R to Restart", cx - 90, cy + 60, 20, Color.Yellow);
    }

    // Resurssien purkaminen suljettaessa
    public void Unload()
    {
        Raylib.UnloadTexture(playerTex);
        Raylib.UnloadSound(collectSound);
        Raylib.UnloadSound(explodeSound);
    }
}

class Program
{
    static void Main()
    {
        Raylib.InitWindow(800, 600, "Catch & Dodge Game");
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);

        Game game = new Game();
        game.Load();

        while (!Raylib.WindowShouldClose())
        {
            game.Update();
            game.Draw();
        }

        game.Unload();
        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }
}