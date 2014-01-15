using System;
using LahpaMobile.Services;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class GameParserTests
    {
        [TestCase("Game 3 Porter Paints vs Sherwin Williams", "Porter Paints", "Sherwin Williams")]
        [TestCase("Game 4 Icemen vs RinkRats", "Icemen", "RinkRats")]
        public void CanParseResults(string input, string home, string away)
        {
            // arrange
            GameParser parser = new GameParser();

            // act
            SingleGame singleGame = parser.ParseGame(input, DateTime.Now, "Wherever", "My League");

            // assert
            Assert.AreEqual(home, singleGame.HomeTeam);
            Assert.AreEqual(away, singleGame.AwayTeam);
        }
        
    }
}