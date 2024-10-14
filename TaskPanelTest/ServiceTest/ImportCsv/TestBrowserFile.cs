using Microsoft.AspNetCore.Components.Forms;

namespace TaskPanelTest.ServiceTest.ImportCsv;

public class TestBrowserFile : IBrowserFile
{
    private readonly Stream _stream;

    public TestBrowserFile(Stream stream, string name, string contentType)
    {
        _stream = stream;
        Name = name;
        ContentType = contentType;
        Size = _stream.Length;
        LastModified = DateTimeOffset.Now;
    }

    public string Name { get; }
    public string ContentType { get; }
    public long Size { get; }
    public DateTimeOffset LastModified { get; }

    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
    {
        return _stream;
    }
}