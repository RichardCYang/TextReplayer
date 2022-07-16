using System;
using System.Windows.Forms;
using System.Drawing;

public class Program {
    static TextReplayer trply = new TextReplayer();
    static bool bRecord = false;

    internal static void Main(string[] args){
        Form form = new Form();
        form.Text = "Replay Tester";
        form.Size = new Size(700,520);

        // Adding TextBox
        TextBox textBox     = new TextBox();
        textBox.Location    = new Point(10,10);
        textBox.Size        = new Size(665,360);
        textBox.Multiline   = true;
        textBox.Anchor      = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
        textBox.TextChanged+= delegate(object sender,EventArgs args){
            if( bRecord ){
                try{
                    // Save time data
                    trply.AddRecordPoint(textBox.Text);
                }catch{}
            }
        };

        // Adding Controls GroupBox
        GroupBox controlBox = new GroupBox();
        controlBox.Location = new Point(10,380);
        controlBox.Size     = new Size(665,90);
        controlBox.Text     = "Controls";
        controlBox.Anchor   = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

        // Adding Slider
        SlideBar timeSlider      = new SlideBar();
        timeSlider.Location      = new Point(5,25);
        timeSlider.Size          = new Size(480,16);
        timeSlider.AutoSize      = false;
        timeSlider.Anchor        = AnchorStyles.Left | AnchorStyles.Right;
        timeSlider.Slide        += delegate(object sender, EventArgs args){
            if( !bRecord ){
                try{
                    // Restore time data
                    textBox.Text = trply.GetDataFromTime( timeSlider.Value );
                }catch{}
            }
            Console.WriteLine(timeSlider.Value);
        };

        // Adding Recording Button
        Button startRecordBtn    = new Button();
        startRecordBtn.Location  = new Point(500,25);
        startRecordBtn.Size      = new Size(150,32);
        startRecordBtn.Text      = "Start Recording";
        startRecordBtn.Anchor    = AnchorStyles.Right;
        startRecordBtn.Click    += delegate(object sender,EventArgs  args){
            bRecord = !bRecord;
            if(bRecord){
                trply.StartRecord(textBox.Text);
                ((Button)sender).Text = "Stop Recording";
            }else{
                timeSlider.Maximum = trply.StopRecord();
                ((Button)sender).Text = "Start Recording";
            }
        };

        // Adding Pause/Resume Button
        Button prBtn   = new Button();
        prBtn.Location = new Point(5,50);

        // Adding Children 
        controlBox.Controls.Add(timeSlider);
        controlBox.Controls.Add(startRecordBtn);
        controlBox.Controls.Add(prBtn);
        
        // Adding Children
        form.Controls.Add( textBox );
        form.Controls.Add( controlBox );

        form.ShowDialog();
    }
}  