# gzip-compressor-net

Compress and decompress files to archive via plain GZip. Usable for Unity.

## Usage

### Compression

<code>
gzip -c <folder-to-compress> <archive-file> [options]  
  
gzip -c foo archive.gzip -r -l fast -p *.txt -o
  </code>

### Decompression

<code>
gzip -c <archive-file> <target-folder> [options]
  
gzip -c archive.gzip foo -o
</code>
  
### API

<code>
GZip.Compress(new DirectorySource(dir), stream, CompressionLevel.Fastest);

GZip.Decompress(new TemporaryTarget(), stream);
</code>
