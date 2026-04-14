using Raylib_cs;
using System.Drawing;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

using Color = Raylib_cs.Color;
using Font = Raylib_cs.Font;

namespace DvdLogo
{
    class Program
    {
        static void Main(string[] args)
        {
            const int screenWidth = 800;
            const int screenHeight = 600;
            Raylib.InitWindow(screenWidth, screenHeight, "DVD Logo Bounce");

            string text = "DVD";
            int fontSize = 60;
            float spacing = 2;
            Font font = Raylib.GetFontDefault();

            Vector2 position = new Vector2(screenWidth / 2, screenHeight / 2);

            Vector2 direction = new Vector2(1, 1);
            float speed = 200.0f; 

            Vector2 textSize = Raylib.MeasureTextEx(font, text, fontSize, spacing);

            Color textColor = Color.Yellow;

            Raylib.SetTargetFPS(60);

            //pelisilmukka
            while (!Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();

                position += direction * speed * deltaTime;


                //oikea reuna
                if (position.X + textSize.X >= Raylib.GetScreenWidth())
                {
                    direction.X *= -1; 
                    position.X = Raylib.GetScreenWidth() - textSize.X; 
                    textColor = Raylib.ColorFromHSV(Raylib.GetRandomValue(0, 360), 1, 1); 
                }
                //vasen reuna
                if (position.X <= 0)
                {
                    direction.X *= -1;
                    position.X = 0;
                    textColor = Raylib.ColorFromHSV(Raylib.GetRandomValue(0, 360), 1, 1);
                }
                //alareuna
                if (position.Y + textSize.Y >= Raylib.GetScreenHeight())
                {
                    direction.Y *= -1;
                    position.Y = Raylib.GetScreenHeight() - textSize.Y;
                    textColor = Raylib.ColorFromHSV(Raylib.GetRandomValue(0, 360), 1, 1);
                }
                //yläreuna
                if (position.Y <= 0)
                {
                    direction.Y *= -1;
                    position.Y = 0;
                    textColor = Raylib.ColorFromHSV(Raylib.GetRandomValue(0, 360), 1, 1);
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawTextEx(font, text, position, fontSize, spacing, textColor);

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}