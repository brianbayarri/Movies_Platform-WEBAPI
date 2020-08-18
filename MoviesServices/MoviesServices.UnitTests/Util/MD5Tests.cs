using Xunit;
using MoviesServices.Util;

namespace MoviesServices.UnitTests.Util
{
    public class MD5Tests
    {
        [Fact]
        public void CalculateMD5_WhenCalled_ReturnsString()
        {
            string response = MD5.calculateMD5("brian");

            Assert.Equal("CBD44F8B5B48A51F7DAB98ABCDF45D4E", response);
        }
    }
}
