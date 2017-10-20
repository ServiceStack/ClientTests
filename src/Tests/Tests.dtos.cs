/* Options:
Date: 2017-10-19 21:51:03
Version: 4.00
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: http://localhost:5000

GlobalNamespace: Tests
//MakePartial: True
//MakeVirtual: True
//MakeInternal: False
//MakeDataContractsExtensible: False
//AddReturnMarker: True
//AddDescriptionAsComments: True
//AddDataContractAttributes: False
//AddIndexesToDataMembers: False
//AddGeneratedCodeAttributes: False
//AddResponseStatus: False
//AddImplicitVersion: 
//InitializeCollections: True
//ExportValueTypes: False
//IncludeTypes: 
//ExcludeTypes: 
//AddNamespaces: 
//AddDefaultXmlNamespace: http://schemas.servicestack.net/types
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;
using Tests;


namespace Tests
{

    [Route("/echo/collections")]
    public partial class EchoCollections
        : IReturn<EchoCollections>
    {
        public EchoCollections()
        {
            StringList = new List<string>{};
            StringArray = new string[]{};
            StringMap = new Dictionary<string, string>{};
            IntStringMap = new Dictionary<int, string>{};
        }

        public virtual List<string> StringList { get; set; }
        public virtual string[] StringArray { get; set; }
        public virtual Dictionary<string, string> StringMap { get; set; }
        public virtual Dictionary<int, string> IntStringMap { get; set; }
    }

    public partial class EchoComplexTypes
        : IReturn<EchoComplexTypes>
    {
        public virtual SubType SubType { get; set; }
    }

    [Route("/echo/types")]
    public partial class EchoTypes
        : IReturn<EchoTypes>
    {
        public virtual byte Byte { get; set; }
        public virtual short Short { get; set; }
        public virtual int Int { get; set; }
        public virtual long Long { get; set; }
        public virtual ushort UShort { get; set; }
        public virtual uint UInt { get; set; }
        public virtual ulong ULong { get; set; }
        public virtual float Float { get; set; }
        public virtual double Double { get; set; }
        public virtual decimal Decimal { get; set; }
        public virtual string String { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual TimeSpan TimeSpan { get; set; }
        public virtual DateTimeOffset DateTimeOffset { get; set; }
        public virtual Guid Guid { get; set; }
        public virtual Char Char { get; set; }
    }

    [Route("/hello")]
    [Route("/hello/{Name}")]
    public partial class Hello
        : IReturn<HelloResponse>
    {
        public virtual string Name { get; set; }
    }

    [Route("/all-types")]
    public partial class HelloAllTypes
        : IReturn<HelloAllTypesResponse>
    {
        public virtual string Name { get; set; }
        public virtual AllTypes AllTypes { get; set; }
        public virtual AllCollectionTypes AllCollectionTypes { get; set; }
    }

    public partial class HelloAllTypesResponse
    {
        public virtual string Result { get; set; }
        public virtual AllTypes AllTypes { get; set; }
        public virtual AllCollectionTypes AllCollectionTypes { get; set; }
    }

    public partial class HelloResponse
    {
        public virtual string Result { get; set; }
    }

    public partial class HelloString
        : IReturn<string>
    {
        public virtual string Name { get; set; }
    }

    public partial class RequiresAdmin
        : IReturn<RequiresAdmin>
    {
        public virtual int Id { get; set; }
    }

    public partial class SendDefault
        : IReturn<SendVerbResponse>
    {
        public virtual int Id { get; set; }
    }

    public partial class SendGet
        : IReturn<SendVerbResponse>, IGet
    {
        public virtual int Id { get; set; }
    }

    public partial class SendPost
        : IReturn<SendVerbResponse>, IPost
    {
        public virtual int Id { get; set; }
    }

    public partial class SendPut
        : IReturn<SendVerbResponse>, IPut
    {
        public virtual int Id { get; set; }
    }

    [Route("/sendrestget/{Id}", "GET")]
    public partial class SendRestGet
        : IReturn<SendVerbResponse>, IGet
    {
        public virtual int Id { get; set; }
    }

    public partial class SendReturnVoid
        : IReturnVoid
    {
        public virtual int Id { get; set; }
    }

    public partial class SendVerbResponse
    {
        public virtual int Id { get; set; }
        public virtual string PathInfo { get; set; }
        public virtual string RequestMethod { get; set; }
    }

    [Route("/testauth")]
    public partial class TestAuth
        : IReturn<TestAuthResponse>
    {
    }

    public partial class TestAuthResponse
    {
        public virtual string UserId { get; set; }
        public virtual string SessionId { get; set; }
        public virtual string UserName { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/null-response")]
    public partial class TestNullResponse
    {
    }

    [Route("/void-response")]
    public partial class TestVoidResponse
    {
    }

    [Route("/throw404")]
    [Route("/throw404/{Message}")]
    public partial class Throw404
    {
        public virtual string Message { get; set; }
    }

    [Route("/throwbusinesserror")]
    public partial class ThrowBusinessError
        : IReturn<ThrowBusinessErrorResponse>
    {
    }

    public partial class ThrowBusinessErrorResponse
    {
        public virtual ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/throwcustom400")]
    [Route("/throwcustom400/{Message}")]
    public partial class ThrowCustom400
    {
        public virtual string Message { get; set; }
    }

    [Route("/throwhttperror/{Status}")]
    public partial class ThrowHttpError
    {
        public virtual int? Status { get; set; }
        public virtual string Message { get; set; }
    }

    [Route("/throw/{Type}")]
    public partial class ThrowType
        : IReturn<ThrowTypeResponse>
    {
        public virtual string Type { get; set; }
        public virtual string Message { get; set; }
    }

    public partial class ThrowTypeResponse
    {
        public virtual ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/throwvalidation")]
    public partial class ThrowValidation
        : IReturn<ThrowValidationResponse>
    {
        public virtual int Age { get; set; }
        public virtual string Required { get; set; }
        public virtual string Email { get; set; }
    }

    public partial class ThrowValidationResponse
    {
        public virtual int Age { get; set; }
        public virtual string Required { get; set; }
        public virtual string Email { get; set; }
        public virtual ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/wait/{ForMs}")]
    public partial class Wait
        : IReturn<Wait>
    {
        public virtual int ForMs { get; set; }
    }

    public partial class AllCollectionTypes
    {
        public AllCollectionTypes()
        {
            IntArray = new int[]{};
            IntList = new List<int>{};
            StringArray = new string[]{};
            StringList = new List<string>{};
            PocoArray = new Poco[]{};
            PocoList = new List<Poco>{};
            PocoLookup = new Dictionary<string, List<Poco>>{};
            PocoLookupMap = new Dictionary<string, List<Dictionary<String,Poco>>>{};
        }

        public virtual int[] IntArray { get; set; }
        public virtual List<int> IntList { get; set; }
        public virtual string[] StringArray { get; set; }
        public virtual List<string> StringList { get; set; }
        public virtual Poco[] PocoArray { get; set; }
        public virtual List<Poco> PocoList { get; set; }
        public virtual Dictionary<string, List<Poco>> PocoLookup { get; set; }
        public virtual Dictionary<string, List<Dictionary<String,Poco>>> PocoLookupMap { get; set; }
    }

    public partial class AllTypes
    {
        public AllTypes()
        {
            StringList = new List<string>{};
            StringArray = new string[]{};
            StringMap = new Dictionary<string, string>{};
            IntStringMap = new Dictionary<int, string>{};
        }

        public virtual int Id { get; set; }
        public virtual int? NullableId { get; set; }
        public virtual byte Byte { get; set; }
        public virtual short Short { get; set; }
        public virtual int Int { get; set; }
        public virtual long Long { get; set; }
        public virtual ushort UShort { get; set; }
        public virtual uint UInt { get; set; }
        public virtual ulong ULong { get; set; }
        public virtual float Float { get; set; }
        public virtual double Double { get; set; }
        public virtual decimal Decimal { get; set; }
        public virtual string String { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual TimeSpan TimeSpan { get; set; }
        public virtual DateTimeOffset DateTimeOffset { get; set; }
        public virtual Guid Guid { get; set; }
        public virtual Char Char { get; set; }
        public virtual KeyValuePair<string, string> KeyValuePair { get; set; }
        public virtual DateTime? NullableDateTime { get; set; }
        public virtual TimeSpan? NullableTimeSpan { get; set; }
        public virtual List<string> StringList { get; set; }
        public virtual string[] StringArray { get; set; }
        public virtual Dictionary<string, string> StringMap { get; set; }
        public virtual Dictionary<int, string> IntStringMap { get; set; }
        public virtual SubType SubType { get; set; }
    }

    public partial class Poco
    {
        public virtual string Name { get; set; }
    }

    public partial class SubType
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }
}

