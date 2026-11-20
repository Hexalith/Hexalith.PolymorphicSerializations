"""Pack the Release build and run the quickstart against its exact packages."""

import os
import re
import shutil
import subprocess
import tempfile
import zipfile
from pathlib import Path
from xml.etree import ElementTree

ROOT = Path(__file__).resolve().parents[1]
PACKAGES = ("Hexalith.PolymorphicSerializations", "Hexalith.PolymorphicSerializations.CodeGenerators")
VERSION = "0.0.0"


def pack(feed: Path, readme: bytes) -> None:
    for name in PACKAGES:
        project = ROOT / "src" / "libraries" / name / f"{name}.csproj"
        subprocess.run(
            ["dotnet", "pack", str(project), "--no-build", "--no-restore", "-c", "Release",
             "--output", str(feed), f"-p:PackageVersion={VERSION}", "-p:IDEBuild=false",
             "-p:UseHexalithProjectReferences=false"],
            cwd=ROOT, check=True,
        )
        with zipfile.ZipFile(feed / f"{name}.{VERSION}.nupkg") as archive:
            metadata = ElementTree.fromstring(archive.read(f"{name}.nuspec")).find("{*}metadata")
            if metadata is None or metadata.findtext("{*}readme") != "README.md":
                raise ValueError(f"{name}: package metadata must identify README.md")
            if archive.namelist().count("README.md") != 1 or archive.read("README.md") != readme:
                raise ValueError(f"{name}: packed README differs from the quickstart")


def run_sample(work: Path) -> None:
    readme = (ROOT / "README.md").read_bytes()
    feed = work / "feed"
    feed.mkdir()
    pack(feed, readme)

    document = readme.decode("utf-8-sig").replace("\r\n", "\n")
    section = re.search(r"^## Quick Start\n(.*?)(?=^## |\Z)", document, re.MULTILINE | re.DOTALL)
    if section is None:
        raise ValueError("README.md: missing Quick Start")
    code = re.findall(r"^```csharp\n(.*?)\n```$", section[1], re.MULTILINE | re.DOTALL)
    output = re.findall(r"^```text\n(.*?)\n```$", section[1], re.MULTILINE | re.DOTALL)
    if len(code) != 2 or len(output) != 1:
        raise ValueError("README.md: expected Models.cs, Program.cs and one expected output in Quick Start")

    sample = work / "MyProject"
    sample.mkdir()
    shutil.copyfile(ROOT / "global.json", sample / "global.json")
    # A private package cache plus a local-only feed rules out installed or published copies.
    env = dict(os.environ, NUGET_PACKAGES=str(work / "packages"))
    (sample / "NuGet.Config").write_text(
        '<configuration><packageSources><clear/><add key="local" value="../feed"/></packageSources>'
        '<fallbackPackageFolders><clear/></fallbackPackageFolders>'
        '<auditSources><clear/><add key="nuget.org" value="https://api.nuget.org/v3/index.json"/></auditSources>'
        '</configuration>', encoding="utf-8",
    )
    references = "\n".join(f'    <PackageReference Include="{name}" Version="[{VERSION}]" />' for name in PACKAGES)
    (sample / "MyProject.csproj").write_text(
        f"""<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <OutputType>Exe</OutputType>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
{references}
  </ItemGroup>
</Project>
""", encoding="utf-8",
    )
    (sample / "Models.cs").write_text(code[0] + "\n", encoding="utf-8")
    (sample / "Program.cs").write_text(code[1] + "\n", encoding="utf-8")
    subprocess.run(["dotnet", "restore", "--configfile", "NuGet.Config"], cwd=sample, env=env, check=True)
    subprocess.run(["dotnet", "build", "--no-restore", "-c", "Release"], cwd=sample, env=env, check=True)
    result = subprocess.check_output(["dotnet", "bin/Release/net11.0/MyProject.dll"], cwd=sample, env=env, text=True)
    if result.splitlines() != output[0].splitlines():
        raise ValueError(f"README.md: expected {output[0]!r}, got {result!r}")
    print(f"Verified both packed READMEs and the documented result: {output[0]}")


if __name__ == "__main__":
    with tempfile.TemporaryDirectory(prefix="hexalith-serialization-quickstart-") as directory:
        run_sample(Path(directory))
