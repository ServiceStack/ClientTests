﻿using System;
using System.Collections.Generic;
using ServiceStack;
using Tests.ServiceModel.Types;

namespace Tests.ServiceModel
{
    [Route("/echo/types")]
    public class EchoTypes : IReturn<EchoTypes>
    {
        public byte Byte { get; set; }
        public short Short { get; set; }
        public int Int { get; set; }
        public long Long { get; set; }
        public ushort UShort { get; set; }
        public uint UInt { get; set; }
        public ulong ULong { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }
        public string String { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public Guid Guid { get; set; }
        public char Char { get; set; }
    }

    [Route("/echo/nullables")]
    public class EchoNullableTypes : IReturn<EchoNullableTypes>
    {
        public int? Id { get; set; }
        public byte? Byte { get; set; }
        public short? Short { get; set; }
        public int? Int { get; set; }
        public long? Long { get; set; }
        public ushort? UShort { get; set; }
        public uint? UInt { get; set; }
        public ulong? ULong { get; set; }
        public float? Float { get; set; }
        public double? Double { get; set; }
        public decimal? Decimal { get; set; }
        public DateTime? DateTime { get; set; }
        public TimeSpan? TimeSpan { get; set; }
        public DateTimeOffset? DateTimeOffset { get; set; }
        public Guid? Guid { get; set; }
        public char? Char { get; set; }
    }

    [Route("/echo/collections")]
    public class EchoCollections : IReturn<EchoCollections>
    {
        public List<string> StringList { get; set; }
        public string[] StringArray { get; set; }
        public Dictionary<string, string> StringMap { get; set; }
        public Dictionary<int, string> IntStringMap { get; set; }
    }

    public class EchoComplexTypes : IReturn<EchoComplexTypes>
    {
        public SubType SubType { get; set; }
    }

}