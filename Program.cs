using System;
using OpenTK.Graphics.OpenGL4;

namespace janigoGL
{
  class Program
  {
    static void Main(string[] args)
    {
      // Cria um objeto do tipo Game com tamanho da janela de 800x600px
      using (Game game = new Game(800, 600, "janigoGL"))
      {
        // Carrega a janela do JanigoGL com 60 frames por segundo.
        game.Run(60.0);
      }
    }
  }
}
