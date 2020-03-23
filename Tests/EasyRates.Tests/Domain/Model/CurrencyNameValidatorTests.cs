using EasyRates.Model;
using Xunit;

namespace EasyRates.Tests.Domain.Model
{
    public class CurrencyNameValidatorTests
    {
        private CurrencyNameValidator validator = new CurrencyNameValidator();

        [Theory]
        [InlineData("rub")]
        [InlineData("uSd")]
        [InlineData("ttt")]
        public void ValidNameShouldHave3Symbols(string name)
        {
            validator.Validate(name);
        }

        [Theory]
        [InlineData("rub1")]
        [InlineData("r")]
        public void IfInvalidNameShouldThrowException(string name)
        {
            Assert.Throws<InvalidCurrencyNameException>(
                () => validator.Validate(name));
        }
    }
}