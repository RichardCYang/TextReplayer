
using System;
using System.Windows.Forms;
using System.Drawing;

public class ToggleImageButton : ImageButton {
    private bool m_bToggle;
    /*
    * 커스텀 이벤트 핸들러
    */
    public EventHandler Active;
    public EventHandler Deactive;

    public ToggleImageButton(){
        this.Click += delegate(object sender, EventArgs args){
            this.ActiveState = !this.ActiveState;
        };
    }

    public bool ActiveState
    {
        get
        {
            return m_bToggle;
        }

        set
        {
            m_bToggle = value;
            if( m_bToggle ){
                Active?.Invoke(this, EventArgs.Empty);
            }else{
                Deactive?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}