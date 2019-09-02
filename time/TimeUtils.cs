/**
 * ...
 * @author P.J.Shand
 */
namespace time
{
	public static class TimeUtils {
		
		public static float convert(float value, float from, float to)
		{
			return value * from / to;
		}

		public static float milToYears(float value) { return convert(value, TimeUnit.MILLISECONDS, TimeUnit.YEARS); }
		public static float milToMonths(float value) { return convert(value, TimeUnit.MILLISECONDS, TimeUnit.MONTHS); }
		public static float milToDays(float value) { return convert(value, TimeUnit.MILLISECONDS, TimeUnit.DAYS); }
		public static float milToHours(float value) { return convert(value, TimeUnit.MILLISECONDS, TimeUnit.HOURS); }
		public static float milToMinutes(float value) { return convert(value, TimeUnit.MILLISECONDS, TimeUnit.MINUTES); }
		public static float milToSeconds(float value) { return convert(value, TimeUnit.MILLISECONDS, TimeUnit.SECONDS); }
		
		public static float yearsToMil(float value) { return convert(value, TimeUnit.YEARS, TimeUnit.MILLISECONDS); }
		public static float monthsToMil(float value) { return convert(value, TimeUnit.MONTHS, TimeUnit.MILLISECONDS); }
		public static float daysToMil(float value) { return convert(value, TimeUnit.DAYS, TimeUnit.MILLISECONDS); }
		public static float hoursToMil(float value) { return convert(value, TimeUnit.HOURS, TimeUnit.MILLISECONDS); }
		public static float minutesToMil(float value) { return convert(value, TimeUnit.MINUTES, TimeUnit.MILLISECONDS); }
		public static float secondsToMil(float value) { return convert(value, TimeUnit.SECONDS, TimeUnit.MILLISECONDS); }
	}
}