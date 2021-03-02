open System.IO
open FTicker.Host.Services.TickingService
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection

let setBasePath (path: string) (configuration : IConfigurationBuilder) =
    configuration.SetBasePath(path)
    
let addOptionalJson (path: string) (configuration : IConfigurationBuilder) =
    configuration.AddJsonFile(path, false)
let addEnvVars (configuration : IConfigurationBuilder) =
    configuration.AddEnvironmentVariables()

let configureHost argv  =
    let hostBuilder = Host.CreateDefaultBuilder(argv)
    hostBuilder.ConfigureAppConfiguration( fun hostContest configuration ->
            configuration.Sources.Clear()
            configuration 
                |> setBasePath( Directory.GetCurrentDirectory()) 
                |> addOptionalJson "appsettings.json" 
                |> addEnvVars
                |> ignore
            ) |> ignore
        
    hostBuilder.ConfigureServices (fun hostContext services ->
            services.AddHostedService<TickingHostedService>() |> ignore
            ) |> ignore
    hostBuilder.Build()
    
[<EntryPoint>]
let main argv =
    async {
        do! Async.SwitchToThreadPool ()
        use host = configureHost argv        
        let! _ = host.StartAsync() |> Async.AwaitTask
        
        return! host.WaitForShutdownAsync() |> Async.AwaitTask
        
    } |> Async.RunSynchronously
    0 