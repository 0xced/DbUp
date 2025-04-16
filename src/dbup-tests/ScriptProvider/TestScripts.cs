using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DbUp.Tests.ScriptProvider;

static class TestScripts
{
    public static void Create(out DirectoryInfo testDirectory)
    {
        var assembly = typeof(TestScripts).GetTypeInfo().Assembly;
        testDirectory = CreateTestPathBasedOnAssemblyLocation(assembly);

        foreach (var scriptName in assembly.GetManifestResourceNames().Where(f => f.Contains(".sql")))
        {
            using (var stream = assembly.GetManifestResourceStream(scriptName) ?? throw new FileNotFoundException($"Can't find {scriptName} in {assembly}"))
            {
                var filePath = Path.Combine(testDirectory.FullName, GetScriptPathAndName(scriptName));
                Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? throw new DirectoryNotFoundException($"Can't get directory of {filePath}"));
                using (var writer = new FileStream(filePath, FileMode.Create))
                {
                    stream.CopyTo(writer);
                    writer.Flush();
                }
            }
        }
    }

    static string GetScriptPathAndName(string scriptName)
    {
        var dir = Regex.Match(scriptName, @"\.(Folder\d)");
        if (dir.Success)
        {
            return Path.Combine(dir.Groups[1].Value, scriptName.Replace(dir.Value, ""));
        }

        return scriptName;
    }

    static DirectoryInfo CreateTestPathBasedOnAssemblyLocation(Assembly assembly)
    {
        var directory = new FileInfo(assembly.Location).DirectoryName ?? throw new DirectoryNotFoundException($"Can't get directory of {assembly.Location}");
        var testPath = Path.Combine(directory, "sqlfiles");
        var testDirectory = new DirectoryInfo(testPath);
        testDirectory.Create();
        return testDirectory;
    }
}
