namespace BollGame
{
    internal class Monster : CharacterBase
    {
        //落下速度
        public int Speed { get; set; }

        public Monster()
        {
            Position = new Rectangle(0, 0, Size, Size);
            Brush = new SolidBrush(Color.Red);
            Size = 20;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="e"></param>
        public void Draw(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brush, X, Y, Size, Size);

            //落下
            Y += Speed;
        }

        public void SetColor(Color newcolor)
        {
            Brush.Color = newcolor;
        }
    }
}
