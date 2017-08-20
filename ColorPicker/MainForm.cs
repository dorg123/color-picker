using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorPicker
{
    public partial class MainForm : Form
    {
        Random rnd = new Random();
        Color RandomColor(bool a = false) => Color.FromArgb(a ? rnd.Next(256) : 255, rnd.Next(256), rnd.Next(256), rnd.Next(256));
        Color ForeColorByBack(Color back) => (back.R + back.G + back.B) / 3 < 85 && back.A > 128 ? Color.White : Color.Black;

        public MainForm()
        {
            InitializeComponent();
            comboBoxFormat.SelectedIndex = 1;
            labelColor_Paint(this, null);
            listBoxColors.DataSource = Enum.GetValues(typeof(KnownColor));
            buttonRandomA.BackColor = RandomColor();
            buttonRandomA.ForeColor = ForeColorByBack(buttonRandomA.BackColor);
            buttonRandomB.BackColor = RandomColor();
            buttonRandomB.ForeColor = ForeColorByBack(buttonRandomB.BackColor);
            buttonRandomC.BackColor = RandomColor();
            buttonRandomC.ForeColor = ForeColorByBack(buttonRandomC.BackColor);
            buttonRandomD.BackColor = RandomColor();
            buttonRandomD.ForeColor = ForeColorByBack(buttonRandomD.BackColor);
            buttonRandomE.BackColor = RandomColor();
            buttonRandomE.ForeColor = ForeColorByBack(buttonRandomE.BackColor);
        }

        private void labelColor_Paint(object sender, PaintEventArgs e)
        {
            var c = labelColor.BackColor;
            trackBarAlpha.Value = c.A;
            trackBarRed.Value = c.R;
            trackBarGreen.Value = c.G;
            trackBarBlue.Value = c.B;
            if (c.IsNamedColor) textBoxName.Text = c.Name;
            string text;
            switch (comboBoxFormat.SelectedIndex)
            {
                case 0:
                    text = $"#{c.R:X2}{c.G:X2}{c.B:X2}";
                    break;
                case 1:
                    text = $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";
                    break;
                case 2:
                    text = $"{c.R},{c.G},{c.B}";
                    break;
                case 3:
                    text = $"{c.A},{c.R},{c.G},{c.B}";
                    break;
                default:
                    text = "";
                    break;
            }
            textBoxColor.Text = text;
            CallPresetButtons(buttonPreset_Background, a:true, rgb:true);
            CallRandomButtons(buttonRandom_Background);
        }

        private void listBoxColors_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxColors.SelectedItem != null)
                labelColor.BackColor = Color.FromName(listBoxColors.SelectedItem.ToString());
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            textBoxName.Text = "";
            trackBarStrength.Value = (trackBarBlue.Value + trackBarGreen.Value + trackBarRed.Value) / 3;
            labelColor.BackColor = Color.FromArgb(trackBarAlpha.Value, trackBarRed.Value, trackBarGreen.Value, trackBarBlue.Value);
        }

        private void textBoxColor_Leave(object sender, EventArgs e)
        {
            labelColor_Paint(sender, null);
        }

        private void textBoxColor_TextChanged(object sender, EventArgs e)
        {
            textBoxColor.Text = textBoxColor.Text.ToUpper();
            var hex = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            switch (comboBoxFormat.SelectedIndex)
            {
                case 0:
                    if (textBoxColor.Text.Length == 7 && textBoxColor.Text.StartsWith("#") &&
                        textBoxColor.Text.Skip(1).All(x => hex.Contains(x)))
                    {
                        labelColor.BackColor = Color.FromArgb(
                            Convert.ToInt32(textBoxColor.Text.Substring(1, 2), 16),
                            Convert.ToInt32(textBoxColor.Text.Substring(3, 2), 16),
                            Convert.ToInt32(textBoxColor.Text.Substring(5, 2), 16));
                    }
                    break;
                case 1:
                    if (textBoxColor.Text.Length == 9 && textBoxColor.Text.StartsWith("#") &&
                        textBoxColor.Text.Skip(1).All(x => hex.Contains(x)))
                    {
                        labelColor.BackColor = Color.FromArgb(
                            Convert.ToInt32(textBoxColor.Text.Substring(1, 2), 16),
                            Convert.ToInt32(textBoxColor.Text.Substring(3, 2), 16),
                            Convert.ToInt32(textBoxColor.Text.Substring(5, 2), 16),
                            Convert.ToInt32(textBoxColor.Text.Substring(7, 2), 16));
                    }
                    break;
                case 2:
                    if (textBoxColor.Text.Sum(x => x == ',' ? 1 : 0) == 2 &&
                        !textBoxColor.Text.Split(',').Any(string.IsNullOrWhiteSpace) &&
                        textBoxColor.Text.Replace(",", "").All(char.IsNumber) &&
                        textBoxColor.Text.Split(',').Select(int.Parse).All(x => x >= 0 && x <= 255))
                    {
                        int[] parts = textBoxColor.Text.Split(',').Select(int.Parse).ToArray();
                        labelColor.BackColor = Color.FromArgb(parts[0], parts[1], parts[2]);
                    }
                    break;
                case 3:
                    if (textBoxColor.Text.Sum(x => x == ',' ? 1 : 0) == 3 &&
                        !textBoxColor.Text.Split(',').Any(string.IsNullOrWhiteSpace) &&
                        textBoxColor.Text.Replace(",", "").All(char.IsNumber) &&
                        textBoxColor.Text.Split(',').Select(int.Parse).All(x => x >= 0 && x <= 255))
                    {
                        int[] parts = textBoxColor.Text.Split(',').Select(int.Parse).ToArray();
                        labelColor.BackColor = Color.FromArgb(parts[0], parts[1], parts[2], parts[3]);
                    }
                    break;
            }
        }

        private void comboBoxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelColor_Paint(sender, null);
        }

        private void buttonPreset_Click(object sender, EventArgs e)
        {
            var preset = (sender as Button).Name.Substring(6);
            var @var = preset[0];
            var val = int.Parse(preset.Substring(1));
            switch (var)
            {
                case 'A':
                    trackBarAlpha.Value = val;
                    break;
                case 'R':
                    trackBarRed.Value = val;
                    break;
                case 'G':
                    trackBarGreen.Value = val;
                    break;
                case 'B':
                    trackBarBlue.Value = val;
                    break;
                case 'S':
                    trackBarRed.Value = val;
                    trackBarGreen.Value = val;
                    trackBarBlue.Value = val;
                    break;
            }
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            labelColor.BackColor = Color.FromArgb(button.BackColor.ToArgb());
            button.BackColor = RandomColor();
            button.ForeColor = ForeColorByBack(button.BackColor);
        }

        private void buttonPreset_Background(Button button)
        {
            var preset = button.Name.Substring(6);
            var @var = preset[0];
            var val = int.Parse(preset.Substring(1));
            var c = labelColor.BackColor;
            switch (var)
            {
                case 'A':
                    c = Color.FromArgb(val, c.R, c.G, c.B);
                    break;
                case 'R':
                    c = Color.FromArgb(c.A, val, c.G, c.B);
                    break;
                case 'G':
                    c = Color.FromArgb(c.A, c.R, val, c.B);
                    break;
                case 'B':
                    c = Color.FromArgb(c.A, c.R, c.G, val);
                    break;
                case 'S':
                    c = Color.FromArgb(c.A, val, val, val);
                    break;
            }
            button.BackColor = c;
            button.ForeColor = ForeColorByBack(c);
        }

        private void buttonRandom_Background(Button button)
        {
            var c = button.BackColor;
            button.BackColor = Color.FromArgb(labelColor.BackColor.A, c.R, c.G, c.G);
            button.ForeColor = ForeColorByBack(button.BackColor);
        }

        private void CallPresetButtons(Action<Button> action, bool a = false, bool rgb = false)
        {
            List<char> options =  new List<char> { 'R', 'G', 'B' };
            if (a) options.Add('A');
            if (rgb) options.Add('S');
            var controls = tableLayoutPanel.Controls.OfType<Button>().Where(x => options.Contains(x.Name[6]) && x.Name.Any(char.IsNumber));
            foreach (var button in controls)
                action(button);
        }

        private void CallRandomButtons(Action<Button> action)
        {
            var controls = tableLayoutPanel.Controls.OfType<Button>().Where(x => x.Name.StartsWith("buttonRandom"));
            foreach (var button in controls)
                action(button);
        }

        private void trackBarStrength_Scroll(object sender, EventArgs e)
        {
            var avrg = (trackBarBlue.Value + trackBarGreen.Value + trackBarRed.Value) / 3;
            var diff = trackBarStrength.Value - avrg;
            trackBarRed.Value += trackBarRed.Value + diff > 255 || trackBarRed.Value + diff < 0 ? 0 : diff;
            trackBarGreen.Value += trackBarGreen.Value + diff > 255 || trackBarGreen.Value + diff < 0 ? 0 : diff;
            trackBarBlue.Value += trackBarBlue.Value + diff> 255 || trackBarBlue.Value + diff < 0 ? 0 : diff;
        }
    }
}
