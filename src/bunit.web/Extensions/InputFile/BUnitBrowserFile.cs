﻿using Microsoft.AspNetCore.Components.Forms;

namespace Bunit;

internal class BUnitBrowserFile : IBrowserFile
{
	public string Name { get; }
	public DateTimeOffset LastModified { get; }
	public long Size { get; }
	public string ContentType { get; }
	public byte[] Content { get; }

	public BUnitBrowserFile(
		string name,
		DateTimeOffset lastModified,
		long size,
		string contentType,
		byte[] content)
	{
		Name = name;
		LastModified = lastModified;
		Size = size;
		ContentType = contentType;
		Content = content;
	}

	public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
	{
		if (Size > maxAllowedSize)
		{
			throw new IOException($"Supplied file with size {Size} bytes exceeds the maximum of {maxAllowedSize} bytes.");
		}

		return new MemoryStream(Content);
	}
}
