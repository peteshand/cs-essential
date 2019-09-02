
using System;
/**
* @author P.J.Shand
*/
namespace delay
{
	public interface IDelayObject 
	{
		object[] parameters { get; set; }
		Action callback { get; set; }
		bool isComplete { get; }
		int repeat { get; set; }
		int fireCount { get; set; }
		string id { get; set; }
		bool markedForRemoval { get; set; }

		void Dispatch();
		void Reset();
	}
}