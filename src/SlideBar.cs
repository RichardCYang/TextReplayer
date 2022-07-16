using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

public class SlideBar : Panel{
    private bool   m_bDragState = false;
    private float  m_fDragDelta = 0.0f;
    private float  m_fBackDelta = 0.0f;
    public  Panel  Thumb        = null;
    public  Panel  Elapsed      = null;
    public  long   Maximum      = 100;
    public  long   Value        = 0;
    /*
    * 커스텀 이벤트 핸들러
    */
    public EventHandler Slide;
    /*
    * 내부 슬라이드 막대 요소를 생성하는 클래스 생성자
    */
    public SlideBar(){
        this.BackgroundImage = Image.FromFile("./resources/slider_back.png");
        this.BackgroundImageLayout = ImageLayout.Stretch;

        this.Thumb                  = new Panel();
        Thumb.Size                  = new Size(16,16);
        Thumb.Location              = new Point(0,0);
        Thumb.BackColor             = Color.Transparent;
        Thumb.BackgroundImage       = Image.FromFile("./resources/slider_thumb.png");
        Thumb.BackgroundImageLayout = ImageLayout.Stretch;
        Thumb.Enabled               = false; // 막대 트래커(Thumb)를 비활성화 안하면, 트래커 위에서 부모 컨트롤의 마우스 이벤트가 적용 X

        this.Elapsed                  = new Panel();
        Elapsed.Size                  = new Size(16,0);
        Elapsed.Location              = new Point(0,0);
        Elapsed.BackColor             = Color.Transparent;
        Elapsed.BackgroundImage       = Image.FromFile("./resources/slider_elapsed.png");
        Elapsed.BackgroundImageLayout = ImageLayout.Stretch;
        Elapsed.Enabled               = false; // 채움 막대(Elapsed)를 비활성화 안하면, 채움 바 위에서 부모 컨트롤의 마우스 이벤트가 적용 X

        // 막대 트래커(Thumb)의 드래그 기능 구현 (마우스 이벤트 사용)
        this.MouseMove   += delegate(object sender, MouseEventArgs args){
            // 막대 트래커(Thumb) 위치가 슬라이드 바 영역을 벗어나면, 예외처리
            if( args.X < 0 || args.X > (this.Width - Thumb.Width) ){
                return;
            }
            if( m_bDragState ){
                Elapsed.Width = args.X;
                Thumb.Left    = args.X;
                // 현재 슬라이드 바의 넓이와 막대 트래커(Thumb) 위치를 기반으로 현재 슬라이드의 값을 계산
                m_fDragDelta = (float)Math.Round( ((float)args.X / ((float)this.Width - (float)Thumb.Width)) * Maximum ); 
                // 동일한 값이 다시 들어오면 입력 방지
                if( m_fBackDelta != m_fDragDelta ){
                    m_fBackDelta = m_fDragDelta;
                    this.Value   = (long)m_fDragDelta;
                    // 커스텀 스크롤 이벤트 발생
                    Slide?.Invoke(this, EventArgs.Empty);
                }
            }
        };

        this.MouseDown  += delegate(object sender, MouseEventArgs args){
            Thumb.Left   = args.X;
            m_bDragState = true;
        };

        this.MouseUp    += delegate(object sender, MouseEventArgs args){
            m_bDragState = false;
        };
        
        // 부모 요소의 크기(Size) 값이 변경 될 때마다, 막대 트래커(Thumb)의 크기를 재조정
        this.SizeChanged += delegate(object sender, EventArgs args){
            Thumb.Size     = new Size((sender as Panel).Height,(sender as Panel).Height);
            Elapsed.Height = (sender as Panel).Height;
        };

        this.Controls.Add(Thumb);
        this.Controls.Add(Elapsed);
    }
}