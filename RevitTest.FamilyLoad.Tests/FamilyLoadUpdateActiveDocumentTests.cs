using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using System.IO;

namespace RevitTest.FamilyLoad.Tests
{
    public class FamilyLoadUpdateActiveDocumentTests
    {
        protected Document document;
        protected Application application;
        protected UIApplication uiapp;

        protected virtual string FileName => null;

        [OneTimeSetUp]
        public void NewProjectDocument(UIApplication uiApplication)
        {
            this.uiapp = uiApplication;
            this.application = uiapp.Application;
            this.document = uiapp.ActiveUIDocument?.Document;
            Assert.IsNotNull(document, "ActiveUIDocument is null");
        }

        [TestCase("Family*")]
        public void RevitTests_LoadFamily(string familyName)
        {
            var files = Directory.GetFiles("Family", $"{familyName}.rfa", SearchOption.AllDirectories);
            var currentVersion = int.Parse(application.VersionNumber);
            using (Transaction transaction = new Transaction(document))
            {
                transaction.Start("LoadFamily");
                foreach (var file in files)
                {
                    var basicFileInfo = BasicFileInfo.Extract(file);
                    var fileVersion = int.Parse(basicFileInfo.Format);
                    if (fileVersion > currentVersion)
                    {
                        Console.WriteLine($"Skip {file} - {fileVersion}");
                        continue;
                    }
                    Console.WriteLine($"LoadFamily: \t{file} - {fileVersion}");
                    var family = FamilyUtils.LoadFamily(document, file);
                }

                transaction.Commit();
            }
        }
    }

}
