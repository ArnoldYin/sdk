﻿using CodeConverter.Common;
using CodeConverter.CSharp;
using CodeConverter.PowerShell;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSharpToPowerShell.Test
{
    [TestFixture]
    public class LanguageTests
    {
        public IEnumerable<ISyntaxTreeVisitor> SyntaxTreeVisitors { get; private set; }
        public IEnumerable<CodeWriter> CodeWriters { get; private set; }
        public IEnumerable<ConversionTestCase> TestCases { get; private set; }

        [SetUp]
        public void TestInit()
        {
            SyntaxTreeVisitors = new ISyntaxTreeVisitor[]
            {
                new PowerShellSyntaxTreeVisitor(),
                new CSharpSyntaxTreeVisitor()
            };

            CodeWriters = new CodeWriter[]
            {
                new PowerShellCodeWriter()
            };

            TestCases = new ConversionTestCase[]
            {
                new ConversionTestCase("AssignString", "Assign a string to a variable"),
                new ConversionTestCase("AssignVariable", "Assign a constant to a variable"),
                new ConversionTestCase("MethodDeclaration", "Declare a method"),
                new ConversionTestCase("MethodDeclarationWithArguments", "Declare a method with arguments"),
                new ConversionTestCase("ObjectCreation", "Create an object"),
                new ConversionTestCase("ObjectCreationWithArguments", "Create an object with arugments"),
                new ConversionTestCase("Snippet", "Declare a method outside of a class or namespace")
            };
        }

        [Test]
        public void TestLanguages()
        {
            var mdBuilder = new StringBuilder();
            foreach(var syntaxTreeVisitor in SyntaxTreeVisitors)
            {
                foreach(var codeWriter in CodeWriters)
                {
                    if (syntaxTreeVisitor.Language == codeWriter.Language)
                    {
                        continue;
                    }

                    var sourceLanguage = Enum.GetName(typeof(Language), syntaxTreeVisitor.Language);
                    var targetLanguage = Enum.GetName(typeof(Language), codeWriter.Language);

                    mdBuilder.AppendLine($"# Convert {sourceLanguage} to {targetLanguage}");

                    foreach (var testCase in TestCases) {
                        var source = ReadTestData(testCase.Name, syntaxTreeVisitor.Language);
                        var target = ReadTestData(testCase.Name, codeWriter.Language);

                        var ast = syntaxTreeVisitor.Visit(source);
                        var actual = codeWriter.Write(ast).Trim();

                        Assert.That(actual, Is.EqualTo(target));
                        mdBuilder.AppendLine($"## {testCase.Description}");
                        mdBuilder.AppendLine($"### Source: {sourceLanguage}");
                        mdBuilder.AppendLine($"```{sourceLanguage.ToLower()}");
                        mdBuilder.AppendLine(source);
                        mdBuilder.AppendLine($"```");
                        mdBuilder.AppendLine($"### Target: {targetLanguage}");
                        mdBuilder.AppendLine($"```{targetLanguage.ToLower()}");
                        mdBuilder.AppendLine(target);
                        mdBuilder.AppendLine($"```");
                        mdBuilder.AppendLine();
                    }
                } 
            }

            var languageTestsMarkdownPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\..\language-tests.md");
            File.WriteAllText(languageTestsMarkdownPath, mdBuilder.ToString());
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

            var testData = Path.Combine(TestContext.CurrentContext.WorkDirectory, testPath);

            if (!File.Exists(testData))
            {
                Assert.Fail($"No test data found at {testData}");
            }

            return File.ReadAllText(testData);
        }
    }

    public class ConversionTestCase
    {
        public ConversionTestCase(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}
