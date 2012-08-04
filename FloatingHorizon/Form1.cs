using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloatingHorizon
{
    public partial class Form1 : Form
    {
        HorizonDrawer horizonDrawer = null;
        List<functionType> functions = new List<functionType>();
        Point capturePoint;
        bool isMouseCaptured;

        public Form1()
        {
            InitializeComponent();
            DoMyInitialization();
        }

        private void DoMyInitialization()
        {
            #region functions
            List<String> funcNames = new List<String>();
            funcNames.Add("Y = Sin(x + z)");
            funcNames.Add("Y = Sin(Cos(z) - Sin(x))");
            funcNames.Add("Y = e^(Sin(Sqrt(x * x  + z * z )))");


            functions.Add(Functions.SinXplusZ);
            functions.Add(Functions.CosDelta);
            functions.Add(Functions.ExpSinR);

            cmbBoxFunctions.Items.AddRange(funcNames.ToArray());
            cmbBoxFunctions.SelectedIndex = 0;
            #endregion

            #region textBoxes
            txtBoxXBegin.Text = "-5";
            txtBoxXEnd.Text = "5";

            txtBoxZBegin.Text = "-5";
            txtBoxZEnd.Text = "5";
            txtBoxXStep.Text = "0,05";
            txtBoxZStep.Text = "0,2";
            #endregion

            #region colors
            btnMainColor.BackColor = Color.MediumVioletRed;
            btnBackColor.BackColor = Color.Black;
            #endregion

            #region horizontDrawer
            horizonDrawer = new HorizonDrawer(picBox.Width, picBox.Height);
            InitializeHorizonDrawer();
            #endregion

            isMouseCaptured = false;
        }

        private void InitializeHorizonDrawer()
        {
            try
            {
                horizonDrawer.SetBoundsOnX(Convert.ToDouble(txtBoxXBegin.Text), Convert.ToDouble(txtBoxXEnd.Text));
                horizonDrawer.SetBoundsOnZ(Convert.ToDouble(txtBoxZBegin.Text), Convert.ToDouble(txtBoxZEnd.Text));
                horizonDrawer.SetXZsteps(Convert.ToDouble(txtBoxXStep.Text), Convert.ToDouble(txtBoxZStep.Text));
                horizonDrawer.SetAngleX(trackBarX.Value);
                horizonDrawer.SetAngleY(trackBarY.Value);
                horizonDrawer.SetAngleZ(trackBarZ.Value);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Неверные входные данные");
            }
            horizonDrawer.SetBackColor(btnBackColor.BackColor);
            horizonDrawer.SetMainColor(btnMainColor.BackColor);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitializeHorizonDrawer();
            ReDraw();
        }

        private void trackBarX_ValueChanged(object sender, EventArgs e)
        {
            horizonDrawer.SetAngleX(trackBarX.Value);
            ReDraw();
        }

        private void trackBarY_ValueChanged(object sender, EventArgs e)
        {
            horizonDrawer.SetAngleY(trackBarY.Value);
            ReDraw();
        }

        private void trackBarZ_ValueChanged(object sender, EventArgs e)
        {
            horizonDrawer.SetAngleZ(trackBarZ.Value);
            ReDraw();
        }

        private void ReDraw()
        {
            horizonDrawer.Draw(picBox.CreateGraphics(), functions[cmbBoxFunctions.SelectedIndex]);
        }

        private void btnBackColor_Click(object sender, EventArgs e)
        {
            colorDlg.ShowDialog();
            horizonDrawer.SetBackColor(colorDlg.Color);
            btnBackColor.BackColor = colorDlg.Color;
        }

        private void btnMainColor_Click(object sender, EventArgs e)
        {
            colorDlg.ShowDialog();
            horizonDrawer.SetMainColor(colorDlg.Color);
            btnMainColor.BackColor = colorDlg.Color;
        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            capturePoint = e.Location;
            isMouseCaptured = true;
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (isMouseCaptured)
            {
                double deltaAngle;
                if (Math.Abs(e.X - capturePoint.X) < Math.Abs(e.Y - capturePoint.Y))
                {
                    if (e.Y > capturePoint.Y)
                    {
                        deltaAngle = 360 * (e.Y - capturePoint.Y) / (picBox.Height - capturePoint.Y);
                    }
                    else
                    {
                        deltaAngle = 360 * (1 - (e.Y - capturePoint.Y) / (capturePoint.Y - picBox.Height));
                    }
                    trackBarX.Value = Math.Abs((int)Math.Round(deltaAngle) % 361);
                }
                else
                {
                    if (e.X > capturePoint.X)
                    {
                        deltaAngle = 360 * (e.X - capturePoint.X) / (picBox.Width - capturePoint.X);
                    }
                    else
                    {
                        deltaAngle = 360 * (1 - (e.X - capturePoint.X) / (capturePoint.X - picBox.Width));
                    }
                    trackBarY.Value = Math.Abs((int)Math.Round(deltaAngle) % 361);
                }


            }
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseCaptured = false;
        }

    }


    public static class Functions
    {

        public static double SinXplusZ(double x, double z)
        {
            return Math.Sin(x + z);
        }

        public static double CosDelta(double x, double z)
        {
            return Math.Cos(Math.Cos(z) - Math.Sin(x));
        }

        public static double ExpSinR(double x, double z)
        {
            return Math.Exp(Math.Sin(Math.Sqrt(x * x  + z * z )));
        }
    }
}
