using System;
using System.Windows.Forms;
using System.Drawing;

public class Program {
    static TextReplayer s_textreplay = new TextReplayer();
    static ToggleImageButton s_rsBtn = new ToggleImageButton();

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
            if( s_rsBtn.ActiveState ){
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
        timeSlider.AutoSize      = false;
        timeSlider.Anchor        = AnchorStyles.Left | AnchorStyles.Right;
        timeSlider.Slide        += delegate(object sender, EventArgs args){
            if( !s_rsBtn.ActiveState ){
                try{
                    // Restore time data
                    textBox.Text = s_textreplay.GetDataFromTime( timeSlider.Value );
                }catch{}
            }
        };

        // Adding Recording Button
        s_rsBtn.Location        = new Point(10,48);
        s_rsBtn.Size            = new Size(32,32);
        s_rsBtn.Anchor          = AnchorStyles.Left;

        s_rsBtn.SetHint("Start Recording");
        s_rsBtn.AddImagePool("./resources/record_start.png");
        s_rsBtn.AddImagePool("./resources/record_stop.png");
        s_rsBtn.SetImage(0);

        s_rsBtn.Active   += delegate(object sender, EventArgs args){
            s_textreplay.StartRecord(textBox.Text);
            ((ImageButton)sender).SetImage(1);
            ((ImageButton)sender).SetHint("Stop Recording");
        };

        s_rsBtn.Deactive += delegate(object sender, EventArgs args){
            timeSlider.Maximum = s_textreplay.StopRecord();
            ((ImageButton)sender).SetImage(0);
            ((ImageButton)sender).SetHint("Start Recording");
        };

        // Adding Pause/Resume Button
        ToggleImageButton prBtn = new ToggleImageButton();
        prBtn.Location          = new Point(48,48);
        prBtn.Size              = new Size(32,32);
        prBtn.Anchor            = AnchorStyles.Left;

        prBtn.SetHint("Play Record");
        prBtn.AddImagePool("./resources/play.png");
        prBtn.AddImagePool("./resources/pause.png");
        prBtn.SetImage(0);

        prBtn.Active   += delegate(object sender, EventArgs args){
            ((ImageButton)sender).SetImage(1);
            ((ImageButton)sender).SetHint("Pause Record");
        };

        prBtn.Deactive += delegate(object sender, EventArgs args){
            ((ImageButton)sender).SetImage(0);
            ((ImageButton)sender).SetHint("Play Record");
        };

        // Adding Children 
        controlBox.Controls.Add(timeSlider);
        controlBox.Controls.Add(s_rsBtn);
        controlBox.Controls.Add(prBtn);
        
        // Adding Children
        form.Controls.Add( textBox );
        form.Controls.Add( controlBox );

        form.ShowDialog();
    }
}  