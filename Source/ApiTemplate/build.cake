var target = Argument("Target", "Default");
var configuration =
    HasArgument("Configuration") ? Argument<string>("Configuration") :
    EnvironmentVariable("Configuration") is not null ? EnvironmentVariable("Configuration") :
    "Release";
//#if (Docker)
var tag =
    HasArgument("Tag") ? Argument<string>("Tag") :
    EnvironmentVariable("Tag") is not null ? EnvironmentVariable("Tag") :
    null;
var platform =
    HasArgument("Platform") ? Argument<string>("Platform") :
    EnvironmentVariable("Platform") is not null ? EnvironmentVariable("Platform") :
    "linux/amd64,linux/arm64";
var push =
    HasArgument("Push") ? Argument<bool>("Push") :
    EnvironmentVariable("Push") is not null ? bool.Parse(EnvironmentVariable("Push")) :
    false;
//#endif

var artefactsDirectory = Directory("./Artefacts");

Task("Clean")
    .Description("Cleans the artefacts, bin and obj directories.")
    .Does(() =>
    {
        CleanDirectory(artefactsDirectory);
        DeleteDirectories(GetDirectories("**/bin"), new DeleteDirectorySettings() { Force = true, Recursive = true });
        DeleteDirectories(GetDirectories("**/obj"), new DeleteDirectorySettings() { Force = true, Recursive = true });
    });

Task("Restore")
    .Description("Restores NuGet packages.")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

Task("Build")
    .Description("Builds the solution.")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetCoreBuild(
            ".",
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                NoRestore = true,
            });
    });

Task("Test")
    .Description("Runs unit tests and outputs test results to the artefacts directory.")
    .DoesForEach(GetFiles("./Tests/**/*.csproj"), project =>
    {
        DotNetCoreTest(
            project.ToString(),
            new DotNetCoreTestSettings()
            {
                Blame = true,
                Collectors = new string[] { "Code Coverage", "XPlat Code Coverage" },
                Configuration = configuration,
                Loggers = new string[]
                {
                    $"trx;LogFileName={project.GetFilenameWithoutExtension()}.trx",
                    $"html;LogFileName={project.GetFilenameWithoutExtension()}.html",
                },
                NoBuild = true,
                NoRestore = true,
                ResultsDirectory = artefactsDirectory,
            });
    });

Task("Publish")
    .Description("Publishes the solution.")
    .DoesForEach(GetFiles("./Source/**/*.csproj"), project =>
    {
        DotNetCorePublish(
            project.ToString(),
            new DotNetCorePublishSettings()
            {
                Configuration = configuration,
                NoBuild = true,
                NoRestore = true,
                OutputDirectory = artefactsDirectory + Directory("Publish"),
            });
    });

//#if (Docker)
Task("DockerBuild")
    .Description("Builds a Docker image.")
    .DoesForEach(GetFiles("./**/Dockerfile"), dockerfile =>
    {
        tag = tag ?? dockerfile.GetDirectory().GetDirectoryName().ToLower();
        var version = GetVersion();
        var gitCommitSha = GetGitCommitSha();

        // Docker buildx allows you to build Docker images for multiple platforms (including x64, x86 and ARM64) and
        // push them at the same time. To enable buildx, you may need to enable experimental support with these commands:
        // docker buildx create --name builder --driver docker-container --use
        // docker buildx inspect --bootstrap
        // To stop using buildx remove the buildx parameter and the --platform, --progress switches.
        // See https://github.com/docker/buildx
        StartProcess(
            "docker",
            new ProcessArgumentBuilder()
                .Append("buildx")
                .Append("build")
                .AppendSwitchQuoted("--platform", platform)
                .AppendSwitchQuoted("--progress", BuildSystem.IsLocalBuild ? "auto" : "plain")
                .Append($"--push={push}")
                .AppendSwitchQuoted("--tag", $"{tag}:{version}")
                .AppendSwitchQuoted("--build-arg", $"Configuration={configuration}")
                .AppendSwitchQuoted("--label", $"org.opencontainers.image.created={DateTimeOffset.UtcNow:o}")
                .AppendSwitchQuoted("--label", $"org.opencontainers.image.revision={gitCommitSha}")
                .AppendSwitchQuoted("--label", $"org.opencontainers.image.version={version}")
                .AppendSwitchQuoted("--file", dockerfile.ToString())
                .Append(".")
                .RenderSafe());

        // If you'd rather not use buildx, then you can uncomment these lines instead.
        // StartProcess(
        //     "docker",
        //     new ProcessArgumentBuilder()
        //         .Append("build")
        //         .AppendSwitchQuoted("--tag", $"{tag}:{version}")
        //         .AppendSwitchQuoted("--build-arg", $"Configuration={configuration}")
        //         .AppendSwitchQuoted("--label", $"org.opencontainers.image.created={DateTimeOffset.UtcNow:o}")
        //         .AppendSwitchQuoted("--label", $"org.opencontainers.image.revision={gitCommitSha}")
        //         .AppendSwitchQuoted("--label", $"org.opencontainers.image.version={version}")
        //         .AppendSwitchQuoted("--file", dockerfile.ToString())
        //         .Append(".")
        //         .RenderSafe());
        // if (push)
        // {
        //     StartProcess(
        //         "docker",
        //         new ProcessArgumentBuilder()
        //             .AppendSwitchQuoted("push", $"{tag}:{version}")
        //             .RenderSafe());
        // }

        string GetVersion()
        {
            var directoryBuildPropsFilePath = GetFiles("Directory.Build.props").Single().ToString();
            var directoryBuildPropsDocument = System.Xml.Linq.XDocument.Load(directoryBuildPropsFilePath);
            var preReleasePhase = directoryBuildPropsDocument.Descendants("MinVerDefaultPreReleasePhase").Single().Value;

            StartProcess(
                "dotnet",
                new ProcessSettings()
                    .WithArguments(x => x
                        .Append("minver")
                        .AppendSwitch("--default-pre-release-phase", preReleasePhase))
                    .SetRedirectStandardOutput(true),
                    out var versionLines);
            return versionLines.LastOrDefault();
        }

        string GetGitCommitSha()
        {
            StartProcess(
                "git",
                new ProcessSettings()
                    .WithArguments(x => x.Append("rev-parse HEAD"))
                    .SetRedirectStandardOutput(true),
                out var shaLines);
            return shaLines.LastOrDefault();
        }
    });

Task("Default")
    .Description("Cleans, restores NuGet packages, builds the solution, runs unit tests and then builds a Docker image.")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("DockerBuild");
////#else
//Task("Default")
//    .Description("Cleans, restores NuGet packages, builds the solution, runs unit tests and then publishes.")
//    .IsDependentOn("Build")
//    .IsDependentOn("Test")
//    .IsDependentOn("Publish");
//#endif

RunTarget(target);
