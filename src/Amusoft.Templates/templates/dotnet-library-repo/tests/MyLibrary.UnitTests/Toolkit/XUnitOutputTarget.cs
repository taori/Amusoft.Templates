using NLog;
using NLog.Targets;
using Xunit.Abstractions;

namespace MyLibrary.UnitTests.Toolkit
{
	[Target("XUnitOutputTarget")]
	public class XUnitOutputTarget : TargetWithLayout
	{
		protected override void Write(LogEventInfo logEvent)
		{
			OutputHelper.WriteLine(RenderLogEvent(Layout, logEvent));
		}

		public static ITestOutputHelper OutputHelper { get; set; }
	}
}