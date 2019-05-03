using DbRepository;
using Game;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class UnitTest1
    {
        private Mock<ISaveLoader> _saveLoaderMock;
        private GameController _objectUnderTest;
        [SetUp]
        public void Initialize()
        {
            _objectUnderTest = new GameController();
            _saveLoaderMock = new Mock<ISaveLoader>();

            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            };
            Console.SetOut(standardOut);
        }

        [Test]
        public void LoadPlayer_NewPlayer()
        {
            //  arrange
            Player expectedPlayer = new Player() { Name = "Гость", Score = 0 };
            _saveLoaderMock.Setup(l => l.Load(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((Player)null);
            //  SaveLoader должен быть приватным и инициализироваться через конструктор по хорошему.
            _objectUnderTest._saveLoader = _saveLoaderMock.Object;

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                //  здесь мы записываем логин и пароль, чтобы подменить это при вызове Console.ReadLine
                using (StringReader sr = new StringReader(string.Format("Denis{0}qwert123{0}",
                    Environment.NewLine)))
                {
                    Console.SetIn(sr);
                    //  act
                    var loadedPlayer = _objectUnderTest.LoadPlayer();

                    //  assert
                    Assert.AreEqual(loadedPlayer.Name, expectedPlayer.Name);
                    Assert.AreEqual(loadedPlayer.Score, expectedPlayer.Score);
                }
            }
        }

        [Test]
        public void LoadPlayer_ExsistingPlayer()
        {
            //  arrange
            Player expectedPlayer = new Player() { Name = "Denis", Score = 666 };
            _saveLoaderMock.Setup(l => l.Load(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expectedPlayer);
            //  SaveLoader должен быть приватным и инициализироваться через конструктор по хорошему.
            _objectUnderTest._saveLoader = _saveLoaderMock.Object;

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                //  здесь мы записываем логин и пароль, чтобы подменить это при вызове Console.ReadLine
                using (StringReader sr = new StringReader(string.Format("Denis{0}qwert123{0}",
                    Environment.NewLine)))
                {
                    Console.SetIn(sr);
                    //  act
                    var loadedPlayer = _objectUnderTest.LoadPlayer();

                    //  assert
                    Assert.AreEqual(loadedPlayer.Name, expectedPlayer.Name);
                    Assert.AreEqual(loadedPlayer.Score, expectedPlayer.Score);
                }
            }
        }
        /*
        [Test]
        [TestCase("eqwr123", 123)]
        [TestCase("123", 123)]
        [TestCase("eqwr", 0)]
        [TestCase("32eqwr123", 32123)]
        [TestCase("12eqwr", 12)]
        [TestCase("eqwr123rwer", 123)]
        public void ReadNumberFromConsoleTest(string Input, int expect)
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                using (StringReader sr = new StringReader(Input) )
                {
                    Console.SetIn(sr);
                }

                Assert.AreEqual(_objectUnderTest.ReadNumberFromConsole(), expect);
            }
        }*/

        [Test]
        public void Round_SavePlayer()
        {
            bool SaveMethodWasInvoked = false;
            _saveLoaderMock.Setup(l => l.Save(It.IsAny<Player>())).Callback(() => SaveMethodWasInvoked = true);
            _objectUnderTest.Round(new Player(), new Player());
            Assert.IsFalse(SaveMethodWasInvoked);
        }
    }
}
