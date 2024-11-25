﻿using Microsoft.AspNetCore.Components.Forms;

namespace Craft.Utilities.Services;

public class FileUploadService
{
    /// <summary>
    /// Uploads a file to the specified destination file path.
    /// </summary>
    /// <param name="destFilePath">Destination path to save the file</param>
    /// <param name="sourceFile">The source file being uploaded.</param>
    /// <param name="allowedSizeLimit">Maximum allowed file size in MB</param>
    /// <param name="progressCallback">Callback to report upload progress (0-100)</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UploadFileAsync(string? destFilePath, IBrowserFile sourceFile, Action<int> progressCallback = null,
        long allowedSizeLimit = 10)
    {
        ArgumentNullException.ThrowIfNull(sourceFile);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(destFilePath);

        if (sourceFile.Size > (allowedSizeLimit * 1024 * 1024))
            throw new InvalidOperationException($"File size exceeds the allowed limit of {allowedSizeLimit} MB.");

        const int bufferSize = 16 * 1024; // 16 KB buffer size

        byte[] buffer = new byte[bufferSize];
        long totalBytesRead = 0;
        long totalFileSize = sourceFile.Size;

        try
        {
            using (var sourceStream = sourceFile.OpenReadStream())
            using (var destinationStream = new FileStream(destFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, useAsync: true))
            {
                int bytesRead;

                while ((bytesRead = await sourceStream.ReadAsync(buffer)) > 0)
                {
                    await destinationStream.WriteAsync(buffer.AsMemory(0, bytesRead));

                    totalBytesRead += bytesRead;

                    int progress = (int)((totalBytesRead * 100) / totalFileSize);

                    progressCallback?.Invoke(progress);
                }
            }
        }
        catch (Exception ex)
        {
            throw new IOException("An error occurred while uploading the file.", ex);
        }
    }
}
