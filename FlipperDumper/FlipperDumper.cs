using Com.Lego.Flipper.Windows.Content.Providers;
using Microsoft.Extensions.FileProviders;

var destinationFolderRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FlipperDumper");
var contentProvider = new EmbeddedResourcesPrepackedContentProvider();

DumpDirectory("variant");

void DumpDirectory(string? directoryPath)
{
    if (string.IsNullOrWhiteSpace(directoryPath)) return;

    var directoryContents = contentProvider.GetDirectoryContents(directoryPath + "/");
    if (directoryContents == null) return;

    foreach (var fileInfo in directoryContents)
    {
        if (string.IsNullOrWhiteSpace(fileInfo.PhysicalPath)) continue;

        Console.WriteLine(fileInfo.PhysicalPath);
        if (fileInfo.IsDirectory)
        {
            Directory.CreateDirectory(Path.Combine(destinationFolderRoot, fileInfo.PhysicalPath));
            DumpDirectory(fileInfo.PhysicalPath);
        }
        else
        {
            DumpFile(fileInfo);
        }
    }
}

void DumpFile(IFileInfo? fileInfo)
{
    if (fileInfo == null || string.IsNullOrWhiteSpace(fileInfo.PhysicalPath)) return;
        
    var sourceStream = fileInfo.CreateReadStream();
    var destinationStream = File.Create(Path.Combine(destinationFolderRoot, fileInfo.PhysicalPath));

    sourceStream.CopyTo(destinationStream);
    destinationStream.Close();
}
