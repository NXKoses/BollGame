using System.Diagnostics;
using System.Security.Cryptography;
using static System.Formats.Asn1.AsnWriter;
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

        //�G���o�Ă���X�s�[�h(�~���b)
        const int MonsterSpawnTime = 80;

        //�}�E�X���W
        Point screenpoint, clientpoint;

        //�X�R�A
        int Score = 0;

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

            Monstertimer.Interval = MonsterSpawnTime;
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
                //�Q�[���I�[�o�[
                Updatetimer.Stop();
                Monstertimer.Stop();
                Cursor.Show();
                MessageBox.Show("�Q�[���I�[�o�[�I�@�X�R�A�F" + Score);
                return;
            }

            //�X�R�A�̒ǉ�
            Score += 1;


            Debug.WriteLine(Score);
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
                        monsters[i].SetColor(Color.DarkRed);

                        //���񂾃t���O�𗧂Ă�
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

            //�X�R�A�̕\��
            this.Text = Name + " Score: " + Score; 
        }

        /// <summary>
        /// �����X�^�[�̕`��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonsterUpdate(object sender, EventArgs e)
        {
            //�����_���ȑ��x�A�ʒu�ŃC���X�^���X��
            Monster monster = new Monster
            {
                X = RandomNumberGenerator.GetInt32(0, this.Size.Width),
                Y = -20,
                Speed = RandomNumberGenerator.GetInt32(1, 10),
                Size = RandomNumberGenerator.GetInt32(10, 20)
            };

            //�����X�^�[�`�惊�X�g�ɒǉ�
            monsters.Add(monster);
        }

        /// <summary>
        /// �L�[�{�[�h�Ńv���C���[�𓮂���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            ////�v���C���[�̈ړ�
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
    }
}