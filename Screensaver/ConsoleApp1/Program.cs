using Raylib_cs;
using System.Drawing;
using System.Numerics;

class Program
{
    static void Main()
    {
        Raylib.InitWindow(800, 800, "Screensaver");
        Raylib.SetTargetFPS(30);

        //kolmion pisteet
        Vector2 A = new Vector2(Raylib.GetScreenWidth() / 2, 40);
        Vector2 B = new Vector2(40, Raylib.GetScreenHeight() / 2);
        Vector2 C = new Vector2(Raylib.GetScreenWidth() - 40, Raylib.GetScreenHeight() * 3 / 4);

        
        float speed = 200f;

        //suuntavektorit
        Vector2 dirA = new Vector2(1, 1);
        Vector2 dirB = new Vector2(1, -1);
        Vector2 dirC = new Vector2(-1, 1);

        while (!Raylib.WindowShouldClose())
        {
            float dt = Raylib.GetFrameTime();

            //kiikutetaan pisteitä
            A += dirA * speed * dt;
            B += dirB * speed * dt;
            C += dirC * speed * dt;

            int width = Raylib.GetScreenWidth();
            int height = Raylib.GetScreenHeight();

            //A
            if (A.X < 0 || A.X > width)
                dirA.X *= -1.0f;
            if (A.Y < 0 || A.Y > height)
                dirA.Y *= -1.0f;

            //B
            if (B.X < 0 || B.X > width)
                dirB.X *= -1.0f;
            if (B.Y < 0 || B.Y > height)
                dirB.Y *= -1.0f;

            //C
            if (C.X < 0 || C.X > width)
                dirC.X *= -1.0f;
            if (C.Y < 0 || C.Y > height)
                dirC.Y *= -1.0f;

            Raylib.BeginDrawing();

            Raylib.ClearBackground(Raylib_cs.Color.Black);

            Raylib.DrawLineV(A, B, Raylib_cs.Color.Green);
            Raylib.DrawLineV(B, C, Raylib_cs.Color.Yellow);
            Raylib.DrawLineV(C, A, Raylib_cs.Color.SkyBlue);

           
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}