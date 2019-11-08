using System;
using hr.helper.errors;
using Xunit;

namespace hr.test {
    public class ErrorBagTest {
        
        [Fact]
        public void InvalidDate_throwsErrorBag() {
            Action actual = () => {
                using(var x = new ErrorBag()) {
                    x.Required("birthday", "a,b@x").DateValue();
                }
            };

            Assert.Throws<ErrorBag>(actual);
        }

        [InlineData("1990-1-1")]
        [InlineData("1990/1/1")]
        [InlineData("1/1/1990")]
        [InlineData("1-1-1990")]
        [Theory]
        public void CanValidate_DateString(string strDate) {
            using(var x = new ErrorBag()) {
                x.Required("date", strDate);
                var actual = x.Errors.Count;
                
                Assert.Equal(0, actual);
            }
        }

    }
}