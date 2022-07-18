
using System;
using System.Collections.Generic;

public class TextReplayer {
    private Dictionary<long,string> records = new Dictionary<long,string>();
    private long curTick = DateTime.Now.Ticks;

    /*
    * 초기 데이터를 주고 기록을 시작하는 메서드
    */
    public void StartRecord( string inital ){
        this.StartRecord();
        this.AddRecordPoint(inital);
    }
    /*
    * 초기 데이터 없이 기록을 시작하는 메서드
    */
    public void StartRecord(){
        // 새로운 기록을 시작하면 이전의 기록을 삭제하고, 내부 Tick 타이머를 초기화
        records.Clear();
        curTick = DateTime.Now.Ticks;
    }
    /*
    * 현재 Tick 값을 밀리초로 변환하여 반환하는 메서드
    */
    public long GetCurrentTime(){
        // 정확도 향상을 위해 Tick을 이용하여 밀리초로 변환하여 반환
        return (long)((double)(DateTime.Now.Ticks - curTick) * 0.0001);
    }
    /*
    * 총 Tick 값을 밀리초로 변환하여 반환하는 메서드
    */
    public long StopRecord(){
        // 기록을 종료하면 기록을 시작하고 나서, 종료될 때 까지의 총 시간을 반환하고 종료
        return GetCurrentTime();
    }
    /*
    * 입력된 시간으로 부터 해당 시간에 저장 되어 있던 데이터를 검색하는 메서드
    */
    public string GetDataFromTime(long time){
        // 저장된 시간-데이터 쌍에서 시간을 기준으로 저장된 데이터를 가져오기
        return this.records[time];
    }
    public void AddRecordPoint(string data){
        // 레코드 기록 자료구조에 저장시간,데이터를 키-값 쌍으로 저장
        if( !this.records.ContainsKey(GetCurrentTime()) ){
            this.records.Add(GetCurrentTime(),data);
        }else{
            this.records.Add(GetCurrentTime() + 1,data);
        }
    }
}