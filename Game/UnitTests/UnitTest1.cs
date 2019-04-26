using System;
using System.IO;
using System.Text;
using DbRepository;
using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<ISaveLoader> _saveLoaderMock;
        private GameController _objectUnderTest;
        [TestInitialize]
        public void Initialize()
        {
            _objectUnderTest = new GameController();
            _saveLoaderMock = new Mock<ISaveLoader>();

            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);
        }

        [TestMethod]
        public void LoadPlayer_NewPlayer()
        {
            //  arrange
            Player expectedPlayer = new Player() { Name = "Гость", Score = 0 };
            _saveLoaderMock.Setup(l => l.Load(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((Player)null);
            //  SaveLoader должен быть приватным и инициализироваться через конструктор по хорошему.
            _objectUnderTest.SaveLoader = _saveLoaderMock.Object;

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

        [TestMethod]
        public void LoadPlayer_ExsistingPlayer()
        {
            //  arrange
            Player expectedPlayer = new Player() { Name = "Denis", Score = 666 };
            _saveLoaderMock.Setup(l => l.Load(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expectedPlayer);
            //  SaveLoader должен быть приватным и инициализироваться через конструктор по хорошему.
            _objectUnderTest.SaveLoader = _saveLoaderMock.Object;

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
    }
}
