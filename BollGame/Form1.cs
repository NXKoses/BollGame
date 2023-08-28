using System.Drawing.Design;
using System.Security.Cryptography;
using Timer = System.Windows.Forms.Timer;

namespace BollGame
{
    public partial class Form1 : Form
    {
        //�X�V�����̂��߂Ƀ^�C�}�[���쐬
        Timer Updatetimer = new Timer();
        Timer Monstertimer = new Timer();

        //�v���C���[
        Player player = new Player();

        //�����X�^�[�̃��X�g�A���̒��ɂ��郂���X�^�[��\������
        List<Monster> monsters = new List<Monster>();

        //���񂾂��ǂ���
        bool IsDead = false;

        //�}�E�X���W
        Point screenpoint, clientpoint;

        //�X�R�A
        int Score = 0;

        //���x���P�̈ړ����x �����l
        const int LvMaxSpeed = 9;
        const int LvMinSpeed = 3;

        const int LvMaxLeftRightSpeed = 1;
        const int LvMinLeftRightSpeed = 0;

        //-- ���x���ɉ����ĕω�������ϐ� --
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

            //������h�~
            this.DoubleBuffered = true;

            //�J�[�\���ז������猩���Ȃ�����
            Cursor.Hide();

            Updatetimer.Interval = 1;
            Updatetimer.Tick += new EventHandler(Update);
            Updatetimer.Start();

            Monstertimer.Interval = 100;
            Monstertimer.Tick += new EventHandler(MonsterUpdate);
            Monstertimer.Start();
        }

        /// <summary>
        /// �^�C�}�[��1ms���ƂɌĂяo�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update(object sender, EventArgs e)
        {
            if (IsDead)
            {
                /*�Q�[���I�[�o�[*/
                
                //�^�C�}�[���~�߂�
                Updatetimer.Stop();
                Monstertimer.Stop();

                //�J�[�\����\������
                Cursor.Show();

                //�Ē��킷�邩����
                var result = MessageBox.Show("�Q�[���I�[�o�[�I�@�X�R�A�F" + Score + Environment.NewLine +
                    "�Ē��킷��H ", "( ;��;)", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //�ϐ�����������
                    Restart();
                }
                else
                {
                    return;
                }
            }

            //�X�R�A�̒ǉ�
            Score += 1;

            //�ĕ`��   Form1_Paint���Ăяo�����
            Invalidate();
        }

        /// <summary>
        /// �ĕ`�掞�ɌĂяo�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //�����X�^�[�̐��̕��������s
            for (int i = 0; i < monsters.Count; i++)
            {
                /*�v���C���[�ƃ����X�^�[�̓����蔻��*/
                if ((monsters[i].Position.Right > player.Position.Left) &&
                        monsters[i].Position.Left < player.Position.Right)
                {
                    if ((monsters[i].Position.Bottom > player.Position.Top) &&
                            (monsters[i].Position.Top < player.Position.Bottom))
                    {
                        //����������F��ς���
                        monsters[i].ChangeColor(Color.DarkRed);

                        //���񂾃t���O�𗧂Ă�
                        if (!cheat)
                            IsDead = true;
                    }
                }

                /*��ʊO�ɍs���������X�^�[�����X�g����폜*/
                if (this.Size.Height < monsters[i].Y)
                {
                    monsters.Remove(monsters[i]);
                }

                /*�`��*/
                monsters[i].Draw(e);
            }

            /*�v���C���[�̕`��*/
            player.Draw(e);

            /*���x���̕`��*/
            LevelDraw(Level, e);
            
            //�X�R�A�̕\��
            this.Text = Name + " Score: " + Score;
        }

        /// <summary>
        /// �w�肵���^�C�}�[�̃C���^�[�o����ύX���܂��B
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
        /// ���x���̕\��
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="e"></param>
        private void LevelDraw(int lv, PaintEventArgs e)
        {
            using Font Font = new Font("���C���I", 20, FontStyle.Regular);
            SolidBrush brush = new SolidBrush(Color.White);

            if (lv == 2)
                brush.Color = Color.Orange;

            if (lv == 3)
                brush.Color = Color.OrangeRed;

            if (lv == 4)
                brush.Color = Color.PaleVioletRed;

            e.Graphics.DrawString("�yLevel: " + lv + "�z", Font, brush, 150, 10);
        }

        /// <summary>
        /// �����X�^�[�̕`��
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

            //�����_���ȑ��x�A�ʒu�ŃC���X�^���X��
            Monster monster = new Monster
            {
                X = RandomNumberGenerator.GetInt32(0, this.Size.Width),
                Y = -20,
                LeftRightSpeed = RandomNumberGenerator.GetInt32(lvminleftrightspeed, lvmaxleftrightspeed),
                Speed = RandomNumberGenerator.GetInt32(lvminspeed, lvmaxspeed),
                Size = RandomNumberGenerator.GetInt32(10, 20),
            };

            //�����X�^�[�`�惊�X�g�ɒǉ�
            monsters.Add(monster);
        }

        /// <summary>
        /// �L�[�{�[�h����
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
        /// �}�E�X�Ńv���C���[�𓮂���
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