using System;
using System.Drawing;
using System.Windows.Forms;

namespace RounedControl
{ 
    public partial class RounedCheckBox : UserControl
    {
        public delegate void Label_ClickHanddler(Color borderColor);
        public event Label_ClickHanddler Label_ClickEvent;
        public delegate void CheckBoxButton_ClickHanddler(bool btnClick, string labelText);
        public event CheckBoxButton_ClickHanddler CheckBoxBUtton_ClickEvent;

        private string labelText = "Check Box";
        private bool checkBoxBtnClick = false;

        public RounedCheckBox()
        {
            InitializeComponent();
            checkBtn.RounedButton_ClickEvnet += new RounedButton.ButtonClickEventHanddler((check, name) =>
            {
                CheckBoxButtonClicked = check;
                CheckBoxBUtton_ClickEvent(CheckBoxButtonClicked, LabelText);
            }); 
        }

        public string LabelText
        {
            get => labelText;
            set
            {
                labelText = value;
                checkLabel.Text = value;
            }
        }

        public bool CheckBoxButtonClicked
        {
            get => checkBoxBtnClick;
            set
            {
                checkBoxBtnClick = value;
                checkBtn.Clicked = value;
            }
        }

        private void checkLabel_Click(object sender, EventArgs e)
        {
            Label_ClickEvent(checkLabel.BorderColor);
        }
    }
}
