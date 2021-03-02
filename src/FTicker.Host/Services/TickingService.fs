module FTicker.Host.Services.TickingService
open System
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

type TickingHostedService (logger: ILogger<TickingHostedService>) =
    inherit BackgroundService()
       
    override this.ExecuteAsync(stoppingToken :CancellationToken) =
        "starting background process" |> logger.LogInformation 
        async {
            while( not stoppingToken.IsCancellationRequested) do 
                $"tick at {DateTime.Now}" |> logger.LogInformation
                do! Async.Sleep 1000
        } |> Async.StartAsTask :> Task