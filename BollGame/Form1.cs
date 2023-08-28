using System.Drawing.Design;
using System.Security.Cryptography;
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

        //マウス座標
        Point screenpoint, clientpoint;

        //スコア
        int Score = 0;

        //レベル１の移動速度 初期値
        const int LvMaxSpeed = 9;
        const int LvMinSpeed = 3;

        const int LvMaxLeftRightSpeed = 1;
        const int LvMinLeftRightSpeed = 0;

        //-- レベルに応じて変化させる変数 --
        int lvmaxspeed = LvMaxSpeed;
        int lvminspeed = LvMinSpeed;

        int lvmaxleftrightspeed = LvMaxLeftRightSpeed;
        int lvminleftrightspeed = LvMinLeftRightSpeed;

        int Level = 1;
        //---------------------------
        
        bool cheat = false;

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

            Monstertimer.Interval = 100;
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
                /*ゲームオーバー*/
                
                //タイマーを止める
                Updatetimer.Stop();
                Monstertimer.Stop();

                //カーソルを表示する
                Cursor.Show();

                //再挑戦するか聞く
                var result = MessageBox.Show("ゲームオーバー！　スコア：" + Score + Environment.NewLine +
                    "再挑戦する？ ", "( ;∀;)", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //変数初期化する
                    Restart();
                }
                else
                {
                    return;
                }
            }

            //スコアの追加
            Score += 1;

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
                        monsters[i].ChangeColor(Color.DarkRed);

                        //死んだフラグを立てる
                        if (!cheat)
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

            /*レベルの描画*/
            LevelDraw(Level, e);
            
            //スコアの表示
            this.Text = Name + " Score: " + Score;
        }

        /// <summary>
        /// 指定したタイマーのインターバルを変更します。
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="interval"></param>
        private void ChangeTimerInterval(Timer timer, int interval)
        {
            timer.Stop();
            timer.Interval = interval;
            timer.Start();
        }

        /// <summary>
        /// レベルの表示
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="e"></param>
        private void LevelDraw(int lv, PaintEventArgs e)
        {
            using Font Font = new Font("メイリオ", 20, FontStyle.Regular);
            SolidBrush brush = new SolidBrush(Color.White);

            if (lv == 2)
                brush.Color = Color.Orange;

            if (lv == 3)
                brush.Color = Color.OrangeRed;

            if (lv == 4)
                brush.Color = Color.PaleVioletRed;

            e.Graphics.DrawString("【Level: " + lv + "】", Font, brush, 150, 10);
        }

        /// <summary>
        /// モンスターの描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonsterUpdate(object sender, EventArgs e)
        {
            if (600 < Score)
            {
                Level = 2;
                lvminspeed = 4;
                ChangeTimerInterval(Monstertimer, 70);
                lvmaxleftrightspeed= 1;
                lvminleftrightspeed = -1;
            }

            if (1200 < Score)
            {
                Level = 3;
                lvmaxspeed = 11;
                ChangeTimerInterval(Monstertimer, 55);
                lvmaxleftrightspeed = 2;
                lvminleftrightspeed = -2;
            }

            if (1800 < Score)
            {
                Level = 4;
                lvmaxspeed = 12;
                ChangeTimerInterval(Monstertimer, 50);
                lvmaxleftrightspeed = 3;
                lvminleftrightspeed = -3;
            }

            //ランダムな速度、位置でインスタンス化
            Monster monster = new Monster
            {
                X = RandomNumberGenerator.GetInt32(0, this.Size.Width),
                Y = -20,
                LeftRightSpeed = RandomNumberGenerator.GetInt32(lvminleftrightspeed, lvmaxleftrightspeed),
                Speed = RandomNumberGenerator.GetInt32(lvminspeed, lvmaxspeed),
                Size = RandomNumberGenerator.GetInt32(10, 20),
            };

            //モンスター描画リストに追加
            monsters.Add(monster);
        }

        /// <summary>
        /// キーボード処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '1':
                    cheat = true;
                    break;
                case '2':
                    Score += 100;
                    break;

                default:
                    break;
            }
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

        private void Restart()
        {
            IsDead = false;
            Score = 0;
            monsters.Clear();

            Level = 1;

            lvmaxspeed = LvMaxSpeed;
            lvminspeed = LvMinSpeed;

            lvmaxleftrightspeed = LvMaxLeftRightSpeed;
            lvminleftrightspeed = LvMinLeftRightSpeed;

            Updatetimer.Start();
            Monstertimer.Start();

            Cursor.Hide();

        }
    }
}