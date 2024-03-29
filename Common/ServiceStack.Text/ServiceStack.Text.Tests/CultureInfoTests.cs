using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace ServiceStack.Text.Tests
{
	[TestFixture]
	public class CultureInfoTests
		: TestBase
	{
		public class Point
		{
			public double Latitude { get; set; }
			public double Longitude { get; set; }

			public bool Equals(Point other)
			{
				if (ReferenceEquals(null, other)) return false;
				if (ReferenceEquals(this, other)) return true;
				return other.Latitude == Latitude && other.Longitude == Longitude;
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != typeof(Point)) return false;
				return Equals((Point)obj);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return (Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode();
				}
			}
		}

		private CultureInfo previousCulture = CultureInfo.InvariantCulture;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			previousCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			Thread.CurrentThread.CurrentCulture = previousCulture;
		}

		[Test]
		public void Can_deserialize_type_with_doubles_in_different_culture()
		{
			var point = new Point { Latitude = -23.5707, Longitude = -46.57239 };
			SerializeAndCompare(point);
		}
	}
}