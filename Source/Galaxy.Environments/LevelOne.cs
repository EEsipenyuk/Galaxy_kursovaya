#region using

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Galaxy.Core.Actors;
using Galaxy.Core.Collision;
using Galaxy.Core.Environment;
using Galaxy.Environments.Actors;

#endregion

namespace Galaxy.Environments
{
  /// <summary>
  ///   The level class for Open Mario.  This will be the first level that the player interacts with.
  /// </summary>
  public class LevelOne : BaseLevel
  {
    private int m_frameCount;
    private Stopwatch m_timer;
    private bool m_isAnimation;
    private Bitmap m_image { get; set; }
    private Bitmap m_imageAnimation { get; set; }
    private int m_killed_enemy;

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="LevelOne" /> class.
    /// </summary>
    public LevelOne()
    {
       m_timer = new Stopwatch();
       m_isAnimation = false;
       m_killed_enemy = 0;
       m_imageAnimation = (Bitmap)System.Drawing.Image.FromFile(@"Assets\animation.png");
       m_imageAnimation = new Bitmap(m_imageAnimation, new Size(198, 198));
      // Backgrounds
      FileName = @"Assets\LevelOne.png";

      // Enemies
      for (int i = 0; i < 5; i++)
      {
        var ship = new Ship(this);
        ship.Bonus = false;
        int positionY = ship.Height + 10;
        int positionX = 150 + i * (ship.Width + 50);

        ship.Position = new Point(positionX, positionY);

        Actors.Add(ship);
      }
      for (int i = 0; i < 5; i++)
      {
          var starship = new Starship(this);
          starship.Bonus = i % 2 == 0;
          int positionY = starship.Height + 30;
          int positionX = 170 + i * (starship.Width + 50);

          starship.Position = new Point(positionX, positionY);

          Actors.Add(starship);
      }

      // Player
      Player = new PlayerShip(this);
      int playerPositionX = Size.Width / 2 - Player.Width / 2;
      int playerPositionY = Size.Height - Player.Height - 50;
      Player.Position = new Point(playerPositionX, playerPositionY);
      Actors.Add(Player);
    }

    #endregion

    #region Overrides

    private void h_dispatchKey()
    {
      if (!IsPressed(VirtualKeyStates.Space)) return;

      if(m_frameCount % 10 != 0) return;

      Bullet bullet = new Bullet(this)
      {
        Position = Player.Position
      };

      bullet.Load();
      Actors.Add(bullet);
    }

    public override BaseLevel NextLevel()
    {
      return new StartScreen();
    }

    public override void Update()
    {
      m_frameCount++;
      h_dispatchKey();

        if (m_isAnimation==true && m_timer.ElapsedMilliseconds >= 3000)
        {
            m_timer.Stop();
            m_isAnimation = false;
        }

        if (m_isAnimation != true)
        {
            base.Update();

            IEnumerable<BaseActor> killedActors = CollisionChecher.GetAllCollisions(Actors);

            foreach (BaseActor killedActor in killedActors)
            {
                if (killedActor.IsAlive)
                    killedActor.IsAlive = false;
            }

            List<BaseActor> toRemove = Actors.Where(actor => actor.CanDrop).ToList();
            BaseActor[] actors = new BaseActor[toRemove.Count()];
            toRemove.CopyTo(actors);

            foreach (BaseActor actor in actors.Where(actor => actor.CanDrop))
            {
                if (actor.Bonus == true)
                {
                    m_timer.Start();
                    m_isAnimation = true;
                }
                Actors.Remove(actor);
                m_killed_enemy ++;
            }

            if (Player.CanDrop)
                Failed = true;

            //has no enemy
            if (Actors.All(actor => actor.ActorType != ActorType.Enemy))
                Success = true;
        }
        else
        {
            //StartAnimation();
        }
      
    }

    public override void Draw(Graphics graphics)
    {
        base.Draw(graphics);

        if (m_isAnimation == true)
        {
            graphics.DrawImage(m_imageAnimation, 200, 100);
        }
        string fileName = @"Assets\0.png";
        switch (m_killed_enemy)
        {
            case 0:
                fileName = @"Assets\0.png";
                break;
            case 1:
                fileName = @"Assets\1.png";
                break;
            case 2:
                fileName = @"Assets\2.png";
                break;
            case 3:
                fileName = @"Assets\3.png";
                break;
            case 4:
                fileName = @"Assets\4.png";
                break;
            case 5:
                fileName = @"Assets\5.png";
                break;
            case 6:
                fileName = @"Assets\6.png";
                break;
            case 7:
                fileName = @"Assets\7.png";
                break;
            case 8:
                fileName = @"Assets\8.png";
                break;
            case 9:
                fileName = @"Assets\9.png";
                break;
            case 10:
                fileName = @"Assets\10.png";
                break;
        }
        m_image = (Bitmap)System.Drawing.Image.FromFile(fileName);
        m_image = new Bitmap(m_image, new Size(179, 84));
        graphics.DrawImage(m_image, 400, 0);
    }     

    #endregion
  }
}
