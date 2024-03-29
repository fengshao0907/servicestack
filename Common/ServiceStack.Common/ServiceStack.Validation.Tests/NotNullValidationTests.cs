using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using ServiceStack.Validation.Tests.Support;
using ServiceStack.Validation.Validators;

namespace ServiceStack.Validation.Tests
{
	[TestFixture]
	public class NotNullValidationTests
	{
		private class ExampleModel1
		{
			public ExampleModel1()
			{
				EmptyString = "";
			}

			[NotNull]
			public string EmptyString { get; set; }

			[NotNull]
			public string NullString { get; set; }

		}

		[Test]
		public void OnlyNullReferencesFailNotNullValidationTest()
		{
			var result = ModelValidator.ValidateObject(new ExampleModel1());
			Assert.That(result.Errors.Count, Is.EqualTo(1));

			var error = result.Errors[0];
			Assert.That(error.ErrorCode, Is.EqualTo(ValidationErrorCodes.Required.ToString()));
			Assert.That(error.FieldName, Is.EqualTo("NullString"));
		}

	}
}