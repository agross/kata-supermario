module Player

open System

type Bag =
  | Mushroom

type Hearts =
  | Dead
  | Lives of int
  with
  static member (-) (lhs:Hearts, rhs:Hearts) =
    match (lhs, rhs) with
    | (Lives l, Lives r) when (l - r) > 0 ->
      Lives (l - r)
    | _ -> Dead
  static member (+) (lhs:Hearts, rhs:Hearts) =
      match (lhs, rhs) with
      | (Lives l, Lives r) ->
        Lives (l + r)
      | (Dead _, Lives r) ->
        Lives r
      | _ ->
        Dead

type SuperMario =
  {
    Hearts : Hearts
    Bag : Bag option
    State : State
  }

and State =
  | Small
  | Large
  | MaybeImmortal of ImmortalInfo

and ImmortalInfo =
  {
    Until : DateTime
  }

let player =
    {
      State = Small
      Hearts = Lives 3
      Bag = None
    }

type Hit = DateTime -> SuperMario -> SuperMario

let hit : Hit =
  fun hitTime mario->
    match mario.State with
    | MaybeImmortal i when i.Until > hitTime ->
      mario
    | Large ->
      { mario with State = Small; Bag = Some Mushroom }
    | Small | MaybeImmortal _ ->
      match mario.Bag with
      | Some _ ->
        { mario with Bag = None }
      | _ ->
        { mario with Hearts = mario.Hearts - (Lives 1) }

let pickupMushroom (mario : SuperMario) : SuperMario =
  match mario.State with
  | Small | Large ->
    { mario with Bag = Some Mushroom }
  | _ ->
    mario

let findLife (mario : SuperMario) =
  match mario.State with
  | Small ->
    { mario with Hearts = mario.Hearts + (Lives 1) }
  | _ ->
    mario

let findFireFlower (mario : SuperMario) : SuperMario =
  { mario with State = Large; Bag = None }

let findStar (timeFound : DateTime) (mario : SuperMario) : SuperMario =
  let immortalUntil = timeFound.AddSeconds 2.
  { mario with State = MaybeImmortal { Until = immortalUntil } }
