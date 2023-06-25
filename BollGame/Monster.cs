using System.Threading;

namespace BollGame
{
    internal class Monster : CharacterBase
    {
        //落下速度
        public int Speed { get; set; }

        //左右移動
        public int LeftRightSpeed { get; set; }

        public Monster()
        {
            Position = new Rectangle(0, 0, Size, Size);
            Brush = new SolidBrush(Color.Gray);
            Size = 20;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="e"></param>
        public void Draw(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brush, X, Y, Size, Size);

            //スピードになどよって色を変える
            if (Size > 15)
            {
                Brush.Color = Color.DarkGray;
            }

            if (Speed > 5)
            {
                Brush.Color = Color.IndianRed;
            }

            //落下
            Y += Speed;
            X += LeftRightSpeed;
        }
    }
}
