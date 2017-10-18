module Notifications

#r "../../packages/FAKE/tools/FakeLib.dll"

open Fake

type Notification =
  | Info
  | Success
  | Error

let notify title message notification =
  let message = match notification with
                | Info -> (sprintf "ℹ️ %s" message, None)
                | Success -> (sprintf "✅ %s" message, None)
                | Error -> (sprintf "❌ %s" message, Some "Funk")

  match (tryFindFileOnPath "osascript") with
  | Some exe ->
    let subtitle, sound = match message with
                          | (text, Some sound) ->
                              (sprintf """subtitle "%s" """ text,
                                sprintf """sound name "%s" """ sound)
                          | (text, None) -> (sprintf """subtitle "%s" """ text, "")

    let args = sprintf """-e 'display notification "" with title "%s" %s %s'""" title subtitle sound
    let result = Shell.Exec(exe, args)

    // If exe does not exist, Shell.Exec does not seem to error, perhaps because of WatchChanged.
    if result <> 0 then failwithf "%s returned with a non-zero exit code" exe
  | _ -> ()
