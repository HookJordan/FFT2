using System.Windows.Forms;

namespace fft_2
{
    public static class Prompt
    {
        public static string InputBox(string text, string caption, string _default = "")
        {
            Form form = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label lblText = new Label() { Left = 50, Top = 10, Text = text };
            TextBox txtInput = new TextBox() { Left = 50, Top = 40, Width = 400 };
            Button btnConfirm = new Button() { Text = "Confirm", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            btnConfirm.Click += (sender, e) => { form.Close(); };
            form.Controls.AddRange(new Control[] { lblText, txtInput, btnConfirm });
            form.AcceptButton = btnConfirm;
            txtInput.Text = _default;

            return form.ShowDialog() == DialogResult.OK ? txtInput.Text : _default;
        }
    }
}
