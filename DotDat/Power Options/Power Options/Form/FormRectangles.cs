using System.Drawing;
using Power_Options.Classes;

namespace Power_Options.Form
{
    public class FormRectangles
    {
        private const int HEIGHT = 47;
        private const int WIDTH = 250;

        /// <summary>
        /// Creates & return top rectangle dimensions
        /// </summary>
        /// <returns></returns>
        public Rectangle TopRect()
        {
            return new Rectangle(-1, -1, WIDTH + 1, HEIGHT);
        }

        /// <summary>
        /// Creates & return center rectangle dimensions
        /// </summary>
        /// <returns></returns>
        public Rectangle CenterRect()
        {
            return Power.Schemes == null 
                ? 
                new Rectangle() 
                :
                new Rectangle(0, TopRect().Bottom, WIDTH, HEIGHT + (32 * (Power.Schemes.Count - 1)));
        }

        /// <summary>
        /// Creates & return bottom rectangle dimensions
        /// </summary>
        /// <returns></returns>
        public Rectangle BottomRect()
        {
            return new Rectangle(0, CenterRect().Bottom, WIDTH, HEIGHT - 6);
        }

        /// <summary>
        /// Returns total of all rectangles heights
        /// </summary>
        /// <returns></returns>
        public int TotalRectHeight()
        {
            return TopRect().Height + CenterRect().Height + BottomRect().Height;
        }
    }
}