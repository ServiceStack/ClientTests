using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public class TestUtils
    {
        public static void AssertAllTypes(AllTypes actual, AllTypes expected)
        {
            Assert.That(actual.Byte, Is.EqualTo(expected.Byte));
            Assert.That(actual.Char, Is.EqualTo(expected.Char));
            //Assert.That(actual.DateTime, Is.EqualTo(expected.DateTime).Within(TimeSpan.FromSeconds(1))); //Server TimeZone Issue
            //Assert.That(actual.DateTimeOffset, Is.EqualTo(expected.DateTimeOffset));
            Assert.That(actual.Decimal, Is.EqualTo(expected.Decimal));
            Assert.That(actual.Double, Is.EqualTo(expected.Double));
            Assert.That(actual.Float, Is.EqualTo(expected.Float).Within(0.0001));
            Assert.That(actual.Guid, Is.EqualTo(expected.Guid));
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Int, Is.EqualTo(expected.Int));
            Assert.That(actual.IntStringMap, Is.EquivalentTo(expected.IntStringMap));
            Assert.That(actual.KeyValuePair.Key, Is.EqualTo(expected.KeyValuePair.Key));
            Assert.That(actual.KeyValuePair.Value, Is.EqualTo(expected.KeyValuePair.Value));
            Assert.That(actual.Long, Is.EqualTo(expected.Long));
            Assert.That(actual.NullableDateTime, Is.EqualTo(expected.NullableDateTime).Within(TimeSpan.FromSeconds(1)));
            Assert.That(actual.NullableId, Is.EqualTo(expected.NullableId));
            //Assert.That(actual.NullableTimeSpan, Is.EqualTo(expected.NullableTimeSpan));
            Assert.That(actual.Short, Is.EqualTo(expected.Short));
            Assert.That(actual.StringArray, Is.EquivalentTo(expected.StringArray));
            Assert.That(actual.StringList, Is.EquivalentTo(expected.StringList));
            Assert.That(actual.StringMap, Is.EquivalentTo(expected.StringMap));
            Assert.That(actual.String, Is.EqualTo(expected.String));
            Assert.That(actual.SubType.Id, Is.EqualTo(expected.SubType.Id));
            Assert.That(actual.SubType.Name, Is.EqualTo(expected.SubType.Name));
            //Assert.That(actual.TimeSpan, Is.EqualTo(expected.TimeSpan));
            Assert.That(actual.UInt, Is.EqualTo(expected.UInt));
            Assert.That(actual.ULong, Is.EqualTo(expected.ULong));
            Assert.That(actual.UShort, Is.EqualTo(expected.UShort));
        }

        public static void AssertAllCollectionTypes(AllCollectionTypes actual, AllCollectionTypes expected)
        {
            Assert.That(actual.IntArray, Is.EqualTo(expected.IntArray));
            Assert.That(actual.IntList, Is.EqualTo(expected.IntList));
            AssertListPoco(actual.PocoArray, expected.PocoArray);
            AssertListPoco(actual.PocoList, expected.PocoList);

            Assert.That(actual.PocoLookup.Count, Is.EqualTo(expected.PocoLookup.Count));
            foreach (var key in actual.PocoLookup.Keys)
                AssertListPoco(actual.PocoLookup[key], expected.PocoLookup[key]);

            Assert.That(actual.PocoLookupMap.Count, Is.EqualTo(expected.PocoLookupMap.Count));

            foreach (var key in actual.PocoLookupMap.Keys)
            {
                var actualList = actual.PocoLookupMap[key];
                var expectedList = expected.PocoLookupMap[key];

                Assert.That(actualList.Count, Is.EqualTo(expectedList.Count));
                for (int i = 0; i < actualList.Count; i++)
                {
                    Assert.That(actualList[i].Count, Is.EqualTo(expectedList[i].Count));

                    foreach (var key2 in actualList[i].Keys)
                    {
                        Assert.That(actualList[i][key2].Name, Is.EqualTo(expectedList[i][key2].Name));
                    }
                }
            }
        }

        public static void AssertListPoco(IList<Poco> actual, IList<Poco> expected)
        {
            Assert.That(actual.Count, Is.EqualTo(expected.Count));

            for (int i = 0; i < actual.Count; i++)
                Assert.That(actual[i].Name, Is.EqualTo(expected[i].Name));
        }

    }
}