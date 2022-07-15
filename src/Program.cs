using System;
using System.Windows.Forms;
using System.Drawing;

public class Program {
    static TextReplayer trply = new TextReplayer();
    static bool isRecord = false;

    internal static void Main(string[] args){
        Form form = new Form();
        form.Text = "Replay Tester";
        form.Size = new Size(700,500);

        // Adding TextBox
        TextBox textBox     = new TextBox();
        textBox.Location    = new Point(10,10);
        textBox.Size        = new Size(665,360);
        textBox.Multiline   = true;
        textBox.Anchor      = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
        textBox.TextChanged+= delegate(object sender,EventArgs args){
            if( isRecord ){
                try{
                    // Save time data
                    trply.AddRecordPoint(textBox.Text);
                }catch{}
            }
        };

        // Adding Controls GroupBox
        GroupBox controlBox = new GroupBox();
        controlBox.Location = new Point(10,380);
        controlBox.Size     = new Size(665,70);
        controlBox.Text     = "Controls";
        controlBox.Anchor   = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

        // Adding Slider
        TrackBar timeSlider      = new TrackBar();
        timeSlider.Location      = new Point(5,28);
        timeSlider.AutoSize      = false;
        timeSlider.Width         = 480;
        timeSlider.Height        = 30;
        timeSlider.Maximum       = 100;
        timeSlider.TickFrequency = 10;
        timeSlider.Anchor        = AnchorStyles.Left | AnchorStyles.Right;
        timeSlider.LargeChange  = 1;
        timeSlider.SmallChange   = 1;
        // Hide the tick indicator
        timeSlider.TickStyle     = TickStyle.None;
        timeSlider.Scroll       += delegate(object sender,EventArgs args){
            // Restore time data
            try{
                textBox.Text = trply.GetDataFromTime(timeSlider.Value);
            }catch{}
        };

        // Adding Recording Button
        Button startRecordBtn    = new Button();
        startRecordBtn.Location  = new Point(500,25);
        startRecordBtn.Size      = new Size(150,30);
        startRecordBtn.Text      = "Start Recording";
        startRecordBtn.Anchor    = AnchorStyles.Right;
        startRecordBtn.Click    += delegate(object sender,EventArgs  args){
            isRecord = !isRecord;
            if(isRecord){
                trply.StartRecord(textBox.Text);
                ((Button)sender).Text = "Stop Recording";
            }else{
                timeSlider.Maximum = (int)trply.StopRecord();
                ((Button)sender).Text = "Start Recording";
            }
        };
        // Adding Children 
        controlBox.Controls.Add(timeSlider);
        controlBox.Controls.Add(startRecordBtn);
        
        // Adding Children
        form.Controls.Add( textBox );
        form.Controls.Add( controlBox );

        form.ShowDialog();
    }
}  