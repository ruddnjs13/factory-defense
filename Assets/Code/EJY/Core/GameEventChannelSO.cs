using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blade.Core
{
	public class GameEvent
	{ }
	
	[CreateAssetMenu(menuName = "SO/EventChannel")]
    public class GameEventChannelSO : ScriptableObject
    {
	    // GameEvent를 상속받은 클래스 = 하나의 이벤트 채널
	    
	    // 각 이벤트 채널에 구독처리를 위한 딕셔너리
        private Dictionary<Type, Action<GameEvent>> _events = new Dictionary<Type, Action<GameEvent>>();
        // 이미 구독된 같은 메서드가 존재하는지 확인하기 위한 딕셔너리
        private Dictionary<Delegate, Action<GameEvent>> _lockUp = new Dictionary<Delegate, Action<GameEvent>>();

        public void AddListener<T>(Action<T> handler) where T : GameEvent
        {
	        // Delegate인데 어떻게 다른 함수인지 판별하냐? = 함수를 넘겨준 인스턴스로 비교
	        // 즉 같은 인스턴스에서 넘겨준 함수는 구독이 추가로 되지 않는다.
	        
			// 이미 구독된(룩업 테이블에 있는) 함수는 받지 않는다.
	        if (_lockUp.ContainsKey(handler) == false)
	        {
		        // 딕셔너리 상의 밸류값은 GameEvent를 매개변수로 받는 액션이기에
		        // 원래의 T 타입으로 전환해서 매개변수로 넘겨주는 액션
		        Action<GameEvent> castHandler = (evt) => handler(evt as T); //= handler.Invoke(evt as T);
		        // 룩업 테이블에서 매개변수로 받은 함수를 키값으로, 위 액션을 밸류로 세팅
		        // 룩업 테이블에 매개변수로 받은 함수를 올림
		        _lockUp[handler] = castHandler;
		        // 이벤트 채널
		        Type evtType = typeof(T);
		        // 이미 이벤트 채널에 구독목록이 존재하면
		        if (_events.ContainsKey(evtType))
		        {
			        // 원래 있던 이벤트 구독목록에 더해줌
			        _events[evtType] += castHandler;
		        }
		        // 없다면
		        else
		        {
			        // 하나만 구독되니, 액션으로 초기화
			        _events[evtType] = castHandler;
		        }
	        }
        }
        
        public void RemoveListener<T>(Action<T> handler) where T : GameEvent
        {
	        // 이벤트 채널
	       Type evtType = typeof(T);
	       // 구독해지하려는 함수가 룩업 테이블에 있는지 확인
	       // handler가 구독해지하려는 함수, action은 구독한 함수를 실행시키는 액션
	       if (_lockUp.TryGetValue(handler, out Action<GameEvent> action))
	       {
		       // 이벤트에 구독목록이 존재하는지 확인
		       // internalAction은 이벤트에 대한 구독목록
		       if (_events.TryGetValue(evtType, out Action<GameEvent> internalAction))
		       {
			       // 이벤트 구독목록에서 해지
			       internalAction -= action;
			       // 이벤트에 구독목록이 없다면
			       if(internalAction == null)
				       // 딕셔너리에서 삭제
				       _events.Remove(evtType);
			       // 남아있다면
			       else
			           // 구독해지한 목록으로 초기화
				       _events[evtType] = internalAction;
		       }
		       // 룩업 테이블에서 삭제
		       _lockUp.Remove(handler);
	       }

        }

        // GameEvent를 상속받은 클래스를 키값으로 구독한 모든 이벤트들을 실행
        public void RaiseEvent(GameEvent evt)
        {
	        // 이벤트 채널에 구독목록이 있다면, 구독목록에 있는 모든 액션들을 실행
	        if (_events.TryGetValue(evt.GetType(), out Action<GameEvent> handlers))
	        {
		        // 이 액션에 있는 함수들은 구독한 함수들을 실행시키는 함수들임
				// AddListener에 castHandler들이 구독 되어있음
		        handlers?.Invoke(evt);
	        }
        }

        // 딕셔너리 클리어
        public void Clear()
        {
	        _events.Clear();
	        _lockUp.Clear();
        }
    }
}