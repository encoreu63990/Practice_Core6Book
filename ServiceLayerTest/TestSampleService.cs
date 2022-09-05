using NUnit.Framework;
using ServiceLayer.Concrete;

namespace ServiceLayerTest
{
    [TestFixture]
    public class TestSampleService
    {
        private SampleService _SampleService;

        [SetUp]
        public void Setup()
        {
            _SampleService = new SampleService();
        }

        [Test]
        public void IsMoreThan2_InputIs3_ReturnTrue()
        {
            var result = _SampleService.IsMoreThan2(3);
            Assert.IsTrue(result);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void IsMoreThan2_InputLessThan2_ReturnFalse(int value)
        {
            var result = _SampleService.IsMoreThan2(value);
            Assert.IsFalse(result);
        }
    }
}