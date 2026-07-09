# DaemonCopy

DaemonCopy is a small cross-platform console utility for one-way backup copying.

It copies files from a source directory to a destination directory:

- missing files are copied;
- existing files are replaced only when the source file is newer;
- subdirectories are processed recursively;
- extra files in the destination directory are not deleted.

## Requirements

- .NET 10 SDK or newer

## Usage

```bash
dotnet run --project DaemonCopy -- "source path" "destination path"
```

After publishing:

```bash
DaemonCopy "source path" "destination path"
```

## Build

```bash
dotnet build
```

## Tests

```bash
dotnet run --project DaemonCopy.Tests
```

## Publish examples

Framework-dependent build:

```bash
dotnet publish DaemonCopy -c Release
```

Self-contained builds:

```bash
dotnet publish DaemonCopy -c Release -r win-x64 --self-contained true
dotnet publish DaemonCopy -c Release -r linux-x64 --self-contained true
dotnet publish DaemonCopy -c Release -r osx-arm64 --self-contained true
```

Logs are written to `error.log`. On startup, the previous log file is moved to
the `log` directory and log files older than 7 days are removed.

## CI and releases

GitHub Actions builds the project on every push to `master` and on pull requests.

To create release binaries, push a version tag:

```bash
git tag v1.2.0
git push origin v1.2.0
```

The release workflow publishes self-contained single-file builds for Linux,
Windows, and macOS.
