#r "./packages/FAKE/tools/FakeLib.dll"
#load "./lib/tasks/Notifications.fsx"

open Fake
open Fake.ProcessTestRunner
open Notifications
open System

let buildDir  = "./build/"
let dotnetcliVersion = DotNetCli.GetDotNetSDKVersionFromGlobalJson()
let mutable dotnetExePath = "dotnet"

let run' timeout cmd args dir =
  if execProcess (fun info ->
    info.FileName <- cmd
    if not (String.IsNullOrWhiteSpace dir) then
      info.WorkingDirectory <- dir
    info.Arguments <- args
  ) timeout |> not then
    failwithf "Error while running '%s' with args: %s" cmd args

let run = run' System.TimeSpan.MaxValue

let runDotnet workingDir args =
  let result =
    ExecProcess (fun info ->
      info.FileName <- dotnetExePath
      info.WorkingDirectory <- workingDir
      info.Arguments <- args) TimeSpan.MaxValue
  if result <> 0 then failwithf "dotnet %s failed" args

Target "Clean" (fun _ ->
  CleanDirs [buildDir]
)

Target "InstallDotNetCLI" (fun _ ->
  dotnetExePath <- DotNetCli.InstallDotNetSDK dotnetcliVersion
)

Target "Restore" (fun _ ->
  runDotnet "." "restore Mario.sln"
)

let build () =
  runDotnet "." "build Mario.sln"

Target "Build" (fun _ -> build ())

let runTests () =
  ["dotnet", "run --project src/Mario.Specs/Mario.Specs.fsproj"]
  |> RunConsoleTests (fun p -> { p with TimeOut = TimeSpan.FromMinutes 1. })

Target "Test" (fun _ -> runTests ())

Target "Watch" (fun _ ->
  use spec =
    !! "src/**/*.fs"
    |> WatchChanges (fun changes ->
      try
        notify "FAKE" "Build starting" Notification.Info
        build ()
        try
          runTests ()
          notify "Expecto" "Successful" Notification.Success
        with
        | err -> notify "Expecto" "Failed" Notification.Error
      with
      | err -> notify "FAKE" "Failed" Notification.Error
    )

  notify "FAKE" "Started file system watcher" Notification.Info
  System.Console.ReadLine() |> ignore
  spec.Dispose()
)

"Clean"
  ==> "InstallDotNetCLI"
  ==> "Restore"
  ==> "Build"
  ==> "Test"

RunTargetOrDefault "Test"
