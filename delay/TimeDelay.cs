//import haxe.Constraints.Function;
//import utils.FunctionUtil;
//import time.TimeUtils;
//import time.TimeUnit;
//import time.GlobalTime;

using System;
using time;
using utils;

/**
* ...
* @author P.J.Shand
*/
namespace delay
{
	class TimeDelay : IDelayObject
	{
		float startTime;
		float endTime;
		float millisecondsDuration;

        public object[] parameters { get => this.parameters; set => this.parameters = value; }
		public Action callback { get => this.callback; set => this.callback = value; }
		public bool isComplete { 
			get {
				
				if (DateTime.Now.Millisecond >= endTime) return true;
				return false;
			}
		}
		public int repeat { get => this.repeat; set => this.repeat = value; }
		public int fireCount { get => this.fireCount; set => this.fireCount = value; }
		public string id { get => this.id; set => this.id = value; }
		public bool markedForRemoval { get => this.markedForRemoval; set => this.markedForRemoval = value; }
		
        

        public TimeDelay(float duration, Action callback, object[] parameters=null, Nullable<float> timeUnit=null, int repeat=0, string id = null) 
		{
			this.callback = callback;
			this.parameters = parameters;
			this.repeat = repeat;
			this.id = id;

			if (timeUnit == TimeUnit.MILLISECONDS) millisecondsDuration = duration;
			else if (timeUnit == TimeUnit.SECONDS) millisecondsDuration = TimeUtils.secondsToMil(duration);
			else if (timeUnit == TimeUnit.MINUTES) millisecondsDuration = TimeUtils.minutesToMil(duration);
			else if (timeUnit == TimeUnit.HOURS) millisecondsDuration = TimeUtils.hoursToMil(duration);
			else if (timeUnit == TimeUnit.DAYS) millisecondsDuration = TimeUtils.daysToMil(duration);
			
			Reset();
		}

        public void Dispatch()
        {
            FunctionUtil.Dispatch(callback, parameters); 
        }

        public void Reset()
        {
            startTime = DateTime.Now.Millisecond;
			endTime = startTime + millisecondsDuration;
        }
    }
}