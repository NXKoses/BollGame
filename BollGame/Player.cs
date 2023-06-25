namespace BollGame
{
    internal class Player : CharacterBase
    {
        public Player()
        {
            Position = new Rectangle(200, 350, Size, Size);
            Brush = new SolidBrush(Color.DarkCyan);
            Size = 20;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="e"></param>
        public void Draw(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brush, X, Y, Size, Size);
        }
    }
}
