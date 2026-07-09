using DaemonCopy;
using DaemonCopy.Services;

namespace DaemonCopy.Tests
{
    internal static class Program
    {
        private static int Main()
        {
            var tests = new (string Name, Action Body)[]
            {
                ("copies missing files recursively", CopiesMissingFilesRecursively),
                ("updates destination when source is newer", UpdatesDestinationWhenSourceIsNewer),
                ("keeps destination when source is older", KeepsDestinationWhenSourceIsOlder),
                ("creates missing destination directory", CreatesMissingDestinationDirectory),
            };

            var failures = 0;

            foreach (var test in tests)
            {
                try
                {
                    test.Body();
                    Console.WriteLine($"PASS {test.Name}");
                }
                catch (Exception ex)
                {
                    failures++;
                    Console.WriteLine($"FAIL {test.Name}");
                    Console.WriteLine(ex.Message);
                }
            }

            if (failures > 0)
            {
                Console.WriteLine($"{failures} test(s) failed.");
                return 1;
            }

            Console.WriteLine("All tests passed.");
            return 0;
        }

        private static void CopiesMissingFilesRecursively()
        {
            using var workspace = TestWorkspace.Create();
            var source = workspace.CreateDirectory("source");
            var destination = workspace.GetDirectory("destination");

            WriteFile(source, "root.txt", "root");
            WriteFile(source, Path.Combine("nested", "child.txt"), "child");

            Copy(source, destination);

            AssertFileText(destination, "root.txt", "root");
            AssertFileText(destination, Path.Combine("nested", "child.txt"), "child");
        }

        private static void UpdatesDestinationWhenSourceIsNewer()
        {
            using var workspace = TestWorkspace.Create();
            var source = workspace.CreateDirectory("source");
            var destination = workspace.CreateDirectory("destination");

            WriteFile(source, "data.txt", "old destination");
            WriteFile(destination, "data.txt", "old destination");

            var destinationFile = Path.Combine(destination.FullName, "data.txt");
            File.SetLastWriteTimeUtc(destinationFile, DateTime.UtcNow.AddMinutes(-10));

            WriteFile(source, "data.txt", "new source");
            var sourceFile = Path.Combine(source.FullName, "data.txt");
            File.SetLastWriteTimeUtc(sourceFile, DateTime.UtcNow);

            Copy(source, destination);

            AssertFileText(destination, "data.txt", "new source");
        }

        private static void KeepsDestinationWhenSourceIsOlder()
        {
            using var workspace = TestWorkspace.Create();
            var source = workspace.CreateDirectory("source");
            var destination = workspace.CreateDirectory("destination");

            WriteFile(source, "data.txt", "old source");
            WriteFile(destination, "data.txt", "new destination");

            var sourceFile = Path.Combine(source.FullName, "data.txt");
            var destinationFile = Path.Combine(destination.FullName, "data.txt");
            File.SetLastWriteTimeUtc(sourceFile, DateTime.UtcNow.AddMinutes(-10));
            File.SetLastWriteTimeUtc(destinationFile, DateTime.UtcNow);

            Copy(source, destination);

            AssertFileText(destination, "data.txt", "new destination");
        }

        private static void CreatesMissingDestinationDirectory()
        {
            using var workspace = TestWorkspace.Create();
            var source = workspace.CreateDirectory("source");
            var destination = workspace.GetDirectory(Path.Combine("backup", "current"));

            WriteFile(source, "data.txt", "content");

            Copy(source, destination);

            AssertTrue(Directory.Exists(destination.FullName), $"Expected destination to exist: {destination.FullName}");
            AssertFileText(destination, "data.txt", "content");
        }

        private static void Copy(DirectoryInfo source, DirectoryInfo destination)
        {
            var copy = new MirrorCopyFiles(new Log("Tests"));
            copy.CopyFilesLefttoRight(source, destination);
        }

        private static void WriteFile(DirectoryInfo directory, string relativePath, string contents)
        {
            var filePath = Path.Combine(directory.FullName, relativePath);
            var parent = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(parent))
                Directory.CreateDirectory(parent);

            File.WriteAllText(filePath, contents);
        }

        private static void AssertFileText(DirectoryInfo directory, string relativePath, string expected)
        {
            var filePath = Path.Combine(directory.FullName, relativePath);
            AssertTrue(File.Exists(filePath), $"Expected file to exist: {filePath}");

            var actual = File.ReadAllText(filePath);
            AssertEqual(expected, actual, filePath);
        }

        private static void AssertEqual(string expected, string actual, string context)
        {
            if (expected != actual)
                throw new InvalidOperationException($"Expected '{expected}', got '{actual}'. Context: {context}");
        }

        private static void AssertTrue(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }
    }

    internal sealed class TestWorkspace : IDisposable
    {
        private readonly string _previousDirectory;

        private TestWorkspace(string path)
        {
            Root = path;
            _previousDirectory = Environment.CurrentDirectory;
            Directory.CreateDirectory(Root);
            Environment.CurrentDirectory = Root;
        }

        public string Root { get; }

        public static TestWorkspace Create()
        {
            var root = Path.Combine(Path.GetTempPath(), "DaemonCopy.Tests", Guid.NewGuid().ToString("N"));
            return new TestWorkspace(root);
        }

        public DirectoryInfo CreateDirectory(string relativePath)
        {
            var directory = GetDirectory(relativePath);
            directory.Create();
            return directory;
        }

        public DirectoryInfo GetDirectory(string relativePath)
        {
            return new DirectoryInfo(Path.Combine(Root, relativePath));
        }

        public void Dispose()
        {
            Environment.CurrentDirectory = _previousDirectory;

            if (Directory.Exists(Root))
                Directory.Delete(Root, recursive: true);
        }
    }
}
