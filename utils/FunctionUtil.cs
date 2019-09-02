
using System;
/**
* ...
* @author P.J.Shand
*/
namespace utils
{
	public static class FunctionUtil {
		public static dynamic Dispatch(dynamic callback, object[] parameters = null) {
			if (parameters == null) {
				return callback();
			}
			switch (parameters.Length) {
				case 0:
					return callback();
				case 1:
					return callback(parameters[0]);
				case 2:
					return callback(parameters[0], parameters[1]);
				case 3:
					return callback(parameters[0], parameters[1], parameters[2]);
				case 4:
					return callback(parameters[0], parameters[1], parameters[2], parameters[3]);
				case 5:
					return callback(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
				case 6:
					return callback(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
				case 7:
					return callback(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5], parameters[6]);
				case 8:
					return callback(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5], parameters[6], parameters[7]);
				case 9:
					return callback(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5], parameters[6], parameters[7], parameters[8]);
				case 10:
					return callback(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5], parameters[6], parameters[7], parameters[8], parameters[9]);
			}
			return null;
		}
	}
}
