using BookletPagesGenerator.Code;
using Common.SaveLoad;
using NUnit.Framework;

namespace UnitTests.Plugins.BookletPagesGenerator
{
	[TestFixture]
	class When_using_config
	{
		[Test]
		public void Should_set_valid_defaults()
		{
			var config = new Config();

			Assert.AreEqual(0, config.PagesOffset, "PagesOffset");
			Assert.AreEqual(40, config.PagesToPrint, "PagesToPrint");
			Assert.AreEqual(0, config.PrintPageId, "PrintPageId");
			Assert.AreEqual(800, config.TotalPages, "TotalPages");
		}

		[Test]
		public void Should_be_serializable()
		{
			var config = new Config();
			var ser = new ParamSerializer<Config>("temp.xml");

			ser.Save(config);
		}
	}
}
