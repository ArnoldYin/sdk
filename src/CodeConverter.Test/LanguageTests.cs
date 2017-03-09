﻿using CodeConverter.Common;
using CodeConverter.CSharp;
using CodeConverter.PowerShell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace CSharpToPowerShell.Test
{
    public class LanguageTests : IClassFixture<Fixture>
    {
        public LanguageTests(Fixture fixture)
        {

        }

        [Theory]
        [MemberData("TestData", MemberType = typeof(TestCases))]
        public void TestLanguages(ConversionTestCase testCase)
        {
            var mdBuilder = new StringBuilder();
            var sourceLanguage = Enum.GetName(typeof(Language), testCase.SyntaxTreeVisitor.Language);
            var targetLanguage = Enum.GetName(typeof(Language), testCase.CodeWriter.Language);

            mdBuilder.AppendLine($"# Convert {sourceLanguage} to {targetLanguage}");

            var source = ReadTestData(testCase.Name, testCase.SyntaxTreeVisitor.Language);
            var target = ReadTestData(testCase.Name, testCase.CodeWriter.Language);

            var ast = testCase.SyntaxTreeVisitor.Visit(source);
            var actual = testCase.CodeWriter.Write(ast).Trim();

            //Assert.Equal(target, actual);

            if (testCase.OutputToMarkdown)
            {
                mdBuilder.AppendLine($"## {testCase.Description}");
                mdBuilder.AppendLine($"### Source: {sourceLanguage}");
                mdBuilder.AppendLine($"```{sourceLanguage.ToLower()}");
                mdBuilder.AppendLine(source);
                mdBuilder.AppendLine($"```");
                mdBuilder.AppendLine($"### Target: {targetLanguage}");
                mdBuilder.AppendLine($"```{targetLanguage.ToLower()}");
                mdBuilder.AppendLine(actual);
                mdBuilder.AppendLine($"```");
                mdBuilder.AppendLine();

                var languageTestsMarkdownPath = Path.Combine(GetTestDirectory(), @"..\..\..\..\..\language-tests.md");
                File.AppendAllText(languageTestsMarkdownPath, mdBuilder.ToString());
            }

        }

        private string GetLanguageExtension(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    return ".cs";
                case Language.PowerShell:
                    return ".ps1";
            }

            throw new NotImplementedException();
        }

        private string ReadTestData(string testName, Language language)
        {
            var languageName = Enum.GetName(typeof(Language), language);
            var extension = GetLanguageExtension(language);

            var testPath = $"Languages\\{languageName}\\{testName}{extension}";

            var testData = Path.Combine(GetTestDirectory(), testPath);

            if (!File.Exists(testData))
            {
                throw new Exception($"No test data found at {testData}");
            }

            return File.ReadAllText(testData);
        }

        private string GetTestDirectory()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            return Path.GetDirectoryName(codeBasePath);
        }
    }

    public class Fixture : IDisposable
    {
        public Fixture()
        {
            var languageTestsMarkdownPath = Path.Combine(GetTestDirectory(), @"..\..\..\..\..\language-tests.md");
            File.Delete(languageTestsMarkdownPath);

            var mdBuilder = new StringBuilder();

            mdBuilder.AppendLine("# Language Conversion Tests");
            mdBuilder.AppendLine($"### This file was generated by tests that were run on {DateTime.Now}.");
            File.AppendAllText(languageTestsMarkdownPath, mdBuilder.ToString());
        }

        public void Dispose()
        { 
        }

        private string GetTestDirectory()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            return Path.GetDirectoryName(codeBasePath);
        }
    }

    public static class TestCases
    {
        public static IEnumerable<ISyntaxTreeVisitor> SyntaxTreeVisitors { get; private set; }
        public static IEnumerable<CodeWriter> CodeWriters { get; private set; }
        public static Tuple<string, string, bool>[] Cases { get; private set; }

        static TestCases()
        {
            SyntaxTreeVisitors = new ISyntaxTreeVisitor[]
            {
                new PowerShellSyntaxTreeVisitor(),
                new CSharpSyntaxTreeVisitor()
            };

            CodeWriters = new CodeWriter[]
            {
                new PowerShellCodeWriter(),
                //new CSharpCodeWriter()
            };

            Cases = new Tuple<string, string, bool>[]
            {
                new Tuple<string,string, bool>("ArrayCreation", "Array creation initializers", true),
                new Tuple<string,string, bool>("AssignString", "Assign a string to a variable", true),
                new Tuple<string,string, bool>("AssignVariable", "Assign a constant to a variable", true),
                new Tuple<string,string, bool>("Cast", "Cast operator", true),
                new Tuple<string,string, bool>("For", "For loop", true),
                new Tuple<string,string, bool>("Foreach", "Foreach loop", true),
                new Tuple<string,string, bool>("Indexer", "Indexer property", true),
                new Tuple<string,string, bool>("If", "If, Else If, Else", true),
                new Tuple<string,string, bool>("MethodDeclaration", "Declare a method", true),
                new Tuple<string,string, bool>("MethodDeclarationWithArguments", "Declare a method with arguments", true),
                new Tuple<string,string, bool>("ObjectCreation", "Create an object", true),
                new Tuple<string,string, bool>("ObjectCreationWithArguments", "Create an object with arugments", true),
                new Tuple<string,string, bool>("Operators", "Common operators", true),
                new Tuple<string,string, bool>("PropertyAccess", "Access the property of a variable", true),
                //new ConversionTestCase("PInvokeSignature", "Platform invoke signature"),
                new Tuple<string,string, bool>("Return", "Return statement", true),
                new Tuple<string,string, bool>("Snippet", "Declare a method outside of a class or namespace", true),
                new Tuple<string,string, bool>("TryCatchFinally", "Try, catch, finally", true),
                new Tuple<string,string, bool>("While", "While loop with break", true),
                new Tuple<string,string, bool>("Snippet_50", "Error code reported. Conversion number 50.", true),
            };

            _data = new List<object[]>();

            foreach (var syntaxTreeVisitor in SyntaxTreeVisitors)
            {
                foreach (var codeWriter in CodeWriters)
                {
                    if (syntaxTreeVisitor.Language == codeWriter.Language)
                    {
                        continue;
                    }

                    foreach(var testCase in Cases)
                    {
                        _data.Add(new[] { new ConversionTestCase(testCase.Item1, testCase.Item2, syntaxTreeVisitor, codeWriter, testCase.Item3) });
                    }
                }
            }
        }

        private static readonly List<object[]> _data;
        public static IEnumerable<object[]> TestData
        {
            get { return _data; }
        }
    }

    public class ConversionTestCase : IXunitSerializable
    {
        public ConversionTestCase()
        {

        }
        public ConversionTestCase(string name, string description, ISyntaxTreeVisitor syntaxTreeVisitor, CodeWriter codeWriter, bool outputToMarkdown)
        {
            Name = name;
            Description = description;
            SyntaxTreeVisitor = syntaxTreeVisitor;
            CodeWriter = codeWriter;
            OutputToMarkdown = outputToMarkdown;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public ISyntaxTreeVisitor SyntaxTreeVisitor { get; set; }
        public CodeWriter CodeWriter { get; set; }
        public bool OutputToMarkdown { get; set; }

        public void Deserialize(IXunitSerializationInfo info)
        {
            Name = info.GetValue<string>(nameof(Name));
            Description = info.GetValue<string>(nameof(Description));
            var type = Type.GetType(info.GetValue<string>(nameof(SyntaxTreeVisitor)));
            SyntaxTreeVisitor = Activator.CreateInstance(type) as ISyntaxTreeVisitor;
            type = Type.GetType(info.GetValue<string>(nameof(CodeWriter)));
            CodeWriter = Activator.CreateInstance(type) as CodeWriter;
            OutputToMarkdown = bool.Parse(info.GetValue<string>(nameof(OutputToMarkdown)));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Name), Name);
            info.AddValue(nameof(Description), Description);
            info.AddValue(nameof(SyntaxTreeVisitor), SyntaxTreeVisitor.GetType().AssemblyQualifiedName);
            info.AddValue(nameof(CodeWriter), CodeWriter.GetType().AssemblyQualifiedName);
            info.AddValue(nameof(OutputToMarkdown), OutputToMarkdown.ToString());
        }

        public override string ToString()
        {
            return $"{Name}, {SyntaxTreeVisitor.GetType().Name} -> {CodeWriter.GetType().Name}";
        }
    }
}
