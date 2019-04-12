﻿using System;
using DbRepository;
using PlayerModel;
using NUnit.Framework;
using System.Xml.Linq;

namespace UnitTests
{
    [TestFixture]
    public class TestSaveLoad
    {
        [Test]
        [TestCase(1, "paul", "234", 1423)]
        [TestCase(1, "dog", "123", 14)]
        [TestCase(1, "doddi", "65", 32)]
        [TestCase(1, "stiv", "asd", 14)]
        [TestCase(1, "stiv", "4342", 43)]
        [TestCase(1, "stiv", "d12gfg3", 14)]
        public void TestSave(int id, string name, string pass, int score)
        {
            Player player = new Player() { PlayerId = id, Name = name, Password = pass, Score = score };
            player = SaveLoader.Save(player);
            Assert.AreNotEqual(id, player.PlayerId);
            Assert.AreEqual(name, player.Name);
            Assert.AreEqual(pass, player.Password);
            Assert.AreEqual(score, player.Score);
        }
    }
}