using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ServiceStack.Common.Extensions;
using ServiceStack.Text.Jsv;

namespace ServiceStack.Text.Tests
{
	[TestFixture]
	public class CaseConverterTests
		: TestBase
	{
        public enum CasedEnum
        {
            AppleJacks,
            appleJacks,
            FruitLoops,
            friedEggs,
        }

        public class CasedObject
        {
            public string PropertyOne { get; set; }
            public string propertyOne { get; set; }
            public string PropertyTwo { get; set; }
            public string propertyThree { get; set; }
        }

        public class CasedObject2
        {
            public CasedObject[] Objects { get; set; }
        }

        [Test]
        public void CanDeserializeCasedEnumValueCorrectly()
        {
            Assert.AreEqual(CasedEnum.FruitLoops, TypeSerializer.DeserializeFromString<CasedEnum>("FruitLoops"), "FruitLoops");
            Assert.AreEqual(CasedEnum.FruitLoops, TypeSerializer.DeserializeFromString<CasedEnum>("fruitLoops"), "fruitLoops");
            Assert.Throws(Is.InstanceOf<Exception>(), () => TypeSerializer.DeserializeFromString<CasedEnum>("fruitloops"));
        }

        [Test]
        public void CanDeserializeCasedEnumValueCorrectlyWhenBothCasesExist()
        {
            Assert.AreEqual(CasedEnum.AppleJacks, TypeSerializer.DeserializeFromString<CasedEnum>("AppleJacks"), "AppleJacks");
            Assert.AreEqual(CasedEnum.appleJacks, TypeSerializer.DeserializeFromString<CasedEnum>("appleJacks"), "appleJacks");
            Assert.Throws(Is.InstanceOf<Exception>(), () => TypeSerializer.DeserializeFromString<CasedEnum>("applejacks"));
        }

        [Test]
        public void CanDeserializeCorrectlyCasedProperties()
        {
            var ans = JsonSerializer.DeserializeFromString<CasedObject>(@"{""PropertyOne"":""one"",""propertyOne"":""0ne"",""PropertyTwo"":""two"",""propertyThree"":""three""}");
            Assert.AreEqual("one", ans.PropertyOne);
            Assert.AreEqual("0ne", ans.propertyOne);
            Assert.AreEqual("two", ans.PropertyTwo);
            Assert.AreEqual("three", ans.propertyThree);
        }

        [Test]
        public void CanDeserializeIncorrectlyCasedProperties()
        {
            var ans = JsonSerializer.DeserializeFromString<CasedObject>(@"{""PropertyOne"":""one"",""propertyOne"":""0ne"",""propertyTwo"":""two"",""PropertyThree"":""three""}");
            Assert.AreEqual("one", ans.PropertyOne);
            Assert.AreEqual("0ne", ans.propertyOne);
            Assert.AreEqual("two", ans.PropertyTwo);
            Assert.AreEqual("three", ans.propertyThree);
        }

        [Test]
        public void CanDeserializeArraysOfIncorrectlyCasedProperties()
        {
            var ans2 = JsonSerializer.DeserializeFromString<CasedObject2>(@"{""objects"":[{""PropertyOne"":""one"",""propertyOne"":""0ne"",""propertyTwo"":""two"",""PropertyThree"":""three""}]}");
            Assert.AreEqual(1, ans2.Objects.Length);
            var ans = ans2.Objects[0];
            Assert.AreEqual("one", ans.PropertyOne);
            Assert.AreEqual("0ne", ans.propertyOne);
            Assert.AreEqual("two", ans.PropertyTwo);
            Assert.AreEqual("three", ans.propertyThree);
        }
	}
}