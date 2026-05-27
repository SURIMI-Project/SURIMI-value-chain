# Copilot instructions for SURIMI-value-chain

## Build, run, and packaging commands

- Build the solution from the repository root with `dotnet build .\SURIMI-value-chain.sln -v minimal`.
- Run the gRPC host locally with `dotnet run --project .\SURIMI-value-chain\SURIMI-value-chain.csproj`. Development launch settings use `http://localhost:7990` and `https://localhost:7343`.
- The service is gRPC-only. `appsettings.json` configures Kestrel for HTTP/2, and the `/` route only returns an informational message.
- The README documents Docker image publishing with `dotnet build /t:BuildPushDockerImage -v:detailed`.
- `.\build-docker.ps1 <DockerfilePath> <ImageName>` builds and pushes a Docker image, reading `SURIMI_DOCKER_BUILD_GITHUB_TOKEN` and `SURIMI_DOCKER_BUILD_BSR_TOKEN` from the environment.
- `NuGet.config` uses package source mapping: `SURIMI.*` and `Eii.*` come from GitHub Packages, and `BSR.*` comes from Buf's NuGet feed. If restore fails in a clean environment, check feed credentials before changing project files.

## Test and lint status

- Run integration tests with `dotnet test .\SURIMI-value-chain.sln --no-build` from the repository root.
- Integration tests live in `SURIMI-value-chain.IntegrationTests\`. They use `xUnit`, `Microsoft.AspNetCore.Mvc.Testing`, and `Grpc.Net.Client` to spin up the real gRPC host in-process and call every RPC via a `GrpcTestFixture` (`WebApplicationFactory<Program>`).
- JSON request fixtures are stored under `SURIMI-value-chain.IntegrationTests\GrpcMessages\<RpcName>\<MessageType>.json` and loaded at runtime by `GrpcMessageLoader`. Add or update a fixture file whenever the proto contract or test data changes.
- There is no repository-specific lint command checked in. Follow `.editorconfig` and keep changes compatible with `dotnet build`.

## High-level architecture

- This repository is a single .NET 8 ASP.NET Core gRPC host. `Program.cs` wires shared service defaults, gRPC interceptors from `SURIMI.Common.gRPC`, a singleton `IEwEController`, and `ProtocolVersionService`, then maps one gRPC service: `ValueChainService`.
- `ValueChainService` is the adapter from the generated `Grpc.Surimi.ValueChainService` contract into the repository's EwE controller. It owns experiment lifecycle RPCs, serves protocol version information, maps the incoming `Grpc.Surimi.Simulation` payload into `SURIMI.Datamodel.SurimiConfiguration`, and forwards sales-statistics updates to the controller.
- `EwEController` is the stateful boundary to the EwE integration. Its singleton lifetime is important: all gRPC calls share the same controller state (`RunState`, `IsWaiting`, start/continue/stop/update methods).

## Repository-specific conventions

- Keep new service logic in the gRPC service class thin and controller-driven. RPC handlers validate input, log the experiment context, delegate to `IEwEController`, and throw `RpcException` with `StatusCode.Internal` when controller operations fail.
- Keep the gRPC-to-datamodel translation in the service layer, following `ValueChainService.GetSurimiConfiguration()` rather than pushing protobuf types deeper into the EwE controller.
- The codebase uses block-scoped namespaces, explicit types instead of `var`, and `using` directives outside the namespace, matching `.editorconfig`.
- Private fields commonly use the `m_` prefix (`m_logger`, `m_controller`, `m_runstate`). Match that naming when extending existing classes.
- CI is currently a lightweight PR build on `master` via `Official-EwE/Eii.GithubActions/BuildCheckNet80@master`, so keep changes compatible with the existing .NET 8 build path instead of introducing extra solution-level tooling.
