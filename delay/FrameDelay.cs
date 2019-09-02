using System;
using utils;
/**
 * ...
 * @author P.J.Shand
 */
 namespace delay
{
	class FrameDelay : IDelayObject
	{
		public object[] parameters { get => this.parameters; set => this.parameters = value; }
		public Action callback { get => this.callback; set => this.callback = value; }
		public bool isComplete { 
			get {
				
				if (count++ >= frames) return true;
		return false;
			}
		}
		public int repeat { get => this.repeat; set => this.repeat = value; }
		public int fireCount { get => this.fireCount; set => this.fireCount = value; }
		public string id { get => this.id; set => this.id = value; }
		public bool markedForRemoval { get => this.markedForRemoval; set => this.markedForRemoval = value; }

		int frames;
		int count = 0;

		public FrameDelay(int frames, Action callback, object[] parameters=null, int repeat=0, string id = null) 
		{
			this.frames = frames;
			this.callback = callback;
			this.parameters = parameters;
			this.repeat = repeat;
			this.id = id;
		}
		
		public void Dispatch()
		{
			FunctionUtil.Dispatch(callback, parameters); 
		}
		
		public void Reset()
		{
			count = 0;
		}
    }
}