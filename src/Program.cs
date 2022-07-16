using System;
using System.Windows.Forms;
using System.Drawing;

public class Program {
    static TextReplayer s_textreplay = new TextReplayer();
    static bool         s_bRecord    = false;

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
            if( s_bRecord ){
                try{
                    // Save time data
                    s_textreplay.AddRecordPoint(textBox.Text);
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
        timeSlider.Location      = new Point(10,25);
        timeSlider.Size          = new Size(645,16);
        timeSlider.Dock          = DockStyle.Fill;
        timeSlider.AutoSize      = false;
        timeSlider.Anchor        = AnchorStyles.Left | AnchorStyles.Right;
        timeSlider.Slide        += delegate(object sender, EventArgs args){
            if( !s_bRecord ){
                try{
                    // Restore time data
                    textBox.Text = s_textreplay.GetDataFromTime( timeSlider.Value );
                }catch{}
            }
        };

        // Adding Recording Button
        ImageButton srBtn     = new ImageButton();
        srBtn.Location        = new Point(10,48);
        srBtn.Size            = new Size(32,32);
        srBtn.Anchor          = AnchorStyles.Left;

        srBtn.SetHint("Start Recording");
        srBtn.AddImagePool("./resources/record_start.png");
        srBtn.AddImagePool("./resources/record_stop.png");
        srBtn.SetImage(0);

        srBtn.Click    += delegate(object sender,EventArgs  args){
            s_bRecord = !s_bRecord;
            if(s_bRecord){
                s_textreplay.StartRecord(textBox.Text);
                ((ImageButton)sender).SetImage(1);
                ((ImageButton)sender).SetHint("Stop Recording");
            }else{
                timeSlider.Maximum = s_textreplay.StopRecord();
                ((ImageButton)sender).SetImage(0);
                ((ImageButton)sender).SetHint("Start Recording");
            }
        };

        // Adding Pause/Resume Button
        Button prBtn   = new Button();
        prBtn.Location = new Point(5,50);

        // Adding Children 
        controlBox.Controls.Add(timeSlider);
        controlBox.Controls.Add(srBtn);
        //controlBox.Controls.Add(prBtn);
        
        // Adding Children
        form.Controls.Add( textBox );
        form.Controls.Add( controlBox );

        form.ShowDialog();
    }
}  