using System.Drawing;

namespace Galaxy.Core.Environment
{
  public interface ILevelInfo
  {
    Point GetPlayerPosition();
    Size GetLevelSize();
  }
}