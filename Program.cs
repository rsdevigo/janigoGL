using System;

namespace opentk_example
{
  class Program
  {
    static void Main(string[] args)
    {
      using (Game game = new Game(800, 600, "janigoGL"))
      {
        //Run takes a double, which is how many frames per second it should strive to reach.
        //You can leave that out and it'll just update as fast as the hardware will allow it.
        game.Run(60.0);
      }
      //ObjLoaderTest tester = new ObjLoaderTest();
    }
  }
}
