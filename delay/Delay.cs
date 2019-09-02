
//import time.EnterFrame;
//import time.TimeUnit;

/**
 * ...
 * @author P.J.Shand
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using notifier;
using time;

namespace delay
{
	class Delay 
	{
		private static List<IDelayObject> delayObjects;
		static bool initialized = false;

		static void init() { 
			
			if (initialized) return;
			initialized = true;
			delayObjects = new List<IDelayObject>();
			EnterFrame.Add(OnTick);
	}
		
		static private void OnTick() 
		{
			foreach (var delayObject in delayObjects) {
				if (delayObject.isComplete) {
					delayObject.Dispatch();
					if (delayObject.repeat != -1 && delayObject.fireCount >= delayObject.repeat){
						delayObject.markedForRemoval = true;
					} else {
						delayObject.fireCount++;
						delayObject.Reset();
					}
				}
			}
			

			int i = delayObjects.Count-1;
			while (i >= 0) 
			{
				if (delayObjects[i] == null || delayObjects[i].markedForRemoval){
					delayObjects.RemoveAt(i);
				}
				i--;
			}
		}
		
		public Delay() { }
		
		public static void nextFrame(Action callback, object[] parameters = null)
		{
			Delay.byFrames(1, callback, parameters);
		}
		
		public static void byFrames(int frames, Action callback, object[] parameters = null, int repeat=0) 
		{
			Delay.init();
			delayObjects.Add(new FrameDelay(frames, callback, parameters, repeat));
		}
		
		public static void byTime(float duration, Action callback, object[] parameters = null, Nullable<float> timeUnit = null, int repeat = 0) 
		{
			Delay.init();
			if (timeUnit == null) timeUnit = TimeUnit.SECONDS;
			delayObjects.Add(new TimeDelay(duration, callback, parameters, timeUnit, repeat));
		}
		
		public static void killDelay(Action callback) 
		{
			Delay.init();
			var i = delayObjects.Count - 1;
			while (i >= 0) {
				if (delayObjects[i].callback == callback) {
					delayObjects.RemoveAt(i);
				}
				i--;
			}
		}
	}
}