using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System;
using GPUBench;

using (var game = new Game(800, 600, "GPU Bench"))
{
    game.Run();
    
    /*game.Load += (sender, e) =>
    {
        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

    // Set the viewport
    GL.Viewport(0, 0, game.Size.X, game.Size.Y);
        };
    
        Stopwatch stopwatch = new Stopwatch();
    
        game.RenderFrame += (sender, e) =>
        {
            stopwatch.Reset();
            stopwatch.Start();
    
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity(); 
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0); // Set projection to identity and ortho
    
            GL.Clear(ClearBufferMask.ColorBufferBit);
    
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex2(-1.0f, -1.0f);
            GL.Color3(0.0f, 1.0f, 0.0f); GL.Vertex2(0.0f, 1.0f);
            GL.Color3(0.0f, 0.0f, 1.0f); GL.Vertex2(1.0f, -1.0f);
            GL.End();
    
            stopwatch.Stop();
            Console.WriteLine($"Frame time: {stopwatch.Elapsed.TotalMilliseconds} ms");
    
            game.SwapBuffers();
        };
    
        game.Run(60.0);*/
}