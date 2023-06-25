using System.Diagnostics;
using System.Security.Cryptography;
using static System.Formats.Asn1.AsnWriter;
using Timer = System.Windows.Forms.Timer;

namespace BollGame
{
    public partial class Form1 : Form
    {
        //更新処理のためにタイマーを作成
        Timer Updatetimer = new Timer();
        Timer Monstertimer = new Timer();

        //プレイヤー
        Player player = new Player();

        //モンスターのリスト、この中にあるモンスターを表示する
        List<Monster> monsters = new List<Monster>();

        //死んだかどうか
        bool IsDead = false;

        //敵が出てくるスピード(ミリ秒)
        const int MonsterSpawnTime = 80;

        //マウス座標
        Point screenpoint, clientpoint;

        //スコア
        int Score = 0;

        public Form1()
        {
            InitializeComponent();

            //ちらつき防止
            this.DoubleBuffered = true;

            //カーソル邪魔だから見えなくする
            Cursor.Hide();

            Updatetimer.Interval = 1;
            Updatetimer.Tick += new EventHandler(Update);
            Updatetimer.Start();

            Monstertimer.Interval = MonsterSpawnTime;
            Monstertimer.Tick += new EventHandler(MonsterUpdate);
            Monstertimer.Start();
        }

        /// <summary>
        /// タイマーで1msごとに呼び出される
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update(object sender, EventArgs e)
        {
            if (IsDead)
            {
                //ゲームオーバー
                Updatetimer.Stop();
                Monstertimer.Stop();
                Cursor.Show();
                MessageBox.Show("ゲームオーバー！　スコア：" + Score);
                return;
            }

            //スコアの追加
            Score += 1;


            Debug.WriteLine(Score);
            //再描画   Form1_Paintが呼び出される
            Invalidate();
        }

        /// <summary>
        /// 再描画時に呼び出される
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //モンスターの数の分だけ実行
            for (int i = 0; i < monsters.Count; i++)
            {
                /*プレイヤーとモンスターの当たり判定*/
                if ((monsters[i].Position.Right > player.Position.Left) &&
                        monsters[i].Position.Left < player.Position.Right)
                {
                    if ((monsters[i].Position.Bottom > player.Position.Top) &&
                            (monsters[i].Position.Top < player.Position.Bottom))
                    {
                        //当たったら色を変える
                        monsters[i].SetColor(Color.DarkRed);

                        //死んだフラグを立てる
                        IsDead = true;
                    }
                }

                /*画面外に行ったモンスターをリストから削除*/
                if (this.Size.Height < monsters[i].Y)
                {
                    monsters.Remove(monsters[i]);
                }

                /*描画*/
                monsters[i].Draw(e);
            }

            /*プレイヤーの描画*/
            player.Draw(e);

            //スコアの表示
            this.Text = Name + " Score: " + Score; 
        }

        /// <summary>
        /// モンスターの描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonsterUpdate(object sender, EventArgs e)
        {
            //ランダムな速度、位置でインスタンス化
            Monster monster = new Monster
            {
                X = RandomNumberGenerator.GetInt32(0, this.Size.Width),
                Y = -20,
                Speed = RandomNumberGenerator.GetInt32(1, 10),
                Size = RandomNumberGenerator.GetInt32(10, 20)
            };

            //モンスター描画リストに追加
            monsters.Add(monster);
        }

        /// <summary>
        /// キーボードでプレイヤーを動かす
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            ////プレイヤーの移動
            //switch (e.KeyChar)
            //{
            //    case 'a':
            //        player.X -= 10;
            //        break;
            //    case 'd':
            //        player.X += 10;
            //        break;
            //    case 'w':
            //        player.Y -= 15;
            //        break;
            //    case 's':
            //        player.Y += 15;
            //        break;
            //    default:
            //        break;
            //}
        }

        /// <summary>
        /// マウスでプレイヤーを動かす
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            screenpoint = Cursor.Position;

            clientpoint = this.PointToClient(screenpoint);

            player.X = clientpoint.X;
            player.Y = clientpoint.Y;

        }
    }
}