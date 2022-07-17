
using System;
using System.Threading;

public class TickTimer {
    private long m_nTick = 0;
    /*
    * 타이머 종료 조건을 저장하는 플래그
    */
    public bool Assert  = false;
    /*
    * 타이머 일시정지 플래그
    */
    public bool IsPause = false;
    /*
    * 커스텀 이벤트 핸들러
    */
    public EventHandler Tick;
    public EventHandler EndTick;
    /*
    * 타이머를 시작하는 메서드
    * tick      : 몇 Tick 마다 반복 할 것인지 설정
    */
    public void Start(long tick){
        this.m_nTick = tick;

        Thread thr = new Thread( this.TimerFunc );
        thr.IsBackground = true;
        thr.Start();
    }
    private void TimerFunc(){
        long curTick = DateTime.Now.Ticks;
        // 반복문 (스핀락 대기 알고리즘)
        while(true){
            if( !IsPause ){
                // 현재 Tick 값과 처음 반복문을 시작할 때의 Tick 값의 차이를 구해서
                // 그 차이가 원하는 Tick 시간보다 커졌을 때, Tick 이벤트 호출
                if( DateTime.Now.Ticks - curTick > this.m_nTick ){
                    // 종료 신호가 입력되면, 종료
                    if( this.Assert ){
                        break;
                    }
                    // Tick 이벤트 호출
                    Tick?.Invoke(this, EventArgs.Empty);
                    // 다음 Tick 계산을 위해 초기화
                    curTick = DateTime.Now.Ticks;
                }
            }
        }
        // 종료 플래그 초기화
        this.Assert = false;

        // 타이머 종료 이벤트 발생
        EndTick?.Invoke(this, EventArgs.Empty);
    }
}