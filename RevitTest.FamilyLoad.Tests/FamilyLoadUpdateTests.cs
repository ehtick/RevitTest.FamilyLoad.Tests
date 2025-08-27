using Autodesk.Revit.DB;
using NUnit.Framework;
using System;
using System.IO;

namespace RevitTest.FamilyLoad.Tests
{
    public class FamilyLoadUpdateTests : Utils.OneTimeOpenDocumentTest
    {
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
