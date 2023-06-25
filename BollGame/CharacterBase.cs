namespace BollGame
{
    internal class CharacterBase
    {
        public int X
        {
            get { return Position.X; }
            set { Position = new Rectangle(value, Y, Size, Size); }
        }

        public int Y
        {
            get { return Position.Y; }
            set { Position = new Rectangle(X, value, Size, Size); }
        }

        /// <summary>
        ///  四角オブジェクト
        /// </summary>
        public Rectangle Position { get; set; }

        /// <summary>
        /// 描画するときのブラシ
        /// </summary>
        public SolidBrush Brush { get; set; }

        /// <summary>
        /// 大きさ
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 色を変更します。
        /// </summary>
        /// <param name="newcolor"></param>
        public void ChangeColor(Color newcolor)
        {
            Brush.Color = newcolor;
        }
    }
}
