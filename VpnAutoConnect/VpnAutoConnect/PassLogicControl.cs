using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VpnAutoConnect
{
    /// <summary>
    /// PassLogicのトークンレスワンタイムパスワードを表示するカスタムコントロール
    /// </summary>
    public partial class PassLogicControl : PictureBox
    {
        private Graphics _graphics { get;}
        private const int NCHR = 4;
        private const int CHR_WIDTH = 25;
        private const int CHR_HEIGHT = 30;
        private const int GRP_WIDTH = CHR_WIDTH * NCHR + CHR_MARGIN * (NCHR - 1);
        private const int GRP_HEIGHT = CHR_HEIGHT * NCHR + CHR_MARGIN * (NCHR - 1);
        private const int NGRP = 3;
        private const int CHR_MARGIN = 3;
        private const int GRP_MARGIN = 10;
        private const int TOTAL_CHR = NCHR * NCHR * NGRP;

        // Index
        // 
        //  0  1  2  3     16 17 18 19     32 33 34 35
        //  4  5  6  7     20 21 22 23     36 37 38 39
        //  8  9 10 11     24 25 26 27     40 41 42 43
        // 12 13 14 15     28 29 30 31     44 45 46 47


        /// <summary>
        /// コンストラクタ
        /// 枠線表示
        /// </summary>
        public PassLogicControl()
        {
            InitializeComponent();

            // 枠描画
            var canvas = new Bitmap(Width, Height);
            _graphics = Graphics.FromImage(canvas);
            var rects = new List<Rectangle>() { OuterFrame, };
            for (int i = 0; i < TOTAL_CHR; i++)
                rects.Add(CharFrame(i));
            _graphics.DrawRectangles(Pens.Black, rects.ToArray());
            
            // 表示
            Image = canvas;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        /// <summary>
        /// 外枠
        /// </summary>
        public Rectangle OuterFrame = new Rectangle(0, 0,
                     GRP_MARGIN * (NGRP + 1) + (CHR_WIDTH * NCHR + CHR_MARGIN * (NCHR - 1)) * NGRP,
                     GRP_MARGIN * 2 + CHR_HEIGHT * NCHR + CHR_MARGIN * (NCHR - 1));

        /// <summary>
        /// 各文字の枠を取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Rectangle CharFrame(int index)
        {
            int grp = index / (NCHR * NCHR);
            int row = (index - grp * NCHR * NCHR) / NCHR;
            int col = index - grp * NCHR * NCHR - row * NCHR;

            return new Rectangle(GRP_MARGIN + grp * (GRP_WIDTH + GRP_MARGIN) + col * (CHR_WIDTH + CHR_MARGIN),
                GRP_MARGIN + row * (CHR_HEIGHT + CHR_MARGIN),
                CHR_WIDTH,
                CHR_HEIGHT);
        }

        /// <summary>
        /// 各文字の枠の内側を取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Rectangle CharInside(int index)
        {
            var frame = CharFrame(index);
            return new Rectangle(frame.X + 1, frame.Y + 1, frame.Width-1, frame.Height-1);
        }

        /// <summary>
        /// パスワード列取得
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string GetPassword(int[] table)
        {
            var ret = "";
            foreach(var idx in _passPhrase)
            {
                ret += table[idx].ToString();
            }
            return ret;
        }

        /// <summary>
        /// Pointに対応するPassLogic文字のIndexを取得
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public int CharIndex(Point pt)
        {
            // 行
            pt.Y -= GRP_MARGIN;
            int row = pt.Y / (CHR_HEIGHT + CHR_MARGIN);
            if (row < 0) return -1;

            // 行間のマージンをクリックした場合は無効
            pt.Y -= row * (CHR_HEIGHT + CHR_MARGIN);
            if (pt.Y > CHR_HEIGHT) return -1;

            // グループ
            pt.X -= GRP_MARGIN;
            int grp = pt.X / (GRP_WIDTH + GRP_MARGIN);
            if (grp < 0) return -1;

            // グループ間のマージンをクリックした場合は無効
            pt.X -= grp * (GRP_WIDTH + GRP_MARGIN);
            if (pt.X > GRP_WIDTH) return -1;

            // 列
            int col = pt.X / (CHR_WIDTH + CHR_MARGIN);

            // 列間のマージンをクリックした場合は無効
            pt.X -= col * (CHR_WIDTH + CHR_MARGIN);
            if (pt.X > CHR_WIDTH) return -1;

            return grp * NCHR * NCHR + row * NCHR + col;
        }

        /// <summary>
        /// パスフレーズ入力中かどうか
        /// </summary>
        public bool IsPassPhraseRegistering { get; private set; }

        /// <summary>
        /// パスフレーズ入力済み
        /// </summary>
        public bool PassPhraseRegistered { get; private set; }

        /// <summary>
        /// パスフレーズ（Index列）
        /// </summary>
        private List<int> _passPhrase { get; set; }

        /// <summary>
        /// パスフレーズ入力開始
        /// </summary>
        public void BeginRegister()
        {
            IsPassPhraseRegistering = true;
            PassPhraseRegistered = false;
            _passPhrase = new List<int>();
        }

        /// <summary>
        /// パスフレーズ入力終了
        /// </summary>
        public void EndRegister()
        {
            IsPassPhraseRegistering = false;
            PassPhraseRegistered = true;
        }

        /// <summary>
        /// マウスクリックイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PassLogicControl_Click(object sender, EventArgs e)
        {
            if(e is MouseEventArgs arg)
            {
                // パスフレーズ入力中以外は何もしない
                if (!IsPassPhraseRegistering) return;

                // クリック位置のインデックスをパスフレーズに追加
                var idx = CharIndex(arg.Location);
                if(idx< 0) return;
                _passPhrase.Add(idx);
                
                // クリック箇所塗りつぶし
                var rect = CharInside(idx);
                _graphics.FillRectangle(Brushes.Black, rect);
                Invalidate();

                // 200ms待って塗りつぶし解除
                await Task.Delay(200);
                _graphics.FillRectangle(new SolidBrush(SystemColors.Control), rect);
                Invalidate();
            }
        }
    }
}
