
namespace ConferenceScheduler.Tests
{
    public class InputParserTests
    {
        [Fact]
        public void Parse_ValidCsv_ReturnsCorrectTalks()
        {
            string csvContent = "Test Talk,60min\nLightning Talk,relâmpago";
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, csvContent);

            try
            {
                var parser = new InputParser();
                List<Talk> talks = parser.Parse(tempFile);
                Assert.Equal(2, talks.Count);
                Assert.Equal("Test Talk", talks[0].Title);
                Assert.Equal(60, talks[0].DurationMinutes);
                Assert.Equal("Lightning Talk", talks[1].Title);
                Assert.Equal(5, talks[1].DurationMinutes);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void Parse_FileNotFound_ThrowsException()
        {
            var parser = new InputParser();
            Assert.Throws<FileNotFoundException>(() => parser.Parse("nao_existe.csv"));
        }

        [Fact]
        public void Parse_InvalidDuration_ThrowsFormatException()
        {
            string csvContent = "Bad,abc";
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, csvContent);

            var parser = new InputParser();
            try
            {
                Assert.Throws<FormatException>(() => parser.Parse(tempFile));
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}