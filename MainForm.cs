using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TreeGenTest.SpaceColonization;

namespace TreeGenTest
{
    public partial class MainForm : Form
    {
        private Bitmap canvas = new Bitmap(512, 512);
        private Graphics gfx;

        Tree tree;

        public MainForm()
        {
            InitializeComponent();

            InitializeTree();

            gfx = Graphics.FromImage(canvas);
            gfx.SmoothingMode = SmoothingMode.AntiAlias;

            Application.Idle += (sender, e) => Draw();
        }

        private void Display_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                InitializeTree();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (SaveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    canvas.Save(SaveImageDialog.FileName);
                }
            }
        }

        private void InitializeTree()
        {
            Random rand = new Random();

            List<Leaf> leaves = new List<Leaf>();

            for (int i = 0; i < 500; i++)
            {
                int normalization = 3;

                var point = Vector2.Zero;
                for(int j = 0; j < normalization; j++)
                {
                    point += new Vector2(rand.Next(canvas.Width), rand.Next(150, canvas.Height));
                }
                point /= normalization;

                leaves.Add(new Leaf(point));
            }

            tree = new Tree(leaves, new Branch(null, new Vector2(canvas.Width / 2, 0), new Vector2(0, 1)));
        }

        private void Draw()
        {
            gfx.Clear(Color.White);

            if (!tree.DoneGrowing)
            {
                tree.Grow();
            }

            foreach (var leaf in tree.DeadLeaves)
            {
                gfx.FillEllipse(new SolidBrush(Color.FromArgb(100, Color.GreenYellow)), new RectangleF(leaf.Position.X - 10, leaf.Position.Y - 10, 20, 20));
            }

            foreach (var leaf in tree.Leaves)
            {
                gfx.FillEllipse(new SolidBrush(Color.FromArgb(100, Color.Red)), new RectangleF(leaf.Position.X - 10, leaf.Position.Y - 10, 20, 20));
            }

            foreach (var branch in tree.Branches)
            {
                if (branch.Parent != null)
                {
                    var p = new Pen(new SolidBrush(Color.FromArgb(200, Color.SandyBrown)), branch.Thickness);
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    p.LineJoin = LineJoin.Round;
                    gfx.DrawLine(p, branch.Position.X, branch.Position.Y, branch.Parent.Position.X, branch.Parent.Position.Y);
                }
            }

            canvas.RotateFlip(RotateFlipType.RotateNoneFlipY);

            if (tree.DoneGrowing) gfx.DrawString("Done", SystemFonts.DefaultFont, Brushes.Green, 10, canvas.Height - 25);
            else gfx.DrawString("Growing...", SystemFonts.DefaultFont, Brushes.Red, 10, canvas.Height - 25);

            gfx.DrawString($"Leaves : {tree.DeadLeaves.Count}", SystemFonts.DefaultFont, Brushes.SlateBlue, 10, canvas.Height - 100);
            gfx.DrawString($"Dead Leaves : {tree.Leaves.Count}", SystemFonts.DefaultFont, Brushes.SlateBlue, 10, canvas.Height - 75);
            gfx.DrawString($"Branches : {tree.Branches.Count}", SystemFonts.DefaultFont, Brushes.SlateBlue, 10, canvas.Height - 50);

            display.Image = canvas;
        }
    }
}
