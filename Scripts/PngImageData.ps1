# Shared PNG payload validation. Dot-source from the installer/static gate.
# Uses .NET available in Windows PowerShell 5.1 and PowerShell 7; no Python dependency.
if (-not ('AMJPngImageData' -as [type])) {
    Add-Type -TypeDefinition @'
using System;
using System.IO;
using System.IO.Compression;

public static class AMJPngImageData
{
    static uint U32(byte[] b, int p)
    {
        return ((uint)b[p] << 24) | ((uint)b[p + 1] << 16)
            | ((uint)b[p + 2] << 8) | b[p + 3];
    }

    static void Require(bool value, string message)
    {
        if (!value) throw new InvalidDataException(message);
    }

    public static void Validate(byte[] png)
    {
        byte[] signature = {137, 80, 78, 71, 13, 10, 26, 10};
        Require(png.Length >= 33, "PNG header is truncated");
        for (int i = 0; i < 8; i++)
            Require(png[i] == signature[i], "PNG signature is invalid");
        Require(U32(png, 8) == 13 && U32(png, 12) == 0x49484452,
            "PNG first chunk must be IHDR");
        uint w = U32(png, 16), h = U32(png, 20);
        int depth = png[24], color = png[25], interlace = png[28];
        Require(w > 0 && h > 0 && w <= 16384 && h <= 16384,
            "PNG dimensions are unsupported");
        Require(png[26] == 0 && png[27] == 0 && interlace <= 1,
            "PNG compression/filter/interlace method is invalid");
        int channels = color == 0 ? 1 : color == 2 ? 3 : color == 3 ? 1
            : color == 4 ? 2 : color == 6 ? 4 : 0;
        bool validDepth = color == 0
            ? depth == 1 || depth == 2 || depth == 4 || depth == 8 || depth == 16
            : color == 3 ? depth == 1 || depth == 2 || depth == 4 || depth == 8
            : depth == 8 || depth == 16;
        Require(channels != 0 && validDepth, "PNG color type/bit depth is invalid");
        using (var packed = new MemoryStream())
        {
            int p = 8;
            bool ended = false, sawIdat = false, idatEnded = false;
            while (p + 12 <= png.Length)
            {
                uint n = U32(png, p), type = U32(png, p + 4);
                Require((long)p + n + 12 <= png.Length, "PNG chunk is truncated");
                if (type == 0x49444154)
                {
                    Require(!idatEnded, "PNG IDAT chunks are not consecutive");
                    packed.Write(png, p + 8, (int)n);
                    sawIdat = true;
                }
                else if (sawIdat) idatEnded = true;
                p += (int)n + 12;
                if (type == 0x49454e44)
                {
                    Require(n == 0 && p == png.Length, "PNG IEND/trailing data is invalid");
                    ended = true;
                    break;
                }
            }
            Require(ended && sawIdat, "PNG lacks IDAT/IEND");
            byte[] z = packed.ToArray();
            Require(z.Length >= 6, "PNG zlib stream is truncated");
            Require((z[0] & 15) == 8 && (z[0] >> 4) <= 7
                && (((int)z[0] << 8) + z[1]) % 31 == 0 && (z[1] & 32) == 0,
                "PNG zlib header is invalid");
            uint storedAdler = U32(z, z.Length - 4), a = 1, b = 0;
            int[] x = interlace == 0 ? new[] {0} : new[] {0, 4, 0, 2, 0, 1, 0};
            int[] y = interlace == 0 ? new[] {0} : new[] {0, 0, 4, 0, 2, 0, 1};
            int[] dx = interlace == 0 ? new[] {1} : new[] {8, 8, 4, 4, 2, 2, 1};
            int[] dy = interlace == 0 ? new[] {1} : new[] {8, 8, 8, 4, 4, 2, 2};
            long total = 0;
            using (var source = new MemoryStream(z, 2, z.Length - 6))
            using (var stream = new DeflateStream(source, CompressionMode.Decompress))
            {
                for (int pass = 0; pass < x.Length; pass++)
                {
                    int width = w <= x[pass] ? 0 : ((int)w - x[pass] + dx[pass] - 1) / dx[pass];
                    int height = h <= y[pass] ? 0 : ((int)h - y[pass] + dy[pass] - 1) / dy[pass];
                    if (width == 0 || height == 0) continue;
                    int rowBytes = (width * channels * depth + 7) / 8;
                    total += (long)(rowBytes + 1) * height;
                    Require(total <= 268435456, "PNG decoded payload exceeds 256 MiB limit");
                    var row = new byte[rowBytes + 1];
                    for (int r = 0; r < height; r++)
                    {
                        int received = 0;
                        while (received < row.Length)
                        {
                            int count = stream.Read(row, received, row.Length - received);
                            Require(count > 0, "PNG decoded scanlines are truncated");
                            received += count;
                        }
                        Require(row[0] <= 4, "PNG scanline filter is invalid");
                        for (int i = 0; i < row.Length; i++)
                        {
                            a = (a + row[i]) % 65521;
                            b = (b + a) % 65521;
                        }
                    }
                }
                Require(stream.ReadByte() == -1, "PNG decoded payload exceeds dimensions");
            }
            Require(((b << 16) | a) == storedAdler, "PNG zlib Adler-32 is invalid");
        }
    }
}
'@
}
