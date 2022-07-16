using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

public class ImageButton : Button {
    /*
    * Image 객체 생성 최적화를 위한 Image Pooling 저장소
    */
    private List<Image> m_imagePool = new List<Image>();
    /*
    * 이미지 버튼 위에 표시되는 Tooltip 정보를 저장하고 있는 변수
    */
    private ToolTip m_tooltip = new ToolTip();

    public ImageButton(){
        this.FlatStyle                 = FlatStyle.Flat;
        this.BackgroundImageLayout     = ImageLayout.Stretch;
        this.FlatAppearance.BorderSize = 0;
        // 포커스 잡혔을 때, 생성되는 테두리 삭제
        this.SetStyle(ControlStyles .Selectable, false);
    }
    public void SetHint( string hintmsg ){
        this.m_tooltip.SetToolTip(this, hintmsg);
    }
    public void AddImagePool( string url ){
        this.m_imagePool.Add( Image.FromFile(url) );
    }
    public void SetImage( string url ){
        this.BackgroundImage = Image.FromFile( url );
    }
    public void SetImage( int pool_idx ){
        if( this.m_imagePool.Count > pool_idx ){
            this.BackgroundImage = this.m_imagePool[ pool_idx ];
        }
    }
}